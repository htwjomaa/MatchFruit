using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FruitMatch.Scripts.Core;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LuckCalculator : MonoBehaviour
{
    [SerializeField] public float NeededPieces;
    [SerializeField] public float NeededPiecesOverTime;
    [SerializeField] public float BeneficialExtras;
    [SerializeField] public float BeneficialExtrasOverTime;
    [SerializeField] public float MaliciousExtras;
    [SerializeField] public float MaliciousExtrasOverTime;


    public HashSet<int> neededList;
    public HashSet<int> avoidedList;
    public HashSet<int> restList;


    private readonly float _spawnRateMultiplier = 2.5f;
    private readonly float _spawnRateMultiplierOverTime = 1f;

    private void Start()
    {
        LevelManager.MoveMadeEvent += TickOverTime;
    }

    private void OnDestroy()
    {
        LevelManager.MoveMadeEvent -= TickOverTime;
    }

    private float TranslateFloats(float v, float multiplier)
    {
        if(v > 0.5f)
            return MathLibrary.Remap(0.5f,1f,0f,_spawnRateMultiplier,v);
     return MathLibrary.Remap(0.5f,0,0f,-_spawnRateMultiplier,v);
    }
    private void TranslateTo100S()
    {
        NeededPieces = TranslateFloats(NeededPieces, _spawnRateMultiplier);
       NeededPiecesOverTime = TranslateFloats(NeededPiecesOverTime, _spawnRateMultiplierOverTime);
     BeneficialExtras =  TranslateFloats(BeneficialExtras, _spawnRateMultiplier); 
     BeneficialExtrasOverTime = TranslateFloats(BeneficialExtrasOverTime, _spawnRateMultiplierOverTime);
    MaliciousExtras = TranslateFloats(MaliciousExtras, _spawnRateMultiplier);
    MaliciousExtrasOverTime = TranslateFloats(MaliciousExtrasOverTime, _spawnRateMultiplierOverTime);
    }

    public void LoadNumbers(LuckConfig luckConfig)
    {
        NeededPieces = luckConfig.NeededPieces;
        NeededPiecesOverTime =  luckConfig.NeededPiecesOverTime;
        BeneficialExtras =   luckConfig.BeneficialExtras;
        BeneficialExtrasOverTime =  luckConfig.BeneficialExtrasOverTime;
        MaliciousExtras =  luckConfig.MaliciousExtras;
        MaliciousExtrasOverTime =  luckConfig.MaliciousExtrasOverTime;
        
        TranslateTo100S();
        TranslateToTickRate(LevelManager.THIS.MaxLimit, ref NeededPiecesOverTime, ref BeneficialExtrasOverTime, ref MaliciousExtrasOverTime);
    }

    public void TickOverTime()
    {
        NeededPieces += NeededPiecesOverTime;
        BeneficialExtras += NeededPiecesOverTime;
        MaliciousExtras += MaliciousExtrasOverTime;
        
    }
    public void LoadCurrentTargets(HashSet<int> needed, HashSet<int>  avoided, int colorLimit)
    {

        neededList = new HashSet<int>();
        avoidedList = new HashSet<int>();
        restList = new HashSet<int>();
        
        neededList = needed;
        avoidedList = avoided;

        for (int i = 0; i < colorLimit + 1; i++)  //really colorlimit +1=?????
        {
            bool nothingFound = true;
            for (int j = 0; j < neededList.Count; j++)
            {
                if (neededList.Contains(i)) nothingFound = false;
            }
            
            for (int k = 0; k < avoidedList.Count; k++)
            {
                if (avoidedList.Contains(i)) nothingFound = false;
            }

            if (!nothingFound) restList.Add(i);
        }
    }
    private void  TranslateToTickRate(float MaxMoves, ref float NeededPiecesOvertime, ref float BeneficialExtrasOverTime, ref float MaliciousExtrasOverTime)
    {
        NeededPiecesOvertime /= MaxMoves;
        BeneficialExtrasOverTime /= MaxMoves;
        MaliciousExtrasOverTime /= MaxMoves;
    }

    private int _allCount = 0;
    private void GetAllCount()
    {
        _allCount = 0;
        if (neededList == null || avoidedList == null || restList == null)
        {
            LevelManager.THIS.LoadLuckTargets();
        }
        if(neededList != null) _allCount += neededList.Count;
        if( avoidedList != null) _allCount += avoidedList.Count;
        if( restList != null) _allCount += restList.Count;
    }

    private float RegionNeeded() => (float)neededList.Count / (float)_allCount;
    private float RegionAvoided() => (float)avoidedList.Count / (float)_allCount;
    private float RegionRest() => (float)restList.Count / (float)_allCount;

    [SerializeField] private float  spawnRateNeededDebug;
    [SerializeField] private float  spawnRateMalDebug;

    public (float,float)  GetSpawnrateItems()
    { 
        GetAllCount();
       float spawnRateNeeded = CalculateSpawnrate(RegionNeeded(), NeededPieces);
       float spawnRateMal = CalculateSpawnrate(RegionAvoided(), MaliciousExtras);
       float spawnRateRest = RegionRest();

   
       spawnRateNeededDebug = spawnRateNeeded;
       spawnRateMalDebug = spawnRateMal;
       
       float allTogether = spawnRateNeeded + spawnRateMal + spawnRateRest;
       spawnRateNeeded = CalculateRealSpawnRate(spawnRateNeeded, allTogether);
       spawnRateMal = CalculateRealSpawnRate(spawnRateMal, allTogether);

       
       return (spawnRateNeeded, spawnRateMal);
    }

    private float CalculateRealSpawnRate(float spawnrate, float alltogether) =>
        MathLibrary.Remap(0, alltogether, 0, 99, spawnrate);
    
    private float CalculateSpawnrate(float region, float v)
    {
        if (v < 0) v = region / v;
        else v = region * v;
        return Mathf.Abs(v);
    }

    [NaughtyAttributes.Button()] private void TestColor()
    {
        var n = GetColor();
    }

    public int GetColor()
    {
        (float, float) spawnRates = GetSpawnrateItems();
        int randomnumber = Random.Range(0, 100);

        if (randomnumber < spawnRates.Item1)
            return neededList.ToArray()[Random.Range(0, neededList.Count)]; 
        if (randomnumber > spawnRates.Item1 && randomnumber < spawnRates.Item2)
            return avoidedList.ToArray()[Random.Range(0, avoidedList.Count)];
        return restList.ToArray()[Random.Range(0, restList.Count)];
        
    }
    
}

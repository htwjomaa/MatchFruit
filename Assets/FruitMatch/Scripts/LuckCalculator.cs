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
    [SerializeField] public float overallMultiplicator;
    [SerializeField] public float overallMultiplicatorForOverTime;

    public bool NeededPiecesOnlyStart;
    public bool BeneficialExtrasOnlyStart;
    public bool MaliciousExtrasOnlyStart;


    private HashSet<int> neededList;
    private HashSet<int> avoidedList;
    private HashSet<int> restList;


    [SerializeField] private  float _spawnRateMultiplier = 2.5f;
    [SerializeField]  private float _spawnRateMultiplierOverTime = 1f;

    private void Start()
    {
        LevelManager.MoveMadeEvent += TickOverTime;
        
        if(LevelManager.THIS?.levelData == null || !LevelManager.THIS.levelLoaded)
                LevelManager.OnLevelLoaded += CheckOnlyStart;
    }


    private void OnDestroy()
    {
        LevelManager.MoveMadeEvent -= TickOverTime;
        LevelManager.OnLevelLoaded -= CheckOnlyStart;
    }

    private float TranslateFloats(float v, float _spawnRateMultiplier)
    {
        return v > 0.5f
            ? MathLibrary.Remap(0.5f, 1f, 0f, _spawnRateMultiplier, v)
            : MathLibrary.Remap(0.5f, 0, 0f, -_spawnRateMultiplier, v);
    }
    
    public void CheckOnlyStart()
    { 
        if (NeededPiecesOnlyStart) NeededPieces = 0;
       if (BeneficialExtrasOnlyStart) BeneficialExtras = 0;
       if (MaliciousExtrasOnlyStart) MaliciousExtras = 0;
    }
    private void TranslateTo100S()
    {
        _spawnRateMultiplier = 2.5f;
        _spawnRateMultiplierOverTime = 1f;

        overallMultiplicator = TranslateFloats(overallMultiplicator, 40);
        overallMultiplicatorForOverTime = TranslateFloats(overallMultiplicatorForOverTime, 7);
        
        if (overallMultiplicator >= 0) 
            _spawnRateMultiplier += overallMultiplicator;
        else
        {
            overallMultiplicator /= 16;
            _spawnRateMultiplier -= overallMultiplicator;
        }
        if (overallMultiplicatorForOverTime >= 0) 
            _spawnRateMultiplierOverTime += overallMultiplicatorForOverTime;
        else
        {
            overallMultiplicatorForOverTime /= 10;
            _spawnRateMultiplierOverTime -= overallMultiplicatorForOverTime;
        }
        
        NeededPieces = TranslateFloats(NeededPieces, _spawnRateMultiplier); 
        NeededPiecesOverTime = TranslateFloats(NeededPiecesOverTime, _spawnRateMultiplierOverTime);
     BeneficialExtras =  TranslateFloats(BeneficialExtras, _spawnRateMultiplier); 
     BeneficialExtrasOverTime = TranslateFloats(BeneficialExtrasOverTime, _spawnRateMultiplierOverTime);
    MaliciousExtras = TranslateFloats(MaliciousExtras, _spawnRateMultiplier);
    MaliciousExtrasOverTime = TranslateFloats(MaliciousExtrasOverTime, _spawnRateMultiplierOverTime);
    }
    
    public void LoadNumbers(LuckConfig luckConfig)
    {
        //[0]  ==  currentField
        NeededPieces = luckConfig.NeededPieces[0];
        NeededPiecesOverTime =  luckConfig.NeededPiecesOverTime[0];
        BeneficialExtras =   luckConfig.BeneficialExtras[0];
        BeneficialExtrasOverTime =  luckConfig.BeneficialExtrasOverTime[0];
        MaliciousExtras =  luckConfig.MaliciousExtras[0];
        MaliciousExtrasOverTime =  luckConfig.MaliciousExtrasOverTime[0];
        overallMultiplicator = luckConfig.Overall[0];
        overallMultiplicatorForOverTime = luckConfig.Overall[0];


        NeededPiecesOnlyStart = luckConfig.BeneficialExtrasOnlyStart[0];
        BeneficialExtrasOnlyStart = luckConfig.BeneficialExtrasOnlyStart[0];
        MaliciousExtrasOnlyStart = luckConfig.MaliciousExtrasOnlyStart[0];
        
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
            foreach (var t in neededList)
            {
                if (neededList.Contains(i)) nothingFound = false;
            }
            
            foreach (var t in avoidedList)
            {
                if (avoidedList.Contains(i)) nothingFound = false;
            }

            if (nothingFound) restList.Add(i);
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
       spawnRateNeeded += AddBaselineSpawnrate(_allCount, neededList.Count);
       spawnRateMal += AddBaselineSpawnrate(_allCount, avoidedList.Count);
       spawnRateRest += AddBaselineSpawnrate(_allCount, restList.Count);
       
       spawnRateNeededDebug = spawnRateNeeded;
       spawnRateMalDebug = spawnRateMal;
       
       float allTogether = spawnRateNeeded + spawnRateMal + spawnRateRest;
       spawnRateNeeded = CalculateRealSpawnRate(spawnRateNeeded, allTogether);
       spawnRateMal = CalculateRealSpawnRate(spawnRateMal, allTogether);

       
       return (spawnRateNeeded, spawnRateMal);
    }

    private float AddBaselineSpawnrate(float allcount, float region)
    {
        return region / allcount;
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

    [NaughtyAttributes.Button()] private void TestRestList()
    {
        foreach( var n in restList)
            Debug.Log(n);
    }
    [NaughtyAttributes.Button()] private void TestNeededList()
    {
        foreach( var n in neededList)
            Debug.Log(n);
    }
    [NaughtyAttributes.Button()] private void TestAvoidedList()
    {
        foreach( var n in avoidedList)
            Debug.Log(n);
    }
    public int GetColor()
    {
        (float, float) spawnRates = GetSpawnrateItems();
        int randomnumber = Random.Range(0, 100);

        if (randomnumber < spawnRates.Item1)
            return neededList.ToArray()[Random.Range(0, neededList.Count)]; 
        if (randomnumber > spawnRates.Item1 && randomnumber < spawnRates.Item2)
            return avoidedList.ToArray()[Random.Range(0, avoidedList.Count)];

        {
            var n = restList.ToArray()[Random.Range(0, restList.Count)];
            Debug.Log("RESTLISTCOLOR: " + n);
            return n;
        }
        
    }
    
}

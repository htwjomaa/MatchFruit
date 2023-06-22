using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SwitchButtonFruit : MonoBehaviour
{
 [SerializeField] private Colors fruitColor;

 [Space] [Header("Button things")] 
 [SerializeField] private GameObject SwitchButtonOn;
 [SerializeField] private GameObject SwitchBackgroundOn;
 [SerializeField] private GameObject SwitchButtonOff;
 [SerializeField] private GameObject SwitchBackgroundOff;
 
 private void Start()
 {
  //has to be on Start because other subscribe on Awake
  SwitchButtonFruitsManager.LoadFruitEvent += Load;
  //Invoke once to avoid a unity bug, where the first fruit doesnt get loaded
  SwitchButtonFruitsManager.InvokeLoadEvent();  
  // SwitchButtonFruitsManager.SwitchButtonFruits.Add(this);
  // SwitchButtonFruitsManager.LoadFruitButtonsfromClipboard.AddListener(delegate { Load(); });
 }

 private void OnDestroy()
 {
  SwitchButtonFruitsManager.LoadFruitEvent -= Load;
 }

 public void Load()
 {
  for (int i = 0; i< Rl.saveClipBoard.FruitClipboardParent.Length; i++)
  {
   if (SwitchButtonFruitsManager.FruitType == Rl.saveClipBoard.FruitClipboardParent[i].FruitType)
   {
   
    for (int j = 0; j < Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards.Length; j++)
    {
     if (fruitColor == Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].FruitColor)
     {
      SwitchButtonFruitsManager.LoadEnableDisableFruitColorFromClipBoard(SwitchBackgroundOn,  SwitchButtonOn, SwitchButtonOff, SwitchBackgroundOff,
      Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].IsEnabled); 
     // Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].FruitPhase = fruitPhase;
     // Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].SpawnStart = spawnStart;
     // Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].SpawnEnd = spawnEnd;
     // Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].SpawnChance = spawnChance;
     // Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].SpawnChanceOverTime = spawnChanceOverTime;
     // Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].SpawnChanceNegative = spawnChanceNegative;
     }
     else if (fruitColor == Colors.AlleFarben)
     {
      SwitchButtonFruitsManager.LoadEnableDisableFruitColorFromClipBoard(SwitchBackgroundOn,  SwitchButtonOn, SwitchButtonOff, SwitchBackgroundOff,Rl.saveClipBoard.FruitClipboardParent[i].FruitEnabled);
     }
    }
   }
  }
 }
 public void SaveEnableDisableFruitColor(bool isEnabled)
 {
 if(isEnabled) Rl.GameManager.PlayAudio(Rl.soundStrings.SwitchButtonSound, Random.Range(2,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
 else Rl.GameManager.PlayAudio(Rl.soundStrings.SwitchButtonSound, Random.Range(0,2), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);

 if (fruitColor == Colors.AlleFarben) SwitchButtonFruitsManager.SaveToClipboard(isEnabled);
  else SwitchButtonFruitsManager.SaveToClipboard(SwitchButtonFruitsManager.FruitType, fruitColor, isEnabled);

 SwitchBackgroundOn.SetActive(isEnabled);
  SwitchButtonOn.SetActive(isEnabled);
        
  SwitchButtonOff.SetActive(!isEnabled);
  SwitchBackgroundOff.SetActive(!isEnabled);
 }

 public void SaveFruitPhase(bool fruitPhase)
 {
  SwitchButtonFruitsManager.SaveToClipboard(SwitchButtonFruitsManager.FruitType, fruitColor, fruitPhase);
 }

 public void SaveSpawnStart(uint spawnStart)
 {
  SwitchButtonFruitsManager.SaveToClipboard(SwitchButtonFruitsManager.FruitType, fruitColor, spawnStart);
 }

 public void SaveSpawnEnd(int spawnEnd)
 {
  SwitchButtonFruitsManager.SaveToClipboard(SwitchButtonFruitsManager.FruitType, fruitColor, spawnEnd);
 }

 public void SaveSpawnChance(byte spawnChance)
 {
  SwitchButtonFruitsManager.SaveToClipboard(SwitchButtonFruitsManager.FruitType, fruitColor, spawnChance);
 }

 public void SaveSpawnChanceOverTime(short spawnChanceOvertimeShort)
 {
  bool spawnChanceNegative;
  if (spawnChanceOvertimeShort > -1)
   spawnChanceNegative = false;
  else spawnChanceNegative = true;
  
  SwitchButtonFruitsManager.SaveToClipboard(SwitchButtonFruitsManager.FruitType, fruitColor,  Math.Abs(spawnChanceOvertimeShort), spawnChanceNegative);
 }
}
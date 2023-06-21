using System;
using UnityEngine;

public static class SwitchButtonFruitsManager
{
    public static FruitType FruitType;

   // public static List<SwitchButtonFruit> SwitchButtonFruits = new List<SwitchButtonFruit>();
  //  public static List<Delegate> loadFunctions = new List<Delegate>(); 
  public delegate void DoSomething();
  public static event DoSomething LoadFruitEvent;

  public static void InvokeLoadEvent()
  {
      LoadFruitEvent?.Invoke();
  }
  
  public static void LoadEnableDisableFruitColorFromClipBoard(GameObject SwitchBackgroundOn, GameObject SwitchButtonOn, GameObject SwitchButtonOff, GameObject SwitchBackgroundOff, bool isEnabled)
  {
      SwitchBackgroundOn.SetActive(isEnabled);
      SwitchButtonOn.SetActive(isEnabled);
        
      SwitchButtonOff.SetActive(!isEnabled);
      SwitchBackgroundOff.SetActive(!isEnabled);
  }
    public  static void LoadAllFruitSettings(BoardDimensionsConfig boardDimensionsConfig)
    {
        Rl.saveClipBoard.FruitClipboardParent = (FruitsConfigParent[])GenericSettingsFunctions.GetDeepCopy(boardDimensionsConfig.FruitsConfigParent);
        InvokeLoadEvent();
    }
    

    public static bool GetFruitEntry(FruitsConfigParent FruitClipboardParent)
    {
        if (FruitClipboardParent.FruitType == FruitType) return true;
        else return false;
    }
    public static void SaveToClipboard(FruitType fruitType, Colors fruitColor, short spawnChanceOverTime,bool spawnChanceNegative)
    {
        for (int i = 0; i< Rl.saveClipBoard.FruitClipboardParent.Length; i++)
        {
            if (fruitType == Rl.saveClipBoard.FruitClipboardParent[i].FruitType)
            {
                for (int j = 0; j < Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards.Length; j++)
                {
                    if (fruitColor == Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].FruitColor)
                    {
                        Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].SpawnChanceOverTime = spawnChanceOverTime;
                        Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].SpawnChanceNegative = spawnChanceNegative;
                    }
                }
                
            }
        }
    }
    
    public static void SaveToClipboard(FruitType fruitType, Colors fruitColor, byte spawnChance)
    {
        for (int i = 0; i< Rl.saveClipBoard.FruitClipboardParent.Length; i++)
        {
            if (fruitType == Rl.saveClipBoard.FruitClipboardParent[i].FruitType)
            {
                for (int j = 0; j < Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards.Length; j++)
                {
                    if (fruitColor == Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].FruitColor)
                    {
                        Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].SpawnChance = spawnChance;
                    }
                }
                
            }
        }
    }
    public static void SaveToClipboard(FruitType fruitType, Colors fruitColor, int spawnEnd)
    {
        for (int i = 0; i< Rl.saveClipBoard.FruitClipboardParent.Length; i++)
        {
            if (fruitType == Rl.saveClipBoard.FruitClipboardParent[i].FruitType)
            {
                for (int j = 0; j < Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards.Length; j++)
                {
                    if (fruitColor == Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].FruitColor)
                    {
                        Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].SpawnEnd = spawnEnd;
                    }
                }
                
            }
        }
    }
    public static void SaveToClipboard(FruitType fruitType, Colors fruitColor, uint spawnStart)
    {
        for (int i = 0; i< Rl.saveClipBoard.FruitClipboardParent.Length; i++)
        {
            if (fruitType == Rl.saveClipBoard.FruitClipboardParent[i].FruitType)
            { 
                for (int j = 0; j < Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards.Length; j++)
                {
                    if (fruitColor == Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].FruitColor)
                    {
                        Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].SpawnStart = spawnStart;
                    }
                }
            }
        }
    }
    public static void SaveToClipboard(FruitType fruitType, Colors fruitColor, Phase phase)
    {
        for (int i = 0; i< Rl.saveClipBoard.FruitClipboardParent.Length; i++)
        {
            if (fruitType == Rl.saveClipBoard.FruitClipboardParent[i].FruitType)
            {
                for (int j = 0; j < Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards.Length; j++)
                {
                    if (fruitColor == Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].FruitColor)
                    {
                        Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].phase = phase;
                    }
                }
                
            }
        }
    }
    public static void SaveToClipboard(FruitType fruitType, Colors fruitColor, bool isEnabled)
    {
        for (int i = 0; i< Rl.saveClipBoard.FruitClipboardParent.Length; i++)
        {
            if (fruitType == Rl.saveClipBoard.FruitClipboardParent[i].FruitType)
            {
                for (int j = 0; j < Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards.Length; j++)
                {
                    if (fruitColor == Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].FruitColor)
                    {
                        Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].IsEnabled = isEnabled;
                    }
                }
                
            }
        }
    }
    
    public static void SaveToClipboard(bool isEnabled)
    {
        for (int i = 0; i< Rl.saveClipBoard.FruitClipboardParent.Length; i++)
        {
            if (FruitType == Rl.saveClipBoard.FruitClipboardParent[i].FruitType)
                Rl.saveClipBoard.FruitClipboardParent[i].FruitEnabled = isEnabled;
        }
    }
    
    public static void SaveToClipboard(FruitType fruitType, Colors fruitColor, bool isEnabled, Phase phase, uint spawnStart, int spawnEnd, byte spawnChance,byte spawnChanceOverTime,bool spawnChanceNegative)
    {
        for (int i = 0; i< Rl.saveClipBoard.FruitClipboardParent.Length; i++)
        {
            if (fruitType == Rl.saveClipBoard.FruitClipboardParent[i].FruitType)
            {
                for (int j = 0; j < Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards.Length; j++)
                {
                    if (fruitColor == Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].FruitColor)
                    {
                        Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].IsEnabled = isEnabled;
                        Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].phase = phase;
                        Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].SpawnStart = spawnStart;
                        Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].SpawnEnd = spawnEnd;
                        Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].SpawnChance = spawnChance;
                        Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].SpawnChanceOverTime = spawnChanceOverTime;
                        Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].SpawnChanceNegative = spawnChanceNegative;
                    }
                }
            }
        }
    }
    
    public static void SaveFruitSettings()
    {
        int level = Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber;
        for (int i = 0; i < Rl.saveClipBoard.FruitClipboardParent.Length; i++)
        {
            if (Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BoardDimensionsConfig
                    .FruitsConfigParent.Length == 0 || Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BoardDimensionsConfig
                    .FruitsConfigParent.Length > 9)
                Rl.saveClipBoard.CreateFruitClipboardParentList(ref Rl.saveFileLevelConfigManagement
                    .AllSaveFileLevelConfigs.LevelConfigs[level].BoardDimensionsConfig.FruitsConfigParent);
            Array.Resize(ref Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BoardDimensionsConfig.FruitsConfigParent[i].fruitClipboards, Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards.Length);
            Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BoardDimensionsConfig.FruitsConfigParent[i]
                = (FruitsConfigParent)GenericSettingsFunctions.GetDeepCopy(Rl.saveClipBoard.FruitClipboardParent[i]);
        }
    }
}
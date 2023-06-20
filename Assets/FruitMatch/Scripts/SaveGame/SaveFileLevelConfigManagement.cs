using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public sealed class SaveFileLevelConfigManagement : ScriptableObject
{
    public Settings settings;
    public World world;
    [FormerlySerializedAs("SaveFileLevelConfigs")] public SaveFileLevelConfigs AllSaveFileLevelConfigs;
    public int howManySaveFilesAllowed = 8;
    public double uniqueIdentifierCounter;
    public int autoSave = 60;
    private const string SaveDirectory = "/Config/";
    private const string FileName = "Level_";
    private const string FileEnding = ".config";  
    
    [Button] private void DebugLogPersistentDataPath() => Debug.Log(Application.persistentDataPath); //so I can go there quickly
    [Button()] private void DelteAllSavFiles() => SaveUtil.DeleteAllSaveFiles(SaveDirectory);
    
    [Button]public bool Save()
    {
        string filePath = Application.persistentDataPath + SaveDirectory;
        SaveUtil.CreateDirectory(SaveDirectory);
        uniqueIdentifierCounter = SaveUtil.CheckForHighestUniqueIdentifierInFiles(filePath, FileName, FileEnding, uniqueIdentifierCounter );
        uniqueIdentifierCounter = SaveUtil.CheckIfResetUniqueIdentifier(filePath, FileName, FileEnding, SaveDirectory, uniqueIdentifierCounter);
         
        List<string> saveFileNamesFromdisk = new List<string>();
        saveFileNamesFromdisk.Clear();
        saveFileNamesFromdisk    = SaveUtil.GetSaveFileNamesFromDisk(filePath, FileName,FileEnding);
 
        SaveUtil.DeleteOldSaveFiles(howManySaveFilesAllowed,saveFileNamesFromdisk);

        SaveFileLevelConfigs saveFileLevelConfigs = CreateSaveFile();
        string json = JsonUtility.ToJson(saveFileLevelConfigs, true);
        string fullFileName = filePath + FileName + saveFileLevelConfigs.UniqueIdentifier.ToString() + FileEnding;
      //  json = SaveUtil.goMainMenu(json);
        File.WriteAllText(fullFileName, json);
        GUIUtility.systemCopyBuffer = filePath;
        return true;
    }

    private SaveFileLevelConfigs CreateSaveFile()
    {
        SaveFileLevelConfigs saveFileLevelConfigs = AllSaveFileLevelConfigs;
        AllSaveFileLevelConfigs.UniqueIdentifier = uniqueIdentifierCounter;
        uniqueIdentifierCounter++;
        return saveFileLevelConfigs;
    }
    
    private SaveFileLevelConfigs GetLatestSaveFile() => GetSpecificSaveFile(SaveUtil.GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding).Count-1);

    [Button] public void LoadLatestSaveFile()
    {
        SaveUtil.CreateDirectory(SaveDirectory);
        SaveFileLevelConfigs latestSaveFile = GetLatestSaveFile();
        AllSaveFileLevelConfigs = latestSaveFile;
       LoadSaveFile(latestSaveFile);
    }
    
    public SaveFileLevelConfigs GetSpecificSaveFile(int saveFile)
    {
        List<string> saveDataPaths = SaveUtil.GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding);
        SaveFileLevelConfigs tempData = new SaveFileLevelConfigs(new LevelConfig[world.GetLevelCount], SaveUtil.GetDateInformation(), uniqueIdentifierCounter);
        try
        {
            File.Exists(saveDataPaths[saveFile]);
        }
        catch
        {
            Save();
            return GetLatestSaveFile();
        }
        
        string json = File.ReadAllText(saveDataPaths[saveFile]);
        
        //json = SaveUtil.goMainMenu(json);
        try
        {
            tempData = JsonUtility.FromJson<SaveFileLevelConfigs>(json);
        }
         catch (ArgumentException e)
         {
             if (saveFile != 0)
             {
                 Debug.Log("SaveFile Corrupt, trying to get an older SaveFile");
                 return  GetSpecificSaveFile(saveFile - 1);
             }
         }

        Debug.Log("savefile number loaded: " + tempData.UniqueIdentifier);
        
        return tempData;
    }
    
    private  void LoadSaveFile(SaveFileLevelConfigs saveFileLevelConfigs)
    {
        uniqueIdentifierCounter = saveFileLevelConfigs.UniqueIdentifier;

        for (int i = 0; i < world.GetLevelCount; i++)
        {
            List<LevelTextConfig> levelTextConfigs = new List<LevelTextConfig>();
            levelTextConfigs?.Clear();

            if (saveFileLevelConfigs.LevelConfigs[i].LevelTextConfig == null)
            {
                saveFileLevelConfigs.LevelConfigs[i].LevelTextConfig = new LevelTextConfig[8];
                GenerateLanguageTextLists();
            }
          
            foreach (LevelTextConfig levelTextConfig in saveFileLevelConfigs.LevelConfigs[i].LevelTextConfig)
            {
                LoadLevelText(ref world.levels[i], levelTextConfig);
            }
            LoadBoardDimensions(ref world.levels[i], saveFileLevelConfigs.LevelConfigs[i].BoardDimensionsConfig);
            LoadScoreGoals(ref world.levels[i], saveFileLevelConfigs.LevelConfigs[i].ScoreGoalsConfig);
            LoadGoal(ref world.levels[i], saveFileLevelConfigs.LevelConfigs[i].GoalConfig);
            LoadAnimation(ref world.levels[i], saveFileLevelConfigs.LevelConfigs[i].AnimationConfig);
          //  LoadSideFruits(ref world.levels[i],  saveFileLevelConfigs.LevelConfigs[i].SideFruitsConfig);
        //    LoadTileTypes(ref world.levels[i],  saveFileLevelConfigs.LevelConfigs[i].TyleTypeSettingsConfig);
          // LoadBombConfigs(ref world.levels[i],  saveFileLevelConfigs.LevelConfigs[i].BombConfigs);
        }
    }

    private void LoadAnimation(ref Level level, AnimationConfig animationConfig)
    {
        //Animation - override ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
    private void LoadSideFruits(ref Level level, SideFruitsConfig sideFruitsConfig)
    {
        //SideFruits ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    private void LoadGoal(ref Level level, GoalConfig goalConfig)
    {
        
        /*
        if (goalConfig.GoalFruits.Count > 0)
        {
            Array.Resize(ref level.levelGoals, goalConfig.GoalFruits.Count );
            for (int i = 0; i < goalConfig.GoalFruits.Count; i++)
            {
                level.levelGoals[i].fruitType = goalConfig.GoalFruits[i].FruitType;
                    //// goalConfig.GoalFruits[i].FruitColor;///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    level.levelGoals[i].numberNeeded = goalConfig.GoalFruits[i].Goal;
                 //   goalConfig.GoalFruits[i].CountBackwards; //////////////////////////////////////////////////////////////////////////////////
            }
        }
        */
        
        //goalConfig.MatchingGoal;  //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // goalConfig.MatchingStyle; //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
    private void LoadEndGameRequirements(ref Level level, EndGameRequirements endGameRequirementsConfig)
    {
        level.endGameRequirements.GameType = endGameRequirementsConfig.GameType;
        level.endGameRequirements.CounterValue = endGameRequirementsConfig.CounterValue;
    }
    private void LoadScoreGoals(ref Level level, ScoreGoalsConfig scoreGoalsConfig)
    {
        Array.Resize(ref level.scoreGoals, 3);
        level.scoreGoals[0] = scoreGoalsConfig.Star1Value;
        level.scoreGoals[1] = scoreGoalsConfig.Star2Value;
        level.scoreGoals[2] = scoreGoalsConfig.Star3Value;

        if (!scoreGoalsConfig.Star1Enabled) level.scoreGoals[0] = 0;
        if (!scoreGoalsConfig.Star2Enabled) level.scoreGoals[1] = 0;
        if (!scoreGoalsConfig.Star3Enabled) level.scoreGoals[2] = 0;
    }
    private void LoadBoardDimensions(ref Level level, BoardDimensionsConfig boardDimensionsConfig)
    {
        level.width = boardDimensionsConfig.Width;
        level.height = boardDimensionsConfig.Height;
        
        List<GameObject> fruits = new List<GameObject>();

      //Fruitsconfig
        //boardDimensionsConfig.LeftRightPadding;  ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //boardDimensionsConfig.TopDownPadding;   ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }

    private void LoadLevelText(ref Level level, LevelTextConfig levelTextConfig)
    {
        level.LevelNameText = levelTextConfig.LevelName;
        level.LevelDescriptionText = levelTextConfig.LevelDescriptionText;
        level.LevelGameTypeDefaultText = levelTextConfig.LevelGameTypeText;
      //  levelTextConfig.LevelGameTypeDefaultTextDefault; ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
    private void LoadLevelTextHelper(ref Level level, LevelTextConfig levelTextConfig)
    {
        
    }  

    public IEnumerator AutoSaveCo()
    {
        yield return new WaitForSeconds(autoSave);
       Save();
    }

   [SerializeField] private float LuckConfigValue = 0.5f;
    [Button()] public void SetAllLuckConfigsToSpecificValue()
    {
        for (int index = 0; index < AllSaveFileLevelConfigs.LevelConfigs.Length; index++)
        {
            AllSaveFileLevelConfigs.LevelConfigs[index].LuckConfig.NeededPieces = LuckConfigValue;
            AllSaveFileLevelConfigs.LevelConfigs[index].LuckConfig.NeededPiecesOverTime = LuckConfigValue;
            AllSaveFileLevelConfigs.LevelConfigs[index].LuckConfig.BeneficialExtras = LuckConfigValue;
            AllSaveFileLevelConfigs.LevelConfigs[index].LuckConfig.BeneficialExtrasOverTime = LuckConfigValue;
            AllSaveFileLevelConfigs.LevelConfigs[index].LuckConfig.MaliciousExtras = LuckConfigValue;
            AllSaveFileLevelConfigs.LevelConfigs[index].LuckConfig.MaliciousExtrasOverTime = LuckConfigValue;
        }
    }

    [Button()]
    public void GenerateLanguageTextLists()
    {
        for (int index = 0; index < AllSaveFileLevelConfigs.LevelConfigs.Length; index++)
        {
            Array.Clear(AllSaveFileLevelConfigs.LevelConfigs[index].LevelTextConfig, 0, AllSaveFileLevelConfigs.LevelConfigs[index].LevelTextConfig.Length);
            AllSaveFileLevelConfigs.LevelConfigs[index].LevelTextConfig = new LevelTextConfig[enumCountLanguage()];
            for (int j = 0; j < AllSaveFileLevelConfigs.LevelConfigs[index].BombConfigs.Length; j++)
            {
                AllSaveFileLevelConfigs.LevelConfigs[index].LevelTextConfig[j].Language = enumGetSpecificLanguageValue(j);
            }
        }
    }
    [Button()]
    public void GenerateBombsLists()
    {
        for (int index = 0; index < AllSaveFileLevelConfigs.LevelConfigs.Length; index++)
        {
            Array.Clear(AllSaveFileLevelConfigs.LevelConfigs[index].BombConfigs, 0, AllSaveFileLevelConfigs.LevelConfigs[index].BombConfigs.Length);
            AllSaveFileLevelConfigs.LevelConfigs[index].BombConfigs = new BombConfig[enumCountBomb()];
            for (int j = 0; j < AllSaveFileLevelConfigs.LevelConfigs[index].BombConfigs.Length; j++)
            {
                AllSaveFileLevelConfigs.LevelConfigs[index].BombConfigs[j].Bomb = enumGetSpecificBombValue(j);
            }
        }
    }

    public static int enumCountLanguage()
    {
        //for now randomize it only
        int counter = 0;
        foreach (Language matchStyle in Enum.GetValues(typeof(Language)))
            counter++;
        
        return counter;
    }
    private static int enumCountBomb()
    {
        //for now randomize it only
        int counter = 0;
        foreach (Bomb matchStyle in Enum.GetValues(typeof(Bomb)))
            counter++;
        
        return counter;
    }

    public static Language enumGetSpecificLanguageValue(int counter)
    {
        //for now randomize it only
        foreach (Language language in Enum.GetValues(typeof(Language)))
            if (counter == (int)language) return language;

        return Language.English;
    }
    
    private static Bomb enumGetSpecificBombValue(int counter)
    {
        //for now randomize it only
        foreach (Bomb bomb in Enum.GetValues(typeof(Bomb)))
            if (counter == (int)bomb) return bomb;
        
        return Bomb.Horizontal;
    }
    
    
}
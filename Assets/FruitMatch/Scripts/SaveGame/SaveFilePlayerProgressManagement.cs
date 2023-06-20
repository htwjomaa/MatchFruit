using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using UnityEngine;


[CreateAssetMenu]

public sealed class SaveFilePlayerProgressManagement : ScriptableObject
{
  
    public Settings settings;
    public World world;
    public int howManySaveFilesAllowed = 8;
    public int SaveFileSystemVersioning = 1;
    public double uniqueIdentifierCounter;
    public int autoSave = 60;
    private const string SaveDirectory = "/SaveData/";
    private const string FileName = "Savegame_";
    private const string FileEnding = ".sav";
    
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
        
        SaveFilePlayerProgress saveFilePlayerProgress = CreateSaveFile();
        string json = JsonUtility.ToJson(saveFilePlayerProgress, false);
        string fullFileName = filePath + FileName + saveFilePlayerProgress.UniqueIdentifier.ToString() + FileEnding;
        //json = SaveUtil.goMainMenu(json);
        File.WriteAllText(fullFileName, json);
        return true;
    }

    private SaveFilePlayerProgress GetLatestSaveFile() => GetSpecificSaveFile(SaveUtil.GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding).Count-1);

    [Button] public void LoadLatestSaveFile()
    {
        SaveUtil.CreateDirectory(SaveDirectory);
        LoadSaveFile(GetLatestSaveFile());
    }

    public SaveFilePlayerProgress GetSpecificSaveFile(int saveFile)
    {
        List<string> saveDataPaths = SaveUtil.GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding);
        SaveFilePlayerProgress tempData = new SaveFilePlayerProgress();
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
            Debug.Log("jSonLength: " + json.Length);
           // json = SaveUtil.goMainMenu(json);
            try
            {
                tempData = JsonUtility.FromJson<SaveFilePlayerProgress>(json);
            }
            catch (ArgumentException e)
            {
                if (saveFile != 0)
                {
                    Debug.Log("SaveFile Corrupt, trying to get an older SaveFile");
                    return  GetSpecificSaveFile(saveFile - 1);
                }
            }
            
                //patcher in case something is going down
                //we create a good base in case something has changed a lot and the json gets rejected - we need it as a string
                 SaveFilePlayerProgress saveFilePlayerProgress = CreateSaveFile();
              //   string jsonBase = JsonConvert.SerializeObject(saveFilePlayerProgress, Formatting.Indented);
                 //now we give the base and the new json to a method and get the patched Version
            //     json = SaveUtil.PatchFiles(jsonBase, json);
                 //finally we can create a save json data that ALWAYS works
                 tempData = JsonUtility.FromJson<SaveFilePlayerProgress>(json);
                 
                 
            Debug.Log("savefile number loaded: " + tempData.UniqueIdentifier);
        
        return tempData;
    }
    
    private void LoadSaveFile(SaveFilePlayerProgress saveFilePlayerProgress)
    {
          //Load AudioSettings
            settings.SetMasterVolumeSettings(saveFilePlayerProgress.Audiosettings.MasterVolume);
            settings.SetMusicVolumeSettings(saveFilePlayerProgress.Audiosettings.MusicVolume);
            settings.SetSFXVolumeSettings(saveFilePlayerProgress.Audiosettings.SfxVolume);
            settings.SetUISoundVolumeSettings(saveFilePlayerProgress.Audiosettings.UISoundVolume);

            settings.MasterMutedProperty = saveFilePlayerProgress.Audiosettings.MasterMuted;
            settings.MusicMutedProperty = saveFilePlayerProgress.Audiosettings.MusicMuted;
            settings.SFXMuted = saveFilePlayerProgress.Audiosettings.SfxMuted;
            settings.UIMuted = saveFilePlayerProgress.Audiosettings.UISoundMuted;

            //Load AnimationSettings
            settings.SetSwipeRejectionSettings(saveFilePlayerProgress.AnimationSettings.SwipeRejectionLevel);
            settings.SetSideIconsAlphaSettings(saveFilePlayerProgress.AnimationSettings.Visibility);
            settings.SetAnimationSpeedSettings(saveFilePlayerProgress.AnimationSettings.AnimationspeedSide);
            settings.SetSwapBackSpeedSettings(saveFilePlayerProgress.AnimationSettings.SwapbackSpeeds);
            
            settings.SetRefillDelay(saveFilePlayerProgress.AnimationSettings.RefillDelay);
            settings.SetShuffleDelay(saveFilePlayerProgress.AnimationSettings.ShuffleDelay);


            //Load Graphic Settings
            settings.SetResolution(saveFilePlayerProgress.GraphicSettings.Resolution);
            settings.FullResolution = saveFilePlayerProgress.GraphicSettings.TexturQuality;
            settings.EffectsQuality = saveFilePlayerProgress.GraphicSettings.EffectQuality;
            settings.ColorBlind = saveFilePlayerProgress.GraphicSettings.ColorBlind;
            
            //admin access
            settings.KeepAdminAccess = saveFilePlayerProgress.accessInformation.KeepAdminAccessEnabled;
            //Set Settings
            settings.SetSettings();

             //load Date
             world.LoadedDay = saveFilePlayerProgress.DateInformation.Day;
             
            //Load PlayerInfo
            world.userName = saveFilePlayerProgress.PlayerInfo.Username;
            world.totalTimePlayedInGame = saveFilePlayerProgress.PlayerInfo.TimeInGame;
            world.totalTimeInMenu = saveFilePlayerProgress.PlayerInfo.TimeInMenu;
            world.totalTimePlayedToday = saveFilePlayerProgress.PlayerInfo.TimePlayedToday;
            world.totalTimeAppUsedSec = saveFilePlayerProgress.PlayerInfo.TimeInGame + saveFilePlayerProgress.PlayerInfo.TimeInMenu;
            LocalisationSystem.CurrentLanguage = saveFilePlayerProgress.PlayerInfo.Language;

            //Load Level Progress

            for (int i = 0; i < saveFilePlayerProgress.ProgressLevel.Length; i++)
            {
                Level level = world.LevelProgresses[saveFilePlayerProgress.ProgressLevel[i].LevelIdentifier - 1].Level;
                bool freigeschaltet = saveFilePlayerProgress.ProgressLevel[i].freigeschaltet;
                List<bool> sterne = new List<bool>();
                sterne.Add(saveFilePlayerProgress.ProgressLevel[i].Star1);
                sterne.Add(saveFilePlayerProgress.ProgressLevel[i].Star2);
                sterne.Add(saveFilePlayerProgress.ProgressLevel[i].Star3);
                bool perfectRun = saveFilePlayerProgress.ProgressLevel[i].PerfectRun;
                double highscore = saveFilePlayerProgress.ProgressLevel[i].Highscore;
                
                
                Sprite imageActive = world.LevelProgresses[saveFilePlayerProgress.ProgressLevel[i].LevelIdentifier - 1].ImageActive;
                Sprite imageInactive =
                    world.LevelProgresses[saveFilePlayerProgress.ProgressLevel[i].LevelIdentifier - 1].ImageInactive;
                Sprite imageSelection =
                    world.LevelProgresses[saveFilePlayerProgress.ProgressLevel[i].LevelIdentifier - 1].ImageSelection;
                world.LevelProgresses[saveFilePlayerProgress.ProgressLevel[i].LevelIdentifier - 1] = new LevelProgress(level,
                    freigeschaltet,
                    sterne.ToArray(), perfectRun, highscore, imageActive, imageInactive, imageSelection);
            }
    }
    
    private SaveFilePlayerProgress CreateSaveFile()
    {
        double uniqueIdentifier = uniqueIdentifierCounter;
        AccessInformation accessInformation = GetAccessInfo();
        DateInformation dateInformation = SaveUtil.GetDateInformation();
        ProgressPlayerInfo playerInfo = GetPlayerInfo();
        ProgressAudioSettings audioSettings = GetAudioSettings();
        ProgressLevel[] progressLevel = GetProgressLevels();
        ProgressAnimationSettings animationSettings = GetAnimationSettings();
        ProgressGraphicSettings graphicSettings = GetGraphicSettings();
        
        return new SaveFilePlayerProgress(SaveFileSystemVersioning, uniqueIdentifier, accessInformation, dateInformation , playerInfo, audioSettings, progressLevel, animationSettings, graphicSettings);
    }

    private ProgressGraphicSettings GetGraphicSettings()
    {
        int resolution = settings.GetResoltionSettings();
        int textureQuality = settings.FullResolution;
        int effectQuality = settings.EffectsQuality;
        bool colorBlind = settings.ColorBlind;
        
        return new ProgressGraphicSettings(resolution, textureQuality, effectQuality,colorBlind);
    }
    private ProgressAudioSettings GetAudioSettings()
    {
        int masterVolume = settings.GetMasterVolumeSettings();
        int musicVolume = settings.GetMusicVolumeSettings();
        int sfxVolume = settings.GetSFXVolumeSettings();
        int uISoundVolume = settings.GetUiSoundVolumeSettings();

        bool masterMuted =  settings.MasterMutedProperty;
        bool musicMuted = settings.MusicMutedProperty;
        bool sfxMuted = settings.SFXMuted;
        bool uIMuted = settings.UIMuted;
        return new ProgressAudioSettings(masterVolume, musicVolume, sfxVolume, uISoundVolume, masterMuted, musicMuted, sfxMuted, uIMuted);
    }

    private ProgressAnimationSettings GetAnimationSettings()
    {
        int swipeRejectionLevel = settings.GetSwipeRejectionSettings();
        int animationspeedSide = settings.GetAnimationSpeedSettings();
        int sideIconsAlpha = settings.GetSideIconsAlphaSettings();
        int swapBackSpeeds = settings.GetSwapBackSettings();
        int refillDelay = settings.GetRefillDealySettings();
        int shuffleDelay = settings.GetShuffleDealySettings();
      
        bool isColorblind = settings.ColorBlind;

         return new ProgressAnimationSettings(swipeRejectionLevel, animationspeedSide, swapBackSpeeds,sideIconsAlpha, refillDelay, shuffleDelay, isColorblind);
    }
    private ProgressLevel[] GetProgressLevels()
    {
        ProgressLevel[] progressLevels = new ProgressLevel[world.LevelCount()];
        for (int i = 0; i < world.LevelCount(); i++)
        {
            progressLevels[i] = GetProgressLevel(i);
        }

        return progressLevels;
    }
    private ProgressLevel GetProgressLevel(int level)
    {
     int levelIdentifier = level+1;
     bool freigeschaltet = world.LevelProgresses[level].Freigeschaltet;
     double highscore = world.LevelProgresses[level].HighScore;
     bool star1 = world.LevelProgresses[level].Sterne[0];
     bool star2 = world.LevelProgresses[level].Sterne[1];
     bool star3 = world.LevelProgresses[level].Sterne[2];
     bool perfectRun = world.LevelProgresses[level].PerfectRun;
     
     return new ProgressLevel(levelIdentifier,freigeschaltet, highscore, star1, star2, star3, perfectRun);
    }

    private ProgressPlayerInfo GetPlayerInfo()
    {
        string userName = world.userName;
        uniqueIdentifierCounter++;
        double timeInMenu = world.totalTimeInMenu;
        double timeInGame = world.totalTimePlayedInGame;
        int timePlayedToday = world.totalTimePlayedToday;
        Language language = LocalisationSystem.CurrentLanguage;
        return new ProgressPlayerInfo(userName, timeInMenu, timeInGame, timePlayedToday, language);
    }
    
    private AccessInformation GetAccessInfo()
    {
        bool keepAdminAccess = settings.KeepAdminAccess;
        return new AccessInformation(keepAdminAccess);
    }
    
    public IEnumerator AutoSaveCo()
    {
        yield return new WaitForSeconds(autoSave);
        Save();
    }
}
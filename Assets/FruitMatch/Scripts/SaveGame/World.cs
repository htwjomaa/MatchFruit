using System;
using System.Collections.Generic;
using FruitMatch.Scripts.MapScripts;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "World", menuName = "World")]
public sealed class World : ScriptableObject
{
    public Level[] levels;
    public Level LevelToLoad;
    public List<GoalLookUpTable> GoalLookUpTable = new List<GoalLookUpTable>();
    public List<GoalLookUpTable> GoalModifierLookUpTable = new List<GoalLookUpTable>();
    public List<PrefabLookUp> PrefabLookUp = new List<PrefabLookUp>();

    public List<GoalTypesStringLookUp> GoalTypesStringLookUps = new List<GoalTypesStringLookUp>();
    
    public List<LevelProgress> LevelProgresses = new List<LevelProgress>();
    public bool StarsDependOnHighScore = true;
    public bool UnlockDepensOnStars = true;
    public double totalTimePlayedInGame;
    public double totalTimeInMenu;
    public double totalTimeAppUsedSec;
    public int totalTimePlayedToday;
    public string userName = "Martin";
    public byte LoadedDay = 0;
    public int basePieceValue = 200;
    public Sprite TimedRunImage;
    public Sprite LimitedMoveImage;
    public Sprite SpriteNotFound;
    private bool[] levelnachSternenFreigeschaltet;
    public List<CollectionStyleSet> CollectionStyleSet;
    


    public  Dictionary<LevelCategory, BgImageSet> allBgImageSetDictionaries;
    public  List<BgImageSet> allBGImageSets;

    // public LevelConfig GetLevelWithNormalInt(int level)
    // {
    //     if (level > levels.Length - 1)
    //     {
    //         return allSave[levels.Length - 1];
    //     }
    // }
    public int GetLevelToLoadInt(Level level)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i] == level)
                return i;
        }

        return 0;
    }
    public GameObject GetFruitPrefab(FruitType fruitType, Colors color )
    {
        GameObject tempObject = null;
        string fruitTag = GetTagFromFruitType(fruitType);
        foreach (PrefabLookUp lookUpCorrectFruitEntry in PrefabLookUp)
        {
            if (lookUpCorrectFruitEntry.Tag == fruitTag)
            {
                if (color == Colors.AlleFarben)
                {
                    tempObject = lookUpCorrectFruitEntry.DefaultGameObject;
                }
                else
                {
                    foreach (FruitPrefab lookUpCorrectColor in lookUpCorrectFruitEntry.AllColorPrefabs)
                    {
                        if (color == lookUpCorrectColor.Color)
                        {
                            tempObject = lookUpCorrectColor.Prefab;
                        }
                    }
                }
                break;
            }
        }

        return tempObject;
    }
    public int GetLevelCount => levels.Length;
    public bool[] GetIfLevelIsUnlockedBasedOnFirstStarFromLastLevel()
    {
        Array.Resize(ref levelnachSternenFreigeschaltet,LevelProgresses.Count);
        for (int i = 0; i < levelnachSternenFreigeschaltet.Length; i++)
        {
            levelnachSternenFreigeschaltet[i] = false;
        }

        levelnachSternenFreigeschaltet[0] = true;
        
        for (int i = 1; i < LevelProgresses.Count; i++)
        {
            if (LevelProgresses[i - 1].Sterne[0]) levelnachSternenFreigeschaltet[i] = true;
        }
        return levelnachSternenFreigeschaltet;
    }

    public double GetHighscore(Level level)
    {
        double highscore = 0;
        foreach (LevelProgress lev in LevelProgresses)
        {
            if (lev.Level == level)
                return lev.HighScore;
        }

        return highscore;
    }
    
    public void UpdateHighScore(Level level, double highscore)
    {
        for (int i = 0; i < LevelProgresses.Count; i++)
        {
            if (LevelProgresses[i].Level == level)
                LevelProgresses[i]  = new LevelProgress(LevelProgresses[i].Level,  LevelProgresses[i].Freigeschaltet,
                    LevelProgresses[i].Sterne,  LevelProgresses[i].PerfectRun, highscore,  LevelProgresses[i].ImageActive,
                    LevelProgresses[i].ImageInactive,  LevelProgresses[i].ImageSelection);
        }
        
    }
    public void UpdateStars(Level level, bool[]Stars)
    {
        for (int i = 0; i < LevelProgresses.Count; i++)
        {
            if (LevelProgresses[i].Level == level)
                LevelProgresses[i]  = new LevelProgress(LevelProgresses[i].Level,  LevelProgresses[i].Freigeschaltet,
                    Stars,  LevelProgresses[i].PerfectRun, LevelProgresses[i].HighScore, LevelProgresses[i].ImageActive,
                    LevelProgresses[i].ImageInactive,  LevelProgresses[i].ImageSelection);
        }
    }
    public int LevelCount()
    {
        int counter = 0;
        foreach (Level t in levels)
            counter++;
        return counter;
    }
    public string GetTagFromFruitType(FruitType fruitType)
    {
        foreach (GoalTypesStringLookUp goal in GoalTypesStringLookUps)
        {
            if (fruitType == goal.fruitType)
                return goal.Tag;
        }
        
        return "";
    }
    public FruitType GetFruitTypeFromTag(string tag)
    {
        foreach (GoalTypesStringLookUp goal in GoalTypesStringLookUps)
        {
            if (tag == goal.Tag)
                return goal.fruitType;
        }

        return FruitType.AlleFrüchte;
    }

   
    public Sprite GetGoalSprite(FruitType fruitType)
    {
        foreach(GoalLookUpTable lookUp in  GoalLookUpTable)
            if (lookUp.Tag == GetTagFromFruitType(fruitType))
            {
                return lookUp.Image;
            }

        return SpriteNotFound;
    }
    
    public double GetHighScore(Level level)
    {
        foreach(LevelProgress highscoreLookUp in LevelProgresses)
            if (level == highscoreLookUp.Level)
            {
                return highscoreLookUp.HighScore;
            }

        return 0;
    }
    public bool GetLevelAccess(Level level)
    {
        foreach(LevelProgress levelAccessLookup in LevelProgresses)
            if (level == levelAccessLookup.Level)
            {
                if (levelAccessLookup.Freigeschaltet) return true;
            }

        return false;
    }
    
    public void UnlockAllLevel()
    {
        StarsDependOnHighScore = false;
        UnlockDepensOnStars = false;
        for (int i = 0; i < LevelProgresses.Count; i++)
        {
            LevelProgresses[i]  = new LevelProgress(LevelProgresses[i].Level, true,
                LevelProgresses[i].Sterne,  LevelProgresses[i].PerfectRun, LevelProgresses[i].HighScore, LevelProgresses[i].ImageActive,
                LevelProgresses[i].ImageInactive,  LevelProgresses[i].ImageSelection);
        }
    }
}

[Serializable]
public struct GoalLookUpTable
{
    public string Tag;
    public Sprite Image;
    public Sprite BackGroundImage;

    public GoalLookUpTable(string tag, Sprite image, Sprite backGroundImage)
    {
        Tag = tag;
        Image = image;
        BackGroundImage = backGroundImage;
    }
}
[Serializable]
public struct GoalTypesStringLookUp
{
    [FormerlySerializedAs("GoalType")] public FruitType fruitType;
    public string Tag;

    public GoalTypesStringLookUp(FruitType fruitType, string tag)
    {
        this.fruitType = fruitType;
        Tag = tag;
    }
}

[Serializable]
public struct LevelProgress
{
    public Level Level;
    public bool Freigeschaltet;
    public bool[] Sterne;
    public bool PerfectRun;
    public double HighScore;
    public Sprite ImageActive;
    public Sprite ImageInactive;
    public Sprite ImageSelection;

    public LevelProgress(Level level, bool freigeschaltet, bool[] sterne, bool perfectRun, double highScore, Sprite imageActive, Sprite imageInactive, Sprite imageSelection)
    {
        Level = level;
        Freigeschaltet = freigeschaltet;
        Sterne = new bool[4];
        Sterne = sterne;
        PerfectRun = perfectRun;
        HighScore = highScore;
        ImageActive = imageActive;
        ImageInactive = imageInactive;
        ImageSelection = imageSelection;
    }
    
}


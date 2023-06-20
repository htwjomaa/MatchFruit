using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class AttributeSystem 
{
    public static Dictionary<string, string> allAttributes;
    // Start is called before the first frame update


    public static void Init(SaveFileLevelConfigManagement saveFileLevelConfigManagement)
    {
        allAttributes = new Dictionary<string, string>();
        InitLevels(saveFileLevelConfigManagement);
    }

    private static void InitLevels(SaveFileLevelConfigManagement saveFileLevelConfigManagement)
    {
        int counter = 1;
        for(int i = 0; i < Rl.world.GetLevelCount; i++)
        {
            string checkIfValueEmpty =
                TestIfStringEmpty(
                    saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[i]
                        .LevelTextConfig[(int)LocalisationSystem.CurrentLanguage].LevelName, i,
                    saveFileLevelConfigManagement);
            
           
           allAttributes.Add("[level"+counter+"]",
                checkIfValueEmpty);

            // allAttributes.Add("[level" + i + "]", "test");
            counter++;
        }
    }
   
    
    private static string TestIfStringEmpty(string stringToTest, int currentLevel, SaveFileLevelConfigManagement saveFileLevelConfigManagement)
    {
        if (stringToTest != string.Empty) return stringToTest;


        for (int j = 0; j < LocalisationSystem.EnumCountLanguages(); j++)
        {
            string newStringToTest = saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[currentLevel]
                .LevelTextConfig[j].LevelName;
            if (newStringToTest != string.Empty)
                return newStringToTest;
        }

        return "Level " + currentLevel;
    }
  
    public static string ChangeAttributes(string description)
    {
        if (allAttributes == null)
        {
            Init(Rl.saveFileLevelConfigManagement);
        }
        foreach (KeyValuePair<string, string> n in allAttributes)
        {
            description = description.Replace(n.Key, n.Value);
        }

        return description;
    }
    
}

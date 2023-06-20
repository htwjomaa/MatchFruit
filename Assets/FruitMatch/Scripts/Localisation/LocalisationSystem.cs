using System;
using System.Collections.Generic;
using UnityEngine;

public enum Language
{
    English,
    German,
    French,
    Spanish,
    Swedish,
    Turkish,
    Polish,
    Italian
}
public static class LocalisationSystem
{
    public static Language CurrentLanguage = Language.German;
    public static HashSet<TextLocaliserUI> AllTextLocaliserUis = new HashSet<TextLocaliserUI>();
    
    
    private static Dictionary<string, string> localisedEN;
    private static Dictionary<string, string> localisedDE;
    private static Dictionary<string, string> localisedFR;
    private static Dictionary<string, string> localisedES;
    private static Dictionary<string, string> localisedSWE;
    private static Dictionary<string, string> localisedTR;
    private static Dictionary<string, string> localisedPL;
    private static Dictionary<string, string> localisedIT;
    public static CSVLoader csvLoader;

    public static bool isInit;

    public static void UpdateDictionaries()
    {
        localisedEN = csvLoader.GetDictionaryValues("en");
        localisedDE = csvLoader.GetDictionaryValues("de");
        localisedFR = csvLoader.GetDictionaryValues("fr");
        localisedES = csvLoader.GetDictionaryValues("es");
        localisedSWE = csvLoader.GetDictionaryValues("swe");
        localisedTR = csvLoader.GetDictionaryValues("tr");
        localisedPL = csvLoader.GetDictionaryValues("pl");
        localisedIT = csvLoader.GetDictionaryValues("it");
    }

    public static Dictionary<string, string> GetDictionaryForEditor()
    {
        if (!isInit) Init();
        return localisedEN;
    }
    public static void Init()
    {
        csvLoader = new CSVLoader();
        csvLoader.LoadCSV();

        UpdateDictionaries();

        isInit = true;
    }

    public static string GetLocalisedString(string stringToCheck)
    {
        if (GetLocalisedValue(stringToCheck) != string.Empty)
            return GetLocalisedValue(stringToCheck);
        
        Debug.Log("String not found: " + stringToCheck);
        return stringToCheck;
    }
    public static string GetLocalisedValue(string key)
    {
        if (!isInit) Init();
        
        string value = key;

        switch (CurrentLanguage)
        {
            case Language.English:
                localisedEN.TryGetValue(key, out value);
                break;
            case Language.German:
                localisedDE.TryGetValue(key, out value);
                break;
            case Language.French:
                localisedFR.TryGetValue(key, out value);
                break;
            case Language.Spanish:
                localisedES.TryGetValue(key, out value);
                break;
            case Language.Swedish:
                localisedSWE.TryGetValue(key, out value);
                break;
            case Language.Turkish:
                localisedTR.TryGetValue(key, out value);
                break;
            case Language.Polish:
                localisedPL.TryGetValue(key, out value);
                break;
            case Language.Italian:
                localisedIT.TryGetValue(key, out value);
                break;
        }

        return value;
    }

    public static int EnumCountLanguages()
    {
        int counter = 0;
        foreach (Language language in Enum.GetValues(typeof(Language)))
        counter++;
        
        return counter;
    }

#if UNITY_EDITOR
    public static void Add(string key, string value)
    {
        if (value.Contains("\"")) value.Replace('"', '\"');
        if (csvLoader == null) csvLoader = new CSVLoader();

        csvLoader.LoadCSV();
        csvLoader.Add(key, value);
        csvLoader.LoadCSV();
        
        UpdateDictionaries();
    }
    
    public static void  Replace(string key, string value)
    {
        if (value.Contains("\"")) value.Replace('"', '\"');
        if (csvLoader == null) csvLoader = new CSVLoader();
        
        csvLoader.LoadCSV();
        csvLoader.Edit(key, value);
        csvLoader.LoadCSV();
        
        UpdateDictionaries();
    }

    public static void Remove(string key)
    {
        if (csvLoader == null) csvLoader = new CSVLoader();

        csvLoader.LoadCSV();
        csvLoader.Remove(key);
        csvLoader.LoadCSV();
        
        UpdateDictionaries();
    }
    
    #endif
}
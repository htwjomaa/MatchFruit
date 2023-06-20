using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;


[CreateAssetMenu]
[Serializable]
public sealed class WorldMapSaveManagement : ScriptableObject
{
    public int howManySaveFilesAllowed = 8;
    public int SaveFileSystemVersioning = 1;
    public double uniqueIdentifierCounter;
    public int autoSave = 60;
    private const string SaveDirectory = "/WorldMap/";
    private const string FileName = "WorldMap_";
    private const string FileEnding = ".map";
    
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
        
       WorldStructure worldStructure  = CreateSaveFile();
       string json = JsonUtility.ToJson(worldStructure, false);
       string fullFileName = filePath + FileName + worldStructure.UniqueIdentifier + FileEnding;
        //json = SaveUtil.goMainMenu(json);
        File.WriteAllText(fullFileName, json);
        return true;
    }

    private WorldStructure GetLatestSaveFile() => GetSpecificSaveFile(SaveUtil.GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding).Count-1);

    [Button] public void LoadLatestSaveFile()
    {
        SaveUtil.CreateDirectory(SaveDirectory);
        LoadSaveFile(GetLatestSaveFile());
    }

    public WorldStructure GetSpecificSaveFile(int saveFile)
    {
        List<string> saveDataPaths = SaveUtil.GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding);
        WorldStructure tempData = new WorldStructure();
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
                tempData= JsonUtility.FromJson<WorldStructure>(json);
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
              //  WorldStructure saveFilePlayerProgress = CreateSaveFile();
               //  string jsonBase = JsonConvert.SerializeObject(saveFilePlayerProgress, Formatting.Indented);
                 //now we give the base and the new json to a method and get the patched Version
                // json = SaveUtil.PatchFiles(jsonBase, json);
                 //finally we can create a save json data that ALWAYS works
                 tempData= JsonUtility.FromJson<WorldStructure>(json);
                 
                 
            Debug.Log("savefile number loaded: " + tempData.UniqueIdentifier);
        
        return tempData;
    }
    
    private void LoadSaveFile(WorldStructure worldStructure)
    {
        foreach(WorldNode worldNode in FindObjectsByType<WorldNode>(FindObjectsSortMode.None)) Destroy(worldNode);
        for (int i = 0; i < worldStructure.WorldNodes.Count; i++)
        {
           // GameObject worldNodeObj = new GameObject();
            //worldNodeObj.AddComponent<WorldNode>();
            WorldNode loadedWorldNode  = (WorldNode)GenericSettingsFunctions.GetCopy( worldStructure.WorldNodes[i]);
            Instantiate(loadedWorldNode , loadedWorldNode .WorldPos, Quaternion.identity);
           // WorldNode n = worldNodeObj.GetComponent<WorldNode>() = loadedWorldNode;
           // newWorldNode.UniqueIdentifier = loadedWorldNode.UniqueIdentifier;
            // newWorldNode.WorldPos = loadedWorldNode.WorldPos;
            // newWorldNode.ActivatedSprite = loadedWorldNode.ActivatedSprite;
            // newWorldNode.DeactivedSprite = loadedWorldNode.DeactivedSprite;
            // newWorldNode.PrevWorldLines = (List<WorldLine>)GenericSettingsFunctions.GetCopy(loadedWorldNode.PrevWorldLines);
            // newWorldNode.NextWorldLines = (List<WorldLine>)GenericSettingsFunctions.GetCopy(loadedWorldNode.NextWorldLines);
            // worldNodeObj.transform.position = newWorldNode.WorldPos;
        }
        //Invoke Draw WorldLines
            
    }
    
    private WorldStructure CreateSaveFile()
    { 
        List<WorldNode> allWorldNodes = new List<WorldNode>();
     foreach(var t in FindObjectsByType<WorldNode>(FindObjectsSortMode.None)) allWorldNodes.Add(t);
     allWorldNodes = allWorldNodes.OrderBy(x => x.UniqueIdentifier).ToList();
     DateInformation dateInformation = SaveUtil.GetDateInformation();
     uniqueIdentifierCounter++;
     
     double uniqueIdentifier = uniqueIdentifierCounter;
     WorldNode firstNode = WorldMap.firstNode; 
     return new WorldStructure(dateInformation, uniqueIdentifier, firstNode, allWorldNodes);
    }
    
    public IEnumerator AutoSaveCo()
    {
        yield return new WaitForSeconds(autoSave);
        Save();
    }
}
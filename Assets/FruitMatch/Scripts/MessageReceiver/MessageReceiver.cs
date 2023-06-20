using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

[CreateAssetMenu]

public sealed class MessageReceiver : ScriptableObject
{
    public int howManySaveFilesAllowed = 16;
    public double uniqueIdentifierCounter;
    private const string SaveDirectory = "/Messages/";
    private const string FileName = "Message_";
    private const string FileEnding = ".msg";
    [Button] public bool Save()
     {
         string filePath = Application.persistentDataPath + SaveDirectory;
         SaveUtil.CreateDirectory(SaveDirectory);
         uniqueIdentifierCounter = SaveUtil.CheckForHighestUniqueIdentifierInFiles(filePath, FileName, FileEnding, uniqueIdentifierCounter );
         uniqueIdentifierCounter = SaveUtil.CheckIfResetUniqueIdentifier(filePath, FileName, FileEnding, SaveDirectory, uniqueIdentifierCounter);
         
         List<string> saveFileNamesFromdisk = new List<string>();
             saveFileNamesFromdisk.Clear();
             saveFileNamesFromdisk    = SaveUtil.GetSaveFileNamesFromDisk(filePath, FileName,FileEnding);
 
        SaveUtil.DeleteOldSaveFiles(howManySaveFilesAllowed,saveFileNamesFromdisk);
        
        Message saveFilePlayerProgress = CreateMessageFile();
        string json = JsonUtility.ToJson(saveFilePlayerProgress, true);
        string fullFileName = filePath + FileName + saveFilePlayerProgress.UniqueIdentifier.ToString() + FileEnding;
        //json = SaveUtil.goMainMenu(json);
        File.WriteAllText(fullFileName, json);
        return true;
    }
    
    private List<Message> GetLatestFourMessages()
    {

        List<string> saveFileNamesFromDisk = SaveUtil
            .GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding);
        List<Message> latestFourMessages = new List<Message>();
        switch (saveFileNamesFromDisk.Count)
        {
            case 0:
                return null;
                break;
            case 1: 
                latestFourMessages.Add(GetSpecificMessage(SaveUtil
                    .GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding)
                    .Count - 1));
                break;
              
            case 2: 
                latestFourMessages.Add(GetSpecificMessage(SaveUtil
                    .GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding)
                    .Count - 2));
                latestFourMessages.Add(GetSpecificMessage(SaveUtil
                    .GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding)
                    .Count - 1));
                break;
            
            case 3: 
                latestFourMessages.Add(GetSpecificMessage(SaveUtil
                    .GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding)
                    .Count - 3));
                latestFourMessages.Add(GetSpecificMessage(SaveUtil
                    .GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding)
                    .Count - 2));
                latestFourMessages.Add(GetSpecificMessage(SaveUtil
                    .GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding)
                    .Count - 1));
                break;
            case 4: 
                latestFourMessages.Add(GetSpecificMessage(SaveUtil
                    .GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding)
                    .Count - 4));
                latestFourMessages.Add(GetSpecificMessage(SaveUtil
                    .GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding)
                    .Count - 3));
                latestFourMessages.Add(GetSpecificMessage(SaveUtil
                    .GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding)
                    .Count - 2));
                latestFourMessages.Add(GetSpecificMessage(SaveUtil
                    .GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding)
                    .Count - 1));
                break;
            case >4:
                latestFourMessages.Add(GetSpecificMessage(SaveUtil
                    .GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding)
                    .Count - 4));
                latestFourMessages.Add(GetSpecificMessage(SaveUtil
                    .GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding)
                    .Count - 3));
                latestFourMessages.Add(GetSpecificMessage(SaveUtil
                    .GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding)
                    .Count - 2));
                latestFourMessages.Add(GetSpecificMessage(SaveUtil
                    .GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding)
                    .Count - 1));
                break;
            
        }

        if (Rl.saveFileLevelConfigManagement == null)
        {
            Rl.saveFileLevelConfigManagement = FindObjectOfType<SaveFileLevelConfigManagement>();
            AttributeSystem.Init(Rl.saveFileLevelConfigManagement);
        }
        for (int index = latestFourMessages.Count - 1; index >= 0; index--)
        {
            
            latestFourMessages[index] = new Message(latestFourMessages[index].UniqueIdentifier,
                latestFourMessages[index].DateInformation, latestFourMessages[index].UserName,
                AttributeSystem.ChangeAttributes(latestFourMessages[index] .MessageBody), latestFourMessages[index].Marker,
                latestFourMessages[index].Mark);
            
        }

        return latestFourMessages;
    }

    private int GetSaveFileCount()
    {
        return SaveUtil
            .GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding).Count;
    }
    [Button] public void LoadLatestMessages()
    {
        SaveUtil.CreateDirectory(SaveDirectory);
        LoadMessages(GetLatestFourMessages());
    }

    public Message GetSpecificMessage(int saveFile)
    {
        List<string> saveDataPaths = SaveUtil.GetSaveFileNamesFromDisk(Application.persistentDataPath + SaveDirectory, FileName, FileEnding);
        Message tempData = new Message();
        try
        {
            File.Exists(saveDataPaths[saveFile]);
        }
        catch
        {
            Save();
          //  return GetLatestFourMessages();
        }
        
            string json = File.ReadAllText(saveDataPaths[saveFile]);
           // json = SaveUtil.goMainMenu(json);
            try
            {
                tempData = JsonUtility.FromJson<Message>(json);
            }
            catch (ArgumentException e)
            {
                if (saveFile != 0)
                {
                    Debug.Log("SaveFile Corrupt, trying to get an older SaveFile");
                    return  GetSpecificMessage(saveFile - 1);
                }
            }
            return tempData;
    }

    private string SetNoMessage(TextMeshProUGUI messageObj) =>
        messageObj.transform.gameObject.GetComponent<TextLocaliserUI>().LocalisedStrings[0].value;


    private void LoadMessages(List<Message> messages)
    {
        if (messages  == null) return;
        switch (messages.Count)
        {
            case 0:
                break;
            case 1:
                MessageReceiverManager.MessageOne.text = messages[0].MessageBody;
                Destroy(MessageReceiverManager.MessageOne.GetComponent<TextLocaliserUI>());
                MessageReceiverManager.MessageTwo.text = SetNoMessage(MessageReceiverManager.MessageTwo);
                MessageReceiverManager.MessageThree.text = SetNoMessage(MessageReceiverManager.MessageThree);
                MessageReceiverManager.MessageFour.text = SetNoMessage(MessageReceiverManager.MessageFour);
                break;
            case 2:
                MessageReceiverManager.MessageOne.text = messages[0].MessageBody;
                Destroy(MessageReceiverManager.MessageOne.GetComponent<TextLocaliserUI>());
                MessageReceiverManager.MessageTwo.text = messages[1].MessageBody;
                Destroy(MessageReceiverManager.MessageTwo.GetComponent<TextLocaliserUI>());
                MessageReceiverManager.MessageThree.text = SetNoMessage(MessageReceiverManager.MessageThree);
                MessageReceiverManager.MessageFour.text = SetNoMessage(MessageReceiverManager.MessageFour);
                break;
            
            case 3:
                MessageReceiverManager.MessageOne.text = messages[0].MessageBody;
                Destroy(MessageReceiverManager.MessageOne.GetComponent<TextLocaliserUI>());
                MessageReceiverManager.MessageTwo.text = messages[1].MessageBody;
                Destroy(MessageReceiverManager.MessageTwo.GetComponent<TextLocaliserUI>());
                MessageReceiverManager.MessageThree.text = messages[2].MessageBody;
                Destroy(MessageReceiverManager.MessageThree.GetComponent<TextLocaliserUI>());
                MessageReceiverManager.MessageFour.text = SetNoMessage(MessageReceiverManager.MessageFour);
                break;
            case 4:
                MessageReceiverManager.MessageOne.text = messages[0].MessageBody;
                Destroy(MessageReceiverManager.MessageOne.GetComponent<TextLocaliserUI>());
                MessageReceiverManager.MessageTwo.text = messages[1].MessageBody;
                Destroy(MessageReceiverManager.MessageTwo.GetComponent<TextLocaliserUI>());
                MessageReceiverManager.MessageThree.text = messages[2].MessageBody;
                Destroy(MessageReceiverManager.MessageThree.GetComponent<TextLocaliserUI>());
                MessageReceiverManager.MessageFour.text = messages[3].MessageBody;
                Destroy(MessageReceiverManager.MessageFour.GetComponent<TextLocaliserUI>());
                break;
        }
    
    }
    [SerializeField] private string testMessage = "";
    private Message CreateMessageFile()
    {
        double uniqueIdentifier = uniqueIdentifierCounter;
        uniqueIdentifierCounter++;
        DateInformation dateInformation = SaveUtil.GetDateInformation();
        string UserName = "abc";
        string MessageBody = testMessage;
        bool marker = false;
        int mark = -1;
        return new Message(uniqueIdentifier, dateInformation, UserName, MessageBody, marker, mark);
    }
}

public struct Message
{
    public double UniqueIdentifier;
    public DateInformation DateInformation;
    public string UserName;
    public string MessageBody;
    public bool Marker;
    public int Mark;

    public Message(double uniqueIdentifier, DateInformation dateInformation, string userName, string messageBody, bool marker, int mark)
    {
        UniqueIdentifier = uniqueIdentifier;
        DateInformation = dateInformation;
        UserName = userName;
        MessageBody = messageBody;
        Marker = marker;
        Mark = mark;
    }
}
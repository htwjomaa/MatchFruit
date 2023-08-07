using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu]
public sealed class AudioDatabase : ScriptableObject
{
    [SerializeField] public Extensions.SerializableDictionary<string, ClipNameandID> allAudioNames;
    [SerializeField] public List<AudioSettings> allAudioSettings; 
    [SerializeField] private string input; 
    [SerializeField] private AudioClip audioClip;
    private static GameManager gameManager;
    private void OnEnable() => gameManager = FindObjectOfType<GameManager>();

    private float[] StandardPitches()
   {
       float[] standardPitches = new float[5];
       standardPitches[0] = -0.2f;
       standardPitches[1] = -0.1f;
       standardPitches[2] = 0f;
       standardPitches[3] = 0.1f;
       standardPitches[4] = 0.2f;
       return standardPitches;
   }
    public void FixBrokenDatabase()
    {
        FixBrokenDatabaseHelper();
    }

    private void FixBrokenDatabaseHelper()
    {
        HashSet<string> audioClipsInKeys = new HashSet<string>();
        Dictionary<string, AudioSettings> audioClipsInValues = new Dictionary<string, AudioSettings>();
        audioClipsInKeys.Clear();
        audioClipsInValues.Clear();
        var helperList = allAudioNames.ToList();

        foreach (KeyValuePair<string, ClipNameandID> f in allAudioNames) 
            audioClipsInKeys.Add(f.Value.clipName);  //add all audio clip strings from Audio Dictionary

        for (int i = 0; i < allAudioSettings.Count; i++)   //add all audio clip strings and Audiosettins
            audioClipsInValues.TryAdd(allAudioSettings[i].audioClip.name.ToString(), allAudioSettings[i]);

        int counter = 0;
        
        foreach (var n in audioClipsInValues)
        {
            bool found = false;
            foreach (string l in audioClipsInKeys)
            {
                if (n.Key == l)
                {
                    found = true; 
                }
            }

            if (!found)
            {
                Debug.Log(" Not found string " + n.Key + " || at Index: " + counter);
                allAudioSettings.Remove(n.Value);
                for (int j = counter; j < helperList.Count; j++)
                {
                    allAudioNames[helperList[j].Key] = new ClipNameandID(allAudioNames[helperList[j].Key].clipName, allAudioNames[helperList[j].Key].index-1);
                }

                FixBrokenDatabaseHelper();
                return;
            }

            counter++;
        }
    }
   public void TestAudio(int pitch)
   {
       string audioName = input;
       for (int k = 0; k < StandardPitches().Length; k++)
       { 
          audioName =  audioName.TrimEnd(k.ToString().ToCharArray());
       }
       audioName =  audioName.TrimEnd('-');

       string pitchValueString = input;
       pitchValueString =  pitchValueString.TrimStart(audioName.ToCharArray());
       pitchValueString =  pitchValueString.TrimStart('-');

       int pitchValueInt = 0;
       int.TryParse(pitchValueString, out pitchValueInt);

       if (!allAudioNames.ContainsKey(audioName) && audioClip == null)
       {
           Debug.Log(audioName + " string was not found! Also no audioClip is inserted.");
           return;
       }

       if (pitchValueInt < StandardPitches().Length && pitchValueInt > StandardPitches().Length || audioClip == null)
       {
           Debug.Log(pitchValueString + " is not a valid entry.  The Length of the saved Pitcharray is 0-" + StandardPitches().Length);
           return;
       }

       int height = 0;

       if (pitchValueString != "") height = pitchValueInt;
       else height = pitch;
       

       if (audioClip != null && string.IsNullOrEmpty(input))
       {
           foreach (KeyValuePair<string, ClipNameandID> searchingForASoundName in allAudioNames)
               if (searchingForASoundName.Value.clipName == audioClip.name)
                   audioName = searchingForASoundName.Key;
       }

       gameManager.PlayAudio(audioName,height, true, 100);
   }

   public void AddClip()
   {
       if (string.IsNullOrEmpty(input))
       {
           Debug.Log("String is empty, please write a correct input.");
           return;
       }

       if (audioClip == null)
       {
           Debug.Log("AudioClip is empty. Please insert an AudioClip so the system knows how to index it.");
           return;
       }

       if(allAudioNames.ContainsKey(input))
       {
           Debug.Log("A name with this string already exists.");
           return;
       }
       allAudioNames.Add(input, new ClipNameandID(audioClip.name, UpdateIndex(audioClip)));
   }

   public void ChangeAudioClipForAudioName()
   { 
       if (audioClip == null)
       {
           Debug.Log("AudioClip missing!. You need the String name and the audioClip slot.");
           return;
       }

       int audioSettingIndex = allAudioSettings.Count - 1;
       for (var index = 0; index < allAudioSettings.Count; index++)
       {
           if (audioClip == allAudioSettings[index].audioClip) audioSettingIndex = index;
       }

       bool didFindAnything = false;
       string changeThisEntry = "";
       foreach (string changeMe in allAudioNames.Keys)
       {
           if (input == changeMe)
           {
               didFindAnything = true;
               changeThisEntry = changeMe;
           }
       }

       if (!didFindAnything)
       {
           Debug.Log("Could not find " + input + ". Please write a correct String.");
           return;
       }

       allAudioNames[changeThisEntry] = new ClipNameandID(audioClip.name, audioSettingIndex);
       Debug.Log("For the Audio name " + input +" the AudioClip has been changed to " + audioClip.name +".  It has the Index [" + audioSettingIndex + "]");
   }

   public void FindAudioNameWithInputLine()
   {
       if (!allAudioNames.ContainsKey(input))
       {
           Debug.Log("Could not find " + input);
           return;
       }

       List<string> helperList = allAudioNames.Keys.ToList();
       for (int index = 0; index < helperList.Count; index++)
       {
           if (helperList[index] == input)
           {
               Debug.Log(input + " found at Element [" + index + "] in All Audio Names.");
               return;
           }
       }
   }
   
   public void FindAudioClipWithClipInput()
   {
       if (audioClip == null)
       {
           Debug.Log("Please insert first an AudioClip.");
           return;
       }
       
       for (int index = 0; index < allAudioSettings.Count; index++)
       {
           if (allAudioSettings[index].audioClip == audioClip)
           {
               Debug.Log(audioClip.name + " found at Element " + index + " in  All Audio Settings.");
               return;
           }
       }

       Debug.Log("Could not find " + audioClip.name + ".  Please try again");
   }
  
   public void ResetAudioSettingsClipInput()
   {
       if (audioClip == null)
       {
           Debug.Log("Please insert first an AudioClip to restList the Element in the List All Audio Settings.");
           return;
       }
       
       for (int index = 0; index < allAudioSettings.Count; index++)
       {
           if (allAudioSettings[index].audioClip == audioClip)
           {
               float[] standardPitches = StandardPitches();
               for (int pitchlevel = 0; pitchlevel < StandardPitches().Length; pitchlevel++)
               {
                   allAudioSettings[index].pitch[pitchlevel] = standardPitches[pitchlevel];
               }
            
               Debug.Log(audioClip.name + " resettet  at Element " + index + " in  All Audio Settings.");
               return;
           }
       }

       Debug.Log("Could not find " + audioClip.name + ".  Please try again.");
   }

   public void ResetAudioSettingsLineInput()
   {
       if (string.IsNullOrEmpty(input))
       {
           Debug.Log("String is Empty, please write a correct input.");
           return;
       }

       if (allAudioNames.ContainsKey(input))
       {
           allAudioSettings[allAudioNames[input].index].pitch = StandardPitches();
           Debug.Log("Standard Pitches reset");
           return;
       }

       foreach (KeyValuePair<string, ClipNameandID> pitchNames in allAudioNames)
       {
           if (pitchNames.Value.clipName == input)
           {
               allAudioSettings[pitchNames.Value.index].pitch = StandardPitches();
               Debug.Log("Standard Pitches reset");
               return;
           }
       }

       Debug.Log("Could not find " + input + ".  Please try again.");
   }
   
   public void DeleteAudioNameLineInput()
   {
       if (string.IsNullOrEmpty(input))
       {
           Debug.Log("String is Empty, please write a correct input.");
           return;
       }

       if (!allAudioNames.ContainsKey(input))
       {
           Debug.Log(input + " as an Audioname not found.");
           return;
       }
       int howManyEntries = 0;
       foreach (KeyValuePair<string, ClipNameandID> deleteAudioName in allAudioNames)
       {
           if (input == deleteAudioName.Key)
           {
               foreach (KeyValuePair<string, ClipNameandID> checkEntryCount in allAudioNames)
               {
                   if (deleteAudioName.Value.index == checkEntryCount.Value.index)
                       howManyEntries++;
               }

               if (howManyEntries < 2)  // if there are  is 1 or less entries, then also remove it from the list
               {
                   allAudioSettings.RemoveAt(deleteAudioName.Value.index);
                   allAudioNames.Remove(deleteAudioName.Key);
                   return;
               }
               allAudioNames.Remove(deleteAudioName.Key);  // else remove the Key only, because other names reference the same thing still.
           }
       }
   }

   public void DelteStringLineInput()
   {
       List<KeyValuePair<string, ClipNameandID>> helperList = allAudioNames.ToList();
       helperList.RemoveAt(Int32.Parse(input));
       allAudioNames.Clear();

       foreach (KeyValuePair<string, ClipNameandID> Dic in helperList)
       {
           allAudioNames.Add(Dic.Key,Dic.Value);
       }
   }
   public void DeleteAudioSettingClipInput()
   {
       if (audioClip == null)
       {
        Debug.Log(" Please insert first an audioClip.");
        return;
       }

       List<string> deleteMe = new List<string>();
       foreach (KeyValuePair<string, ClipNameandID> deleteAudioName in allAudioNames)
       {
           if (audioClip.name == deleteAudioName.Value.clipName) deleteMe.Add(deleteAudioName.Key);
       }

       for (int i = 0; i < deleteMe.Count; i++) allAudioNames.Remove(deleteMe[i]);
       
       for (var index = 0; index < allAudioSettings.Count; index++)
       {
           if (audioClip == allAudioSettings[index].audioClip)
           {
               Debug.Log(audioClip.name +" audioSetting has been removed at index [" + index  +"]");
               allAudioSettings.RemoveAt(index);
           }
       }
   }
   public void ClearAll()
   {
       if (allAudioNames == null && allAudioSettings == null)
       {
           Debug.Log("Audio aatabase is clear. What are you trying to do?");
           return;
       }
       allAudioSettings?.Clear();
       allAudioNames?.Clear();
       
       Debug.Log("Audio database has been cleared. I hope you know what you are doing.");
   }
   

   public void FixWrongStringClipNames()
   {
       List<AudioSettings> audioSettingsList = allAudioSettings.ToList();
       List<KeyValuePair<string, ClipNameandID>> audiNamesList = allAudioNames.ToList();
       for (var index = 0; index < audiNamesList.Count; index++)
       {
          
           audiNamesList[index] = new KeyValuePair<string, ClipNameandID>(audiNamesList[index].Key,
               new ClipNameandID(audioSettingsList[audiNamesList[index].Value.index].audioClip.name, audiNamesList[index].Value.index));
       }
       allAudioNames.Clear();
       foreach (KeyValuePair<string, ClipNameandID> n in audiNamesList)
       {
           allAudioNames.Add(n.Key, n.Value); 
       }
   }
   
   public void DebugEmptyAudioSettings()
   {
       List<int> Ids = new List<int>();
       
       for (int index = 0; index < allAudioSettings.Count; index++)
       {
           bool check = false;
           foreach (var n in allAudioNames)
           {
               if (n.Value.index == index) check = true;
           }
           if(!check) Ids.Add(index);
       }

       foreach (int numberMissing in Ids) Debug.Log("Number not found: " + numberMissing);
        
       Ids.Sort();

       for (var index = Ids.Count; index > 0; index--)
       {
           fixBrokenAudioSettings(Ids[index-1]);
       }
   }
   
   private void fixBrokenAudioSettings(int faultyNumber)
   {
       var settingsList = allAudioSettings.ToList();
       var namesList = allAudioNames.ToList();
       settingsList.RemoveAt(faultyNumber);
       for (int i = 0; i < namesList.Count; i++)
       {
           if(namesList[i].Value.index > faultyNumber)
           namesList[i] = new KeyValuePair<string, ClipNameandID>(namesList[i].Key, new ClipNameandID(namesList[i].Value.clipName, namesList[i].Value.index-1));
       }
       allAudioSettings.Clear();
       allAudioNames.Clear();

       foreach (AudioSettings settings in settingsList)
       {
           allAudioSettings.Add(settings);
       }

       foreach (KeyValuePair<string, ClipNameandID> names in namesList)
       {
           allAudioNames.Add(names.Key, names.Value);
       }
   }
   
   
   private int UpdateIndex(AudioClip audioClip)
   {
       int index = 0;
       bool doesAlreadyExist = false;

       for (var i = 0; i < allAudioSettings.Count; i++)
       {
           if (audioClip == allAudioSettings[i].audioClip)
           {
               doesAlreadyExist = true;
               index = i;
           }
       }
       
       if (!doesAlreadyExist)
       {
           allAudioSettings.Add(new AudioSettings(audioClip, false, false, StandardPitches(), 100f, false));
          
           Debug.Log("Index Values updates. A new Entry has been made at index: ");
           return allAudioSettings.Count - 1;
       }
       Debug.Log("Index Values updates. AudioClip found [" + index + "] - so no new Entry in AudioSettings was made");

       return index;
   }
}
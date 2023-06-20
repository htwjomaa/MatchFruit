using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;
public static class SaveUtil
{
    public static double CheckForHighestUniqueIdentifierInFiles(string filePath, string fileName, string fileEnding, double uniqueIdentifierCounter)
    {
        List<double> doubleList = ConvertToOnlyNumbers(filePath, fileName, fileEnding,GetSaveFileNamesFromDisk(filePath, fileName, fileEnding));
        if (doubleList.Count > 0)
        {
            if (doubleList[doubleList.Count-1] > uniqueIdentifierCounter)
                uniqueIdentifierCounter = doubleList[doubleList.Count - 1] + 2;
        }
        
        return uniqueIdentifierCounter;
    }

    public static byte[] Compress(string value)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
            {
                gZipStream.Write(Encoding.UTF8.GetBytes(value));
            }
            return memoryStream.ToArray();
        }
    }

    public static string Decompress(byte[] bytes)
    {
        using (var memoryStream = new MemoryStream(bytes))
        using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
        using (var memoryStreamOutput = new MemoryStream())
        {
            gZipStream.CopyTo(memoryStreamOutput);
            var outputBytes = memoryStreamOutput.ToArray();

            string decompressed = Encoding.UTF8.GetString(outputBytes);
            return decompressed;
        }
    }

    public static List<string> GetSaveFileNamesFromDisk(string filePath,string fileName, string fileEnding)
    {
        DirectoryInfo dir = new DirectoryInfo(filePath);
        List<FileInfo> info = new List<FileInfo>(dir.GetFiles("*.*"));
        List<string> saveFilenames = new List<string>(info.Count);
        
        for (int i = 0; i < info.Count; i++)
        {
            saveFilenames.Add(info[i].ToString());
        }
        return SortAfterNumbers(filePath, fileName, fileEnding, saveFilenames);
    }

    public static List<double> ConvertToOnlyNumbers(string filePath,string fileName, string fileEnding, List<string> saveFileNames)
    {
        char[] filePathCharArray = (filePath + fileName).ToCharArray();
        int countPrefixLength = filePathCharArray.Length;
        int fileEndingLength = fileEnding.ToCharArray().Length;
        for (int i = 0; i < saveFileNames.Count; i++)
        {
            saveFileNames[i] = saveFileNames[i]
                .Remove(0, countPrefixLength);
            saveFileNames[i] =     saveFileNames[i].Remove(saveFileNames[i].ToCharArray().Length - fileEndingLength , fileEndingLength);
        }
      
        List<double> doubleList = new List<double>();
        double number = -1;
        for (int i = 0; i < saveFileNames.Count; i++)
        {
            if (double.TryParse(saveFileNames[i], out number))
            {
                if (number - (-1) > 0.2f) doubleList.Add(number);
            }
        }
        doubleList.Sort();
        return doubleList;
    }

    public static double CheckIfResetUniqueIdentifier(string filePath,string fileName, string fileEnding, string saveDirectory, double uniqueIdentifier)
    {
        if (uniqueIdentifier < 9999999) return uniqueIdentifier;

        List<string> saveFileNames = SaveUtil.GetSaveFileNamesFromDisk(filePath, fileName,Application.persistentDataPath + saveDirectory);
        string trimString = Application.persistentDataPath + saveDirectory + fileName;
        int newUniqueIdentCounterCounter = 0;
        
        for (int i = 0; i < saveFileNames.Count;i++)
        {
            string newLocation  = saveFileNames[i];
            newLocation  = newLocation.Remove(trimString.ToCharArray().Length, newLocation .ToCharArray().Length-trimString.ToCharArray().Length);
            newLocation  += newUniqueIdentCounterCounter + fileEnding;
            File.Move(saveFileNames[i], newLocation);
            newUniqueIdentCounterCounter++;
        }
        return saveFileNames.Count;
    }

    private static List<string> SortAfterNumbers(string filePath, string fileName, string fileEnding,  List<string> saveFileNames)
    {
        //because normal sort messes up the increment from 99 to 100   (It would could 10..101..102..11..12..13)
        //I decided to strip away the filepath, filename and ending, convert it to int, sort it there and it the strings again
        List<double> sortedSaveNamesAfterNumbers = ConvertToOnlyNumbers(filePath, fileName, fileEnding, saveFileNames);
        List<string> newSortedEmptyListToBeFilledWithSaveFileNames = new List<string>();
    
        foreach (double saveFileNumber in sortedSaveNamesAfterNumbers)
        {
            newSortedEmptyListToBeFilledWithSaveFileNames.Add(saveFileNumber.ToString(CultureInfo.InvariantCulture));
        }
    
        for (int i = 0; i < newSortedEmptyListToBeFilledWithSaveFileNames.Count; i++) 
        {
            // Use string.Format() to concatenate the strings
            newSortedEmptyListToBeFilledWithSaveFileNames[i] = string.Format("{0}{1}{2}{3}", filePath, fileName, newSortedEmptyListToBeFilledWithSaveFileNames[i], fileEnding);
        }
        return newSortedEmptyListToBeFilledWithSaveFileNames;
    }

    public static void DeleteOldSaveFiles(int howManySaveFilesAllowed, List<string> saveFilenames)
    {
        if (saveFilenames.Count < howManySaveFilesAllowed) return;
        for(int i = 0; i < saveFilenames.Count -howManySaveFilesAllowed; i++) 
            File.Delete(saveFilenames[i]);
    }

    public static void CreateDirectory(string SaveDirectory)
    {
        string dir = Application.persistentDataPath + SaveDirectory;
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
    }
   public static void DeleteAllSaveFiles(string SaveDirectory)
    {
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + SaveDirectory);
        FileInfo[] info = dir.GetFiles("*.*");

        foreach (FileInfo f in info)
        {
            File.Delete(f.ToString());
        }
    }

   /*public static string PatchFiles(string basis, string insert)
   {
       // var left = JObject.Parse(basis);
       // var right = JObject.Parse(insert);
       // var patch = new JsonDiffPatch().Diff(left, right);
       // var formatter = new JsonDeltaFormatter();
       //   formatter.Format(patch);
       //
       JsonDiffPatch jdp = new JsonDiffPatch();
       JToken basisToken = JToken.Parse(basis);
       JToken insertToken = JToken.Parse(insert);
       JToken patch = jdp.Diff(basisToken, insertToken);

       JToken output = jdp.Patch(basisToken, patch);
       return output.ToString();
   }*/
   public static DateInformation GetDateInformation() =>
       new ((byte)DateTime.Now.Minute,(byte)DateTime.Now.Hour, (byte)DateTime.Today.Day, (byte)DateTime.Today.Month, DateTime.Today.Year);
   
    private const string goToMainMenu = "goToMainMenu";

    public static string goMainMenu(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ goToMainMenu[i % goToMainMenu.Length]);
        }
        return modifiedData;
    }
}

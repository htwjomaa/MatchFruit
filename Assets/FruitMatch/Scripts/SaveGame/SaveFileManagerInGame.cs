using System;
using UnityEngine;
using UnityEngine.Serialization;

public sealed class SaveFileManagerInGame : MonoBehaviour
{
    public SaveFilePlayerProgressManagement saveFilePlayerProgressManagement;
    public void SaveGame() => saveFilePlayerProgressManagement.Save();

    private void Start() => StartCoroutine(saveFilePlayerProgressManagement.AutoSaveCo());
    
    private void OnApplicationQuit() => SaveGame();
    private void OnDestroy() => SaveGame();
}

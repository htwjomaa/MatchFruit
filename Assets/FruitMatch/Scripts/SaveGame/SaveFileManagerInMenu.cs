using UnityEngine;

public sealed class SaveFileManagerInMenu : MonoBehaviour
{
 public SaveFilePlayerProgressManagement saveFilePlayerProgressManagement;
 public SaveFileLevelConfigManagement SaveFileLevelConfigManagement;
 public void SaveGame() => saveFilePlayerProgressManagement.Save();
 public void LoadGame()
 {
  saveFilePlayerProgressManagement.LoadLatestSaveFile();
  SaveFileLevelConfigManagement.LoadLatestSaveFile();
 }

 private void OnApplicationQuit() => SaveGame();
 private void Awake() => LoadGame();

 private void Start() => StartCoroutine(saveFilePlayerProgressManagement.AutoSaveCo());
}

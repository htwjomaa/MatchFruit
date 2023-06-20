using UnityEngine;
public class LevelTextSetting : MonoBehaviour
{
   [SerializeField] private Language language;
   public void ClickFlagTextLanguage()
   {
      Rl.GameManager.PlayAudio(Rl.soundStrings.LanguageChangeSound  , Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
      Rl.adminLevelSettingsPanel.SetLanguageAndLoadText(language);
   }
}
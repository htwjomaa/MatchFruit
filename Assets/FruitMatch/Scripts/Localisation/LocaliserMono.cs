using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class LocaliserMono : MonoBehaviour
{
    [SerializeField] public Language language;
    
    private void Start()
    {
        LocaliserMono n = this;
        Rl.localiserManager.LocaliserMonos.Add(n);
        Rl.localiserManager.CheckIfActiveLanguageForColor(language, GetComponent<Image>());
    }

    private void OnDestroy()
    {
        Rl.localiserManager.LocaliserMonos.Remove(this);
    }

    public void UpdateLanguage()
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.LanguageChangeSound  , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);

        LocalisationSystem.CurrentLanguage = language;
        
        foreach (TextLocaliserUI textLocaliserUI in LocalisationSystem.AllTextLocaliserUis)
        {
            foreach (LocalisedString localisedString in textLocaliserUI.LocalisedStrings)
            {
                textLocaliserUI.transform.GetComponent<TextMeshProUGUI>().text = LocalisationSystem.GetLocalisedValue(localisedString.key);
            }
        }

        Rl.localiserManager.StartLanguageCheck();
    }

    
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public sealed class SceneSettings : MonoBehaviour
{
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider EffectsSlider;
    public Slider UISoundSlider;
    public Settings settings;
    public bool audioSettingsToggle;
    public SaveFileManagerInGame saveFileManagerInGame;

    private Sprite _cashedMasterButton;
     private Sprite _cashedMusicButton;
    private Sprite _cashedUIButton;
     private Sprite _cashedSFXButton;
  
    [SerializeField] private Sprite MutedMasterButton;
    [SerializeField] private Sprite MutedMusicButton;
    [SerializeField] private Sprite MutedUIButton;
    [SerializeField] private Sprite MutedSFXButton;
    
    private void Awake() => audioSettingsToggle = false;
    
    void Start()
    {
        MusicSlider.value = settings.GetMusicVolume;
        MasterSlider.value = settings.GetMasterVolume;
        EffectsSlider.value = settings.GetSFXVolume;
        UISoundSlider.value = settings.GetUISoundVolume;
        
        ValueChangeCheck();
        
        MasterSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
        MusicSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
        EffectsSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
        UISoundSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});

        CasheButtonSprites();
        SetMutedButtons();
    }
    
    public void ValueChangeCheck()
    {
        settings.GetMusicVolume = MusicSlider.value;
        settings.GetMasterVolume = MasterSlider.value;
        settings.GetSFXVolume = EffectsSlider.value;
        settings.GetUISoundVolume = UISoundSlider.value;
        
        Rl.GameManager.gameManagerAudioSource.volume = settings.GetMusicVolume * settings.GetMasterVolume;
        Rl.effects.audioSource.volume = settings.GetMusicVolume * settings.GetMasterVolume;
        Rl.uiSounds.audioSource.volume = settings.GetUISoundVolume * settings.GetMasterVolume;
        settings.SetSettings();
    }

    [SerializeField] private GameObject MasterButton;
    [SerializeField] private GameObject MusicButton;
    [SerializeField] private GameObject SFXButton;
    [SerializeField] private GameObject UIButton;

    private void CasheButtonSprites()
    {
       _cashedUIButton = UIButton.GetComponent<Image>().sprite;
       _cashedSFXButton = SFXButton.GetComponent<Image>().sprite;
       _cashedMusicButton = MusicButton.GetComponent<Image>().sprite;
       _cashedMasterButton = MasterButton.GetComponent<Image>().sprite;
    }
    private void SetMutedButtons()
    {
        if (Rl.settings.UIMuted) UIButton.GetComponent<Image>().sprite = MutedUIButton;
        else UIButton.GetComponent<Image>().sprite = _cashedUIButton;
        
        if (Rl.settings.SFXMuted) SFXButton.GetComponent<Image>().sprite = MutedSFXButton;
        else SFXButton.GetComponent<Image>().sprite = _cashedSFXButton;
        
        if (Rl.settings.MusicMutedProperty) MusicButton.GetComponent<Image>().sprite = MutedMusicButton;
        else MusicButton.GetComponent<Image>().sprite = _cashedMusicButton;
        
        if (Rl.settings.MasterMutedProperty) MasterButton.GetComponent<Image>().sprite = MutedMasterButton;
        else MasterButton.GetComponent<Image>().sprite = _cashedMasterButton;
    }
    public void ToggleUIMuted()
    {
        
        Rl.settings.UIMuted = !Rl.settings.UIMuted;
        if (Rl.settings.UIMuted)
        {
            Rl.GameManager.PlayAudio(Rl.soundStrings.MuteUnmute, Random.Range(0,2), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
            UIButton.GetComponent<Image>().sprite = MutedUIButton;
        }
        else
        {
            Rl.GameManager.PlayAudio(Rl.soundStrings.MuteUnmute, Random.Range(2,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
            UIButton.GetComponent<Image>().sprite = _cashedUIButton;
        }
    }
    public void ToggleSFXMuted()
    {
        Rl.settings.SFXMuted = !Rl.settings.SFXMuted;
        if (Rl.settings.SFXMuted)
        {
            Rl.GameManager.PlayAudio(Rl.soundStrings.MuteUnmute, Random.Range(0,2), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
            SFXButton.GetComponent<Image>().sprite = MutedSFXButton;
        }
        else
        {
            Rl.GameManager.PlayAudio(Rl.soundStrings.MuteUnmute, Random.Range(2,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
            SFXButton.GetComponent<Image>().sprite = _cashedSFXButton;
        }
    }
    
    public void ToggleMusicMuted()
    {
        Rl.settings.MusicMutedProperty= !Rl.settings.MusicMutedProperty;

        if (Rl.settings.MusicMutedProperty)
        {
            Rl.GameManager.PlayAudio(Rl.soundStrings.MuteUnmute, Random.Range(0,2), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
            MusicButton.GetComponent<Image>().sprite = MutedMusicButton;
        }
        else
        {
            Rl.GameManager.PlayAudio(Rl.soundStrings.MuteUnmute, Random.Range(2,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
            MusicButton.GetComponent<Image>().sprite = _cashedMusicButton;
        }
    }
    public void ToggleMasterMuted()
    {
        Rl.settings.MasterMutedProperty  = !Rl.settings.MasterMutedProperty;

        if (Rl.settings.MasterMutedProperty)
        {
            Rl.GameManager.PlayAudio(Rl.soundStrings.MuteUnmute, Random.Range(0,2), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
            MasterButton.GetComponent<Image>().sprite = MutedMasterButton;
        }
        else
        {
            Rl.GameManager.PlayAudio(Rl.soundStrings.MuteUnmute, Random.Range(2,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
            MasterButton.GetComponent<Image>().sprite = _cashedMasterButton;
        }
    }
    public void ToogleAudioSettingsCanvas()
    {
        if(!audioSettingsToggle) ActivateAudioSettingsCanvas();
        else if(audioSettingsToggle) DeActivateAudioSettingsCanvas();
        audioSettingsToggle = !audioSettingsToggle;
    }
    public void DeActivateAudioSettingsCanvas()
    {
        Rl.fadePanelController.FadeOutAudioSettings();
        Rl.GameManager.PlayAudio(Rl.soundStrings.AudioSettingsButton, Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    }
    public void ActivateAudioSettingsCanvas()
    {
        Rl.fadePanelController.FadeInAudioSettings();
        Rl.GameManager.PlayAudio(Rl.soundStrings.AudioSettingsButton, Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    }

    public void RestartScene()
    {
        float timer = 0;
        if (Rl.GameManager.audiodataBase.allAudioNames.ContainsKey(Rl.soundStrings.RestartLevelButton ))
        {
            Rl.GameManager.PlayAudio(Rl.soundStrings.RestartLevelButton , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
            timer = Rl.GameManager.audiodataBase.allAudioSettings[Rl.GameManager.audiodataBase.allAudioNames[Rl.soundStrings.RestartLevelButton].index].audioClip.length+0.15f;
        }
        else Debug.Log("Audio not found");

        StartCoroutine(DelaySceneLoadCo(timer, UnityEngine.SceneManagement.SceneManager.GetActiveScene().name));
    }

    public void SplashMenu()
    {
        float timer = 0;
     
        if (Rl.GameManager.audiodataBase.allAudioNames.ContainsKey(Rl.soundStrings.SplashMenuButton ))
        {
            Rl.GameManager.PlayAudio(Rl.soundStrings.SplashMenuButton , Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
            timer = Rl.GameManager.audiodataBase
                        .allAudioSettings[Rl.GameManager.audiodataBase.allAudioNames[Rl.soundStrings.SplashMenuButton].index].audioClip
                        .length /
                    2.2f;
        }
        else Debug.Log("Audio not found");

        StartCoroutine(DelaySceneLoadCo(timer, Rl.Game));
    }
    
    IEnumerator DelaySceneLoadCo(float timer, string scene)
    {    
       // saveFileManagerInGame.SaveGame();
        StartCoroutine(DecreaseVolumeCo());
        yield return new WaitForSeconds(timer);
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
    IEnumerator DecreaseVolumeCo()
    {
        Rl.GameManager.gameManagerAudioSource.volume *= 0.9f;
        yield return new WaitForFixedUpdate();
        StartCoroutine(DecreaseVolumeCo());
    }
    public void ExitButtonIngame() => Application.Quit();
}
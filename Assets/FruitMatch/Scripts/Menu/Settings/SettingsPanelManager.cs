using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public sealed class SettingsPanelManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> adminAccessList = new List<GameObject>();
    public GameObject Panel;
    public Animator RedDot;
    public Animator Stripe;
    [SerializeField] private string moveToAudioSettings = "moveToAudioSettings";
    [SerializeField] private string moveToAnimationSettings = "moveToAnimationSettings";
      
    [SerializeField] private string moveToAdmin1Settings= "moveToAdmin1Settings";
    [SerializeField] private string moveToAdmin2Settings = "moveToAdmin2Settings";
    [SerializeField] private string moveToStatisticSettings= "moveToStatisticSettings";

    private const string audioSettingsTarget = "audioSettings";
    private const string animationSettingsTarget = "animationSettings";
    private const string admin1SettingsTarget = "admin1Settings";
    private const string admin2SettingsTarget = "admin2Settings";
    private const string statisticSettingsTarget = "statisticSettings";
         
    [SerializeField] private Slider iPadResolutionbarFill;
    [SerializeField] private TextMeshProUGUI iPadResolutionDimensionsText;
    [SerializeField] private TextMeshProUGUI iPadResolutionIpadsText;
    [SerializeField] private TextMeshProUGUI iPadResolutionIpadsTextTwo;
    
    
    [FormerlySerializedAs("textureQualityBarFill")] [SerializeField] private Slider FullResQualityBarFill;
    [SerializeField] private Slider effectsQualityBarFill;
    
    [SerializeField] private Slider swipeRejectionBarFill;
    [SerializeField] private Slider animationSpeedBarFill;
    [SerializeField] private Slider swapbackSpeedBarFill;
    [SerializeField] private Slider refillDelayBarFill;
    [SerializeField] private Slider shuffleDelayBarFill;
    
    
    [SerializeField] private SwitchButton switchButtonGraphic;

    [SerializeField] private GameObject InfoBox;

  
    public void ToggleInfobox(Transform transform)
    {
        GenericSettingsFunctions.SmallJumpAnimation(transform);
        GenericSettingsFunctions.SmallJumpAnimation(InfoBox.transform);
        if(InfoBox.activeSelf) InfoBox.SetActive(false);
        else
        {
            InfoBox.SetActive(true);
        }
    }

    public void DisableInfoBox()
    {
        InfoBox.SetActive(false);
    }
   
    public void IncreaseDecraseIpadResolution(bool increase)
    {
        if (increase)
        {
            PlayIncreaseSound();
            Rl.settings.IncreaseResolution();
        }

        else
        {
            PlayDecreaseSound();
            Rl.settings.DecreaseResolution();
        }
        ChangeIpadFillBarSlider();
        IPadResolutionDimensionsText();
        IPadResolutionIpadsText();
    }
    
  
    [SerializeField] private  float[] ipadResolutionBarFills = { 0.1f, 0.25f, 0.4f, 0.555f, 0.7f, 0.86f, 1f };
    [FormerlySerializedAs("textureQualityBarFills")] [SerializeField]  private float[] fullRessQualityBarFills = { 0.225f, 0.5f, 0.775f, 1f };
    [SerializeField]   private  float[] effectsQualityBarFills = { 0.225f, 0.5f, 0.775f, 1f };

    
    [SerializeField] private  static float[] swipeRejectionBarFills = { 0.225f, 0.5f, 0.775f, 1f };
    [SerializeField] private  static float[] animationSpeedBarFills  = { 0.225f, 0.5f, 0.775f, 1f };
    [SerializeField] private static float[] swapbackSpeedBarFills = { 0.225f, 0.5f, 0.775f, 1f };
    
    [SerializeField] private static  float[] refillDelayBarFills = { 0.1f, 0.25f, 0.4f, 0.555f, 0.7f, 0.86f, 1f };
    [SerializeField] private static float[] shuffleDelayBarFills = { 0.1f, 0.25f, 0.4f, 0.555f, 0.7f, 0.86f, 1f };
    
    

    private void PlayIncreaseSound() =>  Rl.GameManager.PlayAudio(Rl.soundStrings.IncreaseSound, Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    private void PlayDecreaseSound() => Rl.GameManager.PlayAudio(Rl.soundStrings.DecreaseSound , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    
    private void PlayAcceptGraphicSound() => Rl.GameManager.PlayAudio(Rl.soundStrings.AcceptGraphicSound , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    private void PlayRevertSound() => Rl.GameManager.PlayAudio(Rl.soundStrings.RevertGraphicSound  , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);

    [SerializeField]private TextMeshProUGUI adminAcessPassInput;
    [SerializeField]private string moveToGameMenu = "123";
    public void CheckForAdminPass()
    {
        Debug.Log(adminAcessPassInput.text);
        var n = string.Compare(adminAcessPassInput.text, adminAcessPassInput.text);
        if ( n == 0)
        ActivateOrDeactiveAdminButtons(true);
    }
    
    public void ActivateOrDeactiveAdminButtons(bool setActive)
    {
        Debug.Log("Access two graneted");
        foreach (GameObject adminElement in adminAccessList)
        {
            adminElement.SetActive(setActive);
        }

        SetadminPanelInActive();
    }
    
    public void IncreaseDecreaseSwipeRejection(bool increase)
    {
        if (increase)
        {
            PlayIncreaseSound();
            Rl.settings.IncreaseSwipeRejectionSettings();
        }
        else
        {
            PlayDecreaseSound();
            Rl.settings.DecreaseSwipeRejectionSettings();
        }
        ChangeSwipeRejectionBarSlider();
    }
    
    public void IncreaseDecreaseAnimationSpeed(bool increase)
    {
        if (increase)
        {
            PlayIncreaseSound();
            Rl.settings.IncreaseAnimationSpeed();
        }
        else
        {
            PlayDecreaseSound();
            Rl.settings.DecreaseAnimationSpeed();
        }
        ChangeAnimationSpeedFillBarSlider();
    }
    
    public void IncreaseDecreaseSwapBackSpeed(bool increase)
    {
        if (increase)
        {
            PlayIncreaseSound();
            Rl.settings.IncreaseSwapBackSpeed();
        }
        else
        {
            PlayDecreaseSound();
            Rl.settings.DecreaseSwapBackSpeed();
        }
        ChangeSwapBackSpeedFillBarSlider();
    }
    
    public void IncreaseRefillDelay(bool increase)
    {
        if (increase)
        {
            PlayIncreaseSound();
            Rl.settings.IncreaseRefillDelay();
        }
        else
        {
            PlayDecreaseSound();
            Rl.settings.DecreaseRefillDelay();
        }
        ChangeRefillDelayFillBarSlider();
    }
    
    public void IncreaseShuffleDelay(bool increase)
    {
        if (increase)
        {
            PlayIncreaseSound();
            Rl.settings.IncreaseShuffleDelay();
        }
        else
        {
            PlayDecreaseSound();
            Rl.settings.DecreaseShuffleDelay();
        }
        ChangeShuffleDelayFillBarSlider();
    }

    [SerializeField] private TextMeshProUGUI fullResLabel;
    public void IncreaseDecraseTextureQuality(bool increase)
    {
        if (increase)
        {
            PlayIncreaseSound();
            Rl.settings.IncreaseFullResolution();
        }
        else
        {
            PlayDecreaseSound();
            Rl.settings.DecreaseFullResolution();
        }
        ChangeFullResFillBarSlider();
        fullResLabel.text = LocalisationSystem.GetLocalisedValue(fullResLabels[Rl.settings.FullResolution]);
    }
    private static string[] fullResLabels =
    {
        "quarter_res", "half_res", "threequarter_res", "fullres"
    };
    public void IncreaseDecraseEffectsQuality(bool increase)
    {
        if (increase)
        {
            PlayIncreaseSound();
            Rl.settings.IncreaseEffectsQuality();
        }
        else
        {
            PlayDecreaseSound();
            Rl.settings.DecreaseEffectsQuality();
        }
        ChangeEffectsQualityFillBarSlider();
    }
    public void AcceptSettings(bool setGraphicSettings)
    {
        PlayAcceptGraphicSound();
        Rl.settings.SetSettings();
        if (setGraphicSettings) Rl.settings.SetGraphicSettings();
        Rl.saveFileManagerInMenu.SaveGame();
    }
    
    public void RevertSettings(bool setGraphicSettings)
    {
        PlayRevertSound();
        Rl.saveFileManagerInMenu.LoadGame();
       if(setGraphicSettings) Rl.settings.SetGraphicSettings();
        UpdateAllSliderAndText();
    }
    
    private void UpdateAllSliderAndText()
    {
        ChangeIpadFillBarSlider();
        IPadResolutionDimensionsText();
        IPadResolutionIpadsText();
        
        ChangeEffectsQualityFillBarSlider();
        ChangeFullResFillBarSlider();
        
        ChangeSwipeRejectionBarSlider();
        ChangeSwapBackSpeedFillBarSlider();
        ChangeAnimationSpeedFillBarSlider();
       
        ChangeShuffleDelayFillBarSlider();
        ChangeRefillDelayFillBarSlider();
        
        switchButtonGraphic.SetSwitches();
    }

    private void Start()
    {
        Panel.SetActive(false);
        UpdateAllSliderAndText();
        if(Rl.settings.KeepAdminAccess)Rl.settingsPanelManager.ActivateOrDeactiveAdminButtons(true);
    }

    private void IPadResolutionIpadsText()
    {
        StringBuilder textFieldOne = new StringBuilder();
        StringBuilder textFieldTwo= new StringBuilder();
        int counterIpads = Settings.ipadResolutions[Rl.settings.GetResoltionSettings()].Ipads.Length;
        iPadResolutionIpadsText.text = "";
        iPadResolutionIpadsTextTwo.text = "";

        var ns = Settings.ipadResolutions[Rl.settings.GetResoltionSettings()].Ipads;
        for (var index = 0; index < 5 && index < counterIpads; index++)
        {
            textFieldOne.Append(ns[index]+ "\n");
        }
        for (var index = 5; index < counterIpads; index++)
        {
            textFieldTwo.Append(ns[index]+ "\n");
        }

        iPadResolutionIpadsText.text = textFieldOne.ToString();
        iPadResolutionIpadsTextTwo.text = textFieldTwo.ToString();
    }
    private void IPadResolutionDimensionsText()
    {
        iPadResolutionDimensionsText.text = Settings.ipadResolutions[Rl.settings.GetResoltionSettings()].Width + "x" +
                                            Settings.ipadResolutions[Rl.settings.GetResoltionSettings()].Height;
    }
    private void ChangeIpadFillBarSlider()
    {
        iPadResolutionbarFill.value = ipadResolutionBarFills[Rl.settings.GetResoltionSettings()];
        GenericSettingsFunctions.SmallJumpAnimation(0.123f,iPadResolutionbarFill.transform);
    }

    private void ChangeFullResFillBarSlider()
    {
        FullResQualityBarFill.value = fullRessQualityBarFills[Rl.settings.FullResolution];
        fullResLabel.text = LocalisationSystem.GetLocalisedValue(fullResLabels[Rl.settings.FullResolution]);
        GenericSettingsFunctions.SmallJumpAnimation(0.123f,FullResQualityBarFill.transform);
        GenericSettingsFunctions.SmallJumpAnimation(fullResLabel.transform);
    }

    private void ChangeEffectsQualityFillBarSlider()
    {
        effectsQualityBarFill.value = effectsQualityBarFills[Rl.settings.EffectsQuality];
        GenericSettingsFunctions.SmallJumpAnimation(0.123f,effectsQualityBarFill.transform);
    }

    private void ChangeSwipeRejectionBarSlider()
    {
        swipeRejectionBarFill.value = swipeRejectionBarFills[Rl.settings.GetSwipeRejectionSettings()];
        GenericSettingsFunctions.SmallJumpAnimation(0.123f,swipeRejectionBarFill.transform);
    }

    private void ChangeAnimationSpeedFillBarSlider()
    {
        animationSpeedBarFill.value = animationSpeedBarFills[Rl.settings.GetAnimationSpeedSettings()];
        GenericSettingsFunctions.SmallJumpAnimation(0.123f,animationSpeedBarFill.transform);
    }

    private void ChangeSwapBackSpeedFillBarSlider()
    {
        swapbackSpeedBarFill.value = swapbackSpeedBarFills[Rl.settings.GetSwapBackSettings()];
        GenericSettingsFunctions.SmallJumpAnimation(0.123f,swapbackSpeedBarFill.transform);
    }

    private void ChangeRefillDelayFillBarSlider()
    {
        refillDelayBarFill.value = refillDelayBarFills[Rl.settings.GetRefillDealySettings()];
        GenericSettingsFunctions.SmallJumpAnimation(0.123f,refillDelayBarFill.transform);
    }

    private void ChangeShuffleDelayFillBarSlider()
    {
        shuffleDelayBarFill.value = shuffleDelayBarFills[Rl.settings.GetShuffleDealySettings()];
        GenericSettingsFunctions.SmallJumpAnimation(0.123f,shuffleDelayBarFill.transform);
    }

    public void SetAdminPanelActive()
    {
        if(Rl.splashMenu.AdminAcessEnabled)Panel.SetActive(true);
    }
    public void SetadminPanelInActive()
    {
        Panel.SetActive(false);
        Rl.splashMenu.AdminAcessEnabled = false;
    }

    public void MoveRedDotAndStripe(string target)
    {
        SetAllBoolsToFalse();
        switch (target)
        {
            case audioSettingsTarget:
                SetThisBoolToTrue(moveToAudioSettings);
                break;
            case animationSettingsTarget:
                SetThisBoolToTrue(moveToAnimationSettings);
                break;
            case admin1SettingsTarget:
                SetThisBoolToTrue(moveToAdmin1Settings);
                break;
            case admin2SettingsTarget:
                SetThisBoolToTrue(moveToAdmin2Settings);
                break;
            case statisticSettingsTarget:
                SetThisBoolToTrue(moveToStatisticSettings);
                break;
            default: break;
        }
    }

    private void SetAllBoolsToFalse()
    {
        RedDot.SetBool(moveToAudioSettings, false);
        Stripe.SetBool(moveToAudioSettings, false);
        
        RedDot.SetBool(moveToAnimationSettings, false);
        Stripe.SetBool(moveToAnimationSettings, false);
        
        RedDot.SetBool(moveToAdmin1Settings, false);
        Stripe.SetBool(moveToAdmin1Settings, false);
        
        RedDot.SetBool(moveToAdmin2Settings, false);
        Stripe.SetBool(moveToAdmin2Settings, false);
        
        RedDot.SetBool(moveToStatisticSettings, false);
        Stripe.SetBool(moveToStatisticSettings, false);
    }
    
    private void SetThisBoolToTrue(string boolSetToTrue)
    {
        RedDot.SetBool(boolSetToTrue, true);
        Stripe.SetBool(boolSetToTrue, true);
    }
}
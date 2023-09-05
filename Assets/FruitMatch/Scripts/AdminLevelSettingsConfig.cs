using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdminLevelSettingsConfig : MonoBehaviour
{
    [SerializeField] public Slider CopyAllDataFromSlider;
    [SerializeField] public Slider CopyAllFieldDataFromSlider;
    [SerializeField] public TextMeshProUGUI CopyAllDataFromValueDisplay;
    [SerializeField] public TextMeshProUGUI CopyAllFieldDataFromValueDisplay;
    private List<TextMeshProUGUI> _valueDisplayList = new List<TextMeshProUGUI>();
    private void Awake() => _valueDisplayList = 
        GenericSettingsFunctions.AddToValueDisplayList(CopyAllDataFromValueDisplay, CopyAllFieldDataFromValueDisplay);

    public void ValueChangeCheck()
    {
        GenericSettingsFunctions.UpdateTextFields(ref _valueDisplayList, CopyAllDataFromSlider, CopyAllFieldDataFromSlider);
    }
    public void LoadLayoutSettings()
    {
        RemoveListeners();
        Addlisteners();
        ValueChangeCheck();
    }
    private void RemoveListeners() => GenericSettingsFunctions.RemoveListeners(CopyAllDataFromSlider, CopyAllFieldDataFromSlider);
    
    private void Addlisteners() => GenericSettingsFunctions.Addlisteners(delegate { ValueChangeCheck(); }, CopyAllDataFromSlider, CopyAllFieldDataFromSlider);
    

    public void ClickCopyButton(Transform buttonTransform)
    {
        if(buttonTransform != null) GenericSettingsFunctions.SmallJumpAnimation(buttonTransform);
        Rl.GameManager.PlayAudio(Rl.soundStrings.AcceptSwitchSound, Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber] =
            (LevelConfig)GenericSettingsFunctions.GetDeepCopy(Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[((int)CopyAllDataFromSlider.value )-1]);
        Rl.adminLevelSettingsPanel.RevertChanges(true);
        Rl.saveFileLevelConfigManagement.Save();
    }
    
    public void ClickCopyFieldButton(Transform buttonTransform)
    {
        if(buttonTransform != null) GenericSettingsFunctions.SmallJumpAnimation(buttonTransform);
        Rl.GameManager.PlayAudio(Rl.soundStrings.AcceptSwitchSound, Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        FieldState.CopyAllFields((byte)(CopyAllFieldDataFromSlider.value-1));
    }
}


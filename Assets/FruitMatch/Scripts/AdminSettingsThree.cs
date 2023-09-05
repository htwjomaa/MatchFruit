using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AdminSettingsThree : MonoBehaviour
{
    

    [SerializeField] public Slider CopyAllDataFromSlider;
  [SerializeField] public Slider CopyAllDataToSlider;
    
    [SerializeField] public TextMeshProUGUI CopyAllDataFromValueDisplay;
    [SerializeField] public TextMeshProUGUI CopyAllDataToValueDisplay;
    private List<TextMeshProUGUI> _valueDisplayList = new List<TextMeshProUGUI>();
    private void Awake()
    {
        _valueDisplayList =
            GenericSettingsFunctions.AddToValueDisplayList(CopyAllDataFromValueDisplay, CopyAllDataToValueDisplay);
        LoadAdminThreeSettings();
    }

    public void ValueChangeCheck()
    {
        GenericSettingsFunctions.UpdateTextFields(
            ref _valueDisplayList,
            CopyAllDataFromSlider,
            CopyAllDataToSlider);
        
        GenericSettingsFunctions.UpdateTextFields(ref _valueDisplayList, CopyAllDataFromSlider, CopyAllDataToSlider);
    }
    public void LoadAdminThreeSettings()
    {
        RemoveListeners();
        Addlisteners();
        ValueChangeCheck();
    }
    private void RemoveListeners() => GenericSettingsFunctions.RemoveListeners(CopyAllDataFromSlider, CopyAllDataToSlider);
    
    private void Addlisteners() => GenericSettingsFunctions.Addlisteners(delegate { ValueChangeCheck(); }, CopyAllDataFromSlider, CopyAllDataToSlider);
    
    public void ClickCopyButton(Transform buttonTransform)
    {
        if(buttonTransform != null) GenericSettingsFunctions.SmallJumpAnimation(buttonTransform);
        Rl.GameManager.PlayAudio(Rl.soundStrings.AcceptSwitchSound, Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[((int)CopyAllDataToSlider.value )-1] =
            (LevelConfig)GenericSettingsFunctions.GetDeepCopy(Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[((int)CopyAllDataFromSlider.value )-1]);
        Rl.adminLevelSettingsPanel.RevertChanges(true);
        Rl.saveFileLevelConfigManagement.Save();
    }
}

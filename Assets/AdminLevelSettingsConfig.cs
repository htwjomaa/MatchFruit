using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdminLevelSettingsConfig : MonoBehaviour
{
    private int targetFrom = 0;
    [SerializeField] public Slider CopyAllDataFromSlider;

    [SerializeField] public TextMeshProUGUI CopyAllDataFromValueDisplay;
    private List<TextMeshProUGUI> _valueDisplayList = new List<TextMeshProUGUI>();
    private void Awake() => _valueDisplayList = 
        GenericSettingsFunctions.AddToValueDisplayList(CopyAllDataFromValueDisplay);

    public void ValueChangeCheck()
    {
        GenericSettingsFunctions.UpdateTextFields(
            ref _valueDisplayList,
            CopyAllDataFromSlider);
        
        GenericSettingsFunctions.UpdateTextFields(ref _valueDisplayList, CopyAllDataFromSlider);
    }
    public void LoadLayoutSettings()
    {
        RemoveListeners();
        Addlisteners();
        ValueChangeCheck();
    }
    private void RemoveListeners() => GenericSettingsFunctions.RemoveListeners(CopyAllDataFromSlider);
    
    private void Addlisteners() => GenericSettingsFunctions.Addlisteners(delegate { ValueChangeCheck(); }, CopyAllDataFromSlider);
    
    private void LoadConfigsToClipBoard(LayoutFieldConfig layoutFieldsConfigs)
    {
        // Rl.saveClipBoard.ZoomOutValue = zoomOutValue.ToArray();
        // Rl.saveClipBoard.TopPaddingValue = topPaddingValue.ToArray();
        // Rl.saveClipBoard.BottomPaddingValue = bottomPaddingValue.ToArray();
        // Rl.saveClipBoard.LeftPaddingValue = leftPaddingValue.ToArray();
        // Rl.saveClipBoard.RightPaddingValue = rightPaddingValue.ToArray();
    }

    public void ClickCopyButton(Transform buttonTransform)
    {
        if(buttonTransform != null) GenericSettingsFunctions.SmallJumpAnimation(buttonTransform);
        Rl.GameManager.PlayAudio(Rl.soundStrings.AcceptSwitchSound, Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber] =
            (LevelConfig)GenericSettingsFunctions.GetDeepCopy(Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[((int)CopyAllDataFromSlider.value )-1]);
        Rl.adminLevelSettingsPanel.RevertChanges(true);
        Rl.saveFileLevelConfigManagement.Save();
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;
using Random = UnityEngine.Random;

public sealed class AdminLevelSettingsLayout : MonoBehaviour
{
    [SerializeField] private Slider zoomOutSlider;
    [SerializeField] private Slider topPaddingSlider;
    [SerializeField] private Slider bottomPaddingSlider;
    [SerializeField] private Slider leftPaddingSlider;
    [SerializeField] private Slider rightPaddingSlider;


    [SerializeField] private TextMeshProUGUI zoomOutValue;
    [SerializeField] private TextMeshProUGUI topPaddingValue;
    [SerializeField] private TextMeshProUGUI bottomPaddingValue;
    [SerializeField] private TextMeshProUGUI leftPaddingValue;
    [SerializeField] private TextMeshProUGUI rightPaddingValue;
    private List<TextMeshProUGUI> _valueDisplayList = new List<TextMeshProUGUI>();

    private void Awake() => _valueDisplayList = 
        GenericSettingsFunctions.AddToValueDisplayList(zoomOutValue, topPaddingValue, bottomPaddingValue, leftPaddingValue, rightPaddingValue);

    private void RemoveListeners() => GenericSettingsFunctions.RemoveListeners(zoomOutSlider, topPaddingSlider, bottomPaddingSlider, leftPaddingSlider, rightPaddingSlider);
    
    private void Addlisteners() => GenericSettingsFunctions.Addlisteners(delegate { ValueChangeCheck(); }, zoomOutSlider, topPaddingSlider, bottomPaddingSlider, leftPaddingSlider, rightPaddingSlider);

    public void LoadLayoutSettings(LayoutFieldConfig layoutFieldConfigs)
    {
        if(layoutFieldConfigs.LayoutConfig == null || layoutFieldConfigs.LayoutConfig.Length != SaveFileLevelConfigs.Fields)
            Array.Resize(ref layoutFieldConfigs.LayoutConfig, SaveFileLevelConfigs.Fields);
        RemoveListeners();
        LoadMatchFinderConfigToClipBoard(layoutFieldConfigs);
        ClipBoardToSlider();
        Addlisteners();
        ValueChangeCheck();
    }
    private void LoadMatchFinderConfigToClipBoard(LayoutFieldConfig layoutFieldsConfigs)
    {
        
        //The list to Array memory overhead is okay with those small numbers. It looks bad I know.
        //Keep it this way to be more flexible
        List<float> zoomOutValue = new List<float>();
        List<float> topPaddingValue = new List<float>();
        List<float> bottomPaddingValue = new List<float>();
        List<float> leftPaddingValue = new List<float>();
        List<float> rightPaddingValue = new List<float>();
        for (int i = 0; i < layoutFieldsConfigs.LayoutConfig.Length; i++)
        {
            zoomOutValue.Add(layoutFieldsConfigs.LayoutConfig[i].ZoomOut);
            topPaddingValue.Add(layoutFieldsConfigs.LayoutConfig[i].TopPadding);
            bottomPaddingValue .Add(layoutFieldsConfigs.LayoutConfig[i].BottomPadding);
            leftPaddingValue.Add(layoutFieldsConfigs.LayoutConfig[i].LeftPadding);
            rightPaddingValue.Add(layoutFieldsConfigs.LayoutConfig[i].RightPadding);
        }

        Rl.saveClipBoard.ZoomOutValue = zoomOutValue.ToArray();
        Rl.saveClipBoard.TopPaddingValue = topPaddingValue.ToArray();
        Rl.saveClipBoard.BottomPaddingValue = bottomPaddingValue.ToArray();
        Rl.saveClipBoard.LeftPaddingValue = leftPaddingValue.ToArray();
        Rl.saveClipBoard.RightPaddingValue = rightPaddingValue.ToArray();
    }

    
    public LayoutFieldConfig SaveLayoutSettings()
    {
        LayoutFieldConfig layoutFieldConfig = new LayoutFieldConfig();
        Array.Resize(ref layoutFieldConfig.LayoutConfig, SaveFileLevelConfigs.Fields);
        for (int i = 0; i < SaveFileLevelConfigs.Fields; i++)
        {
            layoutFieldConfig.LayoutConfig[i].ZoomOut = Rl.saveClipBoard.ZoomOutValue[i];
            layoutFieldConfig.LayoutConfig[i].LeftPadding = Rl.saveClipBoard.LeftPaddingValue[i];
            layoutFieldConfig.LayoutConfig[i].RightPadding = Rl.saveClipBoard.RightPaddingValue[i];
            layoutFieldConfig.LayoutConfig[i].TopPadding = Rl.saveClipBoard.TopPaddingValue[i];
            layoutFieldConfig.LayoutConfig[i].BottomPadding = Rl.saveClipBoard.BottomPaddingValue[i];
        }

        return layoutFieldConfig;
    }
    public void ValueChangeCheck()
    {
        Rl.saveClipBoard.ZoomOutValue[FieldState.CurrentField] = zoomOutSlider.value;
        Rl.saveClipBoard.TopPaddingValue[FieldState.CurrentField]  = topPaddingSlider.value;
        Rl.saveClipBoard.LeftPaddingValue[FieldState.CurrentField]  = leftPaddingSlider.value;
        Rl.saveClipBoard.BottomPaddingValue[FieldState.CurrentField]  = bottomPaddingSlider.value;
        Rl.saveClipBoard.RightPaddingValue[FieldState.CurrentField]  = rightPaddingSlider.value;
        
        ClipBoardToSlider();
        GenericSettingsFunctions.UpdateTextFields(ref _valueDisplayList, zoomOutSlider, topPaddingSlider, bottomPaddingSlider, leftPaddingSlider, rightPaddingSlider);
    }

    public void LoadCurrentField()
    {
        RemoveListeners();
        ClipBoardToSlider();
        ValueChangeCheck();
        Addlisteners();
    }
    public void ClipBoardToSlider()
    {
        zoomOutSlider.value =  Rl.saveClipBoard.ZoomOutValue[FieldState.CurrentField];
        topPaddingSlider.value = Rl.saveClipBoard.TopPaddingValue[FieldState.CurrentField];
        leftPaddingSlider.value = Rl.saveClipBoard.LeftPaddingValue[FieldState.CurrentField];
        bottomPaddingSlider.value = Rl.saveClipBoard.BottomPaddingValue[FieldState.CurrentField];
        rightPaddingSlider.value = Rl.saveClipBoard.RightPaddingValue[FieldState.CurrentField] ;
    }

   
    public void ClickZoomOutButton()
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound , Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        Rl.saveClipBoard.ZoomOutValue[FieldState.CurrentField] =
        Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs
            .LevelConfigs[Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber].LayoutFieldConfigs.LayoutConfig[FieldState.CurrentField].ZoomOut;

        zoomOutSlider.value = Rl.saveClipBoard.ZoomOutValue[FieldState.CurrentField];
    }
    
    public void ClickTopPaddingButton()
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound , Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        Rl.saveClipBoard.TopPaddingValue[FieldState.CurrentField] =
            Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs
                .LevelConfigs[Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber].LayoutFieldConfigs.LayoutConfig[FieldState.CurrentField].TopPadding;

        topPaddingSlider.value = Rl.saveClipBoard.TopPaddingValue[FieldState.CurrentField];
    }
    
    public void ClickBottomPaddingButton()
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound , Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        Rl.saveClipBoard.BottomPaddingValue[FieldState.CurrentField] =
            Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs
                .LevelConfigs[Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber].LayoutFieldConfigs.LayoutConfig[FieldState.CurrentField].BottomPadding;

        bottomPaddingSlider.value = Rl.saveClipBoard.BottomPaddingValue[FieldState.CurrentField];
    }
    
    public void ClickLeftPaddingButton()
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound , Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        Rl.saveClipBoard.LeftPaddingValue[FieldState.CurrentField] =
            Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs
                .LevelConfigs[Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber].LayoutFieldConfigs.LayoutConfig[FieldState.CurrentField].LeftPadding;

        leftPaddingSlider.value = Rl.saveClipBoard.LeftPaddingValue[FieldState.CurrentField];
    }
    
    public void ClickRightpaddingButton()
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound , Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        Rl.saveClipBoard.RightPaddingValue[FieldState.CurrentField] =
            Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs
                .LevelConfigs[Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber].LayoutFieldConfigs.LayoutConfig[FieldState.CurrentField].RightPadding;

        rightPaddingSlider.value = Rl.saveClipBoard.RightPaddingValue[FieldState.CurrentField];
    }

    public void CopyField(byte fieldOne, byte fieldTwo)
    {
        Rl.saveClipBoard.RightPaddingValue[fieldTwo] = Rl.saveClipBoard.RightPaddingValue[fieldOne];
        Rl.saveClipBoard.LeftPaddingValue[fieldTwo] = Rl.saveClipBoard.LeftPaddingValue[fieldOne];
        Rl.saveClipBoard.ZoomOutValue[fieldTwo] = Rl.saveClipBoard.ZoomOutValue[fieldOne];
        Rl.saveClipBoard.TopPaddingValue[fieldTwo] = Rl.saveClipBoard.TopPaddingValue[fieldOne];
        Rl.saveClipBoard.BottomPaddingValue[fieldTwo] = Rl.saveClipBoard.BottomPaddingValue[fieldOne];
    }
}

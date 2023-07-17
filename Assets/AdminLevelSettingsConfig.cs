/*
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
    private TextMeshProUGUI _valueDisplayList;
    void Start()
    {
        
    }
    public void ValueChangeCheck()
    {
        GenericSettingsFunctions.UpdateTextFields(
            ref _valueDisplayList,
            CopyAllDataFromSlider);
    }
    private void AddListeners()
    {
        GenericSettingsFunctions.Addlisteners(delegate { ValueChangeCheck(); }, 
            CopyAllDataFromSlider);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void 
        
    public void ClickCopyButton()
    {
        Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber] =
            Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[targetFrom-1];
    }
}
*/

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AdminLevelSettingsMatchFinder : MonoBehaviour
{
    public delegate void MatchFinderEvent();
    public static event MatchFinderEvent matchFinderLoadTrigger;
    
    public delegate void DebugEvent();
    public static event DebugEvent matchFinderLoadTriggerDebug;
    public static void InvokeLoadEvent() => matchFinderLoadTrigger?.Invoke();
    public static void InvokeDebugLoadEvent() => matchFinderLoadTriggerDebug?.Invoke();
    
    public TextMeshProUGUI RowString;
   public TextMeshProUGUI DiagonalString;
   public TextMeshProUGUI Pattern1String;
   public TextMeshProUGUI Pattern2String;
   public TextMeshProUGUI RowValueDisplay;
   public TextMeshProUGUI DiagonalValueDisplay;
   public TextMeshProUGUI Pattern1ValueDisplay;
   public TextMeshProUGUI Pattern2ValueDisplay;
   [SerializeField] private Slider rowSlider;
   [SerializeField] private Slider diagonalSlider;
   [SerializeField] private Slider pattern1Slider;
   [SerializeField] private Slider pattern2Slider;


   [SerializeField] private AdminLevelSettingsMatchFinderAcceptSwitch rowSwitch;
   [SerializeField] private AdminLevelSettingsMatchFinderAcceptSwitch diagonalSwitch;
   [SerializeField] private AdminLevelSettingsMatchFinderAcceptSwitch pattern1Switch;
   [SerializeField] private AdminLevelSettingsMatchFinderAcceptSwitch pattern2Switch;
   private List<TextMeshProUGUI> _valueDisplayList = new List<TextMeshProUGUI>(); 

   public void ClickNext(IsMatchStyle isMatchStyle, ref Row row, ref Diagonal diagonal, ref Pattern pattern)
    { 
        Rl.GameManager.PlayAudio(Rl.soundStrings.NextRowSound, Random.Range(2, 4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        LoadIsMatchStyle(isMatchStyle, ref row, ref diagonal, ref  pattern, true);
    }

    private int CheckIfValueIsNotViable(IsMatchStyle isMatchStyle, Pattern pattern, int newValue)
    {
        if (newValue == 0) return newValue;
        
        int skipToValue = 0;
        switch (isMatchStyle)
        {
            case IsMatchStyle.IsRow or IsMatchStyle.IsDiagonal:
                if (newValue != 1) return newValue;
                 skipToValue = 2;
                break;
            
            case IsMatchStyle.IsPattern1 or IsMatchStyle.IsPattern2:

                switch (pattern)
                {
                    
                    case  Pattern.Block or Pattern.L_Pattern or Pattern.T_Pattern or Pattern.X_Pattern: 
                        if (newValue > 3) return newValue;
                        if (newValue > 0) skipToValue = 4;
                        break;
                    case Pattern.Corners: 
                        skipToValue = 4;
                        break;
                    case Pattern.Cross: 
                        if (newValue > 4) return newValue;
                        if (newValue > 0) skipToValue = 5;
                        break;
                }
                break;
        }

        return skipToValue;
    }
    private void RemoveListeners() => GenericSettingsFunctions.RemoveListeners(rowSlider, diagonalSlider, pattern1Slider, pattern2Slider);
    
    private void Addlisteners() => GenericSettingsFunctions.Addlisteners(delegate { ValueChangeCheck(); }, rowSlider, diagonalSlider, pattern1Slider, pattern2Slider);

    private void LoadMatchFinderConfigToClipBoard(MatchFinderConfig matchFinderConfig)
    {
        Rl.saveClipBoard.RowMatchValue = matchFinderConfig.RowValue;
        Rl.saveClipBoard.DiagonalMatchValue = matchFinderConfig.DiagonalValue;
        Rl.saveClipBoard.Pattern1MatchValue = matchFinderConfig.Pattern1Value;
        Rl.saveClipBoard.Pattern2MatchValue = matchFinderConfig.Pattern2Value;

        Rl.saveClipBoard.Row = matchFinderConfig.Row;
        Rl.saveClipBoard.Diagonal = matchFinderConfig.Diagonal;
        Rl.saveClipBoard.Pattern1 = matchFinderConfig.Pattern1;
        Rl.saveClipBoard.Pattern2 = matchFinderConfig.Pattern2;

        Rl.saveClipBoard.RowPhase = matchFinderConfig.RowPhase;
        Rl.saveClipBoard.DiagonalPhase = matchFinderConfig.DiagonalPhase;
        Rl.saveClipBoard.Pattern1Phase = matchFinderConfig.Pattern1Phase;
        Rl.saveClipBoard.Pattern2Phase = matchFinderConfig.Pattern2Phase;

       // InvokeDebugLoadEvent();
        InvokeLoadEvent();
    }

    public void ClickLoadRowValue(Transform transform)
    { 
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        int level = Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber;
      Rl.saveClipBoard.RowMatchValue =  Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].matchFinderConfig.RowValue;
      rowSlider.value = Rl.saveClipBoard.RowMatchValue;

      Rl.saveClipBoard.Row = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
          .matchFinderConfig.Row;
      
      
      Rl.saveClipBoard.RowPhase = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
          .matchFinderConfig.RowPhase;
      
      
      RowString.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Row.ToString());
      rowSwitch.ClickOnSwitch(false);
     GenericSettingsFunctions.SmallJumpAnimation(transform);
    }
    
    public void ClickLoadDiagonalValue(Transform transform)
    { 
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        int level = Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber;
        Rl.saveClipBoard.DiagonalMatchValue =  Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].matchFinderConfig.DiagonalValue;
        diagonalSlider.value = Rl.saveClipBoard.DiagonalMatchValue;
        
        Rl.saveClipBoard.Diagonal = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
            .matchFinderConfig.Diagonal;

        Rl.saveClipBoard.DiagonalPhase = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
            .matchFinderConfig.DiagonalPhase;
        
        DiagonalString.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Diagonal.ToString());
        diagonalSwitch.ClickOnSwitch(false);
        GenericSettingsFunctions.SmallJumpAnimation(transform);
    }
    
    public void ClickLoadPattern1Value(Transform transform)
    { 
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        int level = Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber;
        Rl.saveClipBoard.Pattern1MatchValue =  Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].matchFinderConfig.Pattern1Value;
        pattern1Slider.value = Rl.saveClipBoard.Pattern1MatchValue;
        
        Rl.saveClipBoard.Pattern1 = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
            .matchFinderConfig.Pattern1;
      
        Rl.saveClipBoard.Pattern1Phase = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
            .matchFinderConfig.Pattern1Phase;
        
        Pattern1String.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Pattern1.ToString());
        pattern1Switch.ClickOnSwitch(false);
        GenericSettingsFunctions.SmallJumpAnimation(transform);
    }
    
    public void ClickLoadPattern2Value(Transform transform)
    { 
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        int level = Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber;
        Rl.saveClipBoard.Pattern2MatchValue =  Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].matchFinderConfig.Pattern2Value;
        pattern2Slider.value = Rl.saveClipBoard.Pattern2MatchValue;
        
        Rl.saveClipBoard.Pattern2 = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
            .matchFinderConfig.Pattern2;
        
        Pattern2String.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Pattern2.ToString());

        Rl.saveClipBoard.Pattern2Phase = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
            .matchFinderConfig.Pattern2Phase;
        pattern2Switch.ClickOnSwitch(false);
        GenericSettingsFunctions.SmallJumpAnimation(transform);
    }

    private void InitStringValues()
    {
        RowString.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Row.ToString());
        DiagonalString.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Diagonal.ToString());
        Pattern1String.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Pattern1.ToString());
        Pattern2String.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Pattern2.ToString());
    }
    public MatchFinderConfig SaveMatchFinderSettings()
    {
        return new MatchFinderConfig(
            Rl.saveClipBoard.Row, Rl.saveClipBoard.RowMatchValue, Rl.saveClipBoard.RowPhase,
            Rl.saveClipBoard.Diagonal, Rl.saveClipBoard.DiagonalMatchValue, Rl.saveClipBoard.DiagonalPhase,
            Rl.saveClipBoard.Pattern1, Rl.saveClipBoard.Pattern1MatchValue, Rl.saveClipBoard.Pattern1Phase,
            Rl.saveClipBoard.Pattern2, Rl.saveClipBoard.Pattern2MatchValue, Rl.saveClipBoard.Pattern2Phase
        );
    }
    public void LoadMatchFinderSettings(MatchFinderConfig matchfinderConfig)
    {
        RemoveListeners();
        LoadMatchFinderConfigToClipBoard(matchfinderConfig);
        ClipBoardToSlider();
        Addlisteners();
        ValueChangeCheck();
        InitStringValues();
        //playNoSoundAcceptSwitch = true;
        // SetP1P2(Rl.saveClipBoard.P1P2);
        // playNoSoundAcceptSwitch = false;
        //  LoadGameType(true, false);
        //  LoadGameType(false, false);
    }
    private void Awake() => _valueDisplayList = GenericSettingsFunctions.AddToValueDisplayList(RowValueDisplay,
        DiagonalValueDisplay, Pattern1ValueDisplay, Pattern2ValueDisplay);
    
    private void ClipBoardToSlider()
    {
        rowSlider.value =    Rl.saveClipBoard.RowMatchValue;
      diagonalSlider.value = Rl.saveClipBoard.DiagonalMatchValue;
      pattern1Slider.value =  Rl.saveClipBoard.Pattern1MatchValue;
      pattern2Slider.value =  Rl.saveClipBoard.Pattern2MatchValue;
    }
    public void ValueChangeCheck()
    {
        Rl.saveClipBoard.RowMatchValue =  (uint)CheckIfValueIsNotViable(IsMatchStyle.IsRow, Pattern.Block, (int)rowSlider.value);
        Rl.saveClipBoard.DiagonalMatchValue = (uint)CheckIfValueIsNotViable(IsMatchStyle.IsDiagonal, Pattern.Block, (int)diagonalSlider.value);
        Rl.saveClipBoard.Pattern1MatchValue = (uint)CheckIfValueIsNotViable(IsMatchStyle.IsPattern1, Rl.saveClipBoard.Pattern1, (int)pattern1Slider.value);
        Rl.saveClipBoard.Pattern2MatchValue = (uint)CheckIfValueIsNotViable(IsMatchStyle.IsPattern2, Rl.saveClipBoard.Pattern2, (int)pattern2Slider.value);

        
        ClipBoardToSlider();
        GenericSettingsFunctions.UpdateTextFields(ref _valueDisplayList, rowSlider, diagonalSlider, pattern1Slider, pattern2Slider);
    }
    private void LoadIsMatchStyle(IsMatchStyle isMatchStyle, ref Row row, ref Diagonal diagonal, ref Pattern pattern, bool next)
    {
        switch (isMatchStyle)
        {
            case IsMatchStyle.IsRow:
                if (next) row = NextRow((int)Rl.saveClipBoard.Row);
                else row = Rl.saveClipBoard.Row;
                Rl.saveClipBoard.Row = row;
                RowString.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Row.ToString());
                break;
            case IsMatchStyle.IsDiagonal:
                if (next) diagonal = NextDiagonal((int)Rl.saveClipBoard.Diagonal);
                else diagonal = Rl.saveClipBoard.Diagonal;
                Rl.saveClipBoard.Diagonal = diagonal;
                DiagonalString.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Diagonal.ToString());
                break;
            case IsMatchStyle.IsPattern1:
                if (next) pattern = NextPattern((int)Rl.saveClipBoard.Pattern1);
                else pattern = Rl.saveClipBoard.Pattern1;
                Rl.saveClipBoard.Pattern1 = pattern;
                Pattern1String.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Pattern1.ToString());
                break;
            case IsMatchStyle.IsPattern2:
                if (next) pattern = NextPattern((int)Rl.saveClipBoard.Pattern2);
                else pattern = Rl.saveClipBoard.Pattern2;
                Rl.saveClipBoard.Pattern2 = pattern;
                Pattern2String.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Pattern2.ToString());
                break;
        }

        ValueChangeCheck();
    }
    
private Row NextRow(int row)
    {
       row += 1;
       Row rowEnum = Row.Both;
        if (row > enumCountRow() - 1) row = 0;

        int counter = 0;
        foreach (Row searchForRow in Enum.GetValues(typeof(Row)))
        {
            if (row == counter) 
                rowEnum = searchForRow;
            counter++;
        }
        return rowEnum;
    }
    
    private Diagonal NextDiagonal(int diagonal)
    {
        diagonal += 1;
        Diagonal diagonalEnum = Diagonal.Both;
        if (diagonal > enumCountDiagonal() - 1) diagonal = 0;

        int counter = 0;
        foreach (Diagonal searchForDiagonal in Enum.GetValues(typeof(Diagonal)))
        {
            if (diagonal == counter) diagonalEnum = searchForDiagonal;
            counter++;
        }
        return diagonalEnum;
    }
    
    private Pattern NextPattern(int pattern)
    {
        pattern += 1;
        Pattern patternEnum = Pattern.Block;
        if (pattern > enumCountPattern() - 1) pattern = 0;

        int counter = 0;
        foreach (Pattern searchForPattern in Enum.GetValues(typeof(Pattern)))
        {
            if (pattern == counter) patternEnum = searchForPattern;
            counter++;
        }
        return patternEnum;
    }

    private static int enumCountRow()
    {
        int counter = 0;
        foreach (Row doesNotMatterAtAll in Enum.GetValues(typeof(Row))) counter++;
        Debug.Log(counter);
        return counter;
    }
    private static int enumCountDiagonal()
    {
        int counter = 0;
        foreach (Diagonal doesNotMatterAtAll in Enum.GetValues(typeof(Diagonal))) counter++;
        
   
        return counter;
    }
    private static int enumCountPattern()
    {
        int counter = 0;
        foreach (Pattern doesNotMatterAtAll in Enum.GetValues(typeof(Pattern))) counter++;
        return counter;
    }

    // public void AvoidOnlyInP2(Phase phase)
    // {
    //     if(phase = phase == Phase.OnlyInP2
    // }
    
    private string StringRow(Row row) => Enum.GetName(typeof(Row), (int)row);
    private string StringDiagonal(Diagonal diagonal) => Enum.GetName(typeof(Diagonal), (int)diagonal);
    private string StringPattern(Pattern pattern) => Enum.GetName(typeof(Pattern), (int)pattern);
}

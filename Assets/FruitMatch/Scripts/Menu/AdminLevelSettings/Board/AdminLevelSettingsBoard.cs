using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AdminLevelSettingsBoard : MonoBehaviour
{
    [SerializeField] public Slider BoardWidthSlider;
    [SerializeField] public Slider BoardHeightSlider;
    [SerializeField] public Slider P1CounterValueSlider;
    [SerializeField] public Slider TipDelaySlider;
    [SerializeField] private TextMeshProUGUI BoardWidthText;
    [SerializeField] private TextMeshProUGUI BoardHeightText;
    [SerializeField] private TextMeshProUGUI P1CounterValueText;
    [SerializeField] private TextMeshProUGUI TipDelayText;
    [Header("Graphic Button")]
    [SerializeField] private GameObject SwitchGraphicOn;
    [SerializeField] private GameObject SwitchGraphicOff;
    
    [SerializeField] private GameObject SwitchGraphicBackgroundOn;
    [SerializeField] private GameObject SwitchGraphicBackgroundOff;
    
    private List<TextMeshProUGUI> _valueDisplayList;
    
    [SerializeField] private TextMeshProUGUI gameTypeP1Button;
    [SerializeField] private Switch AllowTipSwitch;
    [SerializeField] private Switch DestroyTargetOnlySwitch;
    [SerializeField] private Switch AddToLastField;
    [SerializeField] private TextMeshProUGUI AddTolastFieldText;
    private void Awake() => _valueDisplayList = GenericSettingsFunctions.AddToValueDisplayList(BoardWidthText, BoardHeightText, P1CounterValueText, TipDelayText);

    private void RemoveListeners() => GenericSettingsFunctions.RemoveListeners(BoardHeightSlider, BoardWidthSlider, P1CounterValueSlider, TipDelaySlider);
private void AddListener() => GenericSettingsFunctions.Addlisteners(delegate { ValueChangeCheck(); }, BoardHeightSlider, BoardWidthSlider, P1CounterValueSlider, TipDelaySlider);
    public void LoadBoardSettings(BoardDimensionsConfig boardDimensionsConfig)
    {
       
        RemoveListeners();
        CheckArrays();
        Rl.BoardPreview.RemoveBoardPreviewListeners();
        CheckLoadingArrays(ref boardDimensionsConfig);
        LoadBoardSettingsToClipBoard(boardDimensionsConfig);
        ClipBoardToSlider();
        AddListener();
        ValueChangeCheck();
        LoadGameType(true, false);
        LoadGameType(false, false);
        EnableDisableExtraSwitches();
    }

    private void CheckArrays()
    {
        Array.Resize(ref Rl.saveClipBoard.BoardWidth,SaveFileLevelConfigs.Fields);
            Array.Resize(ref Rl.saveClipBoard.BoardHeight,SaveFileLevelConfigs.Fields);
            Array.Resize(ref Rl.saveClipBoard.NoMatches,SaveFileLevelConfigs.Fields);
            
            Array.Resize(ref Rl.saveClipBoard.DestroyTargetOnly,SaveFileLevelConfigs.Fields);
            Array.Resize(ref Rl.saveClipBoard.AddToLastField, SaveFileLevelConfigs.Fields);
            Array.Resize(ref Rl.saveClipBoard.TipDelay,SaveFileLevelConfigs.Fields);
            Array.Resize(ref Rl.saveClipBoard.GameTypeP1,SaveFileLevelConfigs.Fields);
    }

    private void CheckLoadingArrays(ref BoardDimensionsConfig boardDimensionsConfig)
    {
        boardDimensionsConfig.Width ??= new int[SaveFileLevelConfigs.Fields];
        boardDimensionsConfig.Height ??= new int[SaveFileLevelConfigs.Fields];
        boardDimensionsConfig.NoMatches ??= new bool[SaveFileLevelConfigs.Fields];
        boardDimensionsConfig.GameTypeP1 ??= new EndGameRequirements[SaveFileLevelConfigs.Fields];
        boardDimensionsConfig.TipDelay ??= new int[SaveFileLevelConfigs.Fields];
        boardDimensionsConfig.AllowTip ??= new bool[SaveFileLevelConfigs.Fields];
        boardDimensionsConfig.DestroyOnlyTarget ??= new bool[SaveFileLevelConfigs.Fields];
        
    }

    private void ClipBoardToSlider()
    {
        CheckArrays();
        BoardWidthSlider.value =  Rl.saveClipBoard.BoardWidth[FieldState.CurrentField];
        BoardHeightSlider.value = Rl.saveClipBoard.BoardHeight[FieldState.CurrentField];
        P1CounterValueSlider.value = Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].CounterValue;
        TipDelaySlider.value = Rl.saveClipBoard.TipDelay[FieldState.CurrentField];

        if (Rl.saveClipBoard.NoMatches[FieldState.CurrentField]) ActivateSwitch(true);
        else DeactivateSwitch(true);

        ClickAllowTipSwitch(Rl.saveClipBoard.AllowTip[FieldState.CurrentField], true, false);
        ClickDestroyTargetSwitch(Rl.saveClipBoard.DestroyTargetOnly[FieldState.CurrentField], true, false);
        ClickAddToLastFieldSwitch(Rl.saveClipBoard.AddToLastField[FieldState.CurrentField], true, false);
    }

    public void ActivateSwitch(bool playNoSound)
    {
        if(!playNoSound)  Rl.GameManager.PlayAudio(Rl.soundStrings.SwitchButtonSound, Random.Range(2, 5), Rl.settings.GetUISoundVolume,
            Rl.uiSounds.audioSource);
        
        SwitchThings(true);
    }
    public void DeactivateSwitch(bool playNoSound)
    {
        if(!playNoSound)  Rl.GameManager.PlayAudio(Rl.soundStrings.SwitchButtonSound, Random.Range(0, 2), Rl.settings.GetUISoundVolume,
            Rl.uiSounds.audioSource);
        
        SwitchThings(false);
    }

    public void ClickOnAllowTipSwitch(bool on) => ClickAllowTipSwitch(on, false, true);

    private void ClickAllowTipSwitch(bool on, bool playNoSound, bool animation)
    {
        AllowTipSwitch.SwitchButton(on, playNoSound, animation);
        Rl.saveClipBoard.AllowTip[FieldState.CurrentField] = on;
    }
    
    public void ClickOnDestroyTargetSwitch(bool on) => ClickDestroyTargetSwitch(on, false, true);

    private void ClickDestroyTargetSwitch(bool on, bool playNoSound, bool animation)
    {
        DestroyTargetOnlySwitch.SwitchButton(on, playNoSound, animation);
        Rl.saveClipBoard.DestroyTargetOnly[FieldState.CurrentField] = on;
    }
    public void ClickOnAddToLastFieldSwitch(bool on) => ClickAddToLastFieldSwitch(on, false, true);

    private void ClickAddToLastFieldSwitch(bool on, bool playNoSound, bool animation)
    {
        AddToLastField.SwitchButton(on, playNoSound, animation);
        Rl.saveClipBoard.AddToLastField[FieldState.CurrentField] = on;
    }
    private void SwitchThings(bool active)
    {
        SwitchGraphicOn.SetActive(active);
        SwitchGraphicBackgroundOn.SetActive(active);
        
        SwitchGraphicOff.SetActive(!active);
        SwitchGraphicBackgroundOff.SetActive(!active);
        
        Rl.saveClipBoard.NoMatches[FieldState.CurrentField] = active;
    }

    private static void LoadBoardSettingsToClipBoard(BoardDimensionsConfig boardDimensionsConfig)
    {
        Rl.saveClipBoard.BoardWidth = (int[])GenericSettingsFunctions.GetDeepCopy(boardDimensionsConfig.Width);
        Rl.saveClipBoard.BoardHeight = (int[])GenericSettingsFunctions.GetDeepCopy(boardDimensionsConfig.Height);
        Rl.saveClipBoard.NoMatches = (bool[])GenericSettingsFunctions.GetDeepCopy(boardDimensionsConfig.NoMatches); 
        Rl.saveClipBoard.AllowTip = (bool[])GenericSettingsFunctions.GetDeepCopy(boardDimensionsConfig.AllowTip);
        Rl.saveClipBoard.GameTypeP1 = (EndGameRequirements[])GenericSettingsFunctions.GetDeepCopy(boardDimensionsConfig.GameTypeP1);
        Rl.saveClipBoard.TipDelay = (int[])GenericSettingsFunctions.GetDeepCopy(boardDimensionsConfig.TipDelay);
        Rl.saveClipBoard.DestroyTargetOnly = (bool[])GenericSettingsFunctions.GetDeepCopy(boardDimensionsConfig.DestroyOnlyTarget);
        Rl.saveClipBoard.AddToLastField= (bool[])GenericSettingsFunctions.GetDeepCopy(boardDimensionsConfig.AddToLastField);
    }

    private void ValueChangeCheck()
    {
        Rl.saveClipBoard.BoardHeight[FieldState.CurrentField] = (int)BoardHeightSlider.value;
        Rl.saveClipBoard.BoardWidth[FieldState.CurrentField] = (int)BoardWidthSlider.value;
        
        Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].CounterValue = P1CounterValueSlider.value;
        Rl.saveClipBoard.TipDelay[FieldState.CurrentField] = (int)TipDelaySlider.value;
        
        
        GenericSettingsFunctions.UpdateTextFields(
            ref _valueDisplayList,
            ((int)BoardHeightSlider.value).ToString(),
            ((int)BoardWidthSlider.value).ToString(),
            GenericSettingsFunctions.TranslateTimeConstValues(
                GenericSettingsFunctions.GetConstvaluesMovesTime(
                    Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].CounterValue,
                    Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].GameType),
                Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].GameType),
            ((int)TipDelaySlider.value).ToString());
        Rl.BoardPreview.StartDrawBoard();
    }

    public BoardDimensionsConfig SaveBoardsSettingsFromClipBoard() => new
    (
        Rl.saveClipBoard.BoardWidth,
        Rl.saveClipBoard.BoardHeight,
        Rl.saveClipBoard.NoMatches,
        Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs
            .LevelConfigs[Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber].BoardDimensionsConfig
            .FruitsConfigParent,
        Rl.saveClipBoard.GameTypeP1,
        Rl.saveClipBoard.AllowTip,
        Rl.saveClipBoard.TipDelay,
        Rl.saveClipBoard.DestroyTargetOnly,
        Rl.saveClipBoard.AddToLastField
    );

    public void LoadOldValueButton(bool width)
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound, Random.Range(0, 4), Rl.settings.GetUISoundVolume,
            Rl.uiSounds.audioSource);

        int level = Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber;

        if (width)
        {
            Rl.saveClipBoard.BoardWidth[FieldState.CurrentField]= Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
                .BoardDimensionsConfig.Width[FieldState.CurrentField];
            BoardWidthSlider.value = Rl.saveClipBoard.BoardWidth[FieldState.CurrentField];
        }
        else
        {
            Rl.saveClipBoard.BoardHeight[FieldState.CurrentField] = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BoardDimensionsConfig.Height[FieldState.CurrentField];
            BoardHeightSlider.value = Rl.saveClipBoard.BoardHeight[FieldState.CurrentField];
        }
    }
    
    public void ClickNextGameType(bool p1)
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.NextRowSound, Random.Range(2,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        LoadGameType(p1, true);
    }
    
    private void LoadGameType(bool p1, bool nextGameType)
    {
        GameType gameType = GameType.Nothing;
        switch (p1)
        {
            case true:
                if (nextGameType) gameType = NextGameType((int)Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].GameType);
                else gameType = Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].GameType;
                gameTypeP1Button.text = LocalisationSystem.GetLocalisedString(StringMatchStyle(gameType));
                Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].GameType= gameType;
                break;
            
            case false:
                break;
        }
        ValueChangeCheck();
        EnableDisableExtraSwitches();
    }

    private GameType NextGameType(int gameType)
    {
        gameType += 1;
        GameType gameTypeEnum = GameType.Nothing;
        if (gameType > EnumCountMatchStyle() - 1) gameType = 0;

        int counter = 0;
        foreach (GameType searchForGameType in Enum.GetValues(typeof(GameType)))
        {
            if (gameType == counter) gameTypeEnum = searchForGameType;
            counter++;
        }
        
        return gameTypeEnum;
    }
    
    private static int EnumCountMatchStyle()
    {
        int counter = 0;
        foreach (GameType doesNotMatterAtAll in Enum.GetValues(typeof(GameType)))
            counter++;
         
        return counter;
    }
    private string StringMatchStyle(GameType gameType) => Enum.GetName(typeof(GameType), (int)gameType);

    private void EnableDisableExtraSwitches()
    {
        if (FieldState.CurrentField == 0 || Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].GameType == GameType.Nothing || Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].GameType !=  Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField-1].GameType)
        {
            AddToLastField.gameObject.SetActive(false);
            AddTolastFieldText.gameObject.SetActive(false);
        }
        else if( (Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].GameType ==  Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField-1].GameType) && Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].GameType != GameType.Nothing)
        {
            AddToLastField.gameObject.SetActive(true);
            AddTolastFieldText.gameObject.SetActive(true);
        }
    }
   
    public void LoadCurrentField()
    {
        RemoveListeners();
        ClipBoardToSlider();
        ValueChangeCheck();
        Rl.BoardPreview.ResetLastClicked();
        Rl.BoardPreview.StartDrawBoard();
        LoadGameType(true, false);
        AddListener();
        EnableDisableExtraSwitches();
    }
}

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
    [SerializeField] public Slider P2CounterValueSlider;
    [SerializeField] private TextMeshProUGUI BoardWidthText;
    [SerializeField] private TextMeshProUGUI BoardHeightText;
    [SerializeField] private TextMeshProUGUI P1CounterValueText;
    [SerializeField] private TextMeshProUGUI P2CounterValueText;

    [Header("Graphic Button")]
    [SerializeField] private GameObject SwitchGraphicOn;
    [SerializeField] private GameObject SwitchGraphicOff;
    
    [SerializeField] private GameObject SwitchGraphicBackgroundOn;
    [SerializeField] private GameObject SwitchGraphicBackgroundOff;

    [SerializeField] private GameObject P1;
    [SerializeField] private GameObject P1P2;
    private List<TextMeshProUGUI> _valueDisplayList;
    
    [SerializeField] private TextMeshProUGUI gameTypeP1Button;
    [SerializeField] private TextMeshProUGUI gameTypeP2Button;

    private bool playNoSoundAcceptSwitch = true;
    
    private void Awake()
    {
        _valueDisplayList = GenericSettingsFunctions.AddToValueDisplayList(BoardWidthText, BoardHeightText,
            P1CounterValueText, P2CounterValueText);
    }

    public void SetP1P2(bool p1)
    {
        P1.SetActive(!p1);
        P1P2.SetActive(p1);
        Rl.saveClipBoard.P1P2[FieldState.CurrentField] = p1;
        if(!playNoSoundAcceptSwitch)  Rl.GameManager.PlayAudio(Rl.soundStrings.AcceptSwitchSound, Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    }

    private void RemoveListeners() => GenericSettingsFunctions.RemoveListeners(BoardHeightSlider, BoardWidthSlider, P1CounterValueSlider, P2CounterValueSlider);
private void AddListener() => GenericSettingsFunctions.Addlisteners(delegate { ValueChangeCheck(); }, BoardHeightSlider, BoardWidthSlider, P1CounterValueSlider, P2CounterValueSlider);
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
        playNoSoundAcceptSwitch = true;
        SetP1P2(Rl.saveClipBoard.P1P2[FieldState.CurrentField]);
        playNoSoundAcceptSwitch = false;
        LoadGameType(true, false);
        LoadGameType(false, false);
    }

    public void CheckArrays()
    {
        // if (Rl.saveClipBoard.BoardWidth.Length < 4 || Rl.saveClipBoard.BoardHeight.Length < 4 ||
        //     Rl.saveClipBoard.GameTypeP1.Length < 4 || Rl.saveClipBoard.GameTypeP2.Length < 4 ||
        //     Rl.saveClipBoard.NoMatches.Length < 4 || Rl.saveClipBoard.P1P2.Length < 4)
        // {
            Array.Resize(ref Rl.saveClipBoard.BoardWidth,4);
            Array.Resize(ref Rl.saveClipBoard.BoardHeight,4);
            Array.Resize(ref Rl.saveClipBoard.NoMatches,4);
            
            Array.Resize(ref Rl.saveClipBoard.P1P2,4);
            Array.Resize(ref Rl.saveClipBoard.GameTypeP2,4);
            Array.Resize(ref Rl.saveClipBoard.GameTypeP1,4);
            
      //  }
    }

    public void CheckLoadingArrays(ref BoardDimensionsConfig boardDimensionsConfig)
    {
        boardDimensionsConfig.Width ??= new int[4];
        boardDimensionsConfig.Height ??= new int[4];
        boardDimensionsConfig.NoMatches ??= new bool[4];
        
        boardDimensionsConfig.P1P2 ??= new bool[4];
        boardDimensionsConfig.GameTypeP1 ??= new EndGameRequirements[4];
        boardDimensionsConfig.GameTypeP2 ??= new EndGameRequirements[4];
    }
    public void ClipBoardToSlider()
    {
        CheckArrays();
        BoardWidthSlider.value =  Rl.saveClipBoard.BoardWidth[FieldState.CurrentField];
        BoardHeightSlider.value = Rl.saveClipBoard.BoardHeight[FieldState.CurrentField];
        P1CounterValueSlider.value = Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].CounterValue;
        P2CounterValueSlider.value = Rl.saveClipBoard.GameTypeP2[FieldState.CurrentField].CounterValue;

        if (Rl.saveClipBoard.NoMatches[FieldState.CurrentField]) ActivateSwitch(true);
        else DeactivateSwitch(true);
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

    private void SwitchThings(bool active)
    {
        SwitchGraphicOn.SetActive(active);
        SwitchGraphicBackgroundOn.SetActive(active);
        
        SwitchGraphicOff.SetActive(!active);
        SwitchGraphicBackgroundOff.SetActive(!active);
        
        Rl.saveClipBoard.NoMatches[FieldState.CurrentField] = active;
    }
    public void LoadBoardSettingsToClipBoard(BoardDimensionsConfig boardDimensionsConfig)
    {
        Rl.saveClipBoard.BoardWidth = (int[])GenericSettingsFunctions.GetDeepCopy(boardDimensionsConfig.Width);
        Rl.saveClipBoard.BoardHeight = (int[])GenericSettingsFunctions.GetDeepCopy(boardDimensionsConfig.Height);
        Rl.saveClipBoard.NoMatches = (bool[])GenericSettingsFunctions.GetDeepCopy(boardDimensionsConfig.NoMatches); 
        Rl.saveClipBoard.P1P2 = (bool[])GenericSettingsFunctions.GetDeepCopy(boardDimensionsConfig.P1P2);
        Rl.saveClipBoard.GameTypeP1 = (EndGameRequirements[])GenericSettingsFunctions.GetDeepCopy(boardDimensionsConfig.GameTypeP1);
        Rl.saveClipBoard.GameTypeP2 = (EndGameRequirements[])GenericSettingsFunctions.GetDeepCopy(boardDimensionsConfig.GameTypeP2);
    }

    public void ValueChangeCheck()
    {
        Rl.saveClipBoard.BoardHeight[FieldState.CurrentField] = (int)BoardHeightSlider.value;
        Rl.saveClipBoard.BoardWidth[FieldState.CurrentField] = (int)BoardWidthSlider.value;
        
        Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].CounterValue = P1CounterValueSlider.value;
        Rl.saveClipBoard.GameTypeP2[FieldState.CurrentField].CounterValue = P2CounterValueSlider.value;
        
        GenericSettingsFunctions.UpdateTextFields(
            ref _valueDisplayList, 
            ((int)BoardHeightSlider.value).ToString(),
            ((int)BoardWidthSlider.value).ToString(),
            GenericSettingsFunctions.TranslateTimeConstValues(GenericSettingsFunctions.GetConstvaluesMovesTime(Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].CounterValue, Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].GameType),Rl.saveClipBoard.GameTypeP1[FieldState.CurrentField].GameType),
            GenericSettingsFunctions.TranslateTimeConstValues(GenericSettingsFunctions.GetConstvaluesMovesTime(Rl.saveClipBoard.GameTypeP2[FieldState.CurrentField].CounterValue, Rl.saveClipBoard.GameTypeP2[FieldState.CurrentField].GameType),Rl.saveClipBoard.GameTypeP2[FieldState.CurrentField].GameType));
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
        Rl.saveClipBoard.P1P2,
        Rl.saveClipBoard.GameTypeP1,
        Rl.saveClipBoard.GameTypeP2
    );

    public GraphicConfig SaveBorderGraphicSetting()
    {
        return new GraphicConfig(
            Rl.saveClipBoard.BorderGraphic[FieldState.CurrentField],
        Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs
            .LevelConfigs[Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber].GraphicConfig.LevelCategory,
        Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs
            .LevelConfigs[Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber].GraphicConfig.BGName,
        Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs
            .LevelConfigs[Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber].GraphicConfig.WholeCategory
            );
    }


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
                if (nextGameType) gameType = NextGameType((int)Rl.saveClipBoard.GameTypeP2[FieldState.CurrentField].GameType);
                else gameType = Rl.saveClipBoard.GameTypeP2[FieldState.CurrentField].GameType;
                gameTypeP2Button.text = LocalisationSystem.GetLocalisedString(StringMatchStyle(gameType));
                Rl.saveClipBoard.GameTypeP2[FieldState.CurrentField].GameType = gameType;
                break;
        }
        ValueChangeCheck();
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
    
    
    // public void LoadCurrentField()
    // {
    //     RemoveListeners();
    //
    //     Addlisteners();
    // }
    public void LoadCurrentField()
    {
        RemoveListeners();
        ClipBoardToSlider();
        ValueChangeCheck();
        Rl.BoardPreview.ResetLastClicked();
        Rl.BoardPreview.StartDrawBoard();
        AddListener();
    }
}

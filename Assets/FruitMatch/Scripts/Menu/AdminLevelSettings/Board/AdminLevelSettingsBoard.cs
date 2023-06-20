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
        Rl.saveClipBoard.P1P2 = p1;
        if(!playNoSoundAcceptSwitch)  Rl.GameManager.PlayAudio(Rl.soundStrings.AcceptSwitchSound, Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    }
    
    public void LoadBoardSettings(BoardDimensionsConfig boardDimensionsConfig)
    {
        GenericSettingsFunctions.RemoveListeners(BoardHeightSlider, BoardWidthSlider, P1CounterValueSlider, P2CounterValueSlider);
        Rl.BoardPreview.RemoveBoardPreviewListeners();
        LoadBoardSettingsToClipBoard(boardDimensionsConfig);
        ClipBoardToSlider(); 
        GenericSettingsFunctions.Addlisteners(delegate { ValueChangeCheck(); }, BoardHeightSlider, BoardWidthSlider, P1CounterValueSlider, P2CounterValueSlider);
        ValueChangeCheck();
        playNoSoundAcceptSwitch = true;
        SetP1P2(Rl.saveClipBoard.P1P2);
        playNoSoundAcceptSwitch = false;
        LoadGameType(true, false);
        LoadGameType(false, false);
    }
    
    
    public void ClipBoardToSlider()
    {
        BoardWidthSlider.value =  Rl.saveClipBoard.BoardWidth;
        BoardHeightSlider.value = Rl.saveClipBoard.BoardHeight;
        P1CounterValueSlider.value = Rl.saveClipBoard.GameTypeP1.CounterValue;
        P2CounterValueSlider.value = Rl.saveClipBoard.GameTypeP2.CounterValue;

        if (!Rl.saveClipBoard.BorderGraphic) ActivateSwitch(true);
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
        
        Rl.saveClipBoard.BorderGraphic = active;
    }
    public void LoadBoardSettingsToClipBoard(BoardDimensionsConfig boardDimensionsConfig)
    {
        Rl.saveClipBoard.BoardWidth = boardDimensionsConfig.Width;
        Rl.saveClipBoard.BoardHeight = boardDimensionsConfig.Height;
        Rl.saveClipBoard.BorderGraphic = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs
            .LevelConfigs[Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber].GraphicConfig
            .AllowBorderGraphic;
        Rl.saveClipBoard.P1P2 = boardDimensionsConfig.P1P2;
        Rl.saveClipBoard.GameTypeP1 = boardDimensionsConfig.GameTypeP1;
        Rl.saveClipBoard.GameTypeP2 = boardDimensionsConfig.GameTypeP2;
    }

    public void ValueChangeCheck()
    {
        Rl.saveClipBoard.BoardHeight = (int)BoardHeightSlider.value;
        Rl.saveClipBoard.BoardWidth = (int)BoardWidthSlider.value;
        
        Rl.saveClipBoard.GameTypeP1.CounterValue = P1CounterValueSlider.value;
        Rl.saveClipBoard.GameTypeP2.CounterValue = P2CounterValueSlider.value;
        
        GenericSettingsFunctions.UpdateTextFields(
            ref _valueDisplayList, 
            ((int)BoardHeightSlider.value).ToString(),
            ((int)BoardWidthSlider.value).ToString(),
            GenericSettingsFunctions.TranslateTimeConstValues(GenericSettingsFunctions.GetConstvaluesMovesTime(Rl.saveClipBoard.GameTypeP1.CounterValue, Rl.saveClipBoard.GameTypeP1.GameType),Rl.saveClipBoard.GameTypeP1.GameType),
            GenericSettingsFunctions.TranslateTimeConstValues(GenericSettingsFunctions.GetConstvaluesMovesTime(Rl.saveClipBoard.GameTypeP2.CounterValue, Rl.saveClipBoard.GameTypeP2.GameType),Rl.saveClipBoard.GameTypeP2.GameType));
        Rl.BoardPreview.StartDrawBoard();
    }

    public BoardDimensionsConfig SaveBoardsSettingsFromClipBoard() => new
    (
        Rl.saveClipBoard.BoardWidth,
        Rl.saveClipBoard.BoardHeight,
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
            Rl.saveClipBoard.BorderGraphic,
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
            Rl.saveClipBoard.BoardWidth= Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
                .BoardDimensionsConfig.Width;
            BoardWidthSlider.value = Rl.saveClipBoard.BoardWidth;
        }
        else
        {
            Rl.saveClipBoard.BoardHeight = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
                .BoardDimensionsConfig.Height;
            BoardHeightSlider.value = Rl.saveClipBoard.BoardHeight;
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
                if (nextGameType) gameType = NextGameType((int)Rl.saveClipBoard.GameTypeP1.GameType);
                else gameType = Rl.saveClipBoard.GameTypeP1.GameType;
                gameTypeP1Button.text = LocalisationSystem.GetLocalisedString(StringMatchStyle(gameType));
                Rl.saveClipBoard.GameTypeP1.GameType= gameType;
                break;
            
            case false:
                if (nextGameType) gameType = NextGameType((int)Rl.saveClipBoard.GameTypeP2.GameType);
                else gameType = Rl.saveClipBoard.GameTypeP2.GameType;
                gameTypeP2Button.text = LocalisationSystem.GetLocalisedString(StringMatchStyle(gameType));
                Rl.saveClipBoard.GameTypeP2.GameType = gameType;
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
    
}

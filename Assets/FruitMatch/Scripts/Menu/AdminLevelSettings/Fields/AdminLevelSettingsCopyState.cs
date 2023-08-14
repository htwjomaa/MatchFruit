
using System;
using UnityEngine;

public static class FieldState
{
    
    private static byte _currentField = 0;
    public static byte CurrentField
    {
        get => _currentField;
        set
        {
            _currentField = value;
            LoadCurrentSectionField();
        }
    }
    
    public static CopySection CurrentSection;
    
    public delegate void LoadFields();
    public static event LoadFields LoadFieldsEvent;

    public static void InvokeLoadFieldsEvent() => LoadFieldsEvent?.Invoke();


    private static void LoadCurrentSectionField()
    {
        switch (CurrentSection)
        {
            case CopySection.NotApplicable:
                break;
            case CopySection.BoardSettings:
                Rl.adminLevelSettingsBoard.LoadCurrentField();
                break;
            case CopySection.TileSettings:
                Rl.adminLevelSettingsTiles.LoadCurrentField();
                break;
            case CopySection.SideDotSettings:
                Rl.adminLevelSettingsSideDots.LoadCurrentField();
                break;
            case CopySection.FruitSettings:
                break;
            case CopySection.GoalSettings:
                break;
            case CopySection.BombSettings:
                break;
            case CopySection.MatchSettings:
                break;
            case CopySection.LuckSettings:
                Rl.adminLevelLuckSettings.LoadCurrentField();
                break;
            case CopySection.DisplaySettings:
                Rl.adminLevelSettingsLayout.LoadCurrentField();
                break;
            case CopySection.AnimationSettings:
                break;
            case CopySection.TutorialSettings:
                break;
            case CopySection.ConfigSettings:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static void CopyField(byte fieldOne, byte fieldTwo)
    {
        switch (CurrentSection)
        {
            case CopySection.NotApplicable:
                break;
            case CopySection.BoardSettings:
                Rl.adminLevelSettingsBoard.CopyField(fieldOne, fieldTwo);
                break;
            case CopySection.TileSettings:
                Rl.adminLevelSettingsTiles.CopyField(fieldOne, fieldTwo);
                break;
            case CopySection.SideDotSettings:
                Rl.adminLevelSettingsSideDots.CopyField(fieldOne, fieldTwo);
                break;
            case CopySection.FruitSettings:
                break;
            case CopySection.GoalSettings:
                break;
            case CopySection.BombSettings:
                break;
            case CopySection.MatchSettings:
                break;
            case CopySection.LuckSettings:
                Rl.adminLevelLuckSettings.CopyField(fieldOne, fieldTwo);
                break;
            case CopySection.DisplaySettings:
                Rl.adminLevelSettingsLayout.CopyField(fieldOne, fieldTwo);
                break;
            case CopySection.AnimationSettings:
                break;
            case CopySection.TutorialSettings:
                break;
            case CopySection.ConfigSettings:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        Rl.splashMenu.ChangeWorldText("Field Copied from:"," "+ (fieldOne+1) +" -> " + (fieldTwo+1), true);
    }

    public static void CopyAllFields(byte fieldOne)
    {
        Debug.Log("FIELD ONE IS " + fieldOne);
        for (byte i = 0; i < SaveFileLevelConfigs.Fields; i++)
        {
            if (i != fieldOne)
            {
                Rl.adminLevelSettingsBoard.CopyField(fieldOne, i);
                Rl.adminLevelSettingsTiles.CopyField(fieldOne, i);
                Rl.adminLevelSettingsSideDots.CopyField(fieldOne, i);
                Rl.adminLevelLuckSettings.CopyField(fieldOne, i);
                Rl.adminLevelSettingsLayout.CopyField(fieldOne, i);
            }
      
        }
    }
}

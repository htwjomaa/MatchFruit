using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class SwitchButtonsBombs : MonoBehaviour
{
    [Header("Bomb Swithes")] 
    [SerializeField] private Switch SwitchColorBomb;
    [SerializeField] private Switch SwitchFruitBomb;
    [FormerlySerializedAs("SwitchDiagonalBomb")] [SerializeField] private Switch SwitchSearchBomb;
    [SerializeField] private Switch SwitchVerticalBomb;
    [SerializeField] private Switch SwitchHorizontalBomb;
    [SerializeField] private Switch SwitchAllBombs;

    [Header("Bomb Slider")] 
    [SerializeField] public Slider SameColorBombSlider;
    [SerializeField] public Slider SameFruitBombSlider;
    [FormerlySerializedAs("DiagonalBombSlider")] [SerializeField] public Slider SearchBombSlider;
    [SerializeField] public Slider VertBombSlider;
    [SerializeField] public Slider HorBombSlider;
    
    [Header("Other Settings")]
    [SerializeField] private List<BombMatchStyleList> bombMatchStyleLists = new List<BombMatchStyleList>();
    public void SetSwitches()
    {
        if (GetBombSetting(Bomb.Color)) SwitchColorBombOn(true);
        else SwitchColorBombOff(true);

        if (GetBombSetting(Bomb.Package)) SwitchFruitBombOn(true);
        else SwitchFruitBombOff(true);

        if (GetBombSetting(Bomb.Vertical)) SwitchVerticalBombOn(true);
        else SwitchVerticalBombOff(true);
        
        if (GetBombSetting(Bomb.Horizontal)) SwitchHorizontalBombOn(true);
        else SwitchHorizontalBombOff(true);
        
        if (GetBombSetting(Bomb.Search)) SwitchSearchBombOn(true);
        else SwitchSearchBombOff(true);
        
        if (GetBombSetting(Bomb.AllBombs)) SwitchAllBombsOn(true);
        else SwitchAllBombsOff(true);
    }
    public void SwitchAllBombsOn (bool playNoSound)
    {
        SetBombSettingsToClipBoard(Bomb.AllBombs ,true);
        SwitchThings(Bomb.AllBombs,true, playNoSound);
    }
    public void SwitchAllBombsOff (bool playNoSound)
    {
        SetBombSettingsToClipBoard(Bomb.AllBombs ,false);
        SwitchThings(Bomb.AllBombs,false, playNoSound);
    }
    public void SwitchColorBombOn(bool playNoSound)
    {
        SetBombSettingsToClipBoard(Bomb.Color,true);
        SwitchThings(Bomb.Color,true, playNoSound);
    }
    public void SwitchColorBombOff(bool playNoSound)
    {
        SetBombSettingsToClipBoard(Bomb.Color ,false);
        SwitchThings(Bomb.Color,false, playNoSound);
    }
    public void SwitchFruitBombOn(bool playNoSound)
    {
        SetBombSettingsToClipBoard(Bomb.Package, true);
        SwitchThings(Bomb.Package,true, playNoSound);
    }
    public void SwitchFruitBombOff(bool playNoSound)
    {
        SetBombSettingsToClipBoard(Bomb.Package, false);
        SwitchThings(Bomb.Package,false, playNoSound);
    }
    public void SwitchVerticalBombOn(bool playNoSound)
    {
        SetBombSettingsToClipBoard(Bomb.Vertical, true);
        SwitchThings(Bomb.Vertical,true, playNoSound);
    }
    public void SwitchVerticalBombOff(bool playNoSound)
    {
        SetBombSettingsToClipBoard(Bomb.Vertical, false);
        SwitchThings(Bomb.Vertical,false, playNoSound);
    }
    public void SwitchHorizontalBombOn(bool playNoSound)
    {
        SetBombSettingsToClipBoard(Bomb.Horizontal ,true);
        SwitchThings(Bomb.Horizontal,true, playNoSound);
    }
    public void SwitchHorizontalBombOff(bool playNoSound)
    {
        SetBombSettingsToClipBoard(Bomb.Horizontal, false);
        SwitchThings(Bomb.Horizontal,false, playNoSound);
    }
    public void SwitchSearchBombOn(bool playNoSound)
    {
        SetBombSettingsToClipBoard(Bomb.Search, true);
        SwitchThings(Bomb.Search,true, playNoSound);
    }
    public void SwitchSearchBombOff(bool playNoSound)
    {
        SetBombSettingsToClipBoard(Bomb.Search, false);
        SwitchThings(Bomb.Search,false, playNoSound);
    }
    private bool GetBombSetting(Bomb bomb)
    {
        int level = Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber;
        for (int i = 0;
             i < Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BombConfigs.Length;
             i++)
        {
            if (Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BombConfigs[i].Bomb == bomb)
            {
                return Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BombConfigs[i]
                    .Active;
            }
        }
        return false;
    }
    private List<TextMeshProUGUI> _valueDisplayList;
    private void Awake()
    {
        List<TextMeshProUGUI> valueTextFields = new List<TextMeshProUGUI>();
        for (int i = 0; i < bombMatchStyleLists.Count; i++)
        {
            valueTextFields.Add(bombMatchStyleLists[i].ValueTextField); 
        }
        
        _valueDisplayList = GenericSettingsFunctions.AddToValueDisplayList(valueTextFields.ToArray());
    }
    public void LoadSettingsToclipboard(BombConfig[] bombConfigs)
    {//-----------------
        GenericSettingsFunctions.RemoveListeners(SameColorBombSlider,
            SameFruitBombSlider,
            SearchBombSlider,
            VertBombSlider,
            HorBombSlider);
        foreach (BombConfig bombconfig in bombConfigs)
        {
            switch (bombconfig.Bomb)
            {
                case Bomb.Color:
                {
                    Rl.saveClipBoard.colorBomb = bombconfig.Active; 
                    Rl.saveClipBoard.matchStyleColorBomb = bombconfig.MatchStyle;
                    Rl.saveClipBoard.colorBombMatchNumber = bombconfig.Matchnumber;
                    break;
                }
                case Bomb.Package:
                {
                    Rl.saveClipBoard.fruitBomb = bombconfig.Active;
                    Rl.saveClipBoard.matchStyleFruitBomb = bombconfig.MatchStyle;
                    Rl.saveClipBoard.fruitBombMatchNumber= bombconfig.Matchnumber;
                    break;
                }
                case Bomb.Search:
                {
                    Rl.saveClipBoard.searchBomb = bombconfig.Active; 
                    Rl.saveClipBoard.matchStyleSearchBomb = bombconfig.MatchStyle;
                    Rl.saveClipBoard.searchBombMatchNumber = bombconfig.Matchnumber;
                    break;
                }
                case Bomb.Vertical:
                {
                    Rl.saveClipBoard.verticalBomb = bombconfig.Active;
                    Rl.saveClipBoard.matchStyleVertBomb = bombconfig.MatchStyle;
                    Rl.saveClipBoard.vertBombMatchNumber = bombconfig.Matchnumber;
                    break;
                }
                case Bomb.Horizontal:
                {
                    Rl.saveClipBoard.horizontalBomb = bombconfig.Active;
                    Rl.saveClipBoard.matchStyleHorBomb = bombconfig.MatchStyle;
                    Rl.saveClipBoard.horBombMatchNumber = bombconfig.Matchnumber;
                    break;
                }
                case Bomb.AllBombs:
                {
                    Rl.saveClipBoard.allBombs = bombconfig.Active;
                    break;
                }
            }
        }
        
        for (int i = 0; i < bombMatchStyleLists.Count; i++) LoadMatchStyle(bombMatchStyleLists[i].Button, false);
        GenericSettingsFunctions.Addlisteners(delegate { ValueChangeCheck(); }, 
            SameColorBombSlider,
            SameFruitBombSlider,
            SearchBombSlider,
            VertBombSlider,
            HorBombSlider);
       //-------------------
    }
    private void SetBombSettingsToClipBoard(Bomb bomb, bool active)
    {
        switch (bomb)
        {
            case Bomb.Color:
            {
                Rl.saveClipBoard.colorBomb = active;
                break;
            }

            case Bomb.Package:
            {
                Rl.saveClipBoard.fruitBomb = active; 
                break;
            }
            case Bomb.Search:
            {
                Rl.saveClipBoard.searchBomb = active;
                break;
            }
            case Bomb.Vertical:
            {
                Rl.saveClipBoard.verticalBomb = active;
                break;
            }
            case Bomb.Horizontal:
            {
                Rl.saveClipBoard.horizontalBomb = active;
                break;
            }
            case Bomb.AllBombs:
            {
                Rl.saveClipBoard.allBombs = active;
                break;
            }
        }
    }
    public void SaveBombSettings()
    {
        SaveBombHelperMethod(Bomb.Color, Rl.saveClipBoard.colorBomb, Rl.saveClipBoard.matchStyleColorBomb, Rl.saveClipBoard.colorBombMatchNumber);
        SaveBombHelperMethod(Bomb.Package, Rl.saveClipBoard.fruitBomb, Rl.saveClipBoard.matchStyleFruitBomb, Rl.saveClipBoard.fruitBombMatchNumber);
        SaveBombHelperMethod(Bomb.Search, Rl.saveClipBoard.searchBomb,Rl.saveClipBoard.matchStyleSearchBomb, Rl.saveClipBoard.searchBombMatchNumber) ;
        
        SaveBombHelperMethod(Bomb.Vertical, Rl.saveClipBoard.verticalBomb, Rl.saveClipBoard.matchStyleVertBomb, Rl.saveClipBoard.vertBombMatchNumber);
        SaveBombHelperMethod(Bomb.Horizontal, Rl.saveClipBoard.horizontalBomb, Rl.saveClipBoard.matchStyleHorBomb, Rl.saveClipBoard.horBombMatchNumber);
        SaveBombHelperMethod(Bomb.AllBombs, Rl.saveClipBoard.allBombs, Rl.saveClipBoard.matchStyleColorBomb, Rl.saveClipBoard.colorBombMatchNumber); //dummy
    }

    private void SaveBombHelperMethod(Bomb bomb, bool active, MatchStyle matchStyle, ushort matchNumber)
    {
        int level = Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber;
        
        for (int i = 0; i < Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BombConfigs.Length; i++)
         {
             if (Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BombConfigs[i].Bomb == bomb)
             {
                 Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BombConfigs[i].Active =
                    active;
                 Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BombConfigs[i]
                     .MatchStyle = matchStyle;
                 Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BombConfigs[i]
                     .Matchnumber = matchNumber;
             }
         }
    }
    public void ValueChangeCheck()
    {
        for (int i = 0; i< bombMatchStyleLists.Count; i++)
        {
            switch (bombMatchStyleLists[i].bomb)
            {
                case Bomb.Color:
                    Rl.saveClipBoard.colorBombMatchNumber = (ushort)SameColorBombSlider.value;
                    break;
                
                case Bomb.Package:
                    Rl.saveClipBoard.fruitBombMatchNumber = (ushort)SameFruitBombSlider.value;
                    break;
                
                case Bomb.Search:
                    Rl.saveClipBoard.searchBombMatchNumber = (ushort)SearchBombSlider.value;
                    break;

                case Bomb.Vertical:
                    Rl.saveClipBoard.vertBombMatchNumber = (ushort)VertBombSlider.value;
                    break;
                    
                case Bomb.Horizontal: 
                    Rl.saveClipBoard.horBombMatchNumber = (ushort)HorBombSlider.value;
                    break;
            }
        }
        GenericSettingsFunctions.UpdateTextFields(
            ref _valueDisplayList,
            SameColorBombSlider,
            SameFruitBombSlider,
            SearchBombSlider,
            VertBombSlider,
            HorBombSlider);
    }
    private void SwitchThings(Bomb bomb, bool on, bool playNoSound)
    {
        switch (bomb)
        {
            case Bomb.Color:
                SwitchColorBomb.SwitchButton(on, playNoSound);
                break;
            
            case Bomb.Package:
                SwitchFruitBomb.SwitchButton(on, playNoSound);
                break;
            
            case Bomb.Search:
                SwitchSearchBomb.SwitchButton(on, playNoSound);
                break;
            case Bomb.Horizontal:
                SwitchHorizontalBomb.SwitchButton(on, playNoSound);
                break;
            
            case Bomb.Vertical:
                SwitchVerticalBomb.SwitchButton(on, playNoSound);
                break;
            
            case Bomb.AllBombs:
                SwitchAllBombs.SwitchButton(on, playNoSound);
                break;
        }
    }
    public void ClickNextMatchStyle(TextMeshProUGUI button)
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.NextRowSound, Random.Range(2,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        LoadMatchStyle(button, true);
    }
    private void LoadMatchStyle(TextMeshProUGUI button, bool nextMatchStyle)
    {
        for (var i = 0; i < bombMatchStyleLists.Count; i++)
        {
            if (button == bombMatchStyleLists[i].Button)
            {
                MatchStyle matchStyle = MatchStyle.row;
                switch (bombMatchStyleLists[i].bomb)
                {
                    case Bomb.Horizontal:
                        if (nextMatchStyle) matchStyle = NextMatchStyle((int)Rl.saveClipBoard.matchStyleHorBomb);
                        else matchStyle = Rl.saveClipBoard.matchStyleHorBomb;
                        bombMatchStyleLists[i].Button.text = GetLocalisedMatchStyleString(StringMatchStyle(matchStyle));
                        bombMatchStyleLists[i].ValueTextField.text = Rl.saveClipBoard.horBombMatchNumber.ToString();
                        Rl.saveClipBoard.matchStyleHorBomb = matchStyle;
                        break;
                    
                    case Bomb.Vertical:
                        if (nextMatchStyle) matchStyle = NextMatchStyle((int)Rl.saveClipBoard.matchStyleVertBomb);
                        else matchStyle = Rl.saveClipBoard.matchStyleVertBomb;
                        bombMatchStyleLists[i].Button.text = GetLocalisedMatchStyleString(StringMatchStyle(matchStyle));
                        bombMatchStyleLists[i].ValueTextField.text = Rl.saveClipBoard.vertBombMatchNumber.ToString();
                        Rl.saveClipBoard.matchStyleVertBomb= matchStyle;
                        break;
                    
                    case Bomb.Color:
                        if (nextMatchStyle) matchStyle = NextMatchStyle((int)Rl.saveClipBoard.matchStyleColorBomb);
                        else matchStyle = Rl.saveClipBoard.matchStyleColorBomb;
                        bombMatchStyleLists[i].Button.text = GetLocalisedMatchStyleString(StringMatchStyle(matchStyle));
                        bombMatchStyleLists[i].ValueTextField.text = Rl.saveClipBoard.colorBombMatchNumber.ToString();
                        Rl.saveClipBoard.matchStyleColorBomb= matchStyle;
                        break;
                    
                    case Bomb.Search:
                        if (nextMatchStyle) matchStyle = NextMatchStyle((int)Rl.saveClipBoard.matchStyleSearchBomb);
                        else matchStyle = Rl.saveClipBoard.matchStyleSearchBomb;
                        bombMatchStyleLists[i].Button.text = GetLocalisedMatchStyleString(StringMatchStyle(matchStyle));
                        bombMatchStyleLists[i].ValueTextField.text = Rl.saveClipBoard.searchBombMatchNumber.ToString();
                        Rl.saveClipBoard.matchStyleSearchBomb = matchStyle;
                        break;
                    
                    case Bomb.Package:
                        if (nextMatchStyle) matchStyle = NextMatchStyle((int)Rl.saveClipBoard.matchStyleFruitBomb);
                        else matchStyle = Rl.saveClipBoard.matchStyleFruitBomb;
                        bombMatchStyleLists[i].Button.text = GetLocalisedMatchStyleString(StringMatchStyle(matchStyle));
                        bombMatchStyleLists[i].ValueTextField.text = Rl.saveClipBoard.fruitBombMatchNumber.ToString();
                        Rl.saveClipBoard.matchStyleFruitBomb = matchStyle;
                        break;
                    
                    case Bomb.AllBombs:
                        break;
                }
            }
        }
    }
    private static int enumCountMatchStyle()
    {
        int counter = 0;
        foreach (MatchStyle matchStyle in Enum.GetValues(typeof(MatchStyle)))
            counter++;
        
        return counter;
    }
    private MatchStyle NextMatchStyle(int matchstyle)
    {
        matchstyle += 1;
        MatchStyle matchStyleEnum = MatchStyle.row;
        if (matchstyle > enumCountMatchStyle() - 1) matchstyle = 0;

        int counter = 0;
        foreach (MatchStyle matchStyle in Enum.GetValues(typeof(MatchStyle)))
        {
            if (matchstyle == counter) matchStyleEnum = matchStyle;
            counter++;
        }
        
        return matchStyleEnum;
    }
    private string GetLocalisedMatchStyleString(string matchstyleString)
    {
        if (LocalisationSystem.GetLocalisedValue(matchstyleString) != String.Empty) 
            return LocalisationSystem.GetLocalisedValue(matchstyleString);
    return matchstyleString;
    }
    private string StringMatchStyle(MatchStyle matchstyle) => Enum.GetName(typeof(MatchStyle), (int)matchstyle);
}
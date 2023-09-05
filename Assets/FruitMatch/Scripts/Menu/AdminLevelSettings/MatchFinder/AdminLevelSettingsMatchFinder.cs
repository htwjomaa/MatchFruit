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
   public TextMeshProUGUI PenaltyDisplay;
   //public TextMeshProUGUI DiagonalValueDisplay;
  // public TextMeshProUGUI Pattern1ValueDisplay;
   //public TextMeshProUGUI Pattern2ValueDisplay;
   [SerializeField] private Slider rowSlider;
   [SerializeField] private Slider penaltySlider;
   
   [SerializeField] private Slider diagonalSlider;
   [SerializeField] private Slider pattern1Slider;
   [SerializeField] private Slider pattern2Slider;

   [SerializeField] private Image Goal1Image;
   [SerializeField] private Image Goal2Image;
   [SerializeField] private Image Goal3Image;

   //[SerializeField] private AdminLevelSettingsMatchFinderAcceptSwitch rowSwitch;
  // [SerializeField] private AdminLevelSettingsMatchFinderAcceptSwitch diagonalSwitch;
   [SerializeField] private AdminLevelSettingsMatchFinderAcceptSwitch pattern1Switch;
   [SerializeField] private AdminLevelSettingsMatchFinderAcceptSwitch pattern2Switch;
   private List<TextMeshProUGUI> _valueDisplayList = new List<TextMeshProUGUI>();
   public void ClickNextFruitOrColor(GoalNumber goalNumber, bool isFruitNotColor, ref TextMeshProUGUI buttonToChangeTheText,  ref TextMeshProUGUI sibling)
   {
       Rl.GameManager.PlayAudio(Rl.soundStrings.NextRowSound, Random.Range(2, 4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
       GetNextFruitOrColor(goalNumber, isFruitNotColor, true, ref buttonToChangeTheText,  ref sibling);
   }
   public void LoadSame(GoalNumber goalNumber, bool isFruitNotColor, ref TextMeshProUGUI buttonToChangeTheText, ref TextMeshProUGUI sibling) 
       => GetNextFruitOrColor(goalNumber, isFruitNotColor, false,  ref buttonToChangeTheText,  ref sibling);
   public void LoadCurrentGoalSetting(GoalNumber goalNumber, bool isFruitNotColor,
       ref TextMeshProUGUI buttonToChangeTheText, ref TextMeshProUGUI sibling) =>
       GetNextFruitOrColor(goalNumber, isFruitNotColor, false, ref buttonToChangeTheText, ref sibling);
   private void GetNextFruitOrColor(GoalNumber goalNumber, bool isFruitNotColor, bool nextType, ref TextMeshProUGUI buttonToChangeTheText, ref TextMeshProUGUI sibling)
    {
        FruitType fruitType = FruitType.AlleFrüchte;
        Colors goalColor = Colors.AlleFarben;
        
                        if (isFruitNotColor)
                        {
                            switch (goalNumber)
                            {
                                case GoalNumber.Goal1:
                                    fruitType = nextType ? NextFruitType((int)Rl.saveClipBoard.GoalFruitOne[FieldState.CurrentField]) : Rl.saveClipBoard.GoalFruitOne[FieldState.CurrentField];
                                    break;
                                case GoalNumber.Goal2:
                                    fruitType = nextType ? NextFruitType((int)Rl.saveClipBoard.GoalFruitTwo[FieldState.CurrentField]) : Rl.saveClipBoard.GoalFruitTwo[FieldState.CurrentField];
                                    break;
                                case GoalNumber.Goal3:
                                    fruitType = nextType ? NextFruitType((int)Rl.saveClipBoard.GoalFruitThree[FieldState.CurrentField]) : Rl.saveClipBoard.GoalFruitThree[FieldState.CurrentField];
                                    break;
                            }
                            
                            buttonToChangeTheText.text = LocalisationSystem.GetLocalisedString(StringFruitType(fruitType));
                            
                            switch (goalNumber)
                            {
                                case GoalNumber.Goal1:
                                   Rl.saveClipBoard.GoalFruitOne[FieldState.CurrentField] = fruitType;;
                                    break;
                                case GoalNumber.Goal2:
                                    Rl.saveClipBoard.GoalFruitTwo[FieldState.CurrentField] = fruitType;;
                                    break;
                                case GoalNumber.Goal3:
                                    Rl.saveClipBoard.GoalFruitThree[FieldState.CurrentField] = fruitType;;
                                    break;
                            }
                            
                            CheckSiblingForInvalidColor(fruitType, ref sibling);
                            GetIconsForButtons(goalNumber, fruitType);  //............... MOVE THIS OUT OF IF ELSE IF COLORS ARE AVAILABLE
                        }

                        else //is Color
                        {
                            switch (goalNumber)
                            {
                                case GoalNumber.Goal1:
                                    goalColor = nextType ? NextGoalColor((int)Rl.saveClipBoard.GoalColorOne[FieldState.CurrentField]) : Rl.saveClipBoard.GoalColorOne[FieldState.CurrentField];
                                    break;
                                case GoalNumber.Goal2:
                                    goalColor = nextType ? NextGoalColor((int)Rl.saveClipBoard.GoalColorTwo[FieldState.CurrentField]) : Rl.saveClipBoard.GoalColorTwo[FieldState.CurrentField];
                                    break;
                                case GoalNumber.Goal3:
                                    goalColor = nextType ? NextGoalColor((int)Rl.saveClipBoard.GoalColorThree[FieldState.CurrentField]) : Rl.saveClipBoard.GoalColorThree[FieldState.CurrentField];
                                    break;
                            }

                            buttonToChangeTheText.text = LocalisationSystem.GetLocalisedString(StringGoalColor(goalColor));
                            
                            switch (goalNumber)
                            {
                                case GoalNumber.Goal1:
                                   Rl.saveClipBoard.GoalColorOne[FieldState.CurrentField] = goalColor;
                                    break;
                                case GoalNumber.Goal2:
                                   Rl.saveClipBoard.GoalColorTwo[FieldState.CurrentField] = goalColor;
                                    break;
                                case GoalNumber.Goal3:
                                   Rl.saveClipBoard.GoalColorThree[FieldState.CurrentField] = goalColor;
                                    break;
                            }
                            
                            switch (goalNumber)
                            {
                                case GoalNumber.Goal1:
                                    fruitType = Rl.saveClipBoard.GoalFruitOne[FieldState.CurrentField];
                                    break;
                                case GoalNumber.Goal2:
                                    fruitType = Rl.saveClipBoard.GoalFruitTwo[FieldState.CurrentField];
                                    break;
                                case GoalNumber.Goal3:
                                    fruitType = Rl.saveClipBoard.GoalFruitThree[FieldState.CurrentField];
                                    break;
                            }
                        
                            CheckSiblingForInvalidColor(fruitType, ref buttonToChangeTheText);
                        }
    }
 private string StringGoalColor(Colors goalColor) => Enum.GetName(typeof(Colors ), (int)goalColor);
 private Colors NextGoalColor(int goalColor)
 {
     goalColor += 1;
     Colors goalColorEnum = Colors.AlleFarben;
   //  if (goalColor > enumCountGoalColor() - 1) goalColor = 0;
   if (goalColor > enumCountGoalColor() - 1) goalColor = 0;
     int counter = 0;
     foreach (Colors searchForGoalColor in Enum.GetValues(typeof(Colors)))
     {
         if (goalColor == counter) goalColorEnum = searchForGoalColor;
         counter++;
     }
        
     return goalColorEnum;
 }
 
 private static int enumCountGoalColor()
 {
     int counter = 0;
     foreach (Colors doesNotMatterAtAll in Enum.GetValues(typeof(Colors )))
         counter++;
        
     return counter;
 }
 
   private void GetIconsForButtons(GoalNumber goalNumber, FruitType fruitType)
   {
       switch (goalNumber)
       {
           case GoalNumber.Goal1:
               Goal1Image.sprite = Rl.world.GetGoalSprite(fruitType);
               break;
           case GoalNumber.Goal2:
               Goal2Image.sprite = Rl.world.GetGoalSprite(fruitType);
               break;
           case GoalNumber.Goal3:
               Goal3Image.sprite = Rl.world.GetGoalSprite(fruitType);
               break;
       }
   }
   private string _notApplicable = "N/A";
   private void CheckSiblingForInvalidColor(FruitType fruitType, ref TextMeshProUGUI sibling)
   {
       if (fruitType is FruitType.Bubble or FruitType.Jelly or FruitType.Lock or FruitType.Truhe or FruitType.Nothing) sibling.text = _notApplicable;
       else if( sibling.text == _notApplicable) sibling.GetComponent<AdminLevelSettingsSequenceGoal>().LoadSameFruitOrColor();  //ERROR
   }
   private string StringFruitType(FruitType fruitType) => Enum.GetName(typeof(FruitType ), (int)fruitType);
   private Image GetImage()
   {
       Image uiImage = null;
       for (int i = 0; i < transform.parent.childCount; i++)
       {
           if (transform.parent.GetChild(i).GetComponent<Image>() != null)
               uiImage = transform.parent.GetChild(i).GetComponent<Image>();
       }

       return uiImage;
   }
   private  TextMeshProUGUI GetSibling()
   {
       int siblingIndex = transform.GetSiblingIndex();
       for (int i = 0; i < transform.parent.childCount; i++)
       {
           if(i != siblingIndex &&  transform.parent.GetChild(i).GetComponent<TextMeshProUGUI>() != null)
               return transform.parent.GetChild(i).GetComponent<TextMeshProUGUI>();
       }
       return null;
   }
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
    private void RemoveListeners() => GenericSettingsFunctions.RemoveListeners(rowSlider, penaltySlider);

    private void Addlisteners() => GenericSettingsFunctions.Addlisteners(delegate { ValueChangeCheck(); }, rowSlider, penaltySlider);

    private void LoadMatchFinderConfigToClipBoard(MatchFinderConfig matchFinderConfig)
    {
        Rl.saveClipBoard.RowMatchValue = (uint[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.RowValue);
        Rl.saveClipBoard.DiagonalMatchValue = (uint[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.DiagonalValue);
        Rl.saveClipBoard.Pattern1MatchValue = (uint[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.Pattern1Value);
        Rl.saveClipBoard.Pattern2MatchValue = (uint[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.Pattern2Value);

        Rl.saveClipBoard.Row = (Row[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.Row);
        Rl.saveClipBoard.Diagonal = (Diagonal[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.Diagonal);
        Rl.saveClipBoard.Pattern1 = (Pattern[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.Pattern1);
        Rl.saveClipBoard.Pattern2 = (Pattern[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.Pattern2);

        Rl.saveClipBoard.RowPhase = (Phase[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.RowPhase);
        Rl.saveClipBoard.DiagonalPhase = (Phase[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.DiagonalPhase);
        Rl.saveClipBoard.Pattern1Phase = (Phase[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.Pattern1Phase);
        Rl.saveClipBoard.Pattern2Phase = (Phase[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.Pattern2Phase);

        Rl.saveClipBoard.penalty = (int[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.PenaltyValue);
        Rl.saveClipBoard.SequenceEnabled = (bool[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.SequenceEnabled);
        Rl.saveClipBoard.BlockCombine = (bool[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.BlockCombinedAllowed);

        Rl.saveClipBoard.GoalFruitOne = (FruitType[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.GoalFruitOne);
        Rl.saveClipBoard.GoalFruitTwo= (FruitType[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.GoalFruitTwo);
        Rl.saveClipBoard.GoalFruitThree = (FruitType[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.GoalFruitThree);
        
        Rl.saveClipBoard.GoalColorOne = (Colors[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.GoalColorOne);
        Rl.saveClipBoard.GoalColorTwo = (Colors[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.GoalColorTwo);
        Rl.saveClipBoard.GoalColorThree = (Colors[])GenericSettingsFunctions.GetDeepCopy(matchFinderConfig.GoalColorThree);
        // InvokeDebugLoadEvent();
        InvokeLoadEvent();
    }

    [SerializeField] private Switch SequenceEnabledSwitch;
    public void ClickOnSequenceEnabledwitch(bool on) => ClickSequenceEnabledSwitch(on, false, false);
    
    public void ClickSequenceEnabledSwitch(bool on, bool playNoSound, bool animation)
    {
        
        SequenceEnabledSwitch.SwitchButton(on, playNoSound, animation);
        SequenceEnabledSwitch.SwitchButton(on, playNoSound, animation);
        Rl.saveClipBoard.SequenceEnabled[FieldState.CurrentField] = on;
    }
    
    [SerializeField] private Switch BlockCombineSwitch;
    public void ClickOnBlockSwitch(bool on) => ClickBlockSwitch(on, false, false);
    
    public void ClickBlockSwitch(bool on, bool playNoSound, bool animation)
    {
        
        BlockCombineSwitch.SwitchButton(on, playNoSound, animation);
        BlockCombineSwitch.SwitchButton(on, playNoSound, animation);
        Rl.saveClipBoard.BlockCombine[FieldState.CurrentField] = on;
    }

    public void ClickLoadRowValue(Transform transform)
    { 
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        int level = Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber;
      Rl.saveClipBoard.RowMatchValue =  Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].matchFinderConfig.RowValue;
      rowSlider.value = Rl.saveClipBoard.RowMatchValue[FieldState.CurrentField];

      Rl.saveClipBoard.Row = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
          .matchFinderConfig.Row;
      
      
      Rl.saveClipBoard.RowPhase = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
          .matchFinderConfig.RowPhase;
      
      
     // RowString.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Row.ToString());
     // rowSwitch.ClickOnSwitch(false);
     GenericSettingsFunctions.SmallJumpAnimation(transform);
    }
    public void ClickLoadPenaltyValue(Transform transform)
    { 
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        int level = Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber;
        Rl.saveClipBoard.penalty =  Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].matchFinderConfig.PenaltyValue;
        penaltySlider.value = Rl.saveClipBoard.penalty[FieldState.CurrentField];
        // RowString.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Row.ToString());
        // rowSwitch.ClickOnSwitch(false);
        GenericSettingsFunctions.SmallJumpAnimation(transform);
    }
    
    public void ClickLoadDiagonalValue(Transform transform)
    { 
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        int level = Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber;
        Rl.saveClipBoard.DiagonalMatchValue =  Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].matchFinderConfig.DiagonalValue;
        diagonalSlider.value = Rl.saveClipBoard.DiagonalMatchValue[FieldState.CurrentField];
        
        Rl.saveClipBoard.Diagonal = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
            .matchFinderConfig.Diagonal;

        Rl.saveClipBoard.DiagonalPhase = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
            .matchFinderConfig.DiagonalPhase;
        
       // DiagonalString.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Diagonal.ToString());
       // diagonalSwitch.ClickOnSwitch(false);
        GenericSettingsFunctions.SmallJumpAnimation(transform);
    }
    
    public void ClickLoadPattern1Value(Transform transform)
    { 
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        int level = Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber;
        Rl.saveClipBoard.Pattern1MatchValue =  Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].matchFinderConfig.Pattern1Value;
        pattern1Slider.value = Rl.saveClipBoard.Pattern1MatchValue[FieldState.CurrentField];
        
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
        pattern2Slider.value = Rl.saveClipBoard.Pattern2MatchValue[FieldState.CurrentField];
        
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
      //  RowString.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Row.ToString());
       // DiagonalString.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Diagonal.ToString());
      //  Pattern1String.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Pattern1.ToString());
       // Pattern2String.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Pattern2.ToString());
    }
    public MatchFinderConfig SaveMatchFinderSettings()
    {
        return new MatchFinderConfig(
            Rl.saveClipBoard.Row, Rl.saveClipBoard.RowMatchValue, Rl.saveClipBoard.RowPhase,
            Rl.saveClipBoard.Diagonal, Rl.saveClipBoard.DiagonalMatchValue, Rl.saveClipBoard.DiagonalPhase,
            Rl.saveClipBoard.Pattern1, Rl.saveClipBoard.Pattern1MatchValue, Rl.saveClipBoard.Pattern1Phase,
            Rl.saveClipBoard.Pattern2, Rl.saveClipBoard.Pattern2MatchValue, Rl.saveClipBoard.Pattern2Phase,
            Rl.saveClipBoard.BlockCombine,
            Rl.saveClipBoard.penalty,
            Rl.saveClipBoard.SequenceEnabled,
        
            Rl.saveClipBoard.GoalFruitOne,
            Rl.saveClipBoard.GoalColorOne,
   
            Rl.saveClipBoard.GoalFruitTwo,
            Rl.saveClipBoard.GoalColorTwo,
            
            Rl.saveClipBoard.GoalFruitThree,
            Rl.saveClipBoard.GoalColorThree
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
    private void Awake() => _valueDisplayList = GenericSettingsFunctions.AddToValueDisplayList(RowValueDisplay, PenaltyDisplay);
    
    private void ClipBoardToSlider()
    {
        ClickBlockSwitch(Rl.saveClipBoard.BlockCombine[FieldState.CurrentField], true, false);
        ClickSequenceEnabledSwitch(Rl.saveClipBoard.SequenceEnabled[FieldState.CurrentField], true, false);
        rowSlider.value =    Rl.saveClipBoard.RowMatchValue[FieldState.CurrentField];
        penaltySlider.value = Rl.saveClipBoard.penalty[FieldState.CurrentField];
//      diagonalSlider.value = Rl.saveClipBoard.DiagonalMatchValue;
//      pattern1Slider.value =  Rl.saveClipBoard.Pattern1MatchValue;
        //   pattern2Slider.value =  Rl.saveClipBoard.Pattern2MatchValue;
    }
    public void ValueChangeCheck()
    {
        Rl.saveClipBoard.RowMatchValue[FieldState.CurrentField] =  (uint)CheckIfValueIsNotViable(IsMatchStyle.IsRow, Pattern.Block, (int)rowSlider.value);
        Rl.saveClipBoard.penalty[FieldState.CurrentField] = (int)penaltySlider.value;
//        Rl.saveClipBoard.DiagonalMatchValue = (uint)CheckIfValueIsNotViable(IsMatchStyle.IsDiagonal, Pattern.Block, (int)diagonalSlider.value);
      //  Rl.saveClipBoard.Pattern1MatchValue = (uint)CheckIfValueIsNotViable(IsMatchStyle.IsPattern1, Rl.saveClipBoard.Pattern1, (int)pattern1Slider.value);
    //    Rl.saveClipBoard.Pattern2MatchValue = (uint)CheckIfValueIsNotViable(IsMatchStyle.IsPattern2, Rl.saveClipBoard.Pattern2, (int)pattern2Slider.value);

        
        ClipBoardToSlider();
        GenericSettingsFunctions.UpdateTextFields(ref _valueDisplayList, rowSlider,penaltySlider);
    }
    private void LoadIsMatchStyle(IsMatchStyle isMatchStyle, ref Row row, ref Diagonal diagonal, ref Pattern pattern, bool next)
    {
        switch (isMatchStyle)
        {
            case IsMatchStyle.IsRow:
                if (next) row = NextRow((int)Rl.saveClipBoard.Row[FieldState.CurrentField]);
                else row = Rl.saveClipBoard.Row[FieldState.CurrentField];
                Rl.saveClipBoard.Row[FieldState.CurrentField] = row;
                RowString.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Row.ToString());
                break;
            case IsMatchStyle.IsDiagonal:
                if (next) diagonal = NextDiagonal((int)Rl.saveClipBoard.Diagonal[FieldState.CurrentField]);
                else diagonal = Rl.saveClipBoard.Diagonal[FieldState.CurrentField];
                Rl.saveClipBoard.Diagonal[FieldState.CurrentField] = diagonal;
                DiagonalString.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Diagonal.ToString());
                break;
            case IsMatchStyle.IsPattern1:
                if (next) pattern = NextPattern((int)Rl.saveClipBoard.Pattern1[FieldState.CurrentField]);
                else pattern = Rl.saveClipBoard.Pattern1[FieldState.CurrentField];
                Rl.saveClipBoard.Pattern1[FieldState.CurrentField] = pattern;
                Pattern1String.text = LocalisationSystem.GetLocalisedValue(Rl.saveClipBoard.Pattern1.ToString());
                break;
            case IsMatchStyle.IsPattern2:
                if (next) pattern = NextPattern((int)Rl.saveClipBoard.Pattern2[FieldState.CurrentField]);
                else pattern = Rl.saveClipBoard.Pattern2[FieldState.CurrentField];
                Rl.saveClipBoard.Pattern2[FieldState.CurrentField] = pattern;
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
    private FruitType NextFruitType(int fruitType)
    {
        fruitType += 1;
        FruitType fruitTypeEnum = FruitType.AlleFrüchte;
     //   if (fruitType > enumCountFruitType() - 1) fruitType = 0;
        if (fruitType > enumCountFruitType() - 7) fruitType = 0;
        int counter = 0;
        foreach (FruitType searchForFruitType in Enum.GetValues(typeof(FruitType)))
        {
            if (fruitType== counter) fruitTypeEnum = searchForFruitType;
            counter++;
        }
        
        return fruitTypeEnum;
    }
    private static int enumCountFruitType()
    {
        int counter = 0;
        foreach (FruitType doesNotMatterAtAll in Enum.GetValues(typeof(FruitType )))
            counter++;
        
        return counter;
    }
    // public void AvoidOnlyInP2(Phase phase)
    // {
    //     if(phase = phase == Phase.OnlyInP2
    // }
    public void LoadCurrentField()
    {
        RemoveListeners();
        ClipBoardToSlider();
        InvokeLoadEvent();
        Addlisteners();
        ValueChangeCheck();
        InitStringValues();
    }
    public void CopyField(byte fieldOne, byte fieldTwo)
    {
        Rl.saveClipBoard.RowMatchValue[fieldTwo] = Rl.saveClipBoard.RowMatchValue[fieldOne];
        Rl.saveClipBoard.DiagonalMatchValue[fieldTwo] = Rl.saveClipBoard.DiagonalMatchValue[fieldOne];
        Rl.saveClipBoard.Pattern1MatchValue[fieldTwo] = Rl.saveClipBoard.Pattern1MatchValue[fieldOne];
        Rl.saveClipBoard.Pattern2MatchValue[fieldTwo] = Rl.saveClipBoard.Pattern2MatchValue[fieldOne];

        Rl.saveClipBoard.Row[fieldTwo] = Rl.saveClipBoard.Row[fieldOne];
        Rl.saveClipBoard.Diagonal[fieldTwo] = Rl.saveClipBoard.Diagonal[fieldOne];
        Rl.saveClipBoard.Pattern1[fieldTwo] = Rl.saveClipBoard.Pattern1[fieldOne];
        Rl.saveClipBoard.Pattern2[fieldTwo] = Rl.saveClipBoard.Pattern2[fieldOne];

        Rl.saveClipBoard.RowPhase[fieldTwo] = Rl.saveClipBoard.RowPhase[fieldOne];
        Rl.saveClipBoard.DiagonalPhase[fieldTwo] = Rl.saveClipBoard.DiagonalPhase[fieldOne];
        Rl.saveClipBoard.Pattern1Phase[fieldTwo] = Rl.saveClipBoard.Pattern1Phase[fieldOne];
        Rl.saveClipBoard.Pattern2Phase[fieldTwo] = Rl.saveClipBoard.Pattern2Phase[fieldOne];

        Rl.saveClipBoard.penalty[fieldTwo] = Rl.saveClipBoard.penalty[fieldOne];
        Rl.saveClipBoard.SequenceEnabled[fieldTwo] = Rl.saveClipBoard.SequenceEnabled[fieldOne];
        Rl.saveClipBoard.BlockCombine[fieldTwo] = Rl.saveClipBoard.BlockCombine[fieldOne];
        
        Rl.saveClipBoard.GoalFruitOne[fieldTwo] = Rl.saveClipBoard.GoalFruitOne[fieldOne];
        Rl.saveClipBoard.GoalFruitTwo[fieldTwo] = Rl.saveClipBoard.GoalFruitTwo[fieldOne];
        Rl.saveClipBoard.GoalFruitThree[fieldTwo] = Rl.saveClipBoard.GoalFruitThree[fieldOne];

        Rl.saveClipBoard.GoalColorOne[fieldTwo] = Rl.saveClipBoard.GoalColorOne[fieldOne];
        Rl.saveClipBoard.GoalColorTwo[fieldTwo] = Rl.saveClipBoard.GoalColorTwo[fieldOne];
        Rl.saveClipBoard.GoalColorThree[fieldTwo] = Rl.saveClipBoard.GoalColorThree[fieldOne];
    }
}
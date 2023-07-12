using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AdminLevelSettingsGoalConfig : MonoBehaviour
{
    [Header("Slider")]
    [SerializeField] public Slider Goal1Slider;
    [SerializeField] public Slider Goal2Slider;
    [SerializeField] public Slider Goal3Slider;
    [SerializeField] public Slider Goal4Slider;
    [SerializeField] public Slider Goal5Slider;
    [Space]
    
    [Header("Slider Value")]
    [SerializeField] private TextMeshProUGUI Goal1Value;
    [SerializeField] private TextMeshProUGUI Goal2Value;
    [SerializeField] private TextMeshProUGUI Goal3Value;
    [SerializeField] private TextMeshProUGUI Goal4Value;
    [SerializeField] private TextMeshProUGUI Goal5Value;

    [Space] [Header("Switch Buttons")] 
    
    [SerializeField] private Switch SwitchButtonEnabledGoal;
    [SerializeField] private Switch SwitchButtonAllowSimilar;
    [SerializeField] private Switch SwitchButtonAdditive;
    [SerializeField] private Switch SwitchButtonGoalOnly;
        
    [Space]
    [Header("ModeSwitch")]
    [SerializeField] private GameObject ObjectiveSettingParent;
    [SerializeField] private GameObject ObjectiveGoalsParent;
    [Space]
    [SerializeField] private string switchSound;
    private string _notApplicable = "N/A";
    public static PhaseNumber PhaseNumber; 
    public static ObjectiveNumber ObjectiveNumber;
    public static bool IsObjectiveSetting;
    public delegate void LoadCurrentObjectiveEvent();
    public static event LoadCurrentObjectiveEvent LoadCurrentObjective;

    // Phase1Goal1Fruit

    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private string buttonsDeactiveColor = "#B1BAFF";
    [SerializeField] private float buttonsDeactiveAlpha = 0.1f;
 
    public delegate void TabButtonFadeEvent();
    public static event TabButtonFadeEvent OnObjectiveSelect;
    public delegate void TabPhaseButtonFadeEvent();
    public static event TabPhaseButtonFadeEvent OnPhaseSelect;
    
    public static AdminLevelGoalStates SelectedObjective;
    public static AdminLevelGoalPhaseState SelectedPhase;
    private Color ResetColorAlmostFull() => new Color(255, 5, 5, 0.95f);
    
    private void Awake()
    {
        LoadCurrentObjective += SetSwitches;
        _valueDisplayList = GenericSettingsFunctions.AddToValueDisplayList(Goal1Value, Goal2Value, Goal3Value, Goal4Value, Goal5Value);
    }
    private void OnDestroy()
    {
        LoadCurrentObjective -= SetSwitches;
    }

    public void ResetSliderToZero(GoalNumber goalNumber)
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound, Random.Range(0, 4), Rl.settings.GetUISoundVolume,
            Rl.uiSounds.audioSource);
        
        switch (goalNumber)
        {
            case GoalNumber.Goal1:
                Goal1Slider.value = 0;
                break;
            case GoalNumber.Goal2: 
                Goal2Slider.value = 0;
                break;
            case GoalNumber.Goal3: 
                Goal3Slider.value = 0;
                break;
            case GoalNumber.Goal4: 
                Goal4Slider.value = 0;
                break;
            case GoalNumber.Goal5: 
                Goal5Slider.value = 0;
                break;
        }
        ValueChangeCheck();
    }
    private void ChangeColor(GameObject objectToChange)
    { 
        Color color;
        ColorUtility.TryParseHtmlString(buttonsDeactiveColor, out color);
        
        objectToChange.GetComponent<Image>().color = color;
        objectToChange.GetComponent<Image>().color = new Color(
            objectToChange.GetComponent<Image>().color.r, objectToChange.GetComponent<Image>().color.g,
            objectToChange.GetComponent<Image>().color.b, buttonsDeactiveAlpha);
    }

    private void ResetColorObjects(GameObject objectToChange)
    {
        objectToChange.transform.GetComponent<Image>().color = ResetColorAlmostFull();
        objectToChange.transform.GetChild(0).gameObject.GetComponent<Image>().color = ResetColorAlmostFull();
    }
    public void ResetPhaseTabs(AdminLevelGoalPhaseState adminLevelGoalPhaseState)
    {
        ChangeColor(adminLevelGoalPhaseState.gameObject);
        ChangeColor(adminLevelGoalPhaseState.transform.GetChild(0).gameObject);

        if (SelectedPhase != null && SelectedPhase  == adminLevelGoalPhaseState) 
            ResetColorObjects(adminLevelGoalPhaseState.gameObject);
    }
    public void ResetTabs(AdminLevelGoalStates adminLevelGoalState)
    {
        ChangeColor(adminLevelGoalState.gameObject);
        ChangeColor(adminLevelGoalState.transform.GetChild(0).gameObject);

        if (SelectedObjective != null && SelectedObjective == adminLevelGoalState) 
            ResetColorObjects(adminLevelGoalState.gameObject);
    }
    public void ChangePhaseNumberState(bool playNoSound, PhaseNumber phaseNumber)
    {
        ToggleBetweenObjectiveAndSeting();
       
        PlayNoSound = true;
       if(!playNoSound) Rl.GameManager.PlayAudio(Rl.soundStrings.PhaseChangeButtonSound, Random.Range(0, 4), Rl.settings.GetUISoundVolume,
            Rl.uiSounds.audioSource);
        PhaseNumber = phaseNumber;
        OnPhaseSelect?.Invoke();
        LoadCurrentObjective?.Invoke();
        PlayNoSound = false;
        ClipBoardToSliderSaveUse();
    }
    public void ChangeObjectiveSettingState(ObjectiveNumber objectiveNumber)
    {
        ToggleBetweenObjectiveAndSeting();
        PlayNoSound = true;
        Rl.GameManager.PlayAudio(Rl.soundStrings.TabButtonSound, Random.Range(0, 4), Rl.settings.GetUISoundVolume,
            Rl.uiSounds.audioSource);
        ObjectiveNumber = objectiveNumber;
        goalText.text = goalText.GetComponent<TextLocaliserUI>().LocalisedStrings[0].value  + " " + ((int)objectiveNumber+1);
        OnObjectiveSelect?.Invoke();
        LoadCurrentObjective?.Invoke();
        PlayNoSound = false;
        ClipBoardToSliderSaveUse();
    }
    private void ToggleBetweenObjectiveAndSeting()
    {
        ObjectiveGoalsParent.SetActive(!IsObjectiveSetting);
        ObjectiveSettingParent.SetActive(IsObjectiveSetting);
    }

    private List<UnityEngine.UIElements.Slider> _sliderList;
    private List<TextMeshProUGUI> _valueDisplayList = new List<TextMeshProUGUI>();

    private void ClipBoardToSliderSaveUse()
    {
        GenericSettingsFunctions.RemoveListeners(Goal1Slider, Goal2Slider,Goal3Slider,Goal4Slider,Goal5Slider);
        ClipBoardToSlider();
        ValueChangeCheck();
        GenericSettingsFunctions.Addlisteners(delegate { ValueChangeCheck(); }, Goal1Slider, Goal2Slider, Goal3Slider, Goal4Slider, Goal5Slider);
    }
    public void ChangeObjectiveNumberState(bool playNoSound, ObjectiveNumber objectiveNumber)
    {
        ToggleBetweenObjectiveAndSeting();
        PlayNoSound = true;
        if(!playNoSound)Rl.GameManager.PlayAudio(Rl.soundStrings.TabButtonSound, Random.Range(0, 4), Rl.settings.GetUISoundVolume,
            Rl.uiSounds.audioSource);
        ObjectiveNumber = objectiveNumber;
        goalText.text = goalText.GetComponent<TextLocaliserUI>().LocalisedStrings[0].value  + " " + ((int)objectiveNumber+1);
        OnObjectiveSelect?.Invoke();
        LoadCurrentObjective?.Invoke();
        PlayNoSound = false;
    }


    private void ValueChangeHelperMethod(bool toSlider)
    {
         if (Rl.saveClipBoard.PhaseGoalsList.Count == 0) Rl.saveClipBoard.InitPhaseGoalList();
        int phaseNumberInt = 0;
        if (PhaseNumber == PhaseNumber.P2) phaseNumberInt = 1;

        for (int o = 0; o < Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray.Length; o++)
        {
            if (ObjectiveNumber == Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].ObjectiveNumber)
            {

                for (int i = 0; i < Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o]
                         .PhaseGoalArray.Length; i++)
                {
                    switch (  Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].PhaseGoalArray[i].GoalNumber)
                    {
                        case GoalNumber.Goal1:
                            if (!toSlider)
                                Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o]
                                    .PhaseGoalArray[i].CollectionAmount = (uint)Goal1Slider.value;
                            else Goal1Slider.value = Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].PhaseGoalArray[i].CollectionAmount;
                            break;
                        case GoalNumber.Goal2:
                            if(!toSlider)    Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].PhaseGoalArray[i].CollectionAmount = (uint)Goal2Slider.value;
                            else Goal2Slider.value = Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].PhaseGoalArray[i].CollectionAmount;
                            break;
                        case GoalNumber.Goal3:
                            if(!toSlider)   Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].PhaseGoalArray[i].CollectionAmount = (uint)Goal3Slider.value;
                            else  Goal3Slider.value = Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].PhaseGoalArray[i].CollectionAmount;
                            break;
                        case GoalNumber.Goal4:
                            if(!toSlider)   Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].PhaseGoalArray[i].CollectionAmount = (uint)Goal4Slider.value;
                            else  Goal4Slider.value = Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].PhaseGoalArray[i].CollectionAmount;
                            break;
                        case GoalNumber.Goal5:
                            if(!toSlider)  Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].PhaseGoalArray[i].CollectionAmount = (uint)Goal5Slider.value;
                            else   Goal5Slider.value = Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].PhaseGoalArray[i].CollectionAmount;
                            break;
                    }
                    
                }
            }
        }
    }
    public void ValueChangeCheck()
    {
        SliderToClipBoard();
        ClipBoardToSlider();
        GenericSettingsFunctions.UpdateTextFields(ref _valueDisplayList, Goal1Slider, Goal2Slider, Goal3Slider, Goal4Slider, Goal5Slider);
    }

    public void SliderToClipBoard() => ValueChangeHelperMethod(false);
    public void ClipBoardToSlider() => ValueChangeHelperMethod(true);
    public void SwitchGoalOnlyOn (bool playNoSound)
    {
        SwitchAdditiveAndGoalOnly(false,true, playNoSound);
        SetButtonsInClipBoard(GoalSwitchButton.GoalOnly, true);
    }
    public void SwitchGoalOnlyOff (bool playNoSound)
    {
        SwitchAdditiveAndGoalOnly(false,false, playNoSound);
        SetButtonsInClipBoard(GoalSwitchButton.GoalOnly, false);
    }
    public void SwitchAdditiveOn (bool playNoSound)
    {
        SwitchAdditiveAndGoalOnly(true,true, playNoSound);
        SetButtonsInClipBoard(GoalSwitchButton.Additive, true);
    }
    public void SwitchAdditiveOff(bool playNoSound)
    {
        SwitchAdditiveAndGoalOnly(true,false, playNoSound);
        SetButtonsInClipBoard(GoalSwitchButton.Additive, false);
    }
    public void SwitchEnabledGoalOn (bool playNoSound)
    {
        SwitchEnabledAndAllowSimilar(false,true, playNoSound);
        SetButtonsInClipBoard(GoalSwitchButton.Enabled, true);
    }
    public void SwitchEnabledGoalOff (bool playNoSound)
    {
        SwitchEnabledAndAllowSimilar(false,false, playNoSound);
        SetButtonsInClipBoard(GoalSwitchButton.Enabled, false);
    }
    
    public void SwitchAllowSimilarOn (bool playNoSound)
    {
        SwitchEnabledAndAllowSimilar(true,true, playNoSound);
        SetButtonsInClipBoard(GoalSwitchButton.AllowSimilar, true);
    }
    public void SwitchAllowSimilarOff (bool playNoSound)
    {
        SwitchEnabledAndAllowSimilar(true,false, playNoSound);
        SetButtonsInClipBoard(GoalSwitchButton.AllowSimilar, false);
    }

    public void LoadGoalConfig(GoalConfig goalConfig)
    {
        PlayNoSound = true;
        LoadToClipboard(goalConfig);
        LoadCurrentObjective?.Invoke();
        PlayNoSound = false;
        ClipBoardToSliderSaveUse();
    }
    public void LoadToClipboard(GoalConfig goalConfig)
    {
        Rl.saveClipBoard.PhaseGoalsList = (List<PhaseGoals>)GenericSettingsFunctions.GetDeepCopy(goalConfig.PhaseGoalsList);
    }

    public GoalConfig SaveGoalConfigs()
    {
        return new GoalConfig((List<PhaseGoals>)GenericSettingsFunctions.GetDeepCopy(Rl.saveClipBoard.PhaseGoalsList));
    }
    public static bool PlayNoSound = false;
    public void SetSwitches()
    {
       
        if (Rl.saveClipBoard.PhaseGoalsList.Count == 0) Rl.saveClipBoard.InitPhaseGoalList();
        int phaseNumberInt = 0;
        if (PhaseNumber == PhaseNumber.P2) phaseNumberInt = 1;

        for (int o = 0; o < Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray.Length; o++)
        {
            if (ObjectiveNumber == Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o]
                    .ObjectiveNumber)
            {
                SwitchEnabledAndAllowSimilar( false,   Rl.saveClipBoard
                    .PhaseGoalsList[phaseNumberInt]
                    .ObjectiveSettingsArray[o].Enabled, PlayNoSound);
                SwitchEnabledAndAllowSimilar( true,   Rl.saveClipBoard
                    .PhaseGoalsList[phaseNumberInt]
                    .ObjectiveSettingsArray[o].AllowSimilar, PlayNoSound);
                SwitchAdditiveAndGoalOnly( false,   Rl.saveClipBoard
                    .PhaseGoalsList[phaseNumberInt]
                    .ObjectiveSettingsArray[o].GoalOnly, PlayNoSound);
                SwitchAdditiveAndGoalOnly( true,   Rl.saveClipBoard
                    .PhaseGoalsList[phaseNumberInt]
                    .ObjectiveSettingsArray[o].Additive, PlayNoSound);
            }
        }
    }

    private void SwitchAdditiveAndGoalOnly(bool isadditiveButton, bool on, bool playNoSound)
    {
        if (isadditiveButton) SwitchButtonAdditive.SwitchButton(on, playNoSound, false);
        else SwitchButtonGoalOnly.SwitchButton(on, playNoSound, false);
    }
    
    private void SwitchEnabledAndAllowSimilar(bool isAllowSimilarButton, bool on, bool playNoSound)
    {
        if (isAllowSimilarButton) SwitchButtonAllowSimilar.SwitchButton(on, playNoSound, false);
        else SwitchButtonEnabledGoal.SwitchButton(on, playNoSound, false);
    }

    public void ClickNextFruitOrColor(GoalNumber goalNumber, bool isFruitNotColor, ref TextMeshProUGUI buttonToChangeTheText,  ref TextMeshProUGUI sibling)
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.NextRowSound, Random.Range(2, 4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        GetNextFruitOrColor(goalNumber, isFruitNotColor, true, ref buttonToChangeTheText,  ref sibling);
    }
    public void LoadSame(GoalNumber goalNumber, bool isFruitNotColor, ref TextMeshProUGUI buttonToChangeTheText, ref TextMeshProUGUI sibling) 
        => GetNextFruitOrColor(goalNumber, isFruitNotColor, false,  ref buttonToChangeTheText,  ref sibling);


    private void CheckSiblingForInvalidColor(FruitType fruitType, ref TextMeshProUGUI sibling)
    {
        if (fruitType is FruitType.Bubble or FruitType.Jelly or FruitType.Lock or FruitType.Truhe or FruitType.Nothing) sibling.text = _notApplicable;
        else if( sibling.text == _notApplicable) sibling.GetComponent<AdminLevelSingleGoalSetting>().LoadSameFruitOrColor();  //ERROR
    }

  
    private void GetNextFruitOrColor(GoalNumber goalNumber, bool isFruitNotColor, bool nextType, ref TextMeshProUGUI buttonToChangeTheText, ref TextMeshProUGUI sibling)
    {
        if (Rl.saveClipBoard.PhaseGoalsList.Count == 0) Rl.saveClipBoard.InitPhaseGoalList();
        FruitType fruitType = FruitType.AlleFrüchte;
        Colors goalColor = Colors.AlleFarben;
        int phaseNumberInt = 0;
        if (PhaseNumber == PhaseNumber.P2) phaseNumberInt = 1;

        for (int o = 0; o < Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray.Length; o++)
        {
            if (ObjectiveNumber == Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].ObjectiveNumber)
            {
                //GoalNumber  1-5

                for (int i = 0; i < Rl.saveClipBoard
                         .PhaseGoalsList[phaseNumberInt]
                         .ObjectiveSettingsArray[o]
                         .PhaseGoalArray
                         .Length; i++)
                {
                    //Phase 1  is int number 0 and Phase 2 is int Number 1.  

                    if (goalNumber == Rl.saveClipBoard
                            .PhaseGoalsList[phaseNumberInt]
                            .ObjectiveSettingsArray[o]
                            .PhaseGoalArray[i]
                            .GoalNumber)
                    {
                        //GoalNumber  1-5

                        if (isFruitNotColor)
                        {
                            if (nextType) fruitType = NextFruitType((int)Rl.saveClipBoard
                                .PhaseGoalsList[phaseNumberInt]
                                .ObjectiveSettingsArray[o]
                                .PhaseGoalArray[i].
                                GoalFruit);
                            else
                                fruitType = Rl.saveClipBoard
                                    .PhaseGoalsList[phaseNumberInt]
                                    .ObjectiveSettingsArray[o]
                                    .PhaseGoalArray[i]
                                    .GoalFruit;
                            
                            buttonToChangeTheText.text = LocalisationSystem.GetLocalisedString(StringFruitType(fruitType));
                            
                            Rl.saveClipBoard
                                .PhaseGoalsList[phaseNumberInt]
                                .ObjectiveSettingsArray[o]
                                .PhaseGoalArray[i]
                                .GoalFruit = fruitType;
                            CheckSiblingForInvalidColor(fruitType, ref sibling);
                            GetIconsForButtons(goalNumber, fruitType);  //............... MOVE THIS OUT OF IF ELSE IF COLORS ARE AVAILABLE
                        }

                        else //is Color
                        {
                            if (nextType)
                                goalColor = NextGoalColor((int)Rl.saveClipBoard
                                    .PhaseGoalsList[phaseNumberInt]
                                    .ObjectiveSettingsArray[o]
                                    .PhaseGoalArray[i]
                                    .GoalColor);
                            else
                                goalColor = Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt]
                                    .ObjectiveSettingsArray[o]
                                    .PhaseGoalArray[i]
                                    .GoalColor;
                            
                            buttonToChangeTheText.text = LocalisationSystem.GetLocalisedString(StringGoalColor(goalColor));
                            
                            Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt]
                                .ObjectiveSettingsArray[o]
                                .PhaseGoalArray[i]
                                .GoalColor = goalColor;
                            
                            fruitType = Rl.saveClipBoard
                                .PhaseGoalsList[phaseNumberInt]
                                .ObjectiveSettingsArray[o]
                                .PhaseGoalArray[i]
                                .GoalFruit;

                        
                            CheckSiblingForInvalidColor(fruitType, ref buttonToChangeTheText);
                        }
                    }
                }
            }
        }
    }
[Serializable]
public enum GoalSwitchButton
{
    Enabled,
    AllowSimilar,
    Additive,
    GoalOnly
}
    public void SetButtonsInClipBoard(GoalSwitchButton goalSwitchButton, bool on)
    {
        if (Rl.saveClipBoard.PhaseGoalsList.Count == 0) Rl.saveClipBoard.InitPhaseGoalList();
        int phaseNumberInt = 0;
        if (PhaseNumber == PhaseNumber.P2) phaseNumberInt = 1;

        for (int o = 0; o < Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray.Length; o++)
        {
            if (ObjectiveNumber == Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].ObjectiveNumber)
            {
                switch (goalSwitchButton)
                {
                    case GoalSwitchButton.Enabled:
                        Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].Enabled = on;
                        break;
                    case GoalSwitchButton.AllowSimilar:
                        Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].AllowSimilar = on;
                        break;
                    case GoalSwitchButton.Additive: 
                        Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].Additive = on;
                        break;
                    case GoalSwitchButton.GoalOnly:
                        Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].GoalOnly = on;
                        break;
                }
            }
        }
    }

    [SerializeField] private Image Goal1Image;
    [SerializeField] private Image Goal2Image;
    [SerializeField] private Image Goal3Image;
    [SerializeField] private Image Goal4Image;
    [SerializeField] private Image Goal5Image;
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
            case GoalNumber.Goal4:
                Goal4Image.sprite = Rl.world.GetGoalSprite(fruitType);
                break;
            case GoalNumber.Goal5:
                Goal5Image.sprite = Rl.world.GetGoalSprite(fruitType);
                break;
        }
    }
    public void SetToNothingButton(GoalNumber goalNumber, ref TextMeshProUGUI buttonToChangeTheText, ref TextMeshProUGUI sibling)
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound, Random.Range(0, 4), Rl.settings.GetUISoundVolume,
            Rl.uiSounds.audioSource);
        
        if (Rl.saveClipBoard.PhaseGoalsList.Count == 0) Rl.saveClipBoard.InitPhaseGoalList();
        int phaseNumberInt = 0;
        if (PhaseNumber == PhaseNumber.P2) phaseNumberInt = 1;

        for (int o = 0; o < Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray.Length; o++)
        {
            if (ObjectiveNumber == Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].ObjectiveNumber)
            {
                //GoalNumber  1-5
                for (int i = 0; i < Rl.saveClipBoard
                         .PhaseGoalsList[phaseNumberInt]
                         .ObjectiveSettingsArray[o]
                         .PhaseGoalArray
                         .Length; i++)
                {
                    //Phase 1  is int number 0 and Phase 2 is int Number 1.  

                    if (goalNumber == Rl.saveClipBoard
                            .PhaseGoalsList[phaseNumberInt]
                            .ObjectiveSettingsArray[o]
                            .PhaseGoalArray[i]
                            .GoalNumber)
                    {
                        Rl.saveClipBoard
                            .PhaseGoalsList[phaseNumberInt]
                            .ObjectiveSettingsArray[o]
                            .PhaseGoalArray[i].GoalFruit = FruitType.Nothing;
                    }
                }
            }
        }
        GetNextFruitOrColor(goalNumber,  true, false,
            ref buttonToChangeTheText, ref sibling);
    }
    
    public void LoadCurrentGoalSetting(GoalNumber goalNumber,
        ref TextMeshProUGUI buttonToChangeTheText) =>
        GetNextCollectionStyle(goalNumber, false, ref buttonToChangeTheText);
    public void LoadCurrentGoalSetting(GoalNumber goalNumber, bool isFruitNotColor,
        ref TextMeshProUGUI buttonToChangeTheText, ref TextMeshProUGUI sibling) =>
        GetNextFruitOrColor(goalNumber, isFruitNotColor, false, ref buttonToChangeTheText, ref sibling);
    private Colors NextGoalColor(int goalColor)
    {
        goalColor += 1;
        Colors goalColorEnum = Colors.AlleFarben;
        if (goalColor > enumCountGoalColor() - 1) goalColor = 0;

        int counter = 0;
        foreach (Colors searchForGoalColor in Enum.GetValues(typeof(Colors)))
        {
            if (goalColor == counter) goalColorEnum = searchForGoalColor;
            counter++;
        }
        
        return goalColorEnum;
    }
    
    public void ClickNextCollectionStyle(GoalNumber goalNumber, ref TextMeshProUGUI buttonToChangeTheText)
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.NextRowSound, Random.Range(2, 4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        GetNextCollectionStyle(goalNumber, true, ref buttonToChangeTheText);
    }
    public void LoadSame(GoalNumber goalNumber, ref TextMeshProUGUI buttonToChangeTheText)
        => GetNextCollectionStyle(goalNumber, false,  ref buttonToChangeTheText);

    private void GetNextCollectionStyle(GoalNumber goalNumber, bool nextType, ref TextMeshProUGUI buttonToChangeTheText)
    {
        if (Rl.saveClipBoard.PhaseGoalsList.Count == 0) Rl.saveClipBoard.InitPhaseGoalList();
        int phaseNumberInt = 0;
        CollectionStyle collectionStyle = CollectionStyle.Nothing;
        if (PhaseNumber == PhaseNumber.P2) phaseNumberInt = 1;

        for (int o = 0; o < Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray.Length; o++)
        {
            if (ObjectiveNumber == Rl.saveClipBoard.PhaseGoalsList[phaseNumberInt].ObjectiveSettingsArray[o].ObjectiveNumber)
            {   //GoalNumber  1-5
                for (int i = 0; i < Rl.saveClipBoard
                         .PhaseGoalsList[phaseNumberInt]
                         .ObjectiveSettingsArray[o]
                         .PhaseGoalArray
                         .Length; i++)
                { //Phase 1  is int number 0 and Phase 2 is int Number 1.  
                    if (goalNumber == Rl.saveClipBoard
                            .PhaseGoalsList[phaseNumberInt]
                            .ObjectiveSettingsArray[o]
                            .PhaseGoalArray[i]
                            .GoalNumber)
                    {
                        if (nextType) collectionStyle = NextCollectionStyle((int)Rl.saveClipBoard
                            .PhaseGoalsList[phaseNumberInt]
                            .ObjectiveSettingsArray[o]
                            .PhaseGoalArray[i].
                            CollectionStyle);
                        else
                            collectionStyle = Rl.saveClipBoard
                                .PhaseGoalsList[phaseNumberInt]
                                .ObjectiveSettingsArray[o]
                                .PhaseGoalArray[i]
                                .CollectionStyle;
                            
                        buttonToChangeTheText.text = LocalisationSystem.GetLocalisedString(StringCollectionStyle(collectionStyle));
                            
                        Rl.saveClipBoard
                            .PhaseGoalsList[phaseNumberInt]
                            .ObjectiveSettingsArray[o]
                            .PhaseGoalArray[i]
                            .CollectionStyle = collectionStyle;
                    }
                }
            }
        }
    }
    private CollectionStyle NextCollectionStyle(int collectionStyle)
    {
        collectionStyle += 1;
        CollectionStyle collectionStyleEnum = CollectionStyle.Nothing;
        if (collectionStyle > enumCountCollectionStyle() - 1) collectionStyle = 0;

        int counter = 0;
        foreach (CollectionStyle searchForCollectionStyle in Enum.GetValues(typeof(CollectionStyle)))
        {
            if (collectionStyle == counter) collectionStyleEnum = searchForCollectionStyle;
            counter++;
        }
        
        return collectionStyleEnum;
    }
    
    private static int enumCountGoalColor()
    {
        int counter = 0;
        foreach (Colors doesNotMatterAtAll in Enum.GetValues(typeof(Colors )))
            counter++;
        
        return counter;
    }
    private static int enumCountCollectionStyle()
    {
        int counter = 0;
        foreach (CollectionStyle doesNotMatterAtAll in Enum.GetValues(typeof(CollectionStyle)))
            counter++;
        
        return counter;
    }
    private string StringGoalColor(Colors goalColor) => Enum.GetName(typeof(Colors ), (int)goalColor);
    public string StringCollectionStyle(CollectionStyle collectionStyle) => Enum.GetName(typeof(CollectionStyle ), (int)collectionStyle);
    
    private FruitType NextFruitType(int fruitType)
    {
        fruitType += 1;
        FruitType fruitTypeEnum = FruitType.AlleFrüchte;
        if (fruitType > enumCountFruitType() - 1) fruitType = 0;

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
    private string StringFruitType(FruitType fruitType) => Enum.GetName(typeof(FruitType ), (int)fruitType);
} 

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class ArchievmentButtons : MonoBehaviour
{
    public delegate void DoSomething();
    public static event DoSomething NeededForNextLevel;
 
    
    public delegate void ChangeTrophyValue();
    public static event ChangeTrophyValue ChangeTrophyValueEvent;
    
    [Header("Buttons")]
    [SerializeField] private Switch SwitchButtonStar1;
    [SerializeField] private Switch SwitchButtonStar2;
    [SerializeField] private Switch SwitchButtonStar3;
    [SerializeField] private Switch SwitchButtonTrophy;
    
    
    [Space]
    [Header("Slider")]
    [SerializeField] public Slider Star1Slider;
    [SerializeField] public Slider Star2Slider;
    [SerializeField] public Slider Star3Slider;
    [SerializeField] public Slider TrophySlider;
    
    [Space]
    [Header("UpdateText")]
    [SerializeField] private TextMeshProUGUI Star1Text;
    [SerializeField] private TextMeshProUGUI Star2Text;
    [SerializeField] private TextMeshProUGUI Star3Text;
    [SerializeField] private TextMeshProUGUI TrophyText;
    
    [Space]
    [Header("UpdateText")]
    [SerializeField] private TMP_InputField Star1InputText;
    [SerializeField] private TMP_InputField Star2InputText;
    [SerializeField] private TMP_InputField Star3InputText;
    [SerializeField] private TMP_InputField TrophyInputText;

    private List<TextMeshProUGUI> _valueDisplayList;
    private List<TMP_InputField> _valueDisplayListInputs;
    private void Awake()
    {
        _valueDisplayList = GenericSettingsFunctions.AddToValueDisplayList(Star1Text, Star2Text, Star3Text, TrophyText);
        _valueDisplayListInputs = GenericSettingsFunctions.AddToValueDisplayList( Star1InputText, Star2InputText, Star3InputText, TrophyInputText);

        ChangeTrophyValueEvent += UpdateInputTextfields;
    }

    private void OnDestroy()
    {
        ChangeTrophyValueEvent -=   UpdateInputTextfields;
    }

    public static void InvokeChangeTrophyEvent()
    {
        ChangeTrophyValueEvent?.Invoke();
    }
    public void LoadArchievmentSettings(ScoreGoalsConfig scoreGoalsConfig)
    {
        GenericSettingsFunctions.RemoveListeners(Star1Slider, Star2Slider, Star3Slider, TrophySlider);
        GenericSettingsFunctions.RemoveListeners(Star1InputText, Star2InputText, Star3InputText, TrophyInputText);
        LoadArchievmentSettingsToClipBoard(scoreGoalsConfig);
        ClipBoardToSlider();
        UpdateTextfields();
        UpdateInputTextfields();
        GenericSettingsFunctions.Addlisteners(delegate { ValueChangeCheck(); }, Star1Slider, Star2Slider, Star3Slider, TrophySlider);
        GenericSettingsFunctions.Addlisteners(delegate { UpdateInputFields(); }, Star1InputText, Star2InputText, Star3InputText, TrophyInputText);
        ValueChangeCheck();
        // playNoSoundAcceptSwitch = true;
        // SetP1P2(Rl.saveClipBoard.P1P2);
        // playNoSoundAcceptSwitch = false;
        // LoadGameType(true, false);
        // LoadGameType(false, false);
    }
    public void LoadArchievmentSettingsToClipBoard(ScoreGoalsConfig scoreGoalsConfig)
    {
        Rl.saveClipBoard.Star1Value = scoreGoalsConfig.Star1Value;
        Rl.saveClipBoard.Star2Value = scoreGoalsConfig.Star2Value;
        Rl.saveClipBoard.Star3Value = scoreGoalsConfig.Star3Value;
        Rl.saveClipBoard.TrophyValue = scoreGoalsConfig.TrophyValue;
        
        //load Stars enabled
        Rl.saveClipBoard.Star1Enabled = scoreGoalsConfig.Star1Enabled;
        Rl.saveClipBoard.Star2Enabled =scoreGoalsConfig.Star2Enabled;
        Rl.saveClipBoard.Star3Enabled = scoreGoalsConfig.Star3Enabled;

        Rl.saveClipBoard.TrophyEnabled = scoreGoalsConfig.TrophyEnabled;
        
        //Load needed for next level
        Rl.saveClipBoard.Star1NeededNextLevel = scoreGoalsConfig.Star1NeededForNextLevel;
        Rl.saveClipBoard.Star2NeededNextLevel = scoreGoalsConfig.Star2NeededForNextLevel;
        Rl.saveClipBoard.Star3NeededNextLevel = scoreGoalsConfig.Star3NeededForNextLevel;
        Rl.saveClipBoard.TrophyNeededNextLevel = scoreGoalsConfig.TrophyNeededForNextLevel;
        
        //load extra settings for trophy

        Rl.saveClipBoard.TrophyNumberAreMovesOrTime = scoreGoalsConfig.TrophyNumberAreMovesOrTime;
        Rl.saveClipBoard.TrophyNoEmpty = scoreGoalsConfig.TrophyNoEmpty;

        SetStarSettings(true);
        NeededForNextLevel?.Invoke();;
    }
    
    public void ClipBoardToSlider()
    {
        Star1Slider.value = Rl.saveClipBoard.Star1Value;
        Star2Slider.value = Rl.saveClipBoard.Star2Value;
        Star3Slider.value = Rl.saveClipBoard.Star3Value;
        TrophySlider.value = Rl.saveClipBoard.TrophyValue;
    }

    public void UpdateInputFields()
    {
        Star1Slider.value = GenericSettingsFunctions.GetFloatValueFromConstValuePoints(int.Parse(Star1InputText.text));
       if(Star2InputText.text != string.Empty)  Star2Slider.value = GenericSettingsFunctions.GetFloatValueFromConstValuePoints(int.Parse(Star2InputText.text));
       if(Star3InputText.text != string.Empty) Star3Slider.value = GenericSettingsFunctions.GetFloatValueFromConstValuePoints(int.Parse(Star3InputText.text));
       if(TrophyInputText.text != string.Empty)  TrophySlider.value = GenericSettingsFunctions.GetFloatValueFromConstValuePoints(int.Parse(TrophyInputText.text));

//parse von input auf slider value
// -> 
       Debug.Log("Star1Slider.value" + Star1Slider.value);
       ValueChangeCheck();
    }
    public void ValueChangeCheck()
    {
        Rl.saveClipBoard.Star1Value = Star1Slider.value;
        Rl.saveClipBoard.Star2Value = Star2Slider.value;
        Rl.saveClipBoard.Star3Value = Star3Slider.value;
        Rl.saveClipBoard.TrophyValue = TrophySlider.value ;
        
        UpdateTextfields();
        UpdateInputTextfields();
        // Star1InputText.text = Star1Text.text;
        //  GenericSettingsFunctions.RemoveListeners(Star1InputText, Star2InputText, Star3InputText, TrophyInputText);
        // DelteTextString(_valueDisplayListInputs.ToArray());
        // GenericSettingsFunctions.Addlisteners(delegate { UpdateInputFields(); }, Star1InputText, Star2InputText, Star3InputText, TrophyInputText);
    }
    private void UpdateTextfields()
    {
        GenericSettingsFunctions.UpdateTextFields(
            ref _valueDisplayList, 
            GenericSettingsFunctions.GetConstValuesPoints(Rl.saveClipBoard.Star1Value).ToString(),
            GenericSettingsFunctions.GetConstValuesPoints(Rl.saveClipBoard.Star2Value).ToString(),
            GenericSettingsFunctions.GetConstValuesPoints(Rl.saveClipBoard.Star3Value).ToString(),
            GenericSettingsFunctions.GetConstValuesPoints(Rl.saveClipBoard.TrophyValue).ToString()
        );
        ConvertToMoveValues();
    }
    private void UpdateInputTextfields()
    {
        GenericSettingsFunctions.UpdateTextFields(
            ref _valueDisplayListInputs, 
            GenericSettingsFunctions.GetConstValuesPoints(Rl.saveClipBoard.Star1Value).ToString(),
            GenericSettingsFunctions.GetConstValuesPoints(Rl.saveClipBoard.Star2Value).ToString(),
            GenericSettingsFunctions.GetConstValuesPoints(Rl.saveClipBoard.Star3Value).ToString(),
            GenericSettingsFunctions.GetConstValuesPoints(Rl.saveClipBoard.TrophyValue).ToString()
        );
        ConvertToMoveValues();
    }
    

    private void DelteTextString(params TMP_InputField[] textToDelete)
    {
        for (int i = 0; i < textToDelete.Length; i++)
        {
            textToDelete[i].text = string.Empty;
        }
    }

    private void DelteTextString(params TextMeshProUGUI[] textToDelete)
    {
        for (int i = 0; i < textToDelete.Length; i++)
        {
            textToDelete[i].text = string.Empty;
        }
    }
    
    public void SwitchButtonOff(string star) => SwitchThings(ParseStringToEnum(star), false, false);

    public void SwitchButtonOn(string star) => SwitchThings(ParseStringToEnum(star), true, false);

    private Star ParseStringToEnum(string star)
    {
        Star starEnum = Star.Star1;
        switch (star)
        {
            case "Star1":
                starEnum = Star.Star1;
                break;
            case "Star2":
                starEnum = Star.Star2;
                break;
            case "Star3":
                starEnum = Star.Star3;
                break;
            case "Trophy":
                starEnum = Star.Trophy;
                break;
        }

        return starEnum;
    }
    private void SwitchThings(Star star,  bool on, bool playNoSound)
    {
       
        switch (star)
        {
            case Star.Star1:
                SwitchButtonStar1.SwitchButton(on, playNoSound);
                Rl.saveClipBoard.Star1Enabled = on;
                break;
            
            case Star.Star2:
                
                SwitchButtonStar2.SwitchButton(on, playNoSound);
                Rl.saveClipBoard.Star2Enabled = on;
                break;
            
            case Star.Star3:
                SwitchButtonStar3.SwitchButton(on, playNoSound);
                Rl.saveClipBoard.Star3Enabled = on;
                break;
            
            case Star.Trophy:
                SwitchButtonTrophy.SwitchButton(on, playNoSound);
                Rl.saveClipBoard.TrophyEnabled = on;
                break;
        }
    }
    
    private void SetStarSettings(bool playNoSound)
    {
        if(Rl.saveClipBoard.Star1Enabled)SwitchThings(Star.Star1, true, playNoSound);
        else SwitchThings(Star.Star1, false, playNoSound);
        
        if(Rl.saveClipBoard.Star2Enabled)SwitchThings(Star.Star2, true, playNoSound);
        else SwitchThings(Star.Star2, false, playNoSound);
        
        if(Rl.saveClipBoard.Star3Enabled)SwitchThings(Star.Star3, true, playNoSound);
        else SwitchThings(Star.Star3, false, playNoSound);
        
        if(Rl.saveClipBoard.TrophyEnabled)SwitchThings(Star.Trophy, true, playNoSound);
        else SwitchThings(Star.Trophy, false, playNoSound);
    }
    
    public ScoreGoalsConfig SaveBoardsSettingsFromClipBoard() => new
    (
        Rl.saveClipBoard.Star1Value,
        Rl.saveClipBoard.Star2Value,
        Rl.saveClipBoard.Star3Value,
        Rl.saveClipBoard.TrophyValue,
        Rl.saveClipBoard.Star1Enabled,
        Rl.saveClipBoard.Star2Enabled,
        Rl.saveClipBoard.Star3Enabled,
        Rl.saveClipBoard.TrophyEnabled,
        Rl.saveClipBoard.Star1NeededNextLevel,
        Rl.saveClipBoard.Star2NeededNextLevel,
        Rl.saveClipBoard.Star3NeededNextLevel,
        Rl.saveClipBoard.TrophyNeededNextLevel,
        Rl.saveClipBoard.TrophyNoEmpty,
        Rl.saveClipBoard.TrophyNumberAreMovesOrTime
    );

    private void ConvertToMoveValues()
    {
        if (Rl.saveClipBoard.TrophyNoEmpty || Rl.saveClipBoard.TrophyNumberAreMovesOrTime)
        {
            string emptyOrMovesValue = GenericSettingsFunctions.GetConstValuesTrophyEM(Rl.saveClipBoard.TrophyValue).ToString();
            TrophyText.text = emptyOrMovesValue;
            TrophyInputText.text = emptyOrMovesValue;
        }
    }
}
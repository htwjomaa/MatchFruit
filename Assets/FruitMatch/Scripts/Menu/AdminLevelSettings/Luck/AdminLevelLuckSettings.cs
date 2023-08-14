using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AdminLevelLuckSettings : MonoBehaviour
{
    [SerializeField] public Slider NeededPieces;
    [SerializeField] public Slider NeededPiecesOverTime;
    [SerializeField] public Slider BeneficialExtras;
    [SerializeField] public Slider BeneficialExtrasOverTime;
    [SerializeField] public Slider MaliciousExtras;
    [SerializeField] public Slider MaliciousExtrasOverTime;
    [SerializeField] public Slider Overall;


    [SerializeField] private AcceptSwitchSimple NeededPiecesAcceptSwitch;
    [SerializeField] private AcceptSwitchSimple  BeneficialExtrasAcceptSwitch;
    [SerializeField] private AcceptSwitchSimple  MaliciousExtrasAcceptSwitch;
    private void RemoveListeners()
    {
        NeededPieces.onValueChanged.RemoveAllListeners();
        NeededPiecesOverTime.onValueChanged.RemoveAllListeners();
        BeneficialExtras.onValueChanged.RemoveAllListeners();
        BeneficialExtrasOverTime.onValueChanged.RemoveAllListeners();
        MaliciousExtras.onValueChanged.RemoveAllListeners();
        MaliciousExtrasOverTime.onValueChanged.RemoveAllListeners();
        Overall.onValueChanged.RemoveAllListeners();
    }
    private void Addlisteners()
    {
        NeededPieces.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        NeededPiecesOverTime.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        BeneficialExtras.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        BeneficialExtrasOverTime.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        MaliciousExtras.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        MaliciousExtrasOverTime.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        Overall.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }
    public void LoadLuckSettings(LuckConfig luckConfig)
    {
        RemoveListeners();
        LoadLuckSettingsToClipBoard(luckConfig);
        ResizeFieldArrays();
        ClipBoardToSlider();
        LoadAcceptSwitches();
        Addlisteners();
        ValueChangeCheck();
    }

    private static void ResizeFieldArrays()
    {
        if (Rl.saveClipBoard.NeededPieces == null || Rl.saveClipBoard.NeededPiecesOverTime == null ||
            Rl.saveClipBoard.BeneficialExtras == null || Rl.saveClipBoard.BeneficialExtrasOverTime == null ||
            Rl.saveClipBoard.MaliciousExtras == null ||
            Rl.saveClipBoard.MaliciousExtrasOverTime == null || Rl.saveClipBoard.Overall == null ||
            Rl.saveClipBoard.NeededPiecesOnlyStart == null || Rl.saveClipBoard.BeneficialExtrasOnlyStart == null ||
            Rl.saveClipBoard.MaliciousExtrasOnlyStart == null)
        {
            Array.Resize(ref Rl.saveClipBoard.NeededPieces, SaveFileLevelConfigs.Fields);
            Array.Resize(ref Rl.saveClipBoard.NeededPiecesOverTime, SaveFileLevelConfigs.Fields);
            Array.Resize(ref Rl.saveClipBoard.BeneficialExtras, SaveFileLevelConfigs.Fields);
            Array.Resize(ref Rl.saveClipBoard.BeneficialExtrasOverTime, SaveFileLevelConfigs.Fields);
            Array.Resize(ref Rl.saveClipBoard.MaliciousExtras, SaveFileLevelConfigs.Fields);
            Array.Resize(ref Rl.saveClipBoard.MaliciousExtrasOverTime, SaveFileLevelConfigs.Fields);
            Array.Resize(ref Rl.saveClipBoard.Overall, SaveFileLevelConfigs.Fields);
            Array.Resize(ref Rl.saveClipBoard.NeededPiecesOnlyStart, SaveFileLevelConfigs.Fields);
            Array.Resize(ref Rl.saveClipBoard.BeneficialExtrasOnlyStart, SaveFileLevelConfigs.Fields);
            Array.Resize(ref Rl.saveClipBoard.MaliciousExtrasOnlyStart, SaveFileLevelConfigs.Fields);


            SetTo05(ref Rl.saveClipBoard.NeededPieces);
            SetTo05(ref Rl.saveClipBoard.NeededPiecesOverTime);
            SetTo05(ref Rl.saveClipBoard.BeneficialExtras);
            SetTo05(ref Rl.saveClipBoard.BeneficialExtrasOverTime);
            SetTo05(ref Rl.saveClipBoard.MaliciousExtras);
            SetTo05(ref Rl.saveClipBoard.MaliciousExtrasOverTime);
            SetTo05(ref Rl.saveClipBoard.Overall);
        }
    }

    private static void SetTo05(ref float[] array)
    {
        for (int i = 0; i < array.Length; i++) array[i] = 0.5f;
    }

    private void ClipBoardToSlider()
    {
        NeededPieces.value = Rl.saveClipBoard.NeededPieces[FieldState.CurrentField] ;
        NeededPiecesOverTime.value = Rl.saveClipBoard.NeededPiecesOverTime[FieldState.CurrentField] ;
        BeneficialExtras.value = Rl.saveClipBoard.BeneficialExtras[FieldState.CurrentField] ;
        BeneficialExtrasOverTime.value = Rl.saveClipBoard.BeneficialExtrasOverTime[FieldState.CurrentField] ;
        MaliciousExtras.value = Rl.saveClipBoard.MaliciousExtras[FieldState.CurrentField] ;
        MaliciousExtrasOverTime.value = Rl.saveClipBoard.MaliciousExtrasOverTime[FieldState.CurrentField] ;
        Overall.value = Rl.saveClipBoard.Overall[FieldState.CurrentField] ;
    }

    private void LoadLuckSettingsToClipBoard(LuckConfig luckConfig)
    {
        Rl.saveClipBoard.NeededPieces =  luckConfig.NeededPieces;
        Rl.saveClipBoard.NeededPiecesOverTime = luckConfig.NeededPiecesOverTime;
        Rl.saveClipBoard.BeneficialExtras = luckConfig.BeneficialExtras;
        Rl.saveClipBoard.BeneficialExtrasOverTime = luckConfig.BeneficialExtrasOverTime;
        Rl.saveClipBoard.MaliciousExtras = luckConfig.MaliciousExtras;
        Rl.saveClipBoard.MaliciousExtrasOverTime = luckConfig.MaliciousExtrasOverTime;
        Rl.saveClipBoard.Overall = luckConfig.Overall;

        Rl.saveClipBoard.NeededPiecesOnlyStart = luckConfig.NeededPiecesOnlyStart;
        Rl.saveClipBoard.BeneficialExtrasOnlyStart = luckConfig.BeneficialExtrasOnlyStart;
        Rl.saveClipBoard.MaliciousExtrasOnlyStart = luckConfig.MaliciousExtrasOnlyStart;
    }

    private void ValueChangeCheck()
    {
       Rl.saveClipBoard.NeededPieces[FieldState.CurrentField] =  NeededPieces.value;
       Rl.saveClipBoard.NeededPiecesOverTime[FieldState.CurrentField]  = NeededPiecesOverTime.value;
       Rl.saveClipBoard.BeneficialExtras[FieldState.CurrentField]  = BeneficialExtras.value;
       Rl.saveClipBoard.BeneficialExtrasOverTime[FieldState.CurrentField]  = BeneficialExtrasOverTime.value; 
       Rl.saveClipBoard.MaliciousExtras[FieldState.CurrentField]  = MaliciousExtras.value;
       Rl.saveClipBoard.MaliciousExtrasOverTime[FieldState.CurrentField]  = MaliciousExtrasOverTime.value;
       Rl.saveClipBoard.Overall[FieldState.CurrentField]  = Overall.value;
       UpdateTextFields();
    }
    
    public LuckConfig SaveLuckSettingsFromClipBoard() => new 
        (
            Rl.saveClipBoard.NeededPieces,
            Rl.saveClipBoard.NeededPiecesOverTime,
            Rl.saveClipBoard.BeneficialExtras,
            Rl.saveClipBoard.BeneficialExtrasOverTime,
            Rl.saveClipBoard.MaliciousExtras,
            Rl.saveClipBoard.MaliciousExtrasOverTime,
            Rl.saveClipBoard.Overall,
            Rl.saveClipBoard.NeededPiecesOnlyStart,
            Rl.saveClipBoard.BeneficialExtrasOnlyStart,
            Rl.saveClipBoard.MaliciousExtrasOnlyStart
        );


    private void LoadAcceptSwitches()
    {
        NeededPiecesAcceptSwitch.SwitchButton(Rl.saveClipBoard.NeededPiecesOnlyStart[FieldState.CurrentField] ,true,false);
        BeneficialExtrasAcceptSwitch.SwitchButton(Rl.saveClipBoard.BeneficialExtrasOnlyStart[FieldState.CurrentField] ,true,false);
        MaliciousExtrasAcceptSwitch.SwitchButton(Rl.saveClipBoard.MaliciousExtrasOnlyStart[FieldState.CurrentField] ,true,false);
    }

    public void ClickSwitchNeededPiecesAcceptSwitch(bool on)
    {
        NeededPiecesAcceptSwitch.SwitchButton(on, false, true);
        Rl.saveClipBoard.NeededPiecesOnlyStart[FieldState.CurrentField]  = on;
    }

    public void ClickSwitchBeneficialExtrasAcceptSwitch(bool on)
    {
        BeneficialExtrasAcceptSwitch.SwitchButton(on, false, true);
        Rl.saveClipBoard.BeneficialExtrasOnlyStart[FieldState.CurrentField]  = on;
    }

    public void ClickSwitchMaliciousExtrasAcceptSwitch(bool on)
    {
        MaliciousExtrasAcceptSwitch.SwitchButton(on, false, true);
        Rl.saveClipBoard.MaliciousExtrasOnlyStart[FieldState.CurrentField]  = on;
    }
    public void SetOverallToMidValue(Transform buttonTransform) => SetToMidvalue(buttonTransform, Overall);
    public void SetNeededPiecesToMidValue(Transform buttonTransform) => SetToMidvalue(buttonTransform, NeededPieces, NeededPiecesOverTime);
    public void SetBeneficialExtrasToMidValue(Transform buttonTransform) => SetToMidvalue(buttonTransform,BeneficialExtras, BeneficialExtrasOverTime);
    public void SetMaliciousExtrasToMidValue(Transform buttonTransform) => SetToMidvalue(buttonTransform, MaliciousExtras, MaliciousExtrasOverTime);
    
    private void SetToMidvalue(Transform buttonTransform, params Slider[] slider)
    {
        if(transform != null) GenericSettingsFunctions.SmallJumpAnimation(buttonTransform);
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        foreach (Slider s in slider) s.value = s.maxValue / 2;
    }

    [SerializeField] private TextMeshProUGUI NeededPiecesText;
    [SerializeField] private TextMeshProUGUI NeededPiecesOverTimeText;
    
    [SerializeField] private TextMeshProUGUI BeneficialExtrasText;
    [SerializeField] private TextMeshProUGUI BeneficialExtrasOverTimeText;
    
    [SerializeField] private TextMeshProUGUI MaliciousExtrasText;
    [SerializeField] private TextMeshProUGUI MaliciousExtrasOverTimeText;
    [SerializeField] private TextMeshProUGUI OverallText;
    private void UpdateTextFields()
    {
        NeededPiecesText.text = HundredString(NeededPieces.value);
        NeededPiecesOverTimeText.text = HundredString(NeededPiecesOverTime.value);
        
        BeneficialExtrasText.text = HundredString(BeneficialExtras.value);
        BeneficialExtrasOverTimeText.text = HundredString(BeneficialExtrasOverTime.value);
        
        MaliciousExtrasText.text = HundredString(MaliciousExtras.value);
        MaliciousExtrasOverTimeText.text = HundredString(MaliciousExtrasOverTime.value);

        OverallText.text = HundredString(Overall.value);
    }

    private string HundredString (float value) => ((int)MathLibrary.Remap(0, 1f, -100f, 100f, value)).ToString();

    public void LoadCurrentField()
    {
        RemoveListeners();
        ClipBoardToSlider();
        LoadAcceptSwitches();
        Addlisteners();
        ValueChangeCheck();
    }

    public void CopyField(byte fieldOne, byte fieldTwo)
    {
        Rl.saveClipBoard.NeededPieces[fieldTwo] = Rl.saveClipBoard.NeededPieces[fieldOne];
        Rl.saveClipBoard.NeededPiecesOverTime[fieldTwo] = Rl.saveClipBoard.NeededPiecesOverTime[fieldOne];
        Rl.saveClipBoard.BeneficialExtras[fieldTwo] = Rl.saveClipBoard.BeneficialExtras[fieldOne];
        Rl.saveClipBoard.BeneficialExtrasOverTime[fieldTwo] = Rl.saveClipBoard.BeneficialExtrasOverTime[fieldOne];
        Rl.saveClipBoard.MaliciousExtras[fieldTwo] = Rl.saveClipBoard.MaliciousExtras[fieldOne];
        Rl.saveClipBoard.MaliciousExtrasOverTime[fieldTwo] = Rl.saveClipBoard.MaliciousExtrasOverTime[fieldOne];
        Rl.saveClipBoard.Overall[fieldTwo] = Rl.saveClipBoard.Overall[fieldOne];
        Rl.saveClipBoard.NeededPiecesOnlyStart[fieldTwo] = Rl.saveClipBoard.NeededPiecesOnlyStart[fieldOne];
        Rl.saveClipBoard.BeneficialExtrasOnlyStart[fieldTwo] = Rl.saveClipBoard.BeneficialExtrasOnlyStart[fieldOne];
        Rl.saveClipBoard.MaliciousExtrasOnlyStart[fieldTwo] = Rl.saveClipBoard.MaliciousExtrasOnlyStart[fieldOne];
    }
}
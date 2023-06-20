using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdminLevelLuckSettings : MonoBehaviour
{
    [SerializeField] public Slider NeededPieces;
    [SerializeField] public Slider NeededPiecesOverTime;
    [SerializeField] public Slider BeneficialExtras;
    [SerializeField] public Slider BeneficialExtrasOverTime;
    [SerializeField] public Slider MaliciousExtras;
    [SerializeField] public Slider MaliciousExtrasOverTime;
    private void RemoveListeners()
    {
        NeededPieces.onValueChanged.RemoveAllListeners();
        NeededPiecesOverTime.onValueChanged.RemoveAllListeners();
        BeneficialExtras.onValueChanged.RemoveAllListeners();
        BeneficialExtrasOverTime.onValueChanged.RemoveAllListeners();
        MaliciousExtras.onValueChanged.RemoveAllListeners();
        MaliciousExtrasOverTime.onValueChanged.RemoveAllListeners();
    }
    private void Addlisteners()
    {
        NeededPieces.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        NeededPiecesOverTime.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        BeneficialExtras.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        BeneficialExtrasOverTime.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        MaliciousExtras.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        MaliciousExtrasOverTime.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }
    public void LoadLuckSettings(LuckConfig luckConfig)
    {
        RemoveListeners();
        LoadLuckSettingsToClipBoard(luckConfig);
        ClipBoardToSlider();
        Addlisteners();
        ValueChangeCheck();
    }
    public void ClipBoardToSlider()
    {
        NeededPieces.value = Rl.saveClipBoard.NeededPieces;
        NeededPiecesOverTime.value = Rl.saveClipBoard.NeededPiecesOverTime;
        BeneficialExtras.value = Rl.saveClipBoard.BeneficialExtras;
        BeneficialExtrasOverTime.value = Rl.saveClipBoard.BeneficialExtrasOverTime;
        MaliciousExtras.value = Rl.saveClipBoard.MaliciousExtras;
        MaliciousExtrasOverTime.value = Rl.saveClipBoard.MaliciousExtrasOverTime;
    }

    public void LoadLuckSettingsToClipBoard(LuckConfig luckConfig)
    {
        Rl.saveClipBoard.NeededPieces =  luckConfig.NeededPieces;
        Rl.saveClipBoard.NeededPiecesOverTime = luckConfig.NeededPiecesOverTime;
        Rl.saveClipBoard.BeneficialExtras = luckConfig.BeneficialExtras;
        Rl.saveClipBoard.BeneficialExtrasOverTime = luckConfig.BeneficialExtrasOverTime;
        Rl.saveClipBoard.MaliciousExtras = luckConfig.MaliciousExtras;
        Rl.saveClipBoard.MaliciousExtrasOverTime = luckConfig.MaliciousExtrasOverTime;
    }

    public void ValueChangeCheck()
    {
       Rl.saveClipBoard.NeededPieces =  NeededPieces.value;
       Rl.saveClipBoard.NeededPiecesOverTime = NeededPiecesOverTime.value;
       Rl.saveClipBoard.BeneficialExtras = BeneficialExtras.value;
       Rl.saveClipBoard.BeneficialExtrasOverTime = BeneficialExtrasOverTime.value; 
       Rl.saveClipBoard.MaliciousExtras = MaliciousExtras.value;
       Rl.saveClipBoard.MaliciousExtrasOverTime = MaliciousExtrasOverTime.value;
       UpdateTextFields();
    }
    
    public LuckConfig SaveLuckSettingsFromClipBoard() => new 
        (
            Rl.saveClipBoard.NeededPieces,
            Rl.saveClipBoard.NeededPiecesOverTime,
            Rl.saveClipBoard.BeneficialExtras,
            Rl.saveClipBoard.BeneficialExtrasOverTime,
            Rl.saveClipBoard.MaliciousExtras,
            Rl.saveClipBoard.MaliciousExtrasOverTime
        );


    public void SetNeededPiecesToMidValue() => SetToMidvalue(NeededPieces, NeededPiecesOverTime);
    public void SetBeneficialExtrasToMidValue() => SetToMidvalue(BeneficialExtras, BeneficialExtrasOverTime);
    public void SetMaliciousExtrasToMidValue() => SetToMidvalue(MaliciousExtras, MaliciousExtrasOverTime);
    
    private void SetToMidvalue(params Slider[] slider)
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        foreach (Slider s in slider) s.value = s.maxValue / 2;
    }

    [SerializeField] private TextMeshProUGUI NeededPiecesText;
    [SerializeField] private TextMeshProUGUI NeededPiecesOverTimeText;
    
    [SerializeField] private TextMeshProUGUI BeneficialExtrasText;
    [SerializeField] private TextMeshProUGUI BeneficialExtrasOverTimeText;
    
    [SerializeField] private TextMeshProUGUI MaliciousExtrasText;
    [SerializeField] private TextMeshProUGUI MaliciousExtrasOverTimeText;
    private void UpdateTextFields()
    {
        NeededPiecesText.text = HundredString(NeededPieces.value);
        NeededPiecesOverTimeText.text = HundredString(NeededPiecesOverTime.value);
        
        BeneficialExtrasText.text = HundredString(BeneficialExtras.value);
        BeneficialExtrasOverTimeText.text = HundredString(BeneficialExtrasOverTime.value);
        
        MaliciousExtrasText.text = HundredString(MaliciousExtras.value);
        MaliciousExtrasOverTimeText.text = HundredString(MaliciousExtrasOverTime.value);
    }

    private string HundredString (float value) => ((int)MathLibrary.Remap(0, 1f, -100f, 100f, value)).ToString();
}
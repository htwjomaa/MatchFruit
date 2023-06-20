using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AdminLevelSettingsCopyHelper : MonoBehaviour
{
    [SerializeField] private List<Sprite> FieldSprites = new();
    [SerializeField] private Button copyButton;
    [SerializeField] private Button firstFieldButton;
    [SerializeField] private Button secondFieldButton;
    public CopySection CopySection;
    public byte firstField = 0;
    public byte secondField = 1;


    public void ClickCopy()
    {
        if (firstField == secondField) return;
        GenericSettingsFunctions.SmallShake(copyButton.transform);
        Rl.GameManager.PlayAudio(Rl.soundStrings.Click, Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        FieldState.CopyField(firstField, secondField);
    }
    
    
    public void ClickNextFieldOne()
    {
        GenericSettingsFunctions.SmallJumpAnimation(firstFieldButton.transform);
        Rl.GameManager.PlayAudio(Rl.soundStrings.Click, Random.Range(0,2), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        firstFieldButton.image.sprite = GetNextField(ref firstField);
    }
    public void ClickNextFieldTwo()
    { 
        GenericSettingsFunctions.SmallJumpAnimation(secondFieldButton.transform);
        Rl.GameManager.PlayAudio(Rl.soundStrings.Click, Random.Range(3,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource); 
        secondFieldButton.image.sprite = GetNextField(ref secondField);
    }
    private Sprite GetNextField(ref byte field)
    {
        field += 1;
        if (field > SaveFileLevelConfigs.Fields-1) field = 0;
        
        return FieldSprites[field];
    }
}
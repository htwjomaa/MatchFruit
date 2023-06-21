using UnityEngine;
using Random = UnityEngine.Random;

public sealed class Switch : MonoBehaviour
{
    
    [SerializeField] private GameObject SwitchButtonOn;
    [SerializeField] private GameObject SwitchBackgroundOn;
    [SerializeField] private GameObject SwitchButtonOff;
    [SerializeField] private GameObject SwitchBackgroundOff;
    [SerializeField] public string UniqueIdentifier;
    
    public void SwitchButton(bool on, bool playNoSound, bool animation = true)
    {
        if (!playNoSound)
        {
            if(on)   Rl.GameManager.PlayAudio(Rl.soundStrings.SwitchSound, Random.Range(2,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
            if(!on)  Rl.GameManager.PlayAudio(Rl.soundStrings.SwitchSound, Random.Range(0,2), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        }
        
        SwitchBackgroundOn.SetActive(on);
        SwitchButtonOn.SetActive(on);
        
        SwitchButtonOff.SetActive(!on);
        SwitchBackgroundOff.SetActive(!on);
        if(animation)GenericSettingsFunctions.SmallJumpAnimation(transform);
    }

    public bool GetEnabledStatus()
    {
        return SwitchButtonOn.activeSelf;
    }
}

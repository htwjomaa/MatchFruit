using UnityEngine;

public class AcceptSwitchSimple : MonoBehaviour
{
    [SerializeField] private GameObject SwitchOff;

    [SerializeField] private GameObject SwitchOn;

    public void SwitchButton(bool on, bool playNoSound, bool animation = true)
    {
        if (!playNoSound)
        {
            if(on)   Rl.GameManager.PlayAudio(Rl.soundStrings.AcceptSwitchSound, Random.Range(2,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
            if(!on)  Rl.GameManager.PlayAudio(Rl.soundStrings.AcceptSwitchSound, Random.Range(0,2), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        }
        
        SwitchOff.SetActive(!on);
        SwitchOn.SetActive(on);
        
        if(animation)GenericSettingsFunctions.SmallJumpAnimation(transform);
    }

    public bool GetEnabledStatus() => SwitchOn.activeSelf;
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class TargetSettings : MonoBehaviour
{
    public static event Action ShowItemEvent;

    public  void InvokeShowItemEvent()
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.ResetToMidValueSound, Random.Range(0,3), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        GenericSettingsFunctions.SmallJumpAnimation(transform);
        ShowItemEvent?.Invoke();
    }
}

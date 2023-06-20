using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ParticleHelper : MonoBehaviour
{
    public ParticleSystem ParticleSystem;

    [Button]
    private void PlayParticletest()
    {
        if(!ParticleSystem.isPlaying) ParticleSystem.Play();
        
    }
}

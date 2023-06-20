using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class Shake : MonoBehaviour
{
    [SerializeField] private CameraShake _cameraShake;


    [UnityEngine.Range(0f, 1)] [SerializeField] private float ShakeNoise = 0.56f;
    [Space]
    [UnityEngine.Range(0f, 1)] [SerializeField] private float ShakeRotation = 0.84f;
    [Space]
    [UnityEngine.Range(0f, 2)] [SerializeField] private float ShakeStrength = 0.56f;

    [Space]
    [UnityEngine.Range(0f, 5)] [SerializeField] private float ShakeDuration = 1.32f;
 
    [Space]
    [UnityEngine.Range(0f, 8)] [SerializeField] private float ShakeSpeed = 0.53f;
[UnityEngine.Range(0f, 8)] [SerializeField] private float damping= 0.53f;
   [Button()] private void StartShake()
    {
      var  shakeAngle = Random.Range(1,9);
        _cameraShake.StartShake(new CameraShake.Einstellungen(shakeAngle, ShakeStrength , ShakeSpeed , 
            ShakeDuration , ShakeNoise , damping , ShakeRotation ));
    }
}

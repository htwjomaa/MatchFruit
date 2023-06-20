using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    public AudioSource audioSource;
    void Awake() => audioSource = GetComponent<AudioSource>();
}

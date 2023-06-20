using UnityEngine;

public sealed class Effects : MonoBehaviour
{
    public AudioSource audioSource;
    void Awake() => audioSource = GetComponent<AudioSource>();
    
}
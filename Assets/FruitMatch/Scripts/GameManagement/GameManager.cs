using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class GameManager : MonoBehaviour
{
    [SerializeField] public AudioDatabase audiodataBase;
    [SerializeField] public AudioSource gameManagerAudioSource;
    List<AudioClip> clipsPlaying = new List<AudioClip>();
    private static float[] fullresolutions =
    {
        0.25f, 0.5f, 0.75f, 1f
    };

    IEnumerator setSettingagainCO(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SetResIOS();
    }
    private void SetResIOS()
    {
        int height = Rl.settings.GetResolution.Height;
        int width = Rl.settings.GetResolution.Width;
        Screen.SetResolution((int)(height*fullresolutions[Rl.settings.FullResolution]), (int)(width*fullresolutions[Rl.settings.FullResolution]), true);
        Debug.Log("WidthPixel:  " + (int)(height*fullresolutions[Rl.settings.FullResolution]) + "| Height Pixel: " + (int)(width*fullresolutions[Rl.settings.FullResolution]));
        Application.targetFrameRate = 60;
        
        Rl.settings.SetGraphicSettings();
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
    #if UNITY_ANDROID && !UNITY_EDITOR
            SetRatio(4,3,   Rl.settings.FullResolution);
#endif

    #if !UNITY_ANDROID && !UNITY_EDITOR
        SetResIOS();
        StartCoroutine(setSettingagainCO(0.5f));
#endif
    }
  
    
    void SetRatio(float w, float h, int fullResolution)
    {
        if (Screen.width / (float)Screen.height > w / h)
        {
            Debug.Log("Width Pixel:  " + (int)(Screen.height *fullresolutions[fullResolution] * (w / h)) + "| Height Pixel: " + (int)(Screen.height*fullresolutions[fullResolution]));
            Screen.SetResolution((int)(Screen.height *fullresolutions[fullResolution] * (w / h)), (int)(Screen.height*fullresolutions[fullResolution]), true);
        }
        else
        {
            Debug.Log("Width Pixel:  " + (int)(Screen.width*fullresolutions[fullResolution]) + "| Height Pixel: " + (int)(Screen.width*fullresolutions[fullResolution] * (h / w)));
            Screen.SetResolution((int)(Screen.width*fullresolutions[fullResolution]), (int)(Screen.width*fullresolutions[fullResolution] * (h / w)), true);
        }
       
    }
 
    public void PlayLimitSound(string audioName, int height, float volume, AudioSource audioSource)
    {
        AudioSettings audiosettings = audiodataBase.allAudioSettings[audiodataBase.allAudioNames[audioName].index];
        if (clipsPlaying.IndexOf(audiosettings.audioClip) < 0)
        {
            clipsPlaying.Add(audiosettings.audioClip);
            PlayAudio(audioName, height, volume, audioSource);
            StartCoroutine(WaitForCompleteSound(audiosettings.audioClip));
        }
    }
    IEnumerator WaitForCompleteSound(AudioClip clip)
    {
        yield return new WaitForSeconds(0.2f);
        clipsPlaying.Remove(clipsPlaying.Find(x => clip));
    }
    public void StopAudio(AudioSource audioSource) => audioSource.Stop();
    // public void StopAudio(AudioSource audioSource, float timer)
    // {
    //     timer =
    //     audioSource.Stop();
    // }
    public void PlayAudio(string audioName, int height, bool playOneShot, float volume, AudioSource audioSource) =>
        PlayAudio(audioName, height, audioSource, playOneShot, false, volume);
    public void PlayAudio(string audioName, float volume) => PlayAudio(audioName, 2, null, true, false, volume);
    public void PlayAudio(string audioName, int height, float volume, AudioSource audioSource) => PlayAudio(audioName, height, audioSource, true, false, volume);
    public void PlayAudio(string audioName, bool playOneShot, float volume) => PlayAudio(audioName, 2, null, playOneShot, false, volume);
    public void PlayAudio(string audioName, int height, float volume) => PlayAudio(audioName, height, null, true, false, volume);
    public void PlayAudio(string audioName, int height, bool playOneShot, float volume) => PlayAudio(audioName, height, null, playOneShot, false, volume);
    public void PlayAudio(string audioName, int height, AudioSource audioSource, float volume) => PlayAudio(audioName, height, audioSource, true, volume);
   public void PlayAudio(string audioName,int height, bool playOneShot, bool randomize, float volume) => PlayAudio(audioName, height, null, playOneShot, randomize, volume);

   public void PlayAudio(string audioName, int height, AudioSource audioSource, bool playOneShot, bool randomize, float volume)
    {
        
        if (audioSource == null) audioSource = this.gameManagerAudioSource;

        if (CheckIfMuted(audioSource)) return;
        
        if (!audiodataBase.allAudioNames.ContainsKey(audioName))
        {
//            Debug.Log(audioName + " string was not found");
            return;
        }
        
        AudioSettings audiosettings = audiodataBase.allAudioSettings[audiodataBase.allAudioNames[audioName].index];
        float pitchValue = 1;
        if (height > audiosettings.pitch.Length - 1) height = audiosettings.pitch.Length - 1;
        else if (height < 0) height = 0;
        
        audioSource.clip = audiosettings.audioClip;
        if (audiosettings.fixedVolume) volume = audiosettings.volume;
        else volume *= audiosettings.volume / 100;
     
        audioSource.volume = volume;
       
       if(!audiosettings.noPitching)
        { 
            pitchValue = ParsePitchValue(audiosettings.pitch[height]);
            if (audiosettings.AddSomeRandomnessToEachPitchValue || randomize) pitchValue = RandomizePitch(pitchValue);
        }

       audioSource.pitch = pitchValue;
        if(playOneShot) audioSource.PlayOneShot(audiosettings.audioClip);
        else audioSource.Play();
    }

    private float RandomizePitch(float pitchValue)
    {
        if (pitchValue >= 0)
        {
            pitchValue =  Random.Range(pitchValue-.3f, pitchValue+.3f);
            if (pitchValue > 3) pitchValue = 3;
        }
        else if (pitchValue < 0)
        {
            pitchValue =  Random.Range(pitchValue-.1f, pitchValue+.1f);
            if (pitchValue < .1) pitchValue = .1f;
        }
        
        return pitchValue;
    }

    private float ParsePitchValue(float pitchValue)  //BLÖDES UNITY.  Muss die 0 umgehen und   0 bei mir ist gleich 1 bei unity
    {
        if(pitchValue >=0) pitchValue = MathLibrary.Remap(0, 1, 1, 3, pitchValue);
        else pitchValue = MathLibrary.Remap(-1, 0, 0.12f, 1, pitchValue);
        return pitchValue;
    }

    private bool CheckIfMuted(AudioSource audioSource)
    {
        if (Rl.settings.MasterMutedProperty) return true;
        
        if (gameManagerAudioSource.Equals(audioSource)  && Rl.settings.MusicMutedProperty) return true ;
        if (Rl.effects.audioSource.Equals(audioSource) && Rl.settings.SFXMuted) return true;
        if (Rl.uiSounds.audioSource.Equals(audioSource) && Rl.settings.UIMuted) return true;
        
        return false;
    }
}
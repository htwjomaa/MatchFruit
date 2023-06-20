using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class PlayAudio : MonoBehaviour
{
    [SerializeField] private List<string> loadedBackgroundMusic;
    private Queue<ClipLengthAndName> randomizedMusic;
    [SerializeField] private LevelName Levelname = LevelName.Jungle;
    private float timer = 0;
    public static PlayAudio THIS;

    private void Awake()
    {
        THIS = this;
    }

    IEnumerator DelayedStartCo(float seconds)
    {
        AddBackgroundMusic(Levelname);
        yield return new WaitForSeconds(seconds);
      
        PlaybackgroundMusic();
    }
    public void StartIngameMusic()
    {
        StartCoroutine(DelayedStartCo(3.2f));
    }
    private void AddBackgroundMusic(LevelName levelName)
    {
        foreach(LevelBackGroundMusic levelBackgroundMusic in Rl.soundStrings.LevelBackGroundMusic)
            if (levelBackgroundMusic.levelName == levelName)
            {
                foreach (string musicTitle in levelBackgroundMusic.BGMusicCollection)
                    loadedBackgroundMusic.Add(musicTitle);
            }
    }

    private string currentMusic;
    private string oldMusic = "null";
    
    IEnumerator Coroutine_WaitTillNextMusic(float timer)
    {
        yield return new WaitForSeconds(timer);
        oldMusic = currentMusic;
        PlaybackgroundMusic();
    }

    public void PlaybackgroundMusic()
    {
        currentMusic = loadedBackgroundMusic[Random.Range(0, loadedBackgroundMusic.Count)];
        Debug.Log(currentMusic);
        while(currentMusic == oldMusic)
            currentMusic = loadedBackgroundMusic[Random.Range(0, loadedBackgroundMusic.Count)];
        timer = Rl.GameManager.audiodataBase.allAudioSettings[Rl.GameManager.audiodataBase.allAudioNames[loadedBackgroundMusic[0]].index].audioClip.length+2f;
        
        Rl.GameManager.PlayAudio(currentMusic, Random.Range(0,5), true, Rl.settings.GetMusicVolume);
        Rl.GameManager.gameManagerAudioSource.volume = Rl.settings.GetMusicVolume * Rl.settings.GetMasterVolume;
        StartCoroutine(Coroutine_WaitTillNextMusic(timer));
    }
}
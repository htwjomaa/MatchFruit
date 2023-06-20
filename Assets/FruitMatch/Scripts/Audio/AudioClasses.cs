using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable] public sealed class AudioSettings
{
    public AudioClip audioClip;
    public bool noPitching;
    public bool AddSomeRandomnessToEachPitchValue;
    [Range(0,100)]public float volume;
    public bool fixedVolume = false;
    [Range(-1,1)]public float[] pitch;
   
    public AudioSettings(AudioClip audioClip, bool noPitching, bool addSomeRandomnessToEachPitchValue, float[] pitch, float volume, bool  fixedVolume)
    {
        this.audioClip = audioClip;
        this.noPitching = noPitching;
        this.AddSomeRandomnessToEachPitchValue = addSomeRandomnessToEachPitchValue;
        this.pitch = pitch;
        if (volume == 0) volume = 100;
        this.volume = volume;
        this.fixedVolume = fixedVolume;
    }
}
[Serializable] public struct ClipNameandID
{
    public string clipName;
    public int index;
    
    public ClipNameandID(string clipName,int index)
    {
        this.clipName = clipName;
        this.index = index;
    }
}

[Serializable] public struct ClipLengthAndName
{
    public float clipLength;
    public string clipName;
    
    public ClipLengthAndName(float clipLength, string clipName)
    {
        this.clipLength = clipLength;
        this.clipName = clipName;
    }
}

[Serializable] public struct LevelBackGroundMusic
{
    public LevelName levelName;
    public List<string> BGMusicCollection;
    LevelBackGroundMusic(LevelName levelName, List<string> BGMusicCollection)
    {
        this.levelName = levelName;
        this.BGMusicCollection= BGMusicCollection;
    }
}
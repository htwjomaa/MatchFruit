using System;
using UnityEngine.Serialization;

[Serializable]
public struct SaveFilePlayerProgress
{
    public int SaveFileSystemVersioning;
    public double UniqueIdentifier;
    public AccessInformation accessInformation;
    public DateInformation DateInformation;
    public ProgressPlayerInfo PlayerInfo;
    public ProgressAudioSettings Audiosettings;
    public ProgressLevel[] ProgressLevel;
    public ProgressAnimationSettings AnimationSettings;
    public ProgressGraphicSettings GraphicSettings;

    public SaveFilePlayerProgress(int saveFileSystemVersioning, double uniqueIdentifier, AccessInformation accessInformation, DateInformation dateInformation, ProgressPlayerInfo playerInfo, ProgressAudioSettings audiosettings, ProgressLevel[] progressLevel, ProgressAnimationSettings animationSettings, ProgressGraphicSettings graphicSettings)
    {
        SaveFileSystemVersioning = saveFileSystemVersioning;
        UniqueIdentifier = uniqueIdentifier;
        this.accessInformation = accessInformation;
        DateInformation = dateInformation;
        PlayerInfo = playerInfo;
        Audiosettings = audiosettings;
        ProgressLevel = progressLevel;
        AnimationSettings = animationSettings;
        GraphicSettings = graphicSettings;
    }
}

[Serializable]
public struct DateInformation
{
    public byte Minute;
    public byte Hour;
    public byte Day;
    public byte Month;
    public int Year;

    public DateInformation(byte minute, byte hour, byte day, byte month, int year)
    {
        Minute = minute;
        Hour = hour;
        Day = day;
        Month = month;
        Year = year;
    }
}

[Serializable]
public struct AccessInformation
{
    public bool KeepAdminAccessEnabled;

    public AccessInformation(bool keepAdminAccessEnabled)
    {
        KeepAdminAccessEnabled = keepAdminAccessEnabled;
    }
}
[Serializable]
public struct ProgressPlayerInfo
{
    public string Username;
    public double TimeInMenu;
    public double TimeInGame;
    public int TimePlayedToday;
    public Language Language;

    public ProgressPlayerInfo(string username, double timeInMenu, double timeInGame, int timePlayedToday, Language language)
    {
        Username = username;
        TimeInMenu = timeInMenu;
        TimeInGame = timeInGame;
        TimePlayedToday = timePlayedToday;
        Language = language;
    }
}
[Serializable]
public struct ProgressAudioSettings
{
    public int MasterVolume;
    public int MusicVolume;
    public int SfxVolume;
    public int UISoundVolume;
    public bool MasterMuted;
    public bool MusicMuted;
    public bool SfxMuted;
    public bool UISoundMuted;

    public ProgressAudioSettings(int masterVolume, int musicVolume, int sfxVolume, int uiSoundVolume, bool masterMuted, bool musicMuted, bool sfxMuted, bool uiSoundMuted)
    {
        MasterVolume = masterVolume;
        MusicVolume = musicVolume;
        SfxVolume = sfxVolume;
        UISoundVolume = uiSoundVolume;
        MasterMuted = masterMuted;
        MusicMuted = musicMuted;
        SfxMuted = sfxMuted;
        UISoundMuted = uiSoundMuted;
    }
}

[Serializable]
public struct ProgressGraphicSettings
{
    public int Resolution;
    public int TexturQuality;
    public int EffectQuality;
    public bool ColorBlind;

    public ProgressGraphicSettings(int resolution, int texturQuality, int effectQuality, bool colorBlind)
    {
        Resolution = resolution;
        TexturQuality = texturQuality;
        EffectQuality = effectQuality;
        ColorBlind = colorBlind;
    }
}


[Serializable]
public struct ProgressLevel
{
    public int LevelIdentifier;
    public bool freigeschaltet;
    public double Highscore;
    public bool Star1;
    public bool Star2;
    public bool Star3;
    public bool PerfectRun;

    public ProgressLevel(int levelIdentifier, bool freigeschaltet, double highscore, bool star1, bool star2, bool star3, bool perfectRun)
    {
        LevelIdentifier = levelIdentifier;
        this.freigeschaltet = freigeschaltet;
        Highscore = highscore;
        Star1 = star1;
        Star2 = star2;
        Star3 = star3;
        PerfectRun = perfectRun;
    }
}
[Serializable]
public struct ProgressAnimationSettings
{
    public int SwipeRejectionLevel;
    public int AnimationspeedSide;
    public int SwapbackSpeeds;
    public int Visibility;
    public int RefillDelay;
    public int ShuffleDelay;
    public bool IsColorblind;

    public ProgressAnimationSettings(int swipeRejectionLevel, int animationspeedSide, int swapbackSpeeds, int visibility, int refillDelay, int shuffleDelay, bool isColorblind)
    {
        SwipeRejectionLevel = swipeRejectionLevel;
        AnimationspeedSide = animationspeedSide;
        SwapbackSpeeds = swapbackSpeeds;
        Visibility = visibility;
        RefillDelay = refillDelay;
        ShuffleDelay = shuffleDelay;
        IsColorblind = isColorblind;
    }
}
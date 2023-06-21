using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public sealed class Settings : ScriptableObject
{
 [Header("General Settings")] 
 [SerializeField] public bool ColorBlind = false;
 [Range(0, 3)] [SerializeField] private int SwipeRejection = 2;
 [Range(0, 4)] [SerializeField] private int SideDotAlpha= 2;
 
 [Space]
 [Header("Animation Settings")]
 [Range(0, 3)] [SerializeField] private int SwapBackSpeed = 2;
 [Range(0, 3)] [SerializeField] private int AnimationSpeedLinePush = 2;
 [Range(0, 6)] [SerializeField] private int RefillDelay = 2;
 [Range(0, 6)] [SerializeField] private int ShuffleDelay = 3;
 
 [Space]
 [Header("Sound Settings")]
 [Range(0, 100)] [SerializeField] private int MasterVolume = 100;
 [Range(0, 100)] [SerializeField] private int MusicVolume = 100;
 [Range(0, 100)] [SerializeField] private int SFXVolume = 100;
 [Range(0, 100)] [SerializeField] private int UISoundVolume = 100;
 
 [SerializeField] private bool MasterMuted;

 public bool MasterMutedProperty
 {
  get => MasterMuted;
  set
  {
   MasterMuted = value;
   CheckBGMusic();
  }
 }

 [SerializeField] private bool MusicMuted;
 
  public bool MusicMutedProperty
  {
   get => MusicMuted;

   set
   {
    MusicMuted = value;
    CheckBGMusic();
   }
  }

  private void CheckBGMusic()
  {
   if ((MusicMuted || MasterMuted) && Rl.GameManager != null && Rl.GameManager.gameManagerAudioSource) Rl.GameManager.gameManagerAudioSource.Pause();
   else 
   {
if(Rl.GameManager != null && Rl.GameManager.gameManagerAudioSource !=null )    Rl.GameManager.gameManagerAudioSource.UnPause();
    if (Rl.GameManager.gameManagerAudioSource.clip == null)
    {
     if (FindObjectOfType<PlayMenuBGMusic>() != null)
     {
      FindObjectOfType<PlayMenuBGMusic>().StartMusic();
     }
     else if (FindObjectOfType<PlayAudio>() != null)
     {
      FindObjectOfType<PlayAudio>().PlaybackgroundMusic();
     }
    }
   }
  }
  [SerializeField] public bool SFXMuted;
 [SerializeField] public bool UIMuted;
 
 
 [Space] 
 [Header("SideDot Icons Settings")] 
 [Range(0f, 3f)] [SerializeField] private float leftPaddingX = .5f;
 [Range(0f, 3f)] [SerializeField] private float leftPaddingY = .5f;
 [Range(0, 3f)] [SerializeField]  private float rightPaddingX = .5f;
 [Range(0, 3f)] [SerializeField]  private float rightPaddingY = .5f;
 [Range(0, 3f)] [SerializeField]  private float topPaddingX = .5f;
 [Range(0, 3f)] [SerializeField]  private float topPaddingY = .5f;
 [Range(0, 3f)] [SerializeField]  private float bottomPaddingX = .5f;
 [Range(0, 3f)] [SerializeField]  private float bottomPaddingY = .5f;
 
 
 [Space]
 [Header("Resolution")] 
 [Range(0, 6)] [SerializeField] private int Resolution = 0;
 [Space]
 [Header("Debug View")]
 [NaughtyAttributes.ReadOnly] public float  GetSwipeRejection = swipeRejectionLevels[2];
 [NaughtyAttributes.ReadOnly] public Vector2 GetSwapBackSpeed = swapBackSpeedLevels[2];
 [NaughtyAttributes.ReadOnly] public float GetAnimationSpeedLinePush = animationSpeedLinePushLevels[2];
 [NaughtyAttributes.ReadOnly] public float GetSideDotAlpha=  sideDotLevels[2];
 [NaughtyAttributes.ReadOnly] public float GetShuffleDelay=  shuffleDelayLevels[2];
 [NaughtyAttributes.ReadOnly] public float GetRefillDelay=  refillDelayLevels[2];
 
 [NaughtyAttributes.ReadOnly] public float GetMasterVolume = 1f;
 [NaughtyAttributes.ReadOnly] public float GetMusicVolume = 1f;
 [NaughtyAttributes.ReadOnly] public float GetSFXVolume = 1f;
 [NaughtyAttributes.ReadOnly] public float GetUISoundVolume = 1f;
 
 [NaughtyAttributes.ReadOnly] public float GetLeftPaddingX = .5f;
 [NaughtyAttributes.ReadOnly] public float GetLeftPaddingY = .5f;
 [NaughtyAttributes.ReadOnly] public float GetRightPaddingX = .5f;
 [NaughtyAttributes.ReadOnly] public float GetRightPaddingY = .5f;
 [NaughtyAttributes.ReadOnly] public float GetTopPaddingX = .5f;
 [NaughtyAttributes.ReadOnly] public float GetTopPaddingY = .5f;
 [NaughtyAttributes.ReadOnly] public float GetBottomPaddingX = .5f;
 [NaughtyAttributes.ReadOnly] public float GetBottomPaddingY = .5f;

 [NaughtyAttributes.ReadOnly]public IpadResolution  GetResolution =  ipadResolutions[0];
 
 
 [Range(0, 3)] [SerializeField] public int FullResolution = 0;
 [Range(0, 3)] [SerializeField] public int EffectsQuality = 0;
 [SerializeField] public bool KeepAdminAccess = false;
 
 private int tempSideDotAlphachecker = 0;
 
 private void OnEnable()
 {
  tempSideDotAlphachecker = SideDotAlpha;
  UpdateSettings();
 }
 
 public int GetAnimationSpeedSettings() => AnimationSpeedLinePush;
 public int GetSideIconsAlphaSettings() => SideDotAlpha;
 public int GetSwipeRejectionSettings() => SwipeRejection;
 public int GetSwapBackSettings() => SwapBackSpeed;
 public int GetMasterVolumeSettings() => MasterVolume;
 public int GetSFXVolumeSettings() => SFXVolume;
 public int GetUiSoundVolumeSettings() => UISoundVolume;
 public int GetMusicVolumeSettings() => MusicVolume;
 public int GetResoltionSettings() => Resolution;
 public int GetRefillDealySettings() => RefillDelay;
 public int GetShuffleDealySettings() => ShuffleDelay;
 public void SetResolution(int setting) => Resolution = setting;
 public void IncreaseResolution()
 {
  if(Resolution < ipadResolutions.Length-1)
  Resolution++;
 }

 public void IncreaseEffectsQuality()
 {
  if(EffectsQuality < 3)
   EffectsQuality++;
 }
 public void DecreaseEffectsQuality()
 {
  if(EffectsQuality > 0)
   EffectsQuality--;
 }
 public void IncreaseFullResolution()
 {
  if (FullResolution < 3)
   FullResolution++;
 }
 public void DecreaseFullResolution()
 {
  if(FullResolution > 0)
   FullResolution--;
 }
 public void DecreaseResolution()
 {
  if(Resolution > 0)
   Resolution--;
 }

 public void IncreaseSwipeRejectionSettings()
 {
  if ( SwipeRejection < 3)
   SwipeRejection++;
 }
 public void DecreaseSwipeRejectionSettings()
 {
  if ( SwipeRejection > 0)
   SwipeRejection--;
 }
 
 public void IncreaseAnimationSpeed()
 {
  if ( AnimationSpeedLinePush < 3)
   AnimationSpeedLinePush++;
 }
 public void DecreaseAnimationSpeed()
 {
  if ( AnimationSpeedLinePush > 0)
   AnimationSpeedLinePush--;
 }
 
 public void IncreaseSwapBackSpeed()
 {
  if ( SwapBackSpeed < 3)
   SwapBackSpeed++;
 }
 public void DecreaseSwapBackSpeed()
 {
  if ( SwapBackSpeed > 0)
   SwapBackSpeed--;
 }
 
 public void IncreaseRefillDelay()
 {
  if ( RefillDelay < 6)
   RefillDelay ++;
 }
 public void DecreaseRefillDelay()
 {
  if ( RefillDelay > 0)
   RefillDelay--;
 }
 
 public void IncreaseShuffleDelay()
 {
  if ( ShuffleDelay < 6)
   ShuffleDelay++;
 }
 public void DecreaseShuffleDelay()
 {
  if ( ShuffleDelay > 0)
   ShuffleDelay--;
 }

 public void SetShuffleDelay(int setting) => ShuffleDelay = setting;
 public void SetRefillDelay(int setting) => RefillDelay = setting;
 public void SetAnimationSpeedSettings(int setting) => AnimationSpeedLinePush = setting;
 public void SetSwapBackSpeedSettings(int setting) => SwapBackSpeed = setting;
 public void SetSideIconsAlphaSettings(int setting) => SideDotAlpha = setting;

 public void SetSwipeRejectionSettings(int setting) => SwipeRejection = setting;
 public void SetMasterVolumeSettings(int setting) => MasterVolume= setting;
 public void SetSFXVolumeSettings(int setting) => SFXVolume = setting;
 public void SetMusicVolumeSettings(int setting) => MusicVolume = setting;
 public void SetUISoundVolumeSettings(int setting) => UISoundVolume = setting;

 private void SetColorBlind()
 {
  
 }
 public void SetGraphicSettings()
 {
  Debug.Log("Graphic Set");
  SetColorBlind();
  GetResolution = ipadResolutions[Resolution];
  int height = GetResolution.Height;
  int width = GetResolution.Width;
  Screen.SetResolution(height, width, true);
 }
 public void SetSettings()
 {
 // UpdateSettings();
  swipeRejectionLevels[SwipeRejection] = GetSwipeRejection;
  swapBackSpeedLevels[SwapBackSpeed] = GetSwapBackSpeed;
  sideDotLevels[SideDotAlpha] = GetSideDotAlpha ;

  refillDelayLevels[RefillDelay] = GetRefillDelay;
  shuffleDelayLevels[ShuffleDelay] = GetShuffleDelay;
 
  animationSpeedLinePushLevels[AnimationSpeedLinePush] = GetAnimationSpeedLinePush;
  MasterVolume = convertToFullNumber(GetMasterVolume);
  MusicVolume = convertToFullNumber(GetMusicVolume);
  SFXVolume = convertToFullNumber(GetSFXVolume );
  UISoundVolume = convertToFullNumber(GetUISoundVolume);
  
  leftPaddingX = GetLeftPaddingX ;
  rightPaddingX = GetRightPaddingX;
  topPaddingX = GetTopPaddingX ;
   bottomPaddingX = GetBottomPaddingX;

  GetLeftPaddingY = leftPaddingY;
  GetRightPaddingY = rightPaddingY;
  GetTopPaddingY = topPaddingY;
  GetBottomPaddingY = bottomPaddingY;
 }

 private void OnValidate()
 {
  UpdateSettings();
  UpdateSideDotAlphaLive();
  if (Application.isPlaying)
  {
if(Rl.GameManager != null && Rl.GameManager.gameManagerAudioSource != null)   Rl.GameManager.gameManagerAudioSource.volume = GetMusicVolume * GetMasterVolume;
   Rl.effects.audioSource.volume = GetSFXVolume * GetMasterVolume;
   Rl.uiSounds.audioSource.volume = GetUISoundVolume * GetMasterVolume;
  }
 }
 private void UpdateSettings()
 {
  GetResolution = ipadResolutions[Resolution];
  GetSwipeRejection = swipeRejectionLevels[SwipeRejection];
  GetSwapBackSpeed = swapBackSpeedLevels[SwapBackSpeed];
  GetSideDotAlpha = sideDotLevels[SideDotAlpha];
  GetAnimationSpeedLinePush= animationSpeedLinePushLevels[AnimationSpeedLinePush];
  
  GetRefillDelay = refillDelayLevels[RefillDelay] ;
  GetShuffleDelay = shuffleDelayLevels[ShuffleDelay];
  
  
  GetMasterVolume = convertToPercent(MasterVolume);
  GetMusicVolume = convertToPercent(MusicVolume);
  GetSFXVolume = convertToPercent(SFXVolume);
  GetUISoundVolume = convertToPercent(UISoundVolume);
  
  GetLeftPaddingX = leftPaddingX;
  GetRightPaddingX = rightPaddingX;
  GetTopPaddingX = topPaddingX;
  GetBottomPaddingX = bottomPaddingX;

  GetLeftPaddingY = leftPaddingY;
  GetRightPaddingY = rightPaddingY;
  GetTopPaddingY = topPaddingY;
  GetBottomPaddingY = bottomPaddingY;
 }
 private float convertToPercent(int oneToHoundred) => (float)oneToHoundred / 100;
 private int convertToFullNumber(float percentToHundred) => (int)(percentToHundred * 100);
 private void UpdateSideDotAlphaLive()
 {
  if (tempSideDotAlphachecker != SideDotAlpha)
  {
   foreach (SideDot sDot in FindObjectsOfType<SideDot>())
    SideDotInit.UpdateSpriteAlpha(sDot.GetComponent<SpriteRenderer>());

   tempSideDotAlphachecker = SideDotAlpha;
  }
 }
 private static float[] swipeRejectionLevels =
 {
  0.035f, 0.2f, 0.5f, 3f
 };
 
 // private static Vector2[] swapBackSpeedLevels =
 // {
 //  new(0.7f,0.9f), new(0.6f,0.8f), new(0.45f,0.6f), new(0.3f,0.45f), new(0.1f,0.2f)
 // };
 private static Vector2[] swapBackSpeedLevels =
 {
  new(5f,5f), new(5f,0.6f), new(5f,5f), new(5f,5f)
 // new(0.7f,0.9f), new(0.45f,0.6f), new(0.3f,0.45f), new(0.1f,0.2f)
 };
 private static float[] sideDotLevels =
 {
  0.03f, 0.1f, 0.25f, 0.45f, 0.9f
 };
 // private static float[] animationSpeedLinePushLevels =
 // {
 //  0.035f, 0.045f, 0.055f, 0.065f, 0.075f, 0.085f, 0.1f, 0.11f, 0.12f, 0.13f,0.14f, 0.15f
 // };
 
 private static float[] animationSpeedLinePushLevels =
 {
  0.068f, 0.095f, 0.125f, 0.2f
 };
 
 private static float[] refillDelayLevels =
 {
  0.08f, 0.22f, 0.33f, 0.44f, 0.66f, 0.88f, 1.8f
 };
 //  0.2f, 0.44f, 0.7f, 1f, 1.5f, 2f, 2.5f
 private static float[] shuffleDelayLevels =
 {
  0.1f, 0.35f, 0.65f, 0.95f, 1.2f, 1.4f, 1.6f
 };


 public static IpadResolution[] ipadResolutions =
 {
  new(new []{"iPad 1", "iPad 2", "iPad mini 1"},768, 1024),
  new(new []{"iPad mini 6"},1448, 2224),
  new(new []{"iPad 3","iPad 4", "iPad 5", "iPad Air 1", "iPad mini 2", "iPad mini 3", "iPad Air 2", "iPad mini 4", "iPad Pro 1  9.7\""},1536, 2048),
  new(new []{"iPad 7", "iPad 8", "iPad 9"},1620, 2160),
  new(new []{"iPad 10", "iPad Air 4", "iPad Air 5"},1640, 2360),
  new(new []{"iPad Air 3", "iPad Pro 2  10.5\" ", "iPad Pro 3  11\"",  "iPad Pro 4  11\"", "iPad Pro  5 11\"", "iPad Pro  6 11\""},1668,2224),
  new (new []{"iPad Pro 1  12.9\"", "iPad Pro 2  12.9\"", "iPad Pro 3  12.9\"", "iPad Pro 4  12.9\"", "iPad Pro 5  12.9\"", "iPad Pro 6  12.9\""},2048, 2732)
};
}
[Serializable]
public enum TabGroupState
{
 UserSettingsTab,
 LevelSettingsTab,
 FruitTab
}
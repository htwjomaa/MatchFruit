using System.Collections.Generic;
using UnityEngine;
public sealed class SoundStrings : MonoBehaviour
{
    [SerializeField] public List<LevelBackGroundMusic> LevelBackGroundMusic = new List<LevelBackGroundMusic>();
    [SerializeField] public List<string> DestroyClips = new List<string>();
    [SerializeField] public List<string> DropClips = new List<string>();
    [SerializeField] public List<string> Complete = new List<string>();
    [SerializeField] public List<string> Stars = new List<string>();
    [SerializeField] public List<string> Swish = new List<string>();
    [SerializeField] public List<string> GameOver = new List<string>();
    
    public string TimeOut = "timeOut";
    public string ExplosionSound = "explosionSound";
    public string SwapBackAudio = "swapDotAudio2";
    public string AppearStripedColorBombSound = "_appearStripedColorBomb";
    public string ColorBombExpl = "colorBombExpl";
    public string StripedExplosion = "stripedExplosion";
    public string DestroyBlockAudio = "matchfound";
    public string AcceptSwitchSound = "acceptSwitch";
    public string SwitchSound ="switchButtonColorBlind";
    public string LanguageChangeSound = "languageChange";
    public string TakeDamageAudio = "takedamage";
    public string DeadLockAudio = "deadlocked";
    public string MatchFound = "matchfound";
    public string MakeBubbleSound = "makeSlime";
    public string  DamageBubbleSound = "damageSlime";
    public string ResetToMidValueSound = "resetToMidValueSound";
    public string TabButtonSound = "tabButtonSound";
    public string NextRowSound = "nextRowSound";
    public string SwitchButtonSound  = "switchButtonColorBlind";
    public string Clickonsidedot = "clickonsidedot";
    public string GameStartAudio = "gameStart";
    public string GameNoStartAudio = "gameNoStart";
    public string AudioSettingsButtonAudio = "audioSettingsButton";
    public string CloseLevelPanelAudio = "closeLevelPanel";
    public string SplashMenuLevelButtonAudio = "splashMenuLevelButton";
    public string SplashMenuLevelButtonNoAccessAudio = "splashMenuLevelButtonNoAccess";
    public string AdminAccessAudio = "adminAccess";
    public string AdminSettingsButtonSound = "adminSettingsButtonSound";
    public string CloseAdminSettingsPanelSound = "closeAdminSettingsPanelSound";
   public string ExitButtonSound = "exitButtonSound";
   
   public string IncreaseSound = "increaseSound";
   public string DecreaseSound = "decreaseSound";
   public string AcceptGraphicSound = "acceptGraphicSound";
   public string RevertGraphicSound = "revertGraphicSound";
   public string OkButtonSound = "okButtonSound";
   public string SliderHandleSound = "sliderHandleSound";
   public  string RestartLevelButton = "restartLevelButton";
   public  string AudioSettingsButton = "audioSettingsButton";
   public  string SplashMenuButton = "splashMenuButton";
   public  string MuteUnmute = "muteUnmute";
   public string AcceptLevelSound = "acceptLevelSound";
   public string RevertChangesSound = "revertChangesSound";
   public string PhaseChangeButtonSound = "phaseChangeButton";
   public string DestroyPackage = "destroyPackage";
   public string GetStarIngr = "getStarIngr";
   public string NoMatch = "noMatch";
   public string BoostColorReplace = "boostColorReplace";
   public string BoostBomb = "boostBomb";
   public string Click = "click";
   public string MenuBackgroundMusic = "menubgmusic";
   public string MenuBackgroundMusic2 = "curiosity";
}
 
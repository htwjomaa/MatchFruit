using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Linq;
using NaughtyAttributes;
using FruitMatch.Scripts.Core;
using FruitMatch.Scripts.Level;
public sealed class SplashMenu : MonoBehaviour
{
 public World world;
 public bool levelselected;
 public static TabGroupState TabGroupState;
 [SerializeField] private FadeControllerSplashMenu fadeControllerSplashMenu;
 public TextMeshProUGUI levelInfoPanelText; 
 public TextMeshProUGUI scoreGoalOneText;
 public TextMeshProUGUI scoreGoalTwoText;
 public TextMeshProUGUI scoreGoalThreeText;
 public TextMeshProUGUI levelGameTypeText;
 public TextMeshProUGUI worldNameText;
 [SerializeField] public GameObject
  LevelCanvasObj;
 public Vector4 worldNameTextColorChange;
 private Vector4  cashedworldNameTextColor;
 public List<Image> goalImages = new List<Image>();
 private Queue<int> secretAdminAccess = new Queue<int>(8);
 [SerializeField] private List<int> AdminCombo = new List<int>(8);
 [SerializeField] private int prevLevel;
 [SerializeField] private int nextLevel;
 public TextMeshProUGUI timePlayedTodayText;
 [SerializeField] private Sprite starArchieved;
 [SerializeField] private Sprite starNotArchieved;
 [SerializeField] private Sprite perfectRunArchieved;
 [SerializeField] private Sprite perfectRunNotArchieved;
 [SerializeField] private GameObject New;
 [SerializeField] private List<GameObject> LevelButtons = new List<GameObject>();
 [SerializeField] private List<GameObject> AdminSettingsButtons= new List<GameObject>();
 public int levelCount = 0;
 [SerializeField] private Canvas SplashMenuCanvasForLevelInfoPanel;
 [SerializeField] private int MIDPOINTCANVAS = 525;
 private Coroutine firstSelectLevel;
 public SaveFileManagerInMenu saveFileManagerInMenu;
 public int numbersAfterKommaAllowed = 1;
 [SerializeField] private bool SplashMenuSettingsOpened;
 public static bool PanelOnLeft = false;
 public bool AdminAcessEnabled = false;
 [Header("Ui Strings")]
 [SerializeField] private string limitedMoves = "limited_moves";
 [SerializeField] private string timedRun = "timed_run";
 [SerializeField] private string firstSelectALevel = "Klicke zuerst ein Level";
 [SerializeField] private string chooseALevel = "choose_level";
 [SerializeField] private string komma = "float_marker";
 [SerializeField] private string thousandShortString = "thousand_marker";

 [SerializeField] private List<GameObject> disableParentList = new List<GameObject>();
 private void Start()
 {
  SplashMenuSettingsOpened = false; 
  levelselected = false;
  cashedworldNameTextColor = worldNameText.color;
  if(world.UnlockDepensOnStars)UnlockAndLockLevels();
  SetupProgressOnLoad();

  FillQueueWithZeros();
  StartCoroutine(AddASecondCo());
  world.totalTimeAppUsedSec = world.totalTimePlayedInGame + world.totalTimeInMenu;
  
  UpdateTimePlayedTodayText();
  levelCount = world.LevelCount();
  DisableChilds(disableParentList);
  if(world.UnlockDepensOnStars)Invoke(nameof(LateFixIfItDoesntUpdate), 0.03f);
 }

 private void DisableChilds(List<GameObject> disableParents)
 {
  foreach (GameObject disableParent in disableParents)
  {
   for (int i = 0; i< disableParent.transform.childCount; i++)
   {
    disableParent.transform.GetChild(i).transform.gameObject.SetActive(false);
   }
  }
 }
 
 private void LateFixIfItDoesntUpdate()
 {
  UnlockAndLockLevels();
  SetupProgressOnLoad();
 }
 [Button()] public void UnlockAllLevels()
 {
  world.UnlockAllLevel();
  SetupProgressOnLoad();
 }
 [Button()]
 public void LockLevelsAgain()
 {
  UnlockAndLockLevels();
  SetupProgressOnLoad();
 }
 private void UnlockAndLockLevels()
 {
  world.StarsDependOnHighScore = true;
  world.UnlockDepensOnStars = true;
   bool[] unlockedOrLockedArray = world.GetIfLevelIsUnlockedBasedOnFirstStarFromLastLevel();
   for (int i = 0; i < unlockedOrLockedArray.Length; i++)
   {
    world.LevelProgresses[i]  = new LevelProgress(world.LevelProgresses[i].Level, unlockedOrLockedArray[i],
     world.LevelProgresses[i].Sterne,  world.LevelProgresses[i].PerfectRun,world.LevelProgresses[i].HighScore,  world.LevelProgresses[i].ImageActive,
     world.LevelProgresses[i].ImageInactive,  world.LevelProgresses[i].ImageSelection);
   }
 }

 public void UpdateLevelAccessText()
 {
  for (int i = 0; i < LevelButtons.Count; i++)
  {
   if (world.GetLevelAccess(ChangeIntToLevel(i + 1)) && LevelButtons[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>() != null)
    LevelButtons[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color =
     new Color(255, 255, 255, 255);
   else if (!world.GetLevelAccess(ChangeIntToLevel(i + 1)) &&
            LevelButtons[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>() != null)
   {
    Color color;
    ColorUtility.TryParseHtmlString("#92A1BC", out color);
    LevelButtons[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = color;
   }
  }
 }


 private GameObject lastAdminSettingsButton;
 public void ClickAdminSettingsButton(GameObject adminSettingsButton)
 {
  PanelOnLeft = IsPanelLeft(adminSettingsButton);
  Level level = GetInformationForAdminSettingsButton(adminSettingsButton);
  UpdateAdminSettingsButtonsColor(adminSettingsButton.GetComponent<Image>());
 Rl.adminLevelSettingsPanel.LevelAdminLevelSettingsLevelNumber = adminSettingsButton.transform.GetSiblingIndex();
 Rl.adminLevelSettingsPanel.LoadSettings();
  
  AdminLevelSettingsTabGroup.tabButtons.First().GetComponent<TabButton>().ClickThisButtonFromOtherScript();
  Rl.GameManager.PlayAudio(Rl.soundStrings.AdminSettingsButtonSound, Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource); 
  //Handheld.Vibrate();
  
  if(lastAdminSettingsButton == null) lastAdminSettingsButton = adminSettingsButton;
 AdminSettingsOpeningAnimation(adminSettingsButton, lastAdminSettingsButton);
 lastAdminSettingsButton = adminSettingsButton;
 GenericSettingsFunctions.SmallJumpAnimation(0.2f,adminSettingsButton.transform);
 }


 public void CloseAdminSettingsPanel(bool PlayNoSound)
 {
  if(!PlayNoSound) Rl.GameManager.PlayAudio(Rl.soundStrings.CloseAdminSettingsPanelSound, Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
  //Handheld.Vibrate();
  fadeControllerSplashMenu.FadeOutAdminSettingsPanel();
 }

 private Level GetInformationForAdminSettingsButton(GameObject adminSettingsButton)
 {
  int counter = 0;
  for (counter  = 0; counter  < AdminSettingsButtons.Count; counter ++)
  {
   if (adminSettingsButton == AdminSettingsButtons[counter ])
   {
    break;
   }
  }
  return  ChangeIntToLevel(counter+1);
 }

 public void UpdateLevelAccessImage()
 {
  for (int i = 0; i < LevelButtons.Count; i++)
  {
   if (world.GetLevelAccess(ChangeIntToLevel(i+1)) &&  world.LevelProgresses[i].ImageActive !=null && LevelButtons[i].transform.GetChild(0).GetComponent<Image>() != null)
    LevelButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = world.LevelProgresses[i].ImageActive;
   else if (!world.GetLevelAccess(ChangeIntToLevel(i+1)) &&  world.LevelProgresses[i].ImageInactive !=null && LevelButtons[i].transform.GetChild(0).GetComponent<Image>() != null)
   LevelButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = world.LevelProgresses[i].ImageInactive;
  }
 }
 public void UpdateLevelAccessStarVisibility()
 {
  for (int i = 0; i < LevelButtons.Count; i++)
  {
   if (world.GetLevelAccess(ChangeIntToLevel(i + 1)))
   {
    foreach (GameObject star in LevelButtons[i].GetComponent<LevelButton>().Stars)
     star.SetActive(true);
   }
   else if (!world.GetLevelAccess(ChangeIntToLevel(i + 1)))
   {
    foreach (GameObject star in LevelButtons[i].GetComponent<LevelButton>().Stars)
     star.SetActive(false);
   }
  }
 }
 
 private void OnDestroy()
 {
  StopCoroutine(AddASecondCo());
  world.totalTimeAppUsedSec = world.totalTimePlayedInGame + world.totalTimeInMenu;
 }

 private void OnApplicationQuit()
 {
  AdminAcessEnabled = false;
  StopCoroutine(AddASecondCo());
  world.totalTimeAppUsedSec = world.totalTimePlayedInGame + world.totalTimeInMenu;
 }


 private void ActivateLevelSelectedImage(int level)
 {
  DeActivateLevelSelectedImage();
  LevelButtons[level-1].GetComponent<Image>().enabled = true;
 }
 private void DeActivateLevelSelectedImage()
 {
  foreach (GameObject levelButton in LevelButtons)
  {
   levelButton.GetComponent<Image>().enabled = false;
  }
 }

 private void UpdateTimePlayedTodayText()
 {
  int toMinutes = (int)(world.totalTimePlayedToday / 60);
  timePlayedTodayText.text = toMinutes .ToString() + " min";
 }
 public void PreviosLevel()
 {
  if (prevLevel == 0) return;
  LevelButtonClick(prevLevel);
 }

 public void NextLevel()
 {
  if (nextLevel == levelCount + 1) return;
  LevelButtonClick(nextLevel);
 }

 private int NextLevelCheck(int thisLevel)
 {
  for (int i = thisLevel+1 ; i < 1; i++)
  {
   if (world.GetLevelAccess(CheckLevel(i))) return i;
  }
  return thisLevel+1;
 }

 private int PrevLevelCheck(int thisLevel)
 {
  for (int i = thisLevel-1 ; i > 0; i--)
  {
   if (world.GetLevelAccess(CheckLevel(i))) return i;

  }
  return thisLevel-1;
 }

 private void LoadLevelData(int level)
 {
  LevelManager.THIS.currentLevel = level;
  LoadingHelper.THIS.allDotPrefabs  = LoadingManager.GetDots(Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs
   .LevelConfigs[level-1].BoardDimensionsConfig, Rl.world);
 }
 
 private void UpdatePrevNextLevelArrows(int level)
 {
  prevLevel = PrevLevelCheck(level);
  nextLevel = NextLevelCheck(level);
  // prevLevel = level - 1;
  // nextLevel = level + 1;

  if (prevLevel < 1) prevLevel = 1;
  if (nextLevel > levelCount ) nextLevel = levelCount ;
 }
 private void FillQueueWithZeros()
 {
  secretAdminAccess.Clear();
  for (int i = 0; i < 8; i++)
  {
   secretAdminAccess.Enqueue(0);
  }
 }

 public IEnumerator AddASecondCo()
 {
  
  yield return new WaitForSeconds(1);
  world.totalTimeInMenu++;
  StartCoroutine(AddASecondCo());
 }
 
 private bool adminAcess(int level)
 {
  secretAdminAccess.Dequeue();
  secretAdminAccess.Enqueue(level);
  List<int> queueCombo = secretAdminAccess.ToList();
  bool adminAcess = true;
  List<bool> accesses = new List<bool>(8);
  for (int i = 0; i < 8; i++)
  {
   accesses.Add(false);
  }

  for (int i = 0; i < queueCombo.Count; i++)
  {
   if (queueCombo[i] == AdminCombo[i]) accesses[i] = true;
  }

  foreach (bool bools in accesses)
  {
   if (!bools) adminAcess = false;
  }
  return adminAcess;
 }

 
 public void LevelButtonClick(int level)
 {
  
  if (adminAcess((level)))
  {
   //Handheld.Vibrate();
   Rl.GameManager.PlayAudio(Rl.soundStrings.AdminAccessAudio, Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
   Debug.Log("Admin access");
   AdminAcessEnabled = true;
  }
  if (world.GetLevelAccess(CheckLevel(level)))
  {
   ActivateLevelSelectedImage(level);
   UpdatePrevNextLevelArrows(level);
   
   ChangeLevelInfoPanel(ChangeIntToLevel(level));
   PanelOnLeft = IsPanelLeft(LevelButtons[level-1]);
   if(PanelOnLeft)CloseSplashSettingsMenu();
   LoadLevelData(level);
   OpenLevelInfoPanel();
   GenericSettingsFunctions.SmallJumpAnimation(0.23f,LevelButtons[level-1].transform);
  }
  else
  {
   Rl.GameManager.PlayAudio(Rl.soundStrings.SplashMenuLevelButtonNoAccessAudio, Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
   // sound abspielen, level geht nicht
   //if (!world.StarsDependOnHighScore && !world.UnlockDepensOnStars)
  // {
   // Debug.Log("hi hallo hey");
  //  Rl.GameManager.PlayAudio(splashMenuLevelButtonNoAccessAudio, Random.Range(0,4), Rl.settings.GetSFXVolume, Rl.effects.audioSource);
 //  }
  }
 }

public void CheckIfSideMoveLevelInfoPanel(GameObject thisButton)
{
 Debug.Log("PanelOnLeft: "+ PanelOnLeft + " PanelLeft"+ IsPanelLeft(thisButton) );
 if (PanelOnLeft != IsPanelLeft(thisButton))
  return;
 
  fadeControllerSplashMenu.LevelInfoPanelSideMove(PanelOnLeft );
 
  StartCoroutine(deactivateAnimation(0.2f, fadeControllerSplashMenu.levelInfoPanelAnim));
}

private void AdminSettingsOpeningAnimation(GameObject thisButton, GameObject lastAdminSettingsButton)
{
 if (IsPanelLeft(lastAdminSettingsButton) == IsPanelLeft(thisButton))
 {
  fadeControllerSplashMenu.FadeInAdminSettingsPanel(PanelOnLeft);
  return;
 }
 fadeControllerSplashMenu.AdminSettingsPanelSideMove(PanelOnLeft );
 fadeControllerSplashMenu.PreviewBoardSideMove(PanelOnLeft);
 StartCoroutine(deactivateAnimation(0.2f, fadeControllerSplashMenu.AdminLevelSettingsAnimator));
 StartCoroutine(deactivateAnimation(0.2f, fadeControllerSplashMenu.PreviewBoardAnimator));
}
IEnumerator deactivateAnimation(float second, Animator panel)
{
 yield return new WaitForSeconds(second);
 panel.SetBool("SideMove", false);
}

 public void OpenLevelInfoPanel()
 {
  CloseAdminSettingsPanel(true);
  //Handheld.Vibrate();
  Rl.GameManager.PlayAudio(Rl.soundStrings.SplashMenuLevelButtonAudio, Random.Range(2,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
  levelselected = true;
  fadeControllerSplashMenu.FadeInLevelInfoPanel(PanelOnLeft);
 }

 
 public void CloseLevelInfoPanel(bool playNoSound)
 {
  StartCoroutine(DelayDeactiveDeActivateLevelSelectedImageeCo(0.4f));
  //Handheld.Vibrate();
  if(!playNoSound) Rl.GameManager.PlayAudio(Rl.soundStrings.CloseLevelPanelAudio, Random.Range(0,2), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
  levelselected = false;
  fadeControllerSplashMenu.FadeOutLevelInfoPanelPanel();
 }
 

 public void ExitButton()
 {
  Rl.saveFileManagerInMenu.SaveGame();

  float timer = 1f;
  if (Rl.GameManager.audiodataBase.allAudioNames.ContainsKey(Rl.soundStrings.ExitButtonSound))
  {
   timer = Rl.GameManager.audiodataBase
    .allAudioSettings[Rl.GameManager.audiodataBase.allAudioNames[Rl.soundStrings.ExitButtonSound].index]
    .audioClip.length - 0.1f;
   
   Rl.GameManager.PlayAudio(Rl.soundStrings.ExitButtonSound, Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
  }
  StartCoroutine(DelayApplicationQuitCo(timer));
 }

 IEnumerator DelayDeactiveDeActivateLevelSelectedImageeCo(float seconds)
 {
  yield return new WaitForSeconds(seconds);
  DeActivateLevelSelectedImage();
 }
 public void ToggleSplashSettingsMenu()
 {
  if (!SplashMenuSettingsOpened) OpenSplashSettingsMenu();
  else if (SplashMenuSettingsOpened) CloseSplashSettingsMenu();
 }

 [SerializeField] public TabGroup SplashSettingsTabGroup;
 [SerializeField] private TabGroup AdminLevelSettingsTabGroup;
 public void OpenSplashSettingsMenu()
 {
  CloseAdminSettingsPanel(true);
  CloseLevelInfoPanel(true);

  if (SplashSettingsTabGroup.selectedTab == null)
   SplashSettingsTabGroup.tabButtons.First().GetComponent<TabButton>().ClickThisButtonFromOtherScript();

  //Handheld.Vibrate();
  Rl.GameManager.PlayAudio(Rl.soundStrings.AudioSettingsButtonAudio, Random.Range(2,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
  SplashMenuSettingsOpened = true;
  fadeControllerSplashMenu.FadeSplashSettingsMenu(true);
  fadeControllerSplashMenu.FadeExitButton(true);
 }
 
 public void CloseSplashSettingsMenu()
 {
  //Handheld.Vibrate();
  Rl.GameManager.PlayAudio(Rl.soundStrings.AudioSettingsButtonAudio, Random.Range(0,2), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
  SplashMenuSettingsOpened = false;
  fadeControllerSplashMenu.FadeSplashSettingsMenu(false);
  fadeControllerSplashMenu.FadeExitButton(false);
 }
 private void CloseSplashSettingsMenuNoSound()
 {
  SplashMenuSettingsOpened = false;
  fadeControllerSplashMenu.FadeSplashSettingsMenu(false);
  fadeControllerSplashMenu.FadeExitButton(false);
 }

 public Level ChangeIntToLevel(int level) => world.LevelToLoad = world.levels[level - 1];
 public Level CheckLevel(int level) => world.levels[level - 1];

 IEnumerator FirstSelectLevelCo(string localizedValue, string extraNonLocalizedString = "",  bool nonLocalized = false)
 {
  worldNameText.color = Color.green;
  if(nonLocalized) worldNameText.text = localizedValue + extraNonLocalizedString;
  else worldNameText.text = LocalisationSystem.GetLocalisedValue(localizedValue) + extraNonLocalizedString;
  yield return new WaitForSeconds(2f);
  worldNameText.text = LocalisationSystem.GetLocalisedValue(chooseALevel);
  worldNameText.color = cashedworldNameTextColor;
 }

 public void StartGame(Transform transform)
 {
  if (levelselected)
  {
   GenericSettingsFunctions.SmallJumpAnimation(transform);
   float timer = 0;
     
   //Handheld.Vibrate();

   if (Rl.GameManager.audiodataBase.allAudioNames.ContainsKey(Rl.soundStrings.GameStartAudio))
   {
    PlayAudio.THIS.StartIngameMusic();
    Rl.GameManager.PlayAudio(Rl.soundStrings.GameStartAudio, Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    timer = Rl.GameManager.audiodataBase.allAudioSettings[Rl.GameManager.audiodataBase.allAudioNames[Rl.soundStrings.GameStartAudio].index].audioClip.length/2.35f;
    Rl.GameManager.StopAudio(Rl.GameManager.gameManagerAudioSource);
   }
   else Debug.Log("Audio not found");
  
   StartCoroutine(DelaySceneLoadCo(timer));

  }
  else
  {
   Rl.GameManager.PlayAudio(Rl.soundStrings.GameNoStartAudio, Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
   ChangeWorldText(firstSelectALevel);
  }
 }

 public void ChangeWorldText(string localizedValue, string extraNonLocalizedString = "", bool nonLocalized = false)
 {
  StopCoroutine(FirstSelectLevelCo(localizedValue, extraNonLocalizedString, nonLocalized ));
  StartCoroutine(FirstSelectLevelCo(localizedValue, extraNonLocalizedString, nonLocalized));
 }
 IEnumerator DelayApplicationQuitCo(float timer)
 {
  yield return new WaitForSeconds(timer);
  Application.Quit();
 }
 
 IEnumerator DelaySceneLoadCo(float timer)
{
 saveFileManagerInMenu.SaveGame(); 
 yield return new WaitForSeconds(timer);
 LoadingManager.CurrentLevelToload = LevelManager.THIS.currentLevel;
LevelManager.THIS.LoadLevel(LevelManager.THIS.currentLevel);
CrosssceneData.passLevelCounter = LevelManager.THIS.currentLevel;

LevelManager.THIS.gameStatus = GameState.PrepareGame;
LevelCanvasObj.SetActive(false);
}
 private void ChangeLevelInfoPanel(Level level)
 {
  GetLevelNameText(level);
  GetLevelDescriptionText(level);
  GetGameTypeLevelType(level);
  GetLevelScoreGoals(level);
  GetLevelGoalImages(level);
  GetLevelGoalImagesOverlay(level);
  GetLevelStars(level);
  GetLevelHighScore(level);
  UpdateGoalSprites(ref level);
  UpdateNewLabel(level);
  GetPerfectRun(level);
 }

 [SerializeField] private GameObject LevelInfoPanelPerfectRunStar;
 private void GetPerfectRun(Level level)
 {
  //Searching for Level instead of an int value might be risky
  foreach (LevelProgress levelProgress in world.LevelProgresses)
  {
   if (level == levelProgress.Level)
   {
   
     if(levelProgress.PerfectRun)
     {
      LevelInfoPanelPerfectRunStar.GetComponent<Image>().sprite = perfectRunArchieved;
      LevelInfoPanelPerfectRunStar.GetComponent<Image>().color = ResetColor();
      LevelInfoPanelPerfectRunStar.GetComponent<Image>().color =
       SetAlphaOnImage(LevelInfoPanelPerfectRunStar.GetComponent<Image>().color, 85);
      return;
     }
     else
     {
      LevelInfoPanelPerfectRunStar.GetComponent<Image>().sprite = perfectRunNotArchieved;
      LevelInfoPanelPerfectRunStar.GetComponent<Image>().color = ResetColor();
      LevelInfoPanelPerfectRunStar.GetComponent<Image>().color =
       SetAlphaOnImage(LevelInfoPanelPerfectRunStar.GetComponent<Image>().color, 10);
     }
   }
  }
 }

 private Color SetAlphaOnImage(Color color, float setAlphaOnimageInPerecent)
 {
  setAlphaOnimageInPerecent /= 100f;
  return new Color(color.r, color.g, color.b, color.a * setAlphaOnimageInPerecent);
 }
 private void UpdateNewLabel(Level level)
 {
  if (world.GetHighScore(level) == 0) New.active = true;
  else New.active = false;
 }
 private void GetLevelNameText(Level level)
 {
  levelInfoPanelText.text = level.LevelNameText;
 }
 private void GetLevelDescriptionText(Level level)
 {
  var t = level.LevelDescriptionText;
 }
 private void GetLevelScoreGoals(Level level)
 {
    List<float> scoreGoals = new List<float>();
   foreach (int n in level.scoreGoals)
   {
    scoreGoals.Add(n);
   }

   if (scoreGoals.Count > 0)
   {
    scoreGoalOneText.text = ReplaceDotWithKomma1000(scoreGoals[0]);
    if (scoreGoals.Count > 1) scoreGoalTwoText.text = ReplaceDotWithKomma1000(scoreGoals[1]);
    if (scoreGoals.Count > 1) scoreGoalThreeText.text = ReplaceDotWithKomma1000(scoreGoals[2]);
   }
   else
   {
    scoreGoalOneText.text = 0.ToString();
    scoreGoalTwoText.text = 0.ToString();
    scoreGoalThreeText.text = 0.ToString();
   }

 }
 [SerializeField] private List<String> allLevelGoalTags= new List<String>();
 private void GetLevelGoalImages(Level level)
 {
allLevelGoalTags.Clear();
foreach(BlankGoal n in level.levelGoals)
   allLevelGoalTags.Add(world.GetTagFromFruitType(n.fruitType));

  int levelGoalCount = allLevelGoalTags.Count;
  
  switch (levelGoalCount)
  {
   case 1:
    break;
   case 2:
    break;
   
   case 3:
    int counter = 0;
    foreach(var f in allLevelGoalTags)
    {
     foreach (var t in world.GoalLookUpTable)
     {
      if (f == t.Tag)
      {
       goalImages[counter].sprite = t.Image;
       goalImages[counter].transform.gameObject.transform.parent.GetComponent<Image>().sprite = t.BackGroundImage;
       counter++;
      }
     }
     
    }
    break;
   default:
    break;
  }
  
 }
void GetLevelGoalImagesOverlay(Level level)
 {
  Sprite[] t = level.LevelGoalImagesOverlay;
 }

 public List<GameObject> LevelInfoPanelStars = new List<GameObject>();
 private void GetLevelStars(Level level)
 {
  //Searching for Level instead of an int value might be risky
  foreach (LevelProgress levelProgress in world.LevelProgresses)
  {
   if (level == levelProgress.Level)
   {
    for (int i = 0 ; i< LevelInfoPanelStars.Count;i++ )
    {
     if(levelProgress.Sterne[i])
     LevelInfoPanelStars[i].GetComponent<Image>().sprite = starArchieved;
     else if(!levelProgress.Sterne[i])
      LevelInfoPanelStars[i].GetComponent<Image>().sprite = starNotArchieved;
    }
    return;
   }
  }
 }

 public TextMeshProUGUI LevelInfoPanelHighScore;
 private void GetLevelHighScore(Level level)
 {
  foreach (LevelProgress levelProgress in world.LevelProgresses)
  {
   if (level == levelProgress.Level)
   {
    double highScore;
    string highscoreText = "";
    if (levelProgress.HighScore  > 1000)
    {
     highscoreText = ReplaceDotWithKomma1000(levelProgress.HighScore);
    }
    else if (levelProgress.HighScore < 1001)
    {
     highScore = levelProgress.HighScore;
     highscoreText = highScore.ToString();
    }

    LevelInfoPanelHighScore.text = highscoreText;
    return;
   }
  }
 }

 [SerializeField] private GameObject levevelTypeIcon;
 private void GetGameTypeLevelType(Level level)
 {
  switch (level.endGameRequirements.GameType)
  {
   case GameType.Moves:
    levelGameTypeText.text = LocalisationSystem.GetLocalisedValue(limitedMoves);
    levevelTypeIcon.GetComponent<Image>().sprite = world.LimitedMoveImage;
    break;

   case GameType.Time:
    levelGameTypeText.text = LocalisationSystem.GetLocalisedValue(timedRun);
    levevelTypeIcon.GetComponent<Image>().sprite = world.TimedRunImage;
    break;

   default:
    levelGameTypeText.text = level.LevelGameTypeDefaultText;
    levevelTypeIcon.GetComponent<Image>().sprite = null;
    break;
  }
 }
 private void UpdateGoalSprites(ref Level level)
 {
  foreach (BlankGoal blankGoal in level.levelGoals)
  {
   blankGoal.goalSprite  = world.GetGoalSprite(blankGoal.fruitType);
  }
 }

 private void SetupProgressOnLoad()
 {
  UpdateAdminSettingsButtonsColorToDeactiveColor();
  if (LevelButtons == null)
  {
   FillLevelButtonsList();
  }

  if (world.StarsDependOnHighScore)
  {
   DeActivateStarGoals();
   ActivateStarGoals(); // we activate it depending on high score
  }
 
  SetupStars(); //puts the right spirte for the stars
  UpdateLevelAccessImage();
  UpdateLevelAccessStarVisibility();
  UpdateLevelAccessText();
 }

 
private bool IsPanelLeft(GameObject LevelButton)
{
 Vector3 LevelButtonPos = CanvasPositioningExtensions.ScreenToCanvasPosition(SplashMenuCanvasForLevelInfoPanel, LevelButton.transform.position);
 if(LevelButtonPos.x > MIDPOINTCANVAS) return true;
 return false;
}
 private void DeActivateStarGoals()
{
 foreach (LevelProgress progress in world.LevelProgresses)
 {
  for (int i = 0; i < progress.Sterne.Length; i++)
  {
   progress.Sterne[i] = false;
  }
 }
}
private void ActivateStarGoals()
{
 foreach (LevelProgress progress in world.LevelProgresses)
 {
  for (int n = 0; n < progress.Level.scoreGoals.Length; n++)
  {
   if (progress.HighScore >= progress.Level.scoreGoals[n])
   {
    progress.Sterne[n] = true;
   }
  }
 }
}
[Button()] private void SetupStars()
 {
  for (int i = 0; i < LevelButtons.Count; i++)
  {
   if (LevelButtons[i].GetComponent<LevelButton>()!= null)
   {
    if (LevelButtons[i].GetComponent<LevelButton>().Stars.Count > 0)
    {
     for (int j = 0; j < LevelButtons[i].GetComponent<LevelButton>().Stars.Count; j++)
     {
      if (world.LevelProgresses[i].Sterne != null)
      {
       if (world.LevelProgresses[i].Sterne[j])
        LevelButtons[i].GetComponent<LevelButton>().Stars[j].GetComponent<Image>().sprite = starArchieved;
       else if (!world.LevelProgresses[i].Sterne[j])
        LevelButtons[i].GetComponent<LevelButton>().Stars[j].GetComponent<Image>().sprite = starNotArchieved;
      }
     
     }
    }
   }
  }
 }

[Button()]private void FillLevelButtonsList()
{
 LevelButtons?.Clear();
 LevelButtons = FillButtonsList(Rl.LevelButtons);
}
[Button()]private void FillAdminSettingsButtonsList()
{
 AdminSettingsButtons?.Clear();
 AdminSettingsButtons = FillButtonsList(Rl.AdminSettingsButtons);
}
 private List<GameObject> FillButtonsList(string objectsToFind )
 {
  HashSet<GameObject> buttonHashsetHelper = new HashSet<GameObject>();
  buttonHashsetHelper.Clear();
  foreach (GameObject o in GameObject.FindGameObjectsWithTag(objectsToFind))
  {
   buttonHashsetHelper.Add(o);
  }

  List<GameObject> Buttons = new List<GameObject>();
  Buttons.AddRange(buttonHashsetHelper);
List<GameObject> orderderListByname =  Buttons.OrderBy(n => n.name).ToList();
return orderderListByname;
 }
 
 public void UpdateAdminSettingsButtonsColor(Image adminSettingsButtonImage)
 {
  CloseLevelInfoPanel(true);
  CloseSplashSettingsMenuNoSound();
  UpdateAdminSettingsButtonsColorToDeactiveColor();
  adminSettingsButtonImage.GetComponent<Image>().color = ResetColor();
 }

 [SerializeField] private string adminSettingsButtonsDeactiveColor = "#B1BAFF";
 [SerializeField] private float adminSettingsButtonsDeactiveAlpha = 0.5f;
 
 [Button]
 private void UpdateAdminSettingsButtonsColorToDeactiveColor()
 {
  Color color;
  ColorUtility.TryParseHtmlString(adminSettingsButtonsDeactiveColor, out color);
  for (int i = 0; i < AdminSettingsButtons.Count; i++)
  {
   AdminSettingsButtons[i].GetComponent<Image>().color = color;
   AdminSettingsButtons[i].GetComponent<Image>().color = new Color(
    AdminSettingsButtons[i].GetComponent<Image>().color.r, AdminSettingsButtons[i].GetComponent<Image>().color.g,
    AdminSettingsButtons[i].GetComponent<Image>().color.b, adminSettingsButtonsDeactiveAlpha);
  }
 }

 private Color ResetColor() => new Color(255, 255, 255, 1);
 public  string ReplaceDotWithKomma1000(double number)
 {
  if (number < 10000) return number.ToString();
  Char kommaCharacter =  LocalisationSystem.GetLocalisedValue(komma).ToCharArray(0, 1)[0];
  string newNumberString = (number / 1000).ToString();
  newNumberString = newNumberString.Replace('.', kommaCharacter);
  
  Char[] counting = newNumberString.ToCharArray();
  int counter = 0;
  int actualNumbersAfterKomma = counting.Length-1;
  for (int i = 0; i < counting.Length; i++)
  {
   if (counting[i] == kommaCharacter) break;
   counter++;
  }

  switch (counting.ToArray().Length)
  {
   case 1: numbersAfterKommaAllowed = 3;
    break;
   case 2: numbersAfterKommaAllowed = 3; 
    break;
   case 3: numbersAfterKommaAllowed = 3;
    break;
   case 4: numbersAfterKommaAllowed = 2;
    break;
   case 5: numbersAfterKommaAllowed = 1;
    break;
   default: numbersAfterKommaAllowed = -1;
    break;
  }
  actualNumbersAfterKomma -= counter;
  int howMuchtoTrim = actualNumbersAfterKomma - numbersAfterKommaAllowed;

  if(howMuchtoTrim > 0)
   newNumberString = newNumberString.Remove(counting.Length-1-howMuchtoTrim, howMuchtoTrim);
  else if (howMuchtoTrim == 0)
  {
   newNumberString = newNumberString.Trim(kommaCharacter);
  }
  newNumberString += LocalisationSystem.GetLocalisedValue(thousandShortString);
  return newNumberString;
 }
}
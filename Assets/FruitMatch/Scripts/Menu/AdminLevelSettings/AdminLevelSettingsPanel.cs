using NaughtyAttributes;
using TMPro;
using UnityEngine;

public sealed class AdminLevelSettingsPanel : MonoBehaviour
{
  [Header("Imports")]
  [SerializeField] private SaveFileLevelConfigManagement saveFileLevelConfigManagement;

  [Space] [Header("General Settings")] 
  [SerializeField] private Language levelTextlanguage;
  
  [Space]
  [SerializeField] private TextMeshProUGUI levelName;
  [SerializeField] private TextMeshProUGUI levelDescription;
  [SerializeField] private TextMeshProUGUI levelTypeDefaultText;
  
  [Space]
  [Header("Level Text English")]
  [SerializeField] private TMP_InputField levelNameInputFieldEnglish;
  [SerializeField] private TextMeshProUGUI levelNameTextFieldEnglish;
  [SerializeField] private TMP_InputField levelDesriptionInputFieldEnglish;
  [SerializeField] private TextMeshProUGUI levelDescirptionTextFieldEnglish;
  [SerializeField] private TMP_InputField levelGameDefaultTextInputFieldEnglish;
  [SerializeField] private TextMeshProUGUI levelGameDefaultTextTextFieldEnglish;


  [Space] [Header("German")]
  [SerializeField] private TMP_InputField levelNameInputFieldGerman;
  [SerializeField] private TextMeshProUGUI levelNameTextFieldGerman;
  [SerializeField] private TMP_InputField levelDesriptionInputFieldGerman;
  [SerializeField] private TextMeshProUGUI levelDescirptionTextFieldGerman;
  [SerializeField] private TMP_InputField levelGameDefaultTextInputFieldGerman;
  [SerializeField] private TextMeshProUGUI levelGameDefaultTextTextFieldGerman;

  [Space] [Header("French")]
  [SerializeField] private TMP_InputField levelNameInputFieldFrench;
  [SerializeField] private TextMeshProUGUI levelNameTextFieldFrench;
  [SerializeField] private TMP_InputField levelDesriptionInputFieldFrench;
  [SerializeField] private TextMeshProUGUI levelDescirptionTextFieldFrench;
  [SerializeField] private TMP_InputField levelGameDefaultTextInputFieldFrench;
  [SerializeField] private TextMeshProUGUI levelGameDefaultTextTextFieldFrench;

  [Space] [Header("Spanish")]
  [SerializeField] private TMP_InputField levelNameInputFieldSpanish;
  [SerializeField] private TextMeshProUGUI levelNameTextFieldSpanish;
  [SerializeField] private TMP_InputField levelDesriptionInputFieldSpanish;
  [SerializeField] private TextMeshProUGUI levelDescirptionTextFieldSpanish;
  [SerializeField] private TMP_InputField levelGameDefaultTextInputFieldSpanish;
  [SerializeField] private TextMeshProUGUI levelGameDefaultTextTextFieldSpanish;

  [Space] [Header("Swedish")]
  [SerializeField] private TMP_InputField levelNameInputFieldSwedish;
  [SerializeField] private TextMeshProUGUI levelNameTextFieldSwedish;
  [SerializeField] private TMP_InputField levelDesriptionInputFieldSwedish;
  [SerializeField] private TextMeshProUGUI levelDescirptionTextFieldSwedish;
  [SerializeField] private TMP_InputField levelGameDefaultTextInputFieldSwedish;
  [SerializeField] private TextMeshProUGUI levelGameDefaultTextTextFieldSwedish;

  [Space] [Header("Turkish")] 

  [SerializeField] private TMP_InputField levelNameInputFieldTurkish;
  [SerializeField] private TextMeshProUGUI levelNameTextFieldTurkish;
  [SerializeField] private TMP_InputField levelDesriptionInputFieldTurkish;
  [SerializeField] private TextMeshProUGUI levelDescirptionTextFieldTurkish;
  [SerializeField] private TMP_InputField levelGameDefaultTextInputFieldTurkish;
  [SerializeField] private TextMeshProUGUI levelGameDefaultTextTextFieldTurkish;

  [Space] [Header("Polish")] 

  [SerializeField] private TMP_InputField levelNameInputFieldPolish;
  [SerializeField] private TextMeshProUGUI levelNameTextFieldPolish;
  [SerializeField] private TMP_InputField levelDesriptionInputFieldPolish;
  [SerializeField] private TextMeshProUGUI levelDescirptionTextFieldPolish;
  [SerializeField] private TMP_InputField levelGameDefaultTextInputFieldPolish;
  [SerializeField] private TextMeshProUGUI levelGameDefaultTextTextFieldPolish;

  [Space] [Header("Italian")] 

  [SerializeField] private TMP_InputField levelNameInputFieldItalian;
  [SerializeField] private TextMeshProUGUI levelNameTextFieldItalian;
  [SerializeField] private TMP_InputField levelDesriptionInputFieldItalian;
  [SerializeField] private TextMeshProUGUI levelDescirptionTextFieldItalian;
  [SerializeField] private TMP_InputField levelGameDefaultTextInputFieldItalian;
  [SerializeField] private TextMeshProUGUI levelGameDefaultTextTextFieldItalian;


  [SerializeField] private Switch SwitchDefaultLang;
  
  public int LevelAdminLevelSettingsLevelNumber = 0;
  

  public void AcceptLevelChanges()
  {
    int level = LevelAdminLevelSettingsLevelNumber;
    Debug.Log("Save Level " + level);
    Rl.GameManager.PlayAudio(Rl.soundStrings.AcceptLevelSound , Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    SwitchButtonFruitsManager.SaveFruitSettings();

    Rl.switchButtonsBombs.SaveBombSettings();
    BoardDimensionsConfig boardDimensionsConfig =  Rl.adminLevelSettingsBoard.SaveBoardsSettingsFromClipBoard();
    ScoreGoalsConfig scoreGoalsConfig = Rl.archievmentButtons.SaveBoardsSettingsFromClipBoard();

  GoalConfig goalConfig = Rl.adminLevelSettingsGoalConfig.SaveGoalConfigs();
  SideFruitsFieldConfig sideFruitsFieldConfig = Rl.adminLevelSettingsSideDots.SaveSideDotSettings();
  AnimationConfig animationConfig = saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].AnimationConfig;
  DateInformation dateInformation = SaveUtil.GetDateInformation();
  LevelTextConfig[] levelTextConfigs = SaveLevelTextConfig();
  BombConfig[] bombConfigs = saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BombConfigs;
  LuckConfig luckConfig = Rl.adminLevelLuckSettings.SaveLuckSettingsFromClipBoard();

  GraphicConfig graphicConfig = Rl.adminLevelSettingsLookDev.SaveBorderGraphicSetting();
  //graphicConfig =  Rl.adminLevelSettingsBoard.SaveBorderGraphicSetting();
  MatchFinderConfig matchFinderConfig = Rl.AdminLevelSettingsMatchFinder.SaveMatchFinderSettings();
  LayoutFieldConfig layoutFieldConfig = Rl.adminLevelSettingsLayout.SaveLayoutSettings();
  TileSettingFieldConfig[] tileSettingFieldConfigs = Rl.adminLevelSettingsTiles.SaveTileSettings();
  //ExtraSettings extraSettings = Rl.adminlevelSettingsExtraSettings.SaveExtraSettings;
  
  
   saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level] = new(
     boardDimensionsConfig, scoreGoalsConfig, levelTextConfigs, goalConfig, sideFruitsFieldConfig,
     animationConfig, dateInformation, bombConfigs,luckConfig, graphicConfig,
      matchFinderConfig, layoutFieldConfig, tileSettingFieldConfigs, new ExtraSettings(true));
   
    saveFileLevelConfigManagement.Save();
  }

  private LevelTextConfig[] SaveLevelTextConfig()
  {
    LevelTextConfig[] levelTextConfigs = new LevelTextConfig[SaveFileLevelConfigManagement.enumCountLanguage()];
    
    for (int i = 0; i < SaveFileLevelConfigManagement.enumCountLanguage(); i++)
    {
      
      switch (SaveFileLevelConfigManagement.enumGetSpecificLanguageValue(i))
      {
        case Language.English:
          levelTextConfigs[(int)Language.English].Language = Language.English;
          levelTextConfigs[(int)Language.English].LevelName = levelNameInputFieldEnglish.text;
          levelTextConfigs[(int)Language.English].LevelDescriptionText = levelDesriptionInputFieldEnglish.text;
          levelTextConfigs[(int)Language.English].LevelGameTypeText = levelGameDefaultTextInputFieldEnglish.text;
          
          levelTextConfigs[(int)Language.English].LevelNameDefaultText = Rl.saveClipBoard.levelNameDefaultTextBool[(int)Language.English];
          levelTextConfigs[(int)Language.English].LevelDescriptionDefaultText = Rl.saveClipBoard.leveldescriptionDefaultTextBool[(int)Language.English];
          levelTextConfigs[(int)Language.English].LevelGameTypeDefaultText = Rl.saveClipBoard.modeDefaultTextBool[(int)Language.English];
      
          // --- insert  boolean defualt text --- //
          //    Rl.saveClipBoard.defaultModeText;
          break;
        case Language.German:
          levelTextConfigs[(int)Language.German].Language = Language.German;
          levelTextConfigs[(int)Language.German].LevelName = levelNameInputFieldGerman.text;
          levelTextConfigs[(int)Language.German].LevelDescriptionText = levelDesriptionInputFieldGerman.text;
          levelTextConfigs[(int)Language.German].LevelGameTypeText = levelGameDefaultTextInputFieldGerman.text;
          
          levelTextConfigs[(int)Language.German].LevelNameDefaultText = Rl.saveClipBoard.levelNameDefaultTextBool[(int)Language.German];
          levelTextConfigs[(int)Language.German].LevelDescriptionDefaultText = Rl.saveClipBoard.leveldescriptionDefaultTextBool[(int)Language.German];
          levelTextConfigs[(int)Language.German].LevelGameTypeDefaultText = Rl.saveClipBoard.modeDefaultTextBool[(int)Language.German];
          break;
        case Language.French:
          levelTextConfigs[(int)Language.French].Language = Language.French;
          levelTextConfigs[(int)Language.French].LevelName = levelNameInputFieldFrench.text;
          levelTextConfigs[(int)Language.French].LevelDescriptionText = levelDesriptionInputFieldFrench.text;
          levelTextConfigs[(int)Language.French].LevelGameTypeText = levelGameDefaultTextInputFieldFrench.text;
          
          levelTextConfigs[(int)Language.French].LevelNameDefaultText = Rl.saveClipBoard.levelNameDefaultTextBool[(int)Language.French];
          levelTextConfigs[(int)Language.French].LevelDescriptionDefaultText = Rl.saveClipBoard.leveldescriptionDefaultTextBool[(int)Language.French];
          levelTextConfigs[(int)Language.French].LevelGameTypeDefaultText = Rl.saveClipBoard.modeDefaultTextBool[(int)Language.French];
          break;
        case Language.Spanish:
          levelTextConfigs[(int)Language.Spanish].Language = Language.Spanish;
          levelTextConfigs[(int)Language.Spanish].LevelName = levelNameInputFieldSpanish.text;
          levelTextConfigs[(int)Language.Spanish].LevelDescriptionText = levelDesriptionInputFieldSpanish.text;
          levelTextConfigs[(int)Language.Spanish].LevelGameTypeText = levelGameDefaultTextInputFieldSpanish.text;
          
          levelTextConfigs[(int)Language.Spanish].LevelNameDefaultText = Rl.saveClipBoard.levelNameDefaultTextBool[(int)Language.Spanish];
          levelTextConfigs[(int)Language.Spanish].LevelDescriptionDefaultText = Rl.saveClipBoard.leveldescriptionDefaultTextBool[(int)Language.Spanish];
          levelTextConfigs[(int)Language.Spanish].LevelGameTypeDefaultText = Rl.saveClipBoard.modeDefaultTextBool[(int)Language.Spanish];
          break;
        case Language.Swedish:
          levelTextConfigs[(int)Language.Swedish].Language = Language.Swedish;
          levelTextConfigs[(int)Language.Swedish].LevelName = levelNameInputFieldSwedish.text;
          levelTextConfigs[(int)Language.Swedish].LevelDescriptionText = levelDesriptionInputFieldSwedish.text;
          levelTextConfigs[(int)Language.Swedish].LevelGameTypeText = levelGameDefaultTextInputFieldSwedish.text;
          
          levelTextConfigs[(int)Language.Swedish].LevelNameDefaultText = Rl.saveClipBoard.levelNameDefaultTextBool[(int)Language.Swedish];
          levelTextConfigs[(int)Language.Swedish].LevelDescriptionDefaultText = Rl.saveClipBoard.leveldescriptionDefaultTextBool[(int)Language.Swedish];
          levelTextConfigs[(int)Language.Swedish].LevelGameTypeDefaultText = Rl.saveClipBoard.modeDefaultTextBool[(int)Language.Swedish];
          break;
        case Language.Turkish:
          levelTextConfigs[(int)Language.Turkish].Language = Language.Turkish;
          levelTextConfigs[(int)Language.Turkish].LevelName = levelNameInputFieldTurkish.text;
          levelTextConfigs[(int)Language.Turkish].LevelDescriptionText = levelDesriptionInputFieldTurkish.text;
          levelTextConfigs[(int)Language.Turkish].LevelGameTypeText = levelGameDefaultTextInputFieldTurkish.text;
          
          levelTextConfigs[(int)Language.Turkish].LevelNameDefaultText = Rl.saveClipBoard.levelNameDefaultTextBool[(int)Language.Turkish];
          levelTextConfigs[(int)Language.Turkish].LevelDescriptionDefaultText = Rl.saveClipBoard.leveldescriptionDefaultTextBool[(int)Language.Turkish];
          levelTextConfigs[(int)Language.Turkish].LevelGameTypeDefaultText = Rl.saveClipBoard.modeDefaultTextBool[(int)Language.Turkish];
          break;
        case Language.Polish:
          levelTextConfigs[(int)Language.Polish].Language = Language.Polish;
          levelTextConfigs[(int)Language.Polish].LevelName = levelNameInputFieldPolish.text;
          levelTextConfigs[(int)Language.Polish].LevelDescriptionText = levelDesriptionInputFieldPolish.text;
          levelTextConfigs[(int)Language.Polish].LevelGameTypeText = levelGameDefaultTextInputFieldPolish.text;
          
          levelTextConfigs[(int)Language.Polish].LevelNameDefaultText = Rl.saveClipBoard.levelNameDefaultTextBool[(int)Language.Polish];
          levelTextConfigs[(int)Language.Polish].LevelDescriptionDefaultText = Rl.saveClipBoard.leveldescriptionDefaultTextBool[(int)Language.Polish];
          levelTextConfigs[(int)Language.Polish].LevelGameTypeDefaultText = Rl.saveClipBoard.modeDefaultTextBool[(int)Language.Polish];
          break;
        case Language.Italian:
          levelTextConfigs[(int)Language.Italian].Language = Language.Italian;
          levelTextConfigs[(int)Language.Italian].LevelName = levelNameInputFieldItalian.text;
          levelTextConfigs[(int)Language.Italian].LevelDescriptionText = levelDesriptionInputFieldItalian.text;
          levelTextConfigs[(int)Language.Italian].LevelGameTypeText = levelGameDefaultTextInputFieldItalian.text;
          
          levelTextConfigs[(int)Language.Italian].LevelNameDefaultText = Rl.saveClipBoard.levelNameDefaultTextBool[(int)Language.Italian];
          levelTextConfigs[(int)Language.Italian].LevelDescriptionDefaultText = Rl.saveClipBoard.leveldescriptionDefaultTextBool[(int)Language.Italian];
          levelTextConfigs[(int)Language.Italian].LevelGameTypeDefaultText = Rl.saveClipBoard.modeDefaultTextBool[(int)Language.Italian];
          break;
      }
    }
    
    return levelTextConfigs;
  }
  public void LoadSettings() => RevertChanges(true);
  
  [Button] public void LoadLevelname(TMP_InputField levelNameInputField, TextMeshProUGUI  levelNameTextField)
  {
    int level = LevelAdminLevelSettingsLevelNumber;
    
    string levelNameTextText = saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].LevelTextConfig[(int)levelTextlanguage].LevelName;
    levelNameInputField.text = levelNameTextText;
    levelNameTextField.text = levelNameTextText;
  }
  
  [Button] public void LoadDescriptionText(TMP_InputField levelDescriptionInputField, TextMeshProUGUI levelDescriptionTextField)
  {
    int level = LevelAdminLevelSettingsLevelNumber;
    string levelDescriptionTextText = saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].LevelTextConfig[(int)levelTextlanguage].LevelDescriptionText;
    levelDescriptionInputField.text = levelDescriptionTextText;
    levelDescriptionTextField.text = levelDescriptionTextText;
  }

  public void LoadTypeDefaultTextDefaultBool()
  {
    int level = LevelAdminLevelSettingsLevelNumber;
    Rl.saveClipBoard.defaultModeText = saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].LevelTextConfig[(int)levelTextlanguage]
      .LevelDefaultLanguage;
    if (Rl.saveClipBoard.defaultModeText) SwitchOn(true);
    else SwitchOff(true);
  }
 [Button] public void LoadTypeDefaultText(TMP_InputField  levelGameDefaultTextInputField, TextMeshProUGUI levelGameDefaultTextTextField)
  {
    int level = LevelAdminLevelSettingsLevelNumber;
    string LevelGameDefaultTextText = saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].LevelTextConfig[(int)levelTextlanguage].LevelGameTypeText;
    levelGameDefaultTextInputField.text = LevelGameDefaultTextText;
    levelGameDefaultTextTextField.text = LevelGameDefaultTextText;
  }
 
 
 public void SwitchOff(bool playNoSound)
 {
   if (Rl.saveClipBoard.languageTextDefault == levelTextlanguage) return;
   Rl.saveClipBoard.defaultModeText = false;
   SwitchThings(false, playNoSound);
 }
    
 public void SwitchOn(bool playNoSound)
 {
   Rl.saveClipBoard.languageTextDefault = levelTextlanguage;
   Rl.saveClipBoard.defaultModeText = true;
   MakeThisLanguageDefaultTextLanguage();
   SwitchThings(true, playNoSound);
 }

 private void SwitchThings(bool on, bool playNoSound) => SwitchDefaultLang.SwitchButton(on, playNoSound);



  public void RevertChanges(bool playNoSound)
  {
    if(!playNoSound) 
      Rl.GameManager.PlayAudio(Rl.soundStrings.RevertChangesSound, Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    int level = LevelAdminLevelSettingsLevelNumber;
    
    Debug.Log("Load Level " + level);
    
   SwitchButtonFruitsManager.LoadAllFruitSettings(saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BoardDimensionsConfig );
   
     LoadText();

      Rl.switchButtonsBombs.LoadSettingsToclipboard(saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BombConfigs);
      Rl.switchButtonsBombs.SetSwitches();
      Rl.adminLevelLuckSettings.LoadLuckSettings(saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].LuckConfig);
      Rl.adminLevelSettingsBoard.LoadBoardSettings(saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BoardDimensionsConfig);
      Rl.AdminLevelSettingsMatchFinder.LoadMatchFinderSettings(saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].matchFinderConfig);
      Rl.archievmentButtons.LoadArchievmentSettings(saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].ScoreGoalsConfig);
      Rl.adminLevelSettingsLayout.LoadLayoutSettings(saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].LayoutFieldConfigs);
      Rl.adminLevelSettingsSideDots.LoadSideDotSettings(saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].SideFruitsFieldConfig);
      Rl.adminLevelSettingsGoalConfig.LoadGoalConfig(saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].GoalConfig);
      Rl.adminLevelSettingsLookDev.LoadFromDisk(saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].GraphicConfig);
      Rl.adminLevelSettingsTiles.LoadFromDisk(saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].TileSettingFieldConfigs);
      
      AnimationConfig animationConfig = saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].AnimationConfig;
      DateInformation dateInformation = SaveUtil.GetDateInformation();
  }

  private void LoadText()
  {
    TMP_InputField levelNameInputField = levelNameInputFieldEnglish; 
      TextMeshProUGUI levelNameTextField = levelNameTextFieldEnglish;; 
      TMP_InputField levelDesriptionInputField = levelDesriptionInputFieldEnglish; 
      TextMeshProUGUI levelDescirptionTextField = levelDescirptionTextFieldEnglish; 
      TMP_InputField levelGameDefaultTextInputField = levelGameDefaultTextInputFieldEnglish; 
      TextMeshProUGUI levelGameDefaultTextTextField = levelGameDefaultTextTextFieldEnglish;
      
      switch (levelTextlanguage)
      {
        case Language.English:
          levelNameInputField = levelNameInputFieldEnglish; 
          levelNameTextField = levelNameTextFieldEnglish; 
          levelDesriptionInputField = levelDesriptionInputFieldEnglish; 
          levelDescirptionTextField = levelDescirptionTextFieldEnglish; 
          levelGameDefaultTextInputField = levelGameDefaultTextInputFieldEnglish; 
          levelGameDefaultTextTextField = levelGameDefaultTextTextFieldEnglish;
          break;
        case Language.German:
          levelNameInputField = levelNameInputFieldGerman; 
          levelNameTextField = levelNameTextFieldGerman; 
          levelDesriptionInputField = levelDesriptionInputFieldGerman; 
          levelDescirptionTextField = levelDescirptionTextFieldGerman; 
          levelGameDefaultTextInputField = levelGameDefaultTextInputFieldGerman; 
          levelGameDefaultTextTextField = levelGameDefaultTextTextFieldGerman;
          break;
        case Language.French:
          levelNameInputField = levelNameInputFieldFrench; 
          levelNameTextField = levelNameTextFieldFrench; 
          levelDesriptionInputField = levelDesriptionInputFieldFrench; 
          levelDescirptionTextField = levelDescirptionTextFieldFrench; 
          levelGameDefaultTextInputField = levelGameDefaultTextInputFieldFrench; 
          levelGameDefaultTextTextField = levelGameDefaultTextTextFieldFrench;
          break;
        case Language.Spanish:
          levelNameInputField = levelNameInputFieldSpanish; 
          levelNameTextField = levelNameTextFieldSpanish; 
          levelDesriptionInputField = levelDesriptionInputFieldSpanish; 
          levelDescirptionTextField = levelDescirptionTextFieldSpanish; 
          levelGameDefaultTextInputField = levelGameDefaultTextInputFieldSpanish; 
          levelGameDefaultTextTextField = levelGameDefaultTextTextFieldSpanish;
          break;
        case Language.Swedish:
          levelNameInputField = levelNameInputFieldSwedish; 
          levelNameTextField = levelNameTextFieldSwedish; 
          levelDesriptionInputField = levelDesriptionInputFieldSwedish; 
          levelDescirptionTextField = levelDescirptionTextFieldSwedish; 
          levelGameDefaultTextInputField = levelGameDefaultTextInputFieldSwedish; 
          levelGameDefaultTextTextField = levelGameDefaultTextTextFieldSwedish;
          break;
        case Language.Turkish:
          levelNameInputField = levelNameInputFieldTurkish; 
          levelNameTextField = levelNameTextFieldTurkish; 
          levelDesriptionInputField = levelDesriptionInputFieldTurkish; 
          levelDescirptionTextField = levelDescirptionTextFieldTurkish; 
          levelGameDefaultTextInputField = levelGameDefaultTextInputFieldTurkish; 
          levelGameDefaultTextTextField = levelGameDefaultTextTextFieldTurkish;
          break;
        case Language.Polish:
          levelNameInputField = levelNameInputFieldPolish; 
          levelNameTextField = levelNameTextFieldPolish; 
          levelDesriptionInputField = levelDesriptionInputFieldPolish; 
          levelDescirptionTextField = levelDescirptionTextFieldPolish; 
          levelGameDefaultTextInputField = levelGameDefaultTextInputFieldPolish; 
          levelGameDefaultTextTextField = levelGameDefaultTextTextFieldPolish;
          break;
        case Language.Italian:
          levelNameInputField = levelNameInputFieldItalian; 
          levelNameTextField = levelNameTextFieldItalian; 
          levelDesriptionInputField = levelDesriptionInputFieldItalian; 
          levelDescirptionTextField = levelDescirptionTextFieldItalian; 
          levelGameDefaultTextInputField = levelGameDefaultTextInputFieldItalian; 
          levelGameDefaultTextTextField = levelGameDefaultTextTextFieldItalian;
          break;
      }
     LoadLevelname(levelNameInputField, levelNameTextField);
     LoadDescriptionText(levelDesriptionInputField, levelDescirptionTextField );
     LoadTypeDefaultText(levelGameDefaultTextInputField, levelGameDefaultTextTextField);
     LoadTypeDefaultTextDefaultBool();
  }
  public void SetLanguageAndLoadText(Language language)
  {
    levelTextlanguage = language;
    if (Rl.saveClipBoard.languageTextDefault != levelTextlanguage)
      SwitchOff(true);
    else SwitchOn(true);
  }
  private void MakeThisLanguageDefaultTextLanguage()
  {
    for (int i = 0; i < saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs.Length; i++)
    {
      for (int j = 0; j < saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[i].LevelTextConfig.Length; j++)
      {
        if (saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[i].LevelTextConfig[j].Language !=
            Rl.saveClipBoard.languageTextDefault)
        {
          
          saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[i].LevelTextConfig[j]
            .LevelDefaultLanguage = false;
        }
        //this is not default text, it is about "make this language the default and only used as a dummy
        else
        {
          saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[i].LevelTextConfig[j]
            .LevelDefaultLanguage = true;
        }
      }
    }
  }
}
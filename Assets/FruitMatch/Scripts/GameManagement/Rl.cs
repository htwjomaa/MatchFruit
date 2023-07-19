//----------------   Reference Library  -------------------//
// This class job is to have all References that we need in the scene 
// It converts all nonstatic references to static references, so all you need
// to do is, to reference  the needed Reference via  RF.REFERENCE
// for example  Rl.PlayerRB
// This class also serialized the objects it wants to null before the start.
// 1. Define the Reference as  typeREF
// 2. Define the Static as  Type
// 3. Assign the ref to static:  Type = typeRef
// (optional)4. Add the Ref to "FillNullHashsetRefLibrary()" 
// (optional)5. Add the Ref  FindObjectOfType to "PopulateList"
//----------------                    -------------------//

using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
public sealed class Rl : MonoBehaviour
{
    //----------------   ReferenceManager -------------------//
    public static List<Object> AllSingleHandedReferences;
    [SerializeField]private List<Object> allReferenceFromRefLibraryToNull;
    //----------------   References in Level -------------------//
    [SerializeField] private Camera cameraRef;
    [SerializeField] private GameManager gameManagerRef;
    [SerializeField] private Settings settingsRef;
    [SerializeField] private Board boardRef;
    [SerializeField] private PatternSettings patternSettingsRef;
    [SerializeField] private DotPrefabs dotPrefabsRef;
    [SerializeField] private FindMatches findMatchesRef;
    [SerializeField] private HintManager HintManagerRef;
    [SerializeField] private ScoreManager ScoreManagerRef;
    [SerializeField] private Effects effectsRef;
    [SerializeField] private GoalManager goalManagerRef;
    [SerializeField] private EndGameManager endGameManagerRef;
    [SerializeField] private FadeController fadePanelControllerRef;
    [SerializeField] private SplashMenu splashMenuRef;
    [SerializeField] private AdminLevelSettingsPanel adminLevelSettingsPaneRef;
    [SerializeField] private SaveFileManagerInMenu saveFileManagerInMenuRef;
    [SerializeField] private RowDecreaser rowDecreaserRef;
    [SerializeField] private DestroyOnBoard destroyOnBoardRef;
    [SerializeField] private BoardUtil boardUtilRef;
    [SerializeField] private DebugBoard debugBoardRef;
    [Space]
    [SerializeField] private SaveClipBoard saveClipBoardRef;
    [SerializeField] private World worldRef;
    [SerializeField] private SaveFileLevelConfigManagement saveFileLevelConfigManagementRef;
    [SerializeField] private SaveFilePlayerProgressManagement saveFilePlayerProgressManagementRef;
    [SerializeField] private SettingsPanelManager settingsPanelManagerRef;
    [SerializeField] private SwitchButtonsBombs switchButtonsBombsRef;
    [SerializeField] private UISounds uiSoundsRef;
    [SerializeField] private ArchievmentButtons archievmentButtonsRef;
    [SerializeField] private LocaliserManager localiserManagerRef;
    [SerializeField] private AdminLevelLuckSettings adminLevelLuckSettingsRef;
    [SerializeField] private AdminLevelSettingsBoard adminLevelSettingsBoardRef;
    [FormerlySerializedAs("adminLevelTileRef")] [SerializeField] private AdminLevelSettingsTiles adminLevelTilesRef;
    [SerializeField] private BoardPreview boardPreviewRef;
    [SerializeField] private FadeControllerSplashMenu fadeControllerSplashMenuRef;
    [SerializeField] private AdminLevelSettingsMatchFinder adminLevelSettingsMatchFinderRef;
    [SerializeField] private AdminLevelSettingsLayout adminLevelSettingsLayoutRef;
    [SerializeField] private AdminLevelSettingsSideDots adminLevelSettingsSideDotsRef;
    [SerializeField] private AdminLevelSettingsGoalConfig adminLevelSettingsGoalConfigRef;
    [SerializeField] private AdminLevelSettingsLookDev adminLevelSettingsLookDevRef;
    [SerializeField] private AdminLevelSettingsConfig adminlevelSettingsConfigRef;
    [SerializeField] private SoundStrings soundStringsRef;
    [SerializeField] private WorldMap worldMapRef;
    [SerializeField] private WorldMapSaveManagement worldMapSaveManagementRef;
    [SerializeField] private BoardPreviewDragNDrop boardPreviewDragNDropRef;
    [SerializeField] private TargetSettings targetSettingsRef;
    //----------------   statics  -------------------//
    public static Camera Cam;
    public static Settings settings;
    public static GameManager GameManager;
    public static Board board;
    public static PatternSettings patternSettings;
    public static DotPrefabs dotPrefabs;
    public static FindMatches findMatches;
    public static HintManager hintManager;
    public static ScoreManager ScoreManager;
    public static Effects effects;
    public static GoalManager goalManager;
    public static EndGameManager endGameManager;
    public static FadeController fadePanelController;
    public static SplashMenu splashMenu;
    public static AdminLevelSettingsPanel adminLevelSettingsPanel;
    public static SaveFileManagerInMenu saveFileManagerInMenu;
    public static RowDecreaser rowDecreaser;
    public static DestroyOnBoard destroyOnBoard;
    public static BoardUtil boardUtil;
    public static DebugBoard debugBoard;
    public static SettingsPanelManager settingsPanelManager;
    public static SwitchButtonsBombs switchButtonsBombs;
    public static UISounds uiSounds;
    public static ArchievmentButtons archievmentButtons;
    public static LocaliserManager localiserManager;
    public static AdminLevelLuckSettings adminLevelLuckSettings;
    public static AdminLevelSettingsBoard adminLevelSettingsBoard;
    public static AdminLevelSettingsTiles adminLevelSettingsTiles;
    public static BoardPreview BoardPreview;
    public static FadeControllerSplashMenu fadeControllerSplashMenu;
    public static AdminLevelSettingsConfig adminlevelSettingsConfig;
    [Space]
    public static World world;
    public static SaveFileLevelConfigManagement saveFileLevelConfigManagement ;
    public static SaveClipBoard saveClipBoard;
    public static SaveFilePlayerProgressManagement saveFilePlayerProgressManagement;
    public static AdminLevelSettingsMatchFinder AdminLevelSettingsMatchFinder;
    public static AdminLevelSettingsLayout adminLevelSettingsLayout;
    public static AdminLevelSettingsSideDots adminLevelSettingsSideDots;
    public static AdminLevelSettingsGoalConfig adminLevelSettingsGoalConfig;
    public static AdminLevelSettingsLookDev adminLevelSettingsLookDev;
    public static SoundStrings soundStrings;
    public static WorldMap worldMap;
    public static WorldMapSaveManagement worldMapSaveManagement;
    public static BoardPreviewDragNDrop boardPreviewDragNDrop;
    public static TargetSettings targetSettings;
    
    [ShowNonSerializedField] public static string Game = "game";
    [ShowNonSerializedField] public static string GameScene = "GameScene";
    [ShowNonSerializedField] public static string LevelButtons = "LevelButtton";
    [ShowNonSerializedField] public static string AdminSettingsButtons = "AdminSettingsButtton";
   

    //----------------   Unity Loops  -------------------//
    private void Awake()
    {
        AssignRefs();
        AddToReferenceNullList(allReferenceFromRefLibraryToNull);
 
    }
    private void Start() => NullEverything();

    //----------------   Methods assign on Game Start -------------------//
    private void AssignRefs()
    {
        Cam = cameraRef;
        GameManager = gameManagerRef;
        settings = settingsRef;
        board = boardRef;
        patternSettings = patternSettingsRef;
        dotPrefabs = dotPrefabsRef;
        findMatches = findMatchesRef;
        hintManager = HintManagerRef;
        ScoreManager = ScoreManagerRef;
        effects = effectsRef;
        goalManager = goalManagerRef;
        endGameManager = endGameManagerRef;
        fadePanelController = fadePanelControllerRef;
        splashMenu = splashMenuRef;
        adminLevelSettingsPanel = adminLevelSettingsPaneRef;
        saveFileManagerInMenu = saveFileManagerInMenuRef;
        world = worldRef; 
        saveFileLevelConfigManagement = saveFileLevelConfigManagementRef;
        saveFilePlayerProgressManagement = saveFilePlayerProgressManagementRef;
        rowDecreaser = rowDecreaserRef;
        destroyOnBoard = destroyOnBoardRef;
        boardUtil = boardUtilRef;
        debugBoard = debugBoardRef;
        settingsPanelManager = settingsPanelManagerRef;
        switchButtonsBombs = switchButtonsBombsRef;
        saveClipBoard = saveClipBoardRef;
        uiSounds = uiSoundsRef;
        archievmentButtons = archievmentButtonsRef;
        localiserManager = localiserManagerRef;
        adminLevelLuckSettings = adminLevelLuckSettingsRef;
        adminLevelSettingsBoard = adminLevelSettingsBoardRef;
        adminLevelSettingsTiles = adminLevelTilesRef;
        BoardPreview = boardPreviewRef;
        fadeControllerSplashMenu = fadeControllerSplashMenuRef;
        AdminLevelSettingsMatchFinder = adminLevelSettingsMatchFinderRef;
        adminLevelSettingsLayout = adminLevelSettingsLayoutRef;
        adminLevelSettingsSideDots = adminLevelSettingsSideDotsRef;
        adminLevelSettingsGoalConfig = adminLevelSettingsGoalConfigRef;
        adminLevelSettingsLookDev = adminLevelSettingsLookDevRef;
        soundStrings = soundStringsRef;
        worldMap = worldMapRef;
        worldMapSaveManagement = worldMapSaveManagementRef;
        boardPreviewDragNDrop = boardPreviewDragNDropRef;
        targetSettings = targetSettingsRef;
        adminlevelSettingsConfig = adminlevelSettingsConfigRef;
    }
    
    //----------------   Null  -------------------//
    void NullEverything()
    {
        if (AllSingleHandedReferences != null)
        {
            for (int i = 0; i < AllSingleHandedReferences.Count; i++) AllSingleHandedReferences[i] = null;
            AllSingleHandedReferences = null;
        }
    }
    //----------------   Subscribe to NullList -------------------//
    public static void AddToReferenceNullList(params Object[] objectListToNull)
    {
        foreach(Object obj in objectListToNull) AllSingleHandedReferences.Add(obj);
    } 
    public static void AddToReferenceNullList(List<Object> PreparedObjectNullList)
    {
        for (int i = 0; i < PreparedObjectNullList.Count; i++) PreparedObjectNullList[i] = null;
    }

    //----------------   Premade Null List  -------------------//
    void AddToReferenceNullListOnlyForRefLibrary(params Object[] objectListToNull)
    {
        if (allReferenceFromRefLibraryToNull == null) return;
        allReferenceFromRefLibraryToNull.Clear();
        foreach(Object obj in objectListToNull)
            allReferenceFromRefLibraryToNull.Add(obj);
        objectListToNull = null;
    }
    [NaughtyAttributes.Button()] public void FillNullHashsetRefLibrary() => 
        AddToReferenceNullListOnlyForRefLibrary( cameraRef, gameManagerRef, settingsRef, boardRef, patternSettingsRef,
            dotPrefabsRef, findMatchesRef, HintManagerRef, ScoreManagerRef, effectsRef, goalManagerRef, endGameManagerRef,
            fadePanelControllerRef, splashMenuRef, adminLevelSettingsPaneRef, saveFileManagerInMenuRef, worldRef, saveFileLevelConfigManagementRef,
            saveFilePlayerProgressManagementRef, rowDecreaserRef, destroyOnBoardRef, boardUtilRef,debugBoardRef, settingsPanelManagerRef, 
            switchButtonsBombsRef, saveClipBoardRef, uiSoundsRef, archievmentButtonsRef, localiserManagerRef, adminLevelLuckSettingsRef, adminLevelSettingsBoardRef,
            adminLevelTilesRef, boardPreviewRef, fadeControllerSplashMenuRef, adminLevelSettingsMatchFinderRef, adminLevelSettingsLayoutRef,
            adminLevelSettingsSideDotsRef, adminLevelSettingsGoalConfigRef, adminLevelSettingsLookDevRef, soundStringsRef, worldMapRef, worldMapSaveManagementRef,
            boardPreviewDragNDropRef, targetSettingsRef,adminlevelSettingsConfigRef);
    
    //----------------   Editor Assign  -------------------//
    [NaughtyAttributes.Button()] public void PopulateList()
    {
        cameraRef = Camera.main;
        gameManagerRef = FindObjectOfType<GameManager>();
        boardRef = FindObjectOfType<Board>();
        dotPrefabsRef = FindObjectOfType<DotPrefabs>();
        findMatchesRef = FindObjectOfType<FindMatches>();
        HintManagerRef = FindObjectOfType<HintManager>();
        ScoreManagerRef = FindObjectOfType<ScoreManager>();
        effectsRef = FindObjectOfType<Effects>();
        goalManagerRef = FindObjectOfType<GoalManager>();
        endGameManagerRef = FindObjectOfType<EndGameManager>();
        fadePanelControllerRef = FindObjectOfType<FadeController>();
        splashMenuRef = FindObjectOfType<SplashMenu>();
        adminLevelSettingsPaneRef = FindObjectOfType<AdminLevelSettingsPanel>();
        saveFileManagerInMenuRef = FindObjectOfType<SaveFileManagerInMenu>();
        rowDecreaserRef = FindObjectOfType<RowDecreaser>();
        destroyOnBoardRef = FindObjectOfType<DestroyOnBoard>();
        boardUtilRef = FindObjectOfType<BoardUtil>();
        debugBoardRef = FindObjectOfType<DebugBoard>();
        settingsPanelManagerRef = FindObjectOfType<SettingsPanelManager>();
        switchButtonsBombsRef = FindObjectOfType<SwitchButtonsBombs>();
        uiSoundsRef = FindObjectOfType<UISounds>();
        archievmentButtonsRef = FindObjectOfType<ArchievmentButtons>();
        localiserManagerRef = FindObjectOfType<LocaliserManager>();
        adminLevelLuckSettingsRef = FindObjectOfType<AdminLevelLuckSettings>();
        adminLevelSettingsBoardRef = FindObjectOfType<AdminLevelSettingsBoard>();
        adminLevelTilesRef = FindObjectOfType<AdminLevelSettingsTiles>();
        boardPreviewRef = FindObjectOfType<BoardPreview>();
        fadeControllerSplashMenuRef = FindObjectOfType<FadeControllerSplashMenu>();
        adminLevelSettingsMatchFinderRef = FindObjectOfType<AdminLevelSettingsMatchFinder>();
        adminLevelSettingsLayoutRef = FindObjectOfType<AdminLevelSettingsLayout>();
        adminLevelSettingsSideDotsRef = FindObjectOfType<AdminLevelSettingsSideDots>();
        adminLevelSettingsGoalConfigRef = FindObjectOfType<AdminLevelSettingsGoalConfig>();
        adminLevelSettingsLookDevRef = FindObjectOfType<AdminLevelSettingsLookDev>();
        soundStringsRef = FindObjectOfType<SoundStrings>();
        worldMapRef = FindObjectOfType<WorldMap>();
        boardPreviewDragNDrop = FindObjectOfType<BoardPreviewDragNDrop>();
        targetSettingsRef = FindObjectOfType<TargetSettings>();
        adminlevelSettingsConfigRef = FindObjectOfType<AdminLevelSettingsConfig>();
    }
}
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using DG.Tweening;
using FruitMatch.Scripts.Core;

public static class ConstValues
{
    public const int MAXVALUETIME = 1800;
    public const int MAXVALUEMOVES = 200;

    public const int MAXVALUEPOINTS = 75000;
}
public sealed class Board : MonoBehaviour
{

    public Ease swapbackEase = Ease.OutBack;
    [Range(0,0.85f)] public float swapSmallerNumber = 0.6f;
    [Range(0,2f)] public float swapBiggerNumber = 1.7f;
   [Range(0,.075f)] public float dotSnapValue = .075f;

   private Queue<SideDot> QueuedUpSideDot = new Queue<SideDot>(1);

   public SideDot QueuedUpSideDotProperty
   {
       get { return QueuedUpSideDot.Dequeue(); }
       set
       {
           QueuedUpSideDot.Enqueue(value);
           StopCoroutine(SideDotQueueTimerCo());
           StartCoroutine(SideDotQueueTimerCo());
       }
   }

   [Header ("Scriptable Object Stuff")]
    [SerializeField] public World world;

    public int level;
    public GameState currentState = GameState.move;
    
    [Header ("Board Dimensions")]
    public int width;
    public int height;
    public int offSet;

    [Header("Prefabs")] 
    public GameObject focus;
    public GameObject tilePrefab;
    public Sprite tileBackgroundDark;
    public Sprite tileBackgroundBright;
    public GameObject[] sideDotIcons;
    public GameObject breakableTilePrefab;
    public GameObject[] dots;
    public GameObject destroyParticle;
	public TileType[] boardLayout;
    private bool[,] _blankSpaces;
    public GameObject[,] AllDots;
    private BackgroundTile[,] _allTiles;
    
    public GameObject lockTilePrefab;
    public GameObject concreteTilePrefab;
    [FormerlySerializedAs("slimePiecePrefab")] public GameObject bubblePiecePrefab;
    
    public BackgroundTile[,] lockTiles;
    public BackgroundTile[,] concreteTiles;
    public BackgroundTile[,] bubbleTiles;
    [SerializeField] private GameObject borderPrefab;
    [SerializeField] private GameObject cornerBorderPrefab;
    
    
    [Header("Match Stuff")]
    public MatchType matchtype;
    public Dot currentDot;
    private FindMatches _findMatches;
    private Dictionary<int, int> iterationsDebug = new Dictionary<int, int>();
    private BackgroundTile[,] breakableTiles;
    private int _streakValue = 1;
    public double[] scoreGoals;
   
  
    private bool makeBubble = true;

    public Dot currentTouchedDot;
    [SerializeField] private List<BackGroundTileSideList> allSideBackGroundTiles = new List<BackGroundTileSideList>(5);
    [SerializeField] private float topOffset =1.3f, bottomOffset = 1.2f , leftOffset = 1.55f, rightOffset = 1.55f; 

    [SerializeField] private Directions directions; 
    

    private bool _allowGraphicBorder;
    private RowDecreaser _rowDecreaser;
    private DestroyOnBoard _destroyOnBoard;
    private BoardUtil _boardUtil;
    private DebugBoard _debugBoard;
    //-------------- Methods other Scripts can Call-------//
    public bool SwitchAndCheck(int column, int row, Vector2 direction) =>
        _boardUtil.SwitchAndCheck(column, row, width, height, ref AllDots, _blankSpaces, direction);
    public void DestroyMatches() => _destroyOnBoard.DestroyMatches(width, height,_streakValue, ref AllDots,
        ref breakableTiles, ref lockTiles, ref  concreteTiles, ref  bubbleTiles, ref _blankSpaces,
        destroyParticle, world, ref  makeBubble, Rl.soundStrings.MatchFound, Rl.soundStrings.DamageBubbleSound,
        _findMatches, ref  currentDot, ref  matchtype, _rowDecreaser);

   // private void RepeatDestroying()
   // {
       // Rl.findMatches.FindAllMatches();
     //   Rl.board.DestroyMatches();
   // }
   public void ReevaluateBoard()
   {
       StopCoroutine(ReEvaluateBoardCo());
       StartCoroutine(ReEvaluateBoardCo());
   }
   public IEnumerator ReEvaluateBoardCo()
    {
        HashSet<Dot> allDotObjects = new HashSet<Dot>();
       // List<SideDot> allSideDots = new List<SideDot>();
        
     //   foreach (SideDot dot in FindObjectsOfType<SideDot>())
      //  {
      //      allSideDots.Add(dot);
    //    }

        // for (int i = 0; i < allSideDots.Count; i++)
        // {
        //     if ((allSideDots[i].columnSideDot < width && allSideDots[i].columnSideDot > 0)
        //         || allSideDots[i].rowSideDot < height && allSideDots[i].rowSideDot > 0)
        //     {
        //         Destroy(allSideDots[i]);
        //         allSideDots.Remove(allSideDots[i]);
        //     }
        // }
        //
        /*
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                bool found = false;
                for (int o = 0; o < allSideDots.Count; o++)
                {
                    if (allSideDots[o].)

                {
                        Destroy(allSideDots[o]);
                        allSideDots.Remove(allSideDots[o]);
                    }
                }
            }
        }*/
        
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                AllDots[i,j] = null;
            }
        }

        
        foreach (Dot dot in FindObjectsOfType<Dot>())
        {
            allDotObjects.Add(dot);
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                AllDots[i,j] = null;
            }
        }

        foreach (Dot dot in allDotObjects)
        {
            AllDots[dot.column, dot.row] = dot.gameObject;
            dot.gameObject.transform.position = new Vector3(dot.column, dot.row, dot.gameObject.transform.position.z);
        }
        currentState = GameState.move;
        StartCoroutine(FillBoardCo());
        Rl.findMatches.FindAllMatches();
        yield return new WaitForSeconds(0.15f);
        Rl.board.DestroyMatches();
    }
   public void StartDestroyingCo() => StartCoroutine(InvokeDestroyingCo());
   public void SetFocusPos(Vector3 pos)
   {
       _focusSprtRndr.color = new Color(_focusSprtRndr.color.r, _focusSprtRndr.color.g, _focusSprtRndr.color.b,
           0.85f);
       focus.transform.position = pos;
   }

   
   IEnumerator FocusFadeCo()
   {
       if (_focusSprtRndr.color.a < 0.05f)
       {
           focus.transform.position = new Vector3(99999, 99999, 99999);
           yield break;
       }
       
       _focusSprtRndr.color = new Color(_focusSprtRndr.color.r, _focusSprtRndr.color.g, _focusSprtRndr.color.b,
           _focusSprtRndr.color.a * 0.6f);
       yield return new WaitForSeconds(0.05f);
       StartCoroutine(FocusFadeCo());
   }

   public void ResetFocusPos()
   {
       StartCoroutine(FocusFadeCo());
   }

   public IEnumerator InvokeDestroyingCo()
    {
        Rl.findMatches.FindAllMatches();
        yield return new WaitForSeconds(0.15f);
        Rl.board.DestroyMatches();
    }
    //-------------- Unity Methods------//
    private SpriteRenderer _focusSprtRndr;
    private void Awake()
    {
        LoadLevelData();
        _focusSprtRndr = focus.GetComponent<SpriteRenderer>();
    }

    [SerializeField] public string SwapdotAudio2 = "swapDotAudio2";
    void Start ()
    {
        //InvokeRepeating(nameof(RepeatDestroying), 2f, 0.2f);
        if (allSideBackGroundTiles.Count < 4) allSideBackGroundTiles = new List<BackGroundTileSideList>(4);
        LoseCoupling();
        SetUpArrays();
        SetUp();
        MarkConcrete(true);
        currentState = GameState.pause;
        _debugBoard.InvokeCheckBrokenElements();
    }
    public void MarkConcrete(bool markBlank) => Concrete.MarkConcreteField(ref concreteTiles, true, ref _blankSpaces, ref AllDots);
    private void Update()
    {
        //StartCoroutine(DestroyMatchesUpdateCo());
        if (lockTiles.Length == 0 && currentState != GameState.wait) return;

       //LockTiles.CorrectBinding(width, height, lockTiles, allDots);
        LockTiles.UpdateLockTiles(width, height, ref lockTiles);
        LockTiles.MoveLockTiles(lockTiles);
    }

    private bool finishedFalling = false;
    IEnumerator DestroyMatchesUpdateCo()
    {
    
        yield return new WaitForSeconds(0.5f);
        if (!finishedFalling)
        {
            StartCoroutine(DestroyMatchesUpdateCo());
            yield break;
        }
        foreach(var t in FindObjectsOfType<Dot>())
                if(t.isMatched) DestroyMatches();
    }
    //-------------- Active Deactive bar------//
   [Button] public void ActivateBar()
   {
       switch (directions)
       {
           case Directions.top: TopActive = true; break;
           case Directions.bottom: BottomActive = true; break;
           case Directions.left: LeftActive = true; break;
           case Directions.right: RightActive = true; break;
       }
   }
   [Button] public void DeactivateBar()
   {
       switch (directions)
       {
           case Directions.top: TopActive = false; break;
           case Directions.bottom: BottomActive = false; break;
           case Directions.left: LeftActive = false; break;
           case Directions.right: RightActive = false; break;
       }
   }
   public bool TopActive
    {
        get => topActive;
        set
        {
            if (value && allSideBackGroundTiles[(int)Directions.top].gameObjectList.Count == 0) 
                BoardInstantiate.InstantiateTopSideDots(width, height, topOffset, tilePrefab, ref allSideBackGroundTiles, tileBackgroundBright,  tileBackgroundDark, ref  AllDots, ref _allTiles, transform);
            else if(!value && allSideBackGroundTiles[(int)Directions.top].gameObjectList.Count > 0) DestroyOnBoard.DestroyLine(width, height, ref allSideBackGroundTiles,Directions.top);
            topActive = value;
        }
    } 
    public bool BottomActive
    {
        get => bottomActive;
        set
        {
            if (value && allSideBackGroundTiles[(int)Directions.bottom].gameObjectList.Count == 0) 
                BoardInstantiate.InstantiateBottomSideDots(width, bottomOffset, tilePrefab,ref _allTiles,  ref allSideBackGroundTiles, tileBackgroundBright, tileBackgroundDark, transform);
            else if(!value && allSideBackGroundTiles[(int)Directions.bottom].gameObjectList.Count > 0) DestroyOnBoard.DestroyLine(width, height, ref allSideBackGroundTiles,Directions.bottom);
            bottomActive = value;
        }
    }
    [SerializeField] private bool  topActive = false, bottomActive = true, leftActive = true, rightActive = true; 
    public bool LeftActive
    {
        get => leftActive;
        set
        {
            if (value && allSideBackGroundTiles[(int)Directions.left].gameObjectList.Count == 0) 
                BoardInstantiate.InstantiateLeftSideDots(height, leftOffset, tilePrefab, ref _allTiles, ref allSideBackGroundTiles, tileBackgroundBright, tileBackgroundDark, transform);
            else if(!value && allSideBackGroundTiles[(int)Directions.left].gameObjectList.Count > 0) DestroyOnBoard.DestroyLine(width, height, ref allSideBackGroundTiles,Directions.left);
            leftActive = value;
        }
    }
    public bool RightActive
    {
        get => rightActive;
        set
        {
            if (value && allSideBackGroundTiles[(int)Directions.right].gameObjectList.Count == 0) 
                BoardInstantiate.InstantiateRightSideDots(width, height, rightOffset, tilePrefab,ref _allTiles,  ref allSideBackGroundTiles, tileBackgroundBright, tileBackgroundDark, this.transform);
            else if(!value && allSideBackGroundTiles[(int)Directions.right].gameObjectList.Count > 0) DestroyOnBoard.DestroyLine(width, height, ref allSideBackGroundTiles,Directions.right);
            rightActive = value;
        }
    }

    [SerializeField] [Range(0, 1.5f)] private float QueueUpSideDotActionResetTimer = 0.7f;
    private IEnumerator SideDotQueueTimerCo()
    {
        yield return new WaitForSeconds(QueueUpSideDotActionResetTimer);
        if(QueuedUpSideDot.Count > 0) QueuedUpSideDot.Clear();
    }
    private void PlayQueue()
    {
        if (QueuedUpSideDot.Count < 1) return;
        QueuedUpSideDotProperty.OnMouseDown();
    }

    public IEnumerator FallingDownCo()
    {
        if (!Rl.boardUtil.RefillBoard(width, height, offSet, ref AllDots, dots,
                _blankSpaces, concreteTiles, bubbleTiles))
        yield break;
        
        yield return new WaitForSeconds(0.3f);
            StartCoroutine(FallingDownCo());
    }
//-------------- Fill and Shuffle -----//
    public IEnumerator FillBoardCo()
    {
       
        StartCoroutine(FallingDownCo());
        
        if (!MatchHelper.MatchesOnBoard(width, height,_findMatches,AllDots))
        {   currentDot = null;
            Bubble.CheckToMakeBubble(width,height,ref AllDots, ref bubbleTiles, ref lockTiles, ref concreteTiles,bubblePiecePrefab,  makeBubble, Rl.soundStrings.MakeBubbleSound);
            makeBubble = true;
            currentState = GameState.move;
            Debug.Log("No Matches found, breaking here");
            PlayQueue();
            finishedFalling = true;
           yield break;
        }
    
        yield return new WaitForSeconds(Rl.settings.GetRefillDelay);
        while(MatchHelper.MatchesOnBoard(width, height,_findMatches,AllDots))
        {
            _streakValue ++;
            DestroyMatches();
            yield break;
        }
        if(DeadLock.IsDeadlocked(width, height, ref AllDots, _blankSpaces, _boardUtil))
        {
            StartCoroutine(ShuffleBoard());
            Rl.GameManager.PlayAudio(Rl.soundStrings.DeadLockAudio, Random.Range(0,5), Rl.settings.GetSFXVolume, Rl.effects.audioSource);
        }
       // yield return new WaitForSeconds(Rl.settings.GetRefillDelay);
        Debug.Log("Done Refilling");
        //System.GC.Collect();
        currentState = GameState.move;
        currentDot = null;
        Bubble.CheckToMakeBubble(width,height,ref AllDots, ref bubbleTiles, ref lockTiles, ref concreteTiles,bubblePiecePrefab,  makeBubble, Rl.soundStrings.MakeBubbleSound);
        makeBubble = true;
        _streakValue = 1;
    }
    
    private IEnumerator ShuffleBoard()
{
    yield return new WaitForSeconds(Rl.settings.GetShuffleDelay);
    Rl.GameManager.PlayAudio(Rl.soundStrings.DeadLockAudio, Random.Range(0, 5), Rl.settings.GetSFXVolume, Rl.effects.audioSource);
    
    List<GameObject> newBoard = new List<GameObject>();
    for (int i = 0; i < width; i++)    //Add every piece to this list
    {
        for (int j = 0; j < height; j++)
        {
            if (AllDots[i, j] != null)
                newBoard.Add(AllDots[i, j]);
        }
    }
    
    yield return new WaitForSeconds(Rl.settings.GetShuffleDelay);
   
    for (int i = 0; i < width; i++)  //for every spot on the board. . . 
    {
        for (int j = 0; j < height; j++)
        {
            if (!_blankSpaces[i, j] && !concreteTiles[i, j] && !bubbleTiles[i, j])  //if this spot shouldn't be blank
            {
                int pieceToUse = Random.Range(0, newBoard.Count); //Pick a random number
                
                int maxIterations = 0;
                while (MatchHelper.MatchesAt(i, j, AllDots, newBoard[pieceToUse]) && maxIterations < 100)
                {
                    pieceToUse = Random.Range(0, newBoard.Count);
                    maxIterations++;
                }
                
                Dot piece = newBoard[pieceToUse].GetComponent<Dot>();    //Make a container for the piece
                piece.column = i;   //Assign the row to the piece
                piece.row = j;    //Fill in the dots array with this new piece
                AllDots[i, j] = newBoard[pieceToUse];
              
                newBoard.Remove(newBoard[pieceToUse]);   //Remove it from the list
            }
        }
    }
    if (DeadLock.IsDeadlocked(width, height, ref AllDots, _blankSpaces, _boardUtil)) StartCoroutine(ShuffleBoard());
}

//-------------- Setup Start -------//

    private void LoadLevelData()
    {
        if (world == null) return;
        
            for (int i = 0; i < world.levels.Length; i++)
            {
                if (world.levels[i] == world.LevelToLoad)
                    level = i;   // idk ???
            }

            GetLevelDimensionFromSaveData(ref world.LevelToLoad, level);
            GetGraphicsFromSaveData(level);
            Debug.Log("THIS IS LEVEL NUMBER: " + level);
            world.levels[level].dots = GetDots(Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level].BoardDimensionsConfig);
            
            if (level < world.levels.Length && world.levels[level] != null)
            {
                    width = world.levels[level].width;
                    height = world.levels[level].height;
                    dots = world.levels[level].dots;
                    scoreGoals = world.levels[level].scoreGoals;
                    boardLayout = world.levels[level].boardLayout;
            }

          
            GetSideDotSettings(Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
                .SideFruitsFieldConfig);
            
    }

    public GameObject[] GetDots(BoardDimensionsConfig boardDimensionsConfig)
    {
        List<FruitType> enabledFruitTypes = new List<FruitType>();
        List<GameObject> allEnabledDots = new List<GameObject>();
        for (int i = 0; i < boardDimensionsConfig.FruitsConfigParent.Length; i++)
        {
            if(boardDimensionsConfig.FruitsConfigParent[i].FruitEnabled)
                enabledFruitTypes.Add(boardDimensionsConfig.FruitsConfigParent[i].FruitType);
        }

        for (int i = 0; i < enabledFruitTypes.Count; i++)
        {
            allEnabledDots.Add(world.GetFruitPrefab(enabledFruitTypes[i], Colors.AlleFarben)); ;  // <<<<<----CHANGE THIS TO THE RIGHT COLOR ONCE IMPLEMENTED
        }
        Debug.Log("DeboardDimensionsConfig.FruitsConfigParent.Length : ::::  " + boardDimensionsConfig.FruitsConfigParent.Length);

        Debug.Log("enabledFruitTypes : ::::  " + enabledFruitTypes );
        Debug.Log("allEnabledDots : ::::  " + allEnabledDots );
        return allEnabledDots.ToArray();
    }
    private void GetSideDotSettings(SideFruitsFieldConfig  sideFruitsConfig)
    {
    
        leftActive = sideFruitsConfig.SideFruitsConfig[FieldState.CurrentField].LeftActive;
        rightActive = sideFruitsConfig.SideFruitsConfig[FieldState.CurrentField].RightActive;
        bottomActive = sideFruitsConfig.SideFruitsConfig[FieldState.CurrentField].BottomActive;
        topActive = sideFruitsConfig.SideFruitsConfig[FieldState.CurrentField].TopActive;
    }
    private void GetGraphicsFromSaveData(int levelNumber)
    {
        _allowGraphicBorder = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[levelNumber]
            .GraphicConfig.AllowBorderGraphic;

    }
    private void GetLevelDimensionFromSaveData(ref Level level, int levelNumber)
    {
        level.height = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[levelNumber].BoardDimensionsConfig.Height;
        level.width = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[levelNumber].BoardDimensionsConfig.Width;
        
    }
    private void LoseCoupling()
{
    _findMatches = Rl.findMatches;
    _rowDecreaser = Rl.rowDecreaser;
    _destroyOnBoard = Rl.destroyOnBoard;
    _boardUtil = Rl.boardUtil;
    _debugBoard = Rl.debugBoard;
}
private void SetUpArrays()
{
    breakableTiles = new BackgroundTile[width, height];
    lockTiles = new BackgroundTile[width, height];
    concreteTiles = new BackgroundTile[width, height];
    bubbleTiles = new BackgroundTile[width, height];
    _blankSpaces = new bool[width, height];
    AllDots = new GameObject[width, height];
}

private void GenerateTiles(TileType[] boardLayout)
{
    _allTiles = new BackgroundTile[width, height];
    BlankSpaces.GenerateBlankSpaces(boardLayout, ref _blankSpaces);
    Breakabletiles.GenerateBreakableTiles(boardLayout, ref breakableTiles, breakableTilePrefab);
    LockTiles.GenerateLockTiles(boardLayout, ref lockTiles, lockTilePrefab);
    Concrete.GenerateConcreteTiles(boardLayout, ref concreteTiles, concreteTilePrefab);
   Bubble.GenerateBubbleTiles(boardLayout, ref bubbleTiles, bubblePiecePrefab);

}

[Range(-0.5f, 0.5f)] [SerializeField]private float paddingBorder = 0.06f;
private void SetUp()
{
    GenerateTiles(boardLayout);
  //  List<Vector3> posBlankSpaces = BoardInstantiate.InstaniteEverythingAtStart(width, height, offSet,leftOffset, rightOffset, bottomOffset, topOffset, tilePrefab, _blankSpaces, ref _allTiles, ref allSideBackGroundTiles,
  //   tileBackgroundBright, tileBackgroundDark, ref dots, ref AllDots, ref _allTiles, transform, bottomActive, leftActive, rightActive, topActive);
   // Rl.Cam.GetComponent<CameraScalar>().MinMaxScalar(posBlankSpaces, Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[level]
     //   .LayoutConfig);
    LockTiles.BindLockTilesToDot(width, height, lockTiles, AllDots);
   if(_allowGraphicBorder)Border.GenerateAllBorders(width, height, paddingBorder, _allTiles, _blankSpaces, borderPrefab, cornerBorderPrefab);
    Debug.Log(_allTiles.Length);

}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FruitMatch.Scriptable.Rewards;
using FruitMatch.Scripts.Blocks;
using FruitMatch.Scripts.Effects;
using FruitMatch.Scripts.GUI;
using FruitMatch.Scripts.GUI.Boost;
using FruitMatch.Scripts.Items;
using FruitMatch.Scripts.Level;
using FruitMatch.Scripts.MapScripts;
using FruitMatch.Scripts.System;
using FruitMatch.Scripts.System.Combiner;
using FruitMatch.Scripts.System.Orientation;
using FruitMatch.Scripts.TargetScripts.TargetSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace FruitMatch.Scripts.Core
{
//game state enum
    public enum GameState
    {
        Map,
        PrepareGame,
        RegenLevel,
        Tutorial,
        Pause,
        Playing,
        PreFailed,
        GameOver,
        ChangeSubLevel,
        PreWinAnimations,
        Win,
        WaitForPopup,
        WaitAfterClose,
        BlockedGame,
        BombFailed,
        wait, move, win, lose, pause, securitymove
    }

    /// <summary>
    /// core-game class, using for handle game states, blocking, sync animations and search mathing and map
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        //public movePlayedtoSubstract;
           [NaughtyAttributes.Button()]
           public void SequencePreview()
           {
               SequencePreviewGUILoadTrigger?.Invoke();
           }
           [NaughtyAttributes.Button()] public void SerializeDictionary()
           {
               Dictionary<Item, Combine> n = CombineManager.GetDic();
             

           
               allCombines?.Clear();
               allItems?.Clear();

               allCombines = new List<Combine>();
               allItems = new List<Item>();
               
               
               foreach (var t in n)
               {
                   if(t.Value.items.Count > 1 )allCombines.Add(t.Value);
               }
           }
        
           public delegate void SequencePreviewGUILoadEvent();
           public static event SequencePreviewGUILoadEvent SequencePreviewGUILoadTrigger;

           
         
           public int LimitLength;
           public int LimitHelper = 0;
        public static LevelManager THIS;
        public float MaxLimit;
        public HashSet<int> DestroyColorIDs = new HashSet<int>();
        public HashSet<int> AvoidColorIDs = new HashSet<int>();
        public int ColorLimit;
        public int PlayCounter = 0;
        public int Vcount = 3;
        public int Hcount = 3;
        public int PenaltyValue = 1;
        public bool BlockCombineAllowed = true;
        public bool[] DestroyOnlyTarget = new bool[SaveFileLevelConfigs.Fields];
        public float squareWidth = 1.2f; //square width for border placement
        public Item lastDraggedItem;  //item which was dragged recently 
        public Item lastSwitchedItem;  //item which was switched succesfully recently 
        public GameObject popupScore; //makes scores visible in the game
        public int currentLevel = 1;  //current game level
        private int currentSubLevel = 1; //current sub-level
        public bool[] NoMatchesBool;
        public bool AllowBorder;
        
        public bool IsSequenceMatching = false;
        public List<int> MatchSequence = new List<int>(){};
        public int CurrentSubLevel
        {
            get { return currentSubLevel; }
            set
            {
                currentSubLevel = value;
                levelData.currentSublevelIndex = currentSubLevel - 1;
            }
        }

        public List<CollectionStyle> TargetCollectionStyle = new List<CollectionStyle>();
        
        
        //current field reference
        public FieldBoard field => fieldBoards[CurrentSubLevel - 1]; //EDITOR: cost of continue after failing 
        public int FailedCost; //EDITOR: moves gived to continue
        public int ExtraFailedMoves = 5; //EDITOR: time gived to continue
        public int ExtraFailedSecs = 30;  //true if thriving block destroyed on current move
        public bool thrivingBlockDestroyed;  //variables for boost which place bonus candies on the field
        public int BoostColorfullBomb;
        public int BoostPackage;
        public int BoostStriped;
        public int BoostMarmalade;  //put some marmalade bears to the field on start
        public bool enableMarmalade;
        public bool enableHorBombs;
        public bool enableVertBombs;
        public bool enableSameColorBomb;
        public bool enablePackageBomb;

        public BoostIcon emptyBoostIcon;  //empty boost reference for system
        public DebugSettings DebugSettings; //debug settings reference
        public BoostIcon activatedBoost;  //activate boost in game
        public BoostIcon ActivatedBoost
        {
            get { return activatedBoost == null ? emptyBoostIcon : activatedBoost; }
            set
            {
                // booost are not yet fully implement but I have done a bit if they ask me if I can make "cheat items"
                if (value == null)
                {
                    if (activatedBoost != null && gameStatus == GameState.Playing)
                    {
                        InitScript.Instance.SpendBoost(activatedBoost.type);
                        UnLockBoosts();
                    }
                }

                //        if (activatedBoost != null) return;
                activatedBoost = value;

                if (value != null)
                {
                    LockBoosts();
                }

                if (activatedBoost == null) return;
                if (activatedBoost.type != BoostType.ExtraMoves && activatedBoost.type != BoostType.ExtraTime) return;
                if (THIS.levelData.limitType == LIMIT.MOVES)
                    THIS.levelData.Limit += 5;
                else
                    THIS.levelData.Limit += 30;

                ActivatedBoost = null;
            }
        }
     
        public static int Score; //score gain on this game
        public int stars; //stars gain on this game
        public GameObject stripesEffect; //striped effect reference
        public bool showPopupScores; //show popup score on field
        public Color[] scoresColors; //popup score color
        public Color[] scoresColorsOutline; //popup score outline
        public GameObject[] gratzWords;  //congratulation words popup reference
        public GameObject Level;   //Level gameobject reference
        public GameObject FieldsParent; //Gameobject reference
        public GameObject NoMoreMatches;  //Gameobject reference
        public GameObject CompleteWord;  //Gameobject reference
        public GameObject FailedWord;  //Gameobject reference
        public BoostIcon[] InGameBoosts;  //in game boost reference
        //levels passed for the current session
        //reference to orientation handler
        public OrientationGameCameraHandle orientationGameCameraHandle;
        [HideInInspector]public List<AnimateItems> animateItems = new List<AnimateItems>();
        //blocking to drag items for a time
        private bool dragBlocked;
        public bool DragBlocked
        {
            get { return dragBlocked; }
            set
            {
                dragBlocked = value;
                var moveID = THIS.moveID;
                StartCoroutine( ResetBlock(moveID));
            }
        }

        private IEnumerator ResetBlock(int moveID)
        {
            yield return new WaitForSeconds(3);
            if (moveID == THIS.moveID && dragBlocked)
                dragBlocked = false;
        }

    
        public int moveID; //current move ID
        public bool onlyFalling; //value for regeneration, items with falling or not
        public bool levelLoaded;  //level loaded, wait until true for some courotines
        public CombineManager combineManager;    //combine manager listener
        public bool findMatchesStarted; //true if search of matches has started
        private bool checkMatchesAgain; //true if need to check matches again
        public bool testByPlay;  //if true - start the level avoind the map for debug

        //game events
        #region EVENTS
        public delegate void GameStateEvents();
        public static event GameStateEvents OnMapState;
        public static event GameStateEvents OnEnterGame;
        public static event GameStateEvents OnLevelLoaded;
        public static event GameStateEvents OnWaitForTutorial;
        public static event GameStateEvents OnMenuPlay;
        public static event GameStateEvents OnSublevelChanged;
        public static event GameStateEvents OnMenuComplete;
        public static event GameStateEvents OnStartPlay;
        public static event GameStateEvents OnWin;
        public static event GameStateEvents OnLose;
        public static event GameStateEvents OnTurnEnd;
        public static event GameStateEvents OnCombo;
        public List<KeyValuePair<Item, Combine>> test;
        public List<Item> allItems;
        public List<Combine> allCombines;

        IEnumerator DelayGenerateLevelCo(float newSec)
        {
            yield return new WaitForSeconds(newSec);
            GenerateLevel();
        }
        //current game state
        [SerializeField] private GameState GameStatus;
        public GameState gameStatus
        {
            get { return GameStatus; }
            set
            {
                GameStatus = value;
                switch (value)
                {
                    case GameState.PrepareGame://preparing and initializing  the game
                        StartCoroutine(AI.THIS.CheckPossibleCombines());
                        //CrosssceneData.passLevelCounter++;
                        PrepareGame();
                        // var firstItemPrefab = THIS.levelData.target.prefabs.FirstOrDefault();
                        // if (firstItemPrefab && firstItemPrefab.GetComponent<Item>() && !firstItemPrefab.GetComponent<ItemSimple>())
                        //     collectIngredients = true;
                        GenerateLevel();
                      //  StartCoroutine(DelayGenerateLevelCo(1f));
                        levelLoaded = true;
                        OnLevelLoaded?.Invoke();
                      
                        break;
                    case GameState.WaitForPopup://waiting for pre game banners
                        StopCoroutine(IdleItemsDirection());
                        StartCoroutine(IdleItemsDirection());
                        OrientationGameCameraHandle.CameraParameters cameraParameters = orientationGameCameraHandle.GetCameraParameters();
                        //1.11f;
                        cameraParameters.size += (LoadingHelper.THIS.width + LoadingHelper.THIS.height) / 10 + LoadingHelper.THIS.width*0.07f;
                        Vector2 cameraCenter = orientationGameCameraHandle.GetCenterOffset();
                        StartCoroutine(AnimateField(field.GetPosition() + cameraCenter, cameraParameters.size));

                        break;
                    case GameState.Tutorial://tutorial state
                        OnWaitForTutorial?.Invoke();
                        break;
                    case GameState.PreFailed://chance to continue the game, shows menu PreFailed
                        LeanTween.delayedCall(1, ()=>FailedWord.SetActive(true));
                        LeanTween.delayedCall(3, () =>
                        {
                            var preFailedGameObject = MenuReference.THIS.PreFailed.gameObject;
                            preFailedGameObject.GetComponent<PreFailed>().SetFailed();
                            preFailedGameObject.SetActive(true);
                        });
                        break;
                    case GameState.BombFailed:
                        LeanTween.delayedCall(0.3f, () =>
                        {
                            var preFailedGameObject = MenuReference.THIS.PreFailed.gameObject;
                            preFailedGameObject.GetComponent<PreFailed>().SetBombFailed();
                            preFailedGameObject.SetActive(true);
                        });
                        break;
                    case GameState.Map://map state
                        //open map or test level
                        if (PlayerPrefs.GetInt("OpenLevelTest") <= 0 || FindObjectOfType<RestartLevel>())
                        {
                            EnableMap(true);
                            OnMapState?.Invoke();
                        }
                        else
                        {
                            THIS.gameStatus = GameState.PrepareGame;
                            if (!testByPlay)
                                PlayerPrefs.SetInt("OpenLevelTest", 0);
                            PlayerPrefs.Save();
                        }
                        break;
                    case GameState.Playing://playing state
                        StartCoroutine(AI.THIS.CheckPossibleCombines());
                        break;
                    case GameState.GameOver://game over
                        MenuReference.THIS.MenuFailed.gameObject.SetActive(true);
                        OnLose?.Invoke();
                        break;
                    case GameState.PreWinAnimations://animations after win
                       // StartCoroutine(PreWinAnimationsCor());
                        break;
                    case GameState.ChangeSubLevel://changing sub level state
                        if (CurrentSubLevel != GetLastSubLevel())
                            ChangeSubLevel();
                        break;
                    case GameState.Win://shows MenuComplete
                        OnMenuComplete?.Invoke();
                        MenuReference.THIS.MenuComplete.gameObject.SetActive(true);
                        Rl.GameManager.PlayAudio(Rl.soundStrings.Complete[1], Random.Range(0,5),true, Rl.settings.GetSFXVolume, Rl.effects.audioSource);
                        if(winRewardAmount > 0)
                            InitScript.Instance.ShowGemsReward(winRewardAmount);// InitScript.Instance.ShowGemsReward(10);
                        if (OnWin != null) OnWin();
                        break;
                }
            }
        }
        //Combine manager reference
        public CombineManager CombineManager
        {
            get
            {
                if(combineManager == null) combineManager = new CombineManager();
                return combineManager;
            }
        }
        public bool StopFall => _stopFall.Any();        //if true - pausing all falling animations
        //returns last sub-level of this level
        private int GetLastSubLevel() => fieldBoards.Count;

        //returns current game state
        public static GameState GetGameStatus() => THIS.gameStatus;
        
        public void MenuPlayEvent() => OnMenuPlay?.Invoke(); //menu play enabled invokes event

        //Switch sub level to next 
        private void ChangeSubLevel()
        {
            CurrentSubLevel++;
            OrientationGameCameraHandle.CameraParameters cameraParameters = orientationGameCameraHandle.GetCameraParameters();
            Vector2 cameraCenter = orientationGameCameraHandle.GetCenterOffset();
            StartCoroutine(AnimateField(field.GetPosition() + cameraCenter, cameraParameters.size));
        }

        #endregion
        //Lock boosts
        private void LockBoosts()
        {
            foreach (var item in InGameBoosts)
            {
                if (item.type != ActivatedBoost.type)
                    item.LockBoost();
            }
        }
        //unlock boosts
        public void UnLockBoosts()
        {
            foreach (var item in InGameBoosts)
                item.UnLockBoost();
            
        }
        //Load the level from "OpenLevel" player pref
        public void LoadLevel()
        {
            if (currentLevel == 0)
                currentLevel = 1;
            LoadLevel(currentLevel);
            LoadingManager.SetSideDotActive
                (Rl.saveFileLevelConfigManagement
                    .AllSaveFileLevelConfigs
                    .LevelConfigs[currentLevel-1]
                    .SideFruitsFieldConfig
                    .SideFruitsConfig[0]);
            LoadingManager.LoadSideDots();
        }
        //enable map
        
        public void LoadLuckTargets() => Rl.luckCalculator.LoadCurrentTargets(DestroyColorIDs, AvoidColorIDs, ColorLimit);

        public void EnableMap(bool enable)
        {
            Rl.splashMenu.LevelCanvasObj.SetActive(true);
            GetComponent<Camera>().orthographicSize = 5.3f;
            if (enable)
            {
                GetComponent<Camera>().GetComponent<MapCamera>()
                    .SetPosition(new Vector2(0, GetComponent<Camera>().transform.position.y));
                if (FindObjectOfType<RestartLevel>() == null && DebugSettings.AI && DebugSettings.testLevel > 0)
                {
                    PlayerPrefs.SetInt("OpenLevel", DebugSettings.testLevel);
                    RestartLevel();
                }
            }
            else
            {
                GetComponent<Camera>().orthographicSize = 4;
                if(ServerTime.THIS.dateReceived)
                    InitScript.DateOfExit = ServerTime.THIS.serverTime.ToString();
                MenuReference.THIS.GetComponent<GraphicRaycaster>().enabled = false;
                MenuReference.THIS.GetComponent<GraphicRaycaster>().enabled = true;
                Level.transform.Find("Canvas").GetComponent<GraphicRaycaster>().enabled = false;
                Level.transform.Find("Canvas").GetComponent<GraphicRaycaster>().enabled = true;
            }

            Rl.Cam.GetComponent<MapCamera>().enabled = enable;
        
            Level.SetActive(!enable);

            if (!enable)
                Camera.main.transform.position = new Vector3(0, 0, -10) - (Vector3)orientationGameCameraHandle.offsetFieldPosition;
            foreach (var item in fieldBoards)
            {
                if (item != null)
                    Destroy(item.gameObject);
            }
        }
        private void Awake()
        {
            THIS = this;
            testByPlay = false;
            // testByPlay = true;//enable to instant level run
        }

        // Use this for initialization
        private void Start()
        {
            DebugSettings = Resources.Load<DebugSettings>("Scriptable/DebugSettings");
            LeanTween.init( 800 );
            LeanTween.reset();
            
//        if (!THIS.enableInApps)
//            GameObject.Find("CanvasMap/SafeArea/Gems").gameObject.SetActive(false);

            gameStatus = GameState.Map;
            winRewardAmount = Resources.Load<WinReward>("Scriptable/WinReward").winRewardAmount;

        }

        private void PrepareGame()
        {
            ActivatedBoost = null;
            Score = 0;
            stars = 0;
            moveID = 0;
            fieldBoards = new List<FieldBoard>();
            transform.position += Vector3.down * 1000;
            // targetUIObject.SetActive(false);

            EnableMap(false);

            LoadLevel();

            CurrentSubLevel = 1;
            if (ProgressBarScript.Instance != null)
                ProgressBarScript.Instance.InitBar();

            if (levelData.limitType == LIMIT.MOVES)
            {
//                InGameBoosts.Where(i => i.type == BoostType.ExtraMoves).ToList().ForEach(i => i.gameObject.SetActive(true));
            //    InGameBoosts.Where(i => i.type == BoostType.ExtraTime).ToList().ForEach(i => i.gameObject.SetActive(false));
            }
            else
            {
           //     InGameBoosts.Where(i => i.type == BoostType.ExtraMoves).ToList().ForEach(i => i.gameObject.SetActive(false));
           //     InGameBoosts.Where(i => i.type == BoostType.ExtraTime).ToList().ForEach(i => i.gameObject.SetActive(true));
            }

            OnEnterGame?.Invoke();
        }

        public List<FieldBoard> fieldBoards = new List<FieldBoard>();
        public GameObject FieldBoardPrefab;
        public LevelData levelData;
        internal bool tutorialTime;

        public static bool avoided;

        public static bool avoidedLateUpdate;
        public delegate void MoveMade();
        public static event MoveMade MoveMadeEvent;
        public void InvokeMoveMadeEvent()
        {
            MoveMadeEvent?.Invoke();

            if (!NoMatchesBool[0]) return;
            PlayCounter++;

            if (PlayCounter == 10)
            {
               // DestroyOnlyTarget[0] = true;

                findMatchesStarted = false;
                FindMatches();
                StartCoroutine(NoMatchesCor());
                PlayCounter = 0;
            }
              
        }

        //Generate loaded level
        private void GenerateLevel()
        {
            Debug.Log("GENERATING LEVEL");
            var fieldPos = new Vector3(-0.9f, 0, -10);
            var latestFieldPos = Vector3.right * ((GetLastSubLevel() - 1) * 10) + Vector3.back * 10;

            var i = 0;
            foreach (var item in fieldBoards)
            {
                var _field = item.gameObject;
                _field.transform.SetParent(FieldsParent.transform);
                _field.transform.position = fieldPos + Vector3.right * (i * 15);
                var fboard = _field.GetComponent<FieldBoard>();

                fboard.CreateField();
                latestFieldPos = fboard.GetPosition();

                i++;
            }
            levelData.TargetCounters.RemoveAll(x => x.targetLevel.setCount == SetCount.FromLevel && x.GetCount()==0);
        
            transform.position = latestFieldPos + Vector3.right * 10 + Vector3.back * 10 - (Vector3)orientationGameCameraHandle.offsetFieldPosition;
            SetPreBoosts();
        }
        
        private bool animStarted;
        /// <summary>
        /// move camera to the field
        /// </summary>
        /// <param name="destPos">position of the field</param>
        /// <param name="cameraParametersSize">camera size</param>
        /// <returns></returns>
        private IEnumerator AnimateField(Vector3 destPos, float cameraParametersSize)
        {
            var _camera = GetComponent<Camera>();
            if(animStarted) yield break;
            animStarted = true;
            var duration = 2f;
            var speed = 10f;
            var startPos = transform.position;
            var distance = Vector2.Distance(startPos, destPos);
            var time = distance / speed;
            var curveX = new AnimationCurve(new Keyframe(0, startPos.x), new Keyframe(time, destPos.x));
            var startTime = Time.time;
            float distCovered = 0;
            while (distCovered < distance)
            {
                distCovered = (Time.time - startTime) * speed;
                transform.localPosition = new Vector3(curveX.Evaluate(Time.time - startTime), transform.position.y, 0);
                _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, cameraParametersSize, Time.deltaTime*5);
                yield return new WaitForFixedUpdate();
            }

            _camera.orthographicSize = cameraParametersSize;
            transform.position = destPos;
            yield return new WaitForSeconds(0.5f);
            animStarted = false;
            GameStart();
        }

        //game start
        private void GameStart()
        {
            if (CurrentSubLevel - 1 == 0)
                MenuReference.THIS.PrePlay.gameObject.SetActive(true);
            else
            {
                OnSublevelChanged?.Invoke();
                gameStatus = GameState.Playing;
            }
        }
        /// Cloud effect animation for different direction levels
        
        private IEnumerator IdleItemsDirection()
        {
            if (field.squaresArray.Select(i => i.direction).Distinct().Count() > 1)
            {
                while (true)
                {
                    yield return new WaitForSeconds(3);
                    if (gameStatus == GameState.Playing && !findMatchesStarted)
                    {
                        var itemList = field.GetItems();
                        for (int i = 0; i < itemList.Count; i++)
                        {
                            if (itemList[i].square == null)
                                itemList.Remove(itemList[i]);

                        }
                        { var orderedEnumerableCol = itemList
                                .GroupBy(i =>
                                {

                                    if ( i.square == null || i.square.squaresGroup == null)
                                        return new List<Square>();
                                    return i.square.squaresGroup;
                                }).ToList()
                                .Select(x => new
                                {
                                    items = x,
                                    Num = x.Max(i =>
                                    {
                                        if (i.square == null)
                                            return 0;
                                        return i.square.orderInSequence;
                                    }),
                                    Count = x.Count(),
                                    x.Key
                                }).OrderByDescending(i => i.Num).ToList();

                         
                            foreach (var items in orderedEnumerableCol)
                            {
                                var animationFinished = false;
                                foreach (var item in items.items)
                                {
                                    if (item.destroying) continue;
                                    StartCoroutine(item.DirectionAnimation(() => { animationFinished = true; }));
                                }

                                yield return new WaitUntil(() => animationFinished);
                            }
                            
                        }
                      
                    }

                    yield return new WaitForSeconds(1);
                }
            }
        }


        private void SubtractiveLimitType(ref bool lose, ref bool win)
        {
            if (levelData.Limit <= 0)
            {
                levelData.Limit = 0;

                if (!levelData.IsTotalTargetReached())
                    lose = true;
                else win = true;
            }

            else
            {
                if (levelData.IsTotalTargetReached() && !levelData.WaitForMoveOut())
                {
                    win = true;
                }
                else if (levelData.IsTargetReachedSublevel() && fieldBoards.Count>1)
                    gameStatus = GameState.ChangeSubLevel;
            }
        }
        
        private void AdditiveLimitType(ref bool lose, ref bool win)
        {
            if (levelData.Limit >= LimitHelper)
            {
                levelData.Limit = 0;

                if (!levelData.IsTotalTargetReached())
                    lose = true;
                else win = true;
            }

            else
            {
                if (levelData.IsTotalTargetReached() && !levelData.WaitForMoveOut())
                {
                    win = true;
                }
                else if (levelData.IsTargetReachedSublevel() && fieldBoards.Count>1)
                    gameStatus = GameState.ChangeSubLevel;
            }
        }
        //Check win or lose conditions
        public void CheckWinLose()
        {
            var lose = false;
            var win = false;
            Debug.Log("Win Limit is : " + levelData.Limit);


            switch (levelData.limitType)
            {
                case LIMIT.MOVES:
                    SubtractiveLimitType(ref lose, ref win);
                    break;
                case LIMIT.TIME:
                    SubtractiveLimitType(ref lose, ref win);
                    break;
                case LIMIT.AVOID:
                    AdditiveLimitType(ref lose, ref win);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
         

            if (lose)
            {
                gameStatus = GameState.PreFailed;
            }

            else if (win && !lose)
                gameStatus = GameState.PreWinAnimations;
            else if (!win && !lose && FindObjectsOfType<itemTimeBomb>().Any(i => i.timer <= 0))
            {
                LevelManager.THIS.gameStatus = GameState.BombFailed;
            }

            if (DebugSettings.AI && (win || lose))
            {
                Debug.Log((win ? "win " : "lose ") + " score " + Score+ " stars " + stars + " moves/time restList " + THIS.levelData.Limit);
                RestartLevel();
            }
        }

        public GameObject winTrail;
        public Transform movesTransform;
        public int destLoopIterations;
        //Animations after win
          private IEnumerator PreWinAnimationsCor()
          { 
              tapToSkip = Instantiate((GameObject) Resources.Load("Prefabs/TapToSkip"),OrientationActivator.ActiveOrientationCanvas.transform);
            if (!InitScript.Instance.losingLifeEveryGame && InitScript.lifes < InitScript.Instance.CapOfLife)
                InitScript.Instance.AddLife(1);
            CompleteWord.SetActive(true);
            var limit = Mathf.Clamp(levelData.Limit, 0, 5);

            if(!skipWin)
            {
                var c1 = StartCoroutine(PreWinLoop(limit));
                yield return c1;
            }
            if(skipWin)
            {
                Score += limit * Random.Range(500, 3000) / levelData.colorLimit;
                CheckStars();
                skipWin = false;
                Destroy(tapToSkip);
            }
            // if (PlayerPrefs.GetInt($"Level.{currentLevel:000}.StarsCount", 0) < stars)
            //     PlayerPrefs.SetInt($"Level.{currentLevel:000}.StarsCount", stars);
            // if (Score > PlayerPrefs.GetInt("Score" + currentLevel))
            // {
            //     PlayerPrefs.SetInt("Score" + currentLevel, Score);
            // }
            // if(PlayerPrefs.GetInt("ReachedLevel") <= currentLevel)
            //     PlayerPrefs.SetInt("ReachedLevel",currentLevel+1);
            // PlayerPrefs.Save();
            // if(Application.isEditor)
            //     Debug.Log("Level " + currentLevel + " score " + Score + " stars " + stars);
            CrosssceneData.win = true;
            gameStatus = GameState.Win;
        }

        
          private IEnumerator PreWinLoop(int limit)
        {
            var items = field.GetRandomItems(levelData.limitType == LIMIT.MOVES ? limit : 5);
            var list = new List<object>();
            for (var i = 0; i < items.Count; i++)
            {
                if (items?[i] == null) continue;
                var go = Instantiate(winTrail);
                go.transform.position = movesTransform.position;
                go.GetComponent<TrailEffect>().target = items[i];
                list.Add(go);
                go.GetComponent<TrailEffect>().StartAnim(target =>
                {
                    if (levelData.limitType == LIMIT.MOVES)
                    {
                        levelData.Limit--;
                    }

                    if (target != null && target.gameObject.activeSelf)
                    {
                        target.NextType = (ItemsTypes) Random.Range(4, 6);
                        target.ChangeType();
                    }
                });
                if(skipWin) yield break;
                yield return new WaitForSeconds(0.3f);
            }


            if (list.Count > 0)
            {
                if(skipWin) yield break;
                yield return new WaitForListNull(list);
            }
            levelData.Limit = 0;

            do
            {
                while (field.GetAllExtaItems().Count > 0 && gameStatus != GameState.Win)
                {
                    var item = field.GetAllExtaItems()[0];
                    if (item != null && item.currentType != ItemsTypes.MULTICOLOR) item.DestroyItem();
                    else if (item.currentType == ItemsTypes.MULTICOLOR) item.Check(item, field.GetRandomItems(1).First());
                    dragBlocked = true;
                    if(skipWin) yield break;
                    yield return new WaitForSeconds(0.1f);
                    FindMatches();
                }
                if(skipWin) yield break;

                yield return new WaitWhile(() => findMatchesStarted);
            } while (field.GetAllExtaItems().Count > 0);

            FindMatches();
            while (dragBlocked)
                yield return new WaitForFixedUpdate();

            if(skipWin) yield break;
            yield return new WaitForSeconds(1f);

            MenuReference.THIS.PreCompleteBanner.gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            MenuReference.THIS.PreCompleteBanner.gameObject.SetActive(false);
        }

        private void Update()
        {
            //  AvctivatedBoostView = ActivatedBoost;
            if (Input.GetKeyDown(DebugSettings.Regen) && DebugSettings.enableHotkeys) NoMatches();

            if (Input.GetKeyDown(DebugSettings.Win) && DebugSettings.enableHotkeys)
            {
                stars = Mathf.Clamp(stars,1, stars);
                gameStatus = GameState.PreWinAnimations;
            }

            if (Input.GetKeyDown(DebugSettings.Lose) && DebugSettings.enableHotkeys)
            {
                levelData.Limit = 1;
            }

            if (Input.GetKeyDown(DebugSettings.Restart) && DebugSettings.enableHotkeys)
            {
                InitScript.Instance.AddLife(1);
                RestartLevel();
            }

// #if UNITY_EDITOR
            if (gameStatus == GameState.Playing)
                Time.timeScale = DebugSettings.TimeScaleItems;
            else
                Time.timeScale = DebugSettings.TimeScaleUI;
// #endif

            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //	DestroyAnimatedItems();
            //}
            if (Input.GetKeyDown(DebugSettings.SubSwitch) && DebugSettings.enableHotkeys)
            {
                gameStatus = GameState.ChangeSubLevel;
            }

            if (Input.GetKeyUp(DebugSettings.Back) && DebugSettings.enableHotkeys)
            {
                if (THIS.gameStatus == GameState.Playing)
                    GameObject.Find("CanvasGlobal").transform.Find("MenuPause").gameObject.SetActive(true);
                else if (THIS.gameStatus == GameState.Map)
                    Application.Quit();
            }
            
            if (gameStatus == GameState.PreWinAnimations)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    skipWin = true;
                }
            }

            if (gameStatus == GameState.Playing || gameStatus == GameState.Tutorial)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var hit = Physics2D.OverlapPoint(Rl.Cam.ScreenToWorldPoint(Input.mousePosition),
                        1 << LayerMask.NameToLayer("Item"));
                    if (hit != null)
                    {
                        lastTouchedItem = hit.gameObject.GetComponent<Item>();
                        if (tutorialTime && !lastTouchedItem.tutorialUsableItem) return;
                        if (!THIS.DragBlocked &&
                            (gameStatus == GameState.Playing || gameStatus == GameState.Tutorial))
                        {
                            OnStartPlay?.Invoke();
                            tutorialTime = false;
                            if (THIS.ActivatedBoost.type == BoostType.ExplodeArea &&
                                lastTouchedItem.currentType != ItemsTypes.MULTICOLOR && lastTouchedItem.currentType != ItemsTypes.INGREDIENT)
                            {
                                Rl.GameManager.PlayAudio(Rl.soundStrings.BoostBomb, Random.Range(0,5),true, Rl.settings.GetSFXVolume, Rl.effects.audioSource);
                                THIS.DragBlocked = true;
                                var obj = Instantiate(Resources.Load("Boosts/area_explosion"), lastTouchedItem.transform.position,
                                    lastTouchedItem.transform.rotation) as GameObject;
                                obj.GetComponent<SpriteRenderer>().sortingOrder = 4;
                                obj.GetComponent<BoostAnimation>().square = lastTouchedItem.square;
                                THIS.ActivatedBoost = null;
                            }
                            else if (THIS.ActivatedBoost.type == BoostType.Bomb &&
                                     lastTouchedItem.currentType != ItemsTypes.MULTICOLOR && lastTouchedItem.currentType != ItemsTypes.INGREDIENT)
                            {
                                Rl.GameManager.PlayAudio(Rl.soundStrings.BoostColorReplace, Random.Range(0,5),true, Rl.settings.GetSFXVolume, Rl.effects.audioSource);
                                THIS.DragBlocked = true;
                                var obj = Instantiate(Resources.Load("Boosts/simple_explosion"), lastTouchedItem.transform.position,
                                    lastTouchedItem.transform.rotation) as GameObject;
                                obj.GetComponent<BoostAnimation>().square = lastTouchedItem.square;
                                obj.GetComponent<SpriteRenderer>().sortingOrder = 4;
                                THIS.ActivatedBoost = null;
                            }
                            else if (lastTouchedItem.square != null && lastTouchedItem.square.GetSubSquare().CanGoOut()/* && lastTouchedItem.Combinable*/)
                            {
                                lastTouchedItem.dragThis = true;
                                lastTouchedItem.mousePos = lastTouchedItem.GetMousePosition();
                                lastTouchedItem.deltaPos = Vector3.zero;
                            }
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (lastTouchedItem != null)
                    {
                        lastTouchedItem.dragThis = false;
                        lastTouchedItem.switchDirection = Vector3.zero;
                    }
                }
                else if (Input.GetMouseButton(1))
                {
                    var hit = Physics2D.OverlapPoint(Rl.Cam.ScreenToWorldPoint(Input.mousePosition),
                        1 << LayerMask.NameToLayer("Item"));
                    if (hit != null)
                    {
                        var item = hit.gameObject.GetComponent<Item>();
                        Rl.Cam.GetComponent<ItemChangerTest>().ShowMenuItems(item);

                    }
                }
            }
        }

        private void RestartLevel()
        {
            GameObject gm = new GameObject();
            gm.AddComponent<RestartLevel>();
            SceneManager.LoadScene(0);
        }

        #region RegenerateLevel
        //No matches detected, regenerate level
        public void NoMatches()
        {
            if (field.fieldData.noRegenLevel || !field.fieldData.noMatches) return;
            StartCoroutine(NoMatchesCor());
        }

        private IEnumerator NoMatchesCor()
        {
            if (gameStatus == GameState.Playing)
            {
                DragBlocked = true;

                Rl.GameManager.PlayAudio(Rl.soundStrings.NoMatch, Random.Range(0,5),true, Rl.settings.GetSFXVolume, Rl.effects.audioSource);

                NoMoreMatches.gameObject.SetActive(true);
                gameStatus = GameState.RegenLevel;
                yield return new WaitForSeconds(1);
                ReGenLevel();
            }
        }
        public void ReGenLevel()
        {
            DragBlocked = true;
            //if (gameStatus != GameState.Playing && gameStatus != GameState.RegenLevel)
            //DestroyAnimatedItems();
//        if (gameStatus == GameState.RegenLevel)
//            DestroyItems(true);
            StartCoroutine(RegenMatches());
            if (OnLevelLoaded != null) OnLevelLoaded();
        }

        private IEnumerator RegenMatches(bool onlyFalling = false)
        {
            if (gameStatus == GameState.RegenLevel)
            {
                yield return new WaitForSeconds(0.5f);
            }
            if (!onlyFalling)
                field.RegenItems(false);
            else
                THIS.onlyFalling = true;
            field.GetItems().ForEach(i=>i.Hide(true));
            yield return new WaitForFixedUpdate();

            var combs = new List<List<Item>>();
            do
            {
                yield return new WaitWhileFall();
                combs = CombineManager.GetCombinedItems(field);
                combs.Add( AI.THIS.GetMarmaladeCombines()?.items);
                combs = combs.WhereNotNull().Where(i => i.Count() > 0).ToList();
                yield return new WaitForEndOfFrame();
                foreach (var comb in combs)
                {
                    foreach (var item in comb)
                    {
                        item.GenColor();
                    }
                }
            } while (combs.Count > 0);

            SetPreBoosts();
            if (!onlyFalling)
                DragBlocked = false;
            THIS.onlyFalling = false;
            if (gameStatus == GameState.RegenLevel)
                gameStatus = GameState.Playing;
            yield return new WaitForEndOfFrame();
            field.GetItems().ForEach(i=>
            {
                var transformLocalScale = i.transform.localScale;
                i.transform.localScale = Vector3.zero;
                LeanTween.scale(i.gameObject, transformLocalScale, .3f); i.Hide(false);  });
        }
    
        public void DestroyItems(bool withoutEffects = false)
        {
            var items = field.GetItems();
            foreach (var item in items)
            {
                if (item != null)
                {
                    if (item.GetComponent<Item>().currentType != ItemsTypes.INGREDIENT &&
                        item.GetComponent<Item>().currentType == ItemsTypes.NONE)
                    {
                        if (!withoutEffects)
                            item.GetComponent<Item>().DestroyItem();
                        else
                            item.GetComponent<Item>().anim.SetTrigger("disappear");
                    }
                }
            }
        }
    
        #endregion


        /// <summary>
        /// Place boosts in the field which bought before in Play menu
        /// </summary>
        private void SetPreBoosts()
        {
            if (BoostPackage > 0)
            {
                InitScript.Instance.SpendBoost(BoostType.Packages);
                foreach (var item in field.GetRandomItems(BoostPackage))
                {
                    item.NextType = ItemsTypes.PACKAGE;
                    item.ChangeType(null, true, false);
                }

                BoostPackage = 0;
            }

            if (BoostColorfullBomb > 0)
            {
                InitScript.Instance.SpendBoost(BoostType.MulticolorCandy);
                foreach (var item in field.GetRandomItems(BoostColorfullBomb))
                {
                    item.NextType = ItemsTypes.MULTICOLOR;
                    item.ChangeType(null, true, false);
                }

                BoostColorfullBomb = 0;
            }

            if (BoostStriped > 0)
            {
                InitScript.Instance.SpendBoost(BoostType.Stripes);
                foreach (var item in field.GetRandomItems(BoostStriped))
                {
                    item.NextType = (ItemsTypes)Random.Range(4, 6);
                    item.ChangeType(null, true, false);
                }

                BoostStriped = 0;
            }

            if (BoostMarmalade > 0)
            {
                InitScript.Instance.SpendBoost(BoostType.Marmalade);
                foreach (var item in field.GetRandomItems(BoostMarmalade))
                {
                    item.NextType = ItemsTypes.MARMALADE;
                    item.ChangeType(null, true, false);
                }

                BoostStriped = 0;
            }
        }


        //Find matches with delay
        public IEnumerator FindMatchDelay()
        {
            yield return new WaitForSeconds(0.2f);
            THIS.FindMatches();
        }

        //start sync matches search then wait while items destroy and fall
        public void FindMatches()
        {
            if (!findMatchesStarted)
                StartCoroutine(FallingDown());
            else
                checkMatchesAgain = true;
        }

        internal List<GameBlocker> _stopFall = new List<GameBlocker>();
        public int combo;
        public AnimationCurve fallingCurve = AnimationCurve.Linear(0, 0, 1, 0);
        public float waitAfterFall = 0.02f;
        [HideInInspector] private bool collectIngredients;

        private Item lastTouchedItem;
        private int winRewardAmount;
        public bool skipWin;
        private GameObject tapToSkip;

        private IEnumerator FallingDown()
        {
            AI.THIS.ResetTimer();
           // avoided = false;
            findMatchesStarted = true;
//        Debug.Log("@@@ Next Move search matches @@@");
            THIS.thrivingBlockDestroyed = false;
            combo = 0;
            AI.THIS.allowShowTip = false;
            List<Item> it = field.GetItems();
            for (var i = 0; i < it.Count; i++)
            {
                var item = it[i];
                if (item != null)
                {
                    item.anim.StopPlayback();
                }
            }

            destLoopIterations = 0;
            while (true)
            {
                destLoopIterations++;
                checkMatchesAgain = false;

                var destroyItemsListed = field.GetItems().Where(i => i.destroyNext).ToList();
                // if (IgnoreDestroyColorIDS)
                // {
                //     for (int i = 0; i < field.squaresArray.Length;i++)
                //     {
                //         if (Random.Range(0, 3) == 2) destroyItemsListed.Add(field.squaresArray[i].Item);
                //
                //     }
                // }
                //
                // IgnoreDestroyColorIDS = false;
                if (destroyItemsListed.Count > 0)
                {
                  //  avoided = false;
                    yield return new WaitWhileDestroyPipeline(destroyItemsListed, new Delays());
                }
                  
              //  else avoided = true;
                yield return new WaitWhileDestroying();
                yield return new WaitWhile(()=>StopFall);
                yield return new WaitWhileFall();
                yield return new WaitWhileCollect();
//            yield return new WaitWhileFallSide();
                List<Combine> combineCount = CombineManager.GetCombines(field);
                if ((combineCount.Count <= 0 || !combineCount.SelectMany(i => i.items).Any()) &&
                    !field.DestroyingItemsExist() && !field.GetEmptySquares().Any() &&
                    !checkMatchesAgain)
                {
                    break;
                }
                
                if(destLoopIterations > 1)
                {
                  //  avoided = false;
                    var combines = combineCount.Where(i=>i.items.All(x=>!x.dontDestroyForThisCombine)).ToList();
                    foreach (var combine in combines)
                    {
                        if(combine.items.Any())
                            combine.items.NextRandom().NextType = combine.nextType;
                    }

                    combines.SelectMany(i => i.items).ToList().ForEach(i => i.destroyNext = true);
                }
                if (!animateItems.Any())
                    levelData.TargetCounters.ForEachY(i => i.GetCount());
            }

            if (combo > 2 && gameStatus == GameState.Playing)
            {
               // avoided = false;
                gratzWords[Random.Range(0, gratzWords.Length)].SetActive(true);
                combo = 0;
                OnCombo?.Invoke();
            }

            //CheckItemsPositions();
            DragBlocked = false;
            findMatchesStarted = false;
            checkMatchesAgain = false;
          //  if (avoided && !GenericFunctions.IsSubstractiveState(levelData.limitType))
           // {
             //   avoided = false;
            //    avoidedLateUpdate = true;
          //  }
            if (gameStatus == GameState.Playing)
                StartCoroutine(AI.THIS.CheckPossibleCombines());

//        Debug.Log("<-next turn->");
            if (gameStatus == GameState.Playing)
            {
                if (!THIS.animateItems.Any())
                {
                    OnTurnEnd?.Invoke();
                    THIS.CheckWinLose();
                }
                else
                {
                    yield return new WaitWhile(()=>THIS.animateItems.Any());
                    OnTurnEnd?.Invoke();
                    THIS.CheckWinLose();
                }
            }

            if (avoidedLateUpdate)
            {
                //levelData.limit--;
            }
            else 
                //levelData.Limit++
                ;

            avoidedLateUpdate = false;
          //  DestroyOnlyTarget[0] = false;
            // StartCoroutine(AvoidedCo(0.00001f));
        }

        private IEnumerator AvoidedCo(float sec)
        {
            yield return new WaitForSeconds(sec);
        
        }

        /// <summary>
        /// Get square by position
        /// </summary>
        ///
        public Square GetSquare(int col, int row, bool safe = false)
        {
            return field.GetSquare(col, row, safe);
        }
        /// <summary>
        /// Get bunch of squares by row number
        /// </summary>
        public List<Square> GetRowSquare(int row)
        {
            var itemsList = new List<Square>();
            for (var col = 0; col < levelData.maxCols; col++)
            {
                Square square = GetSquare(col, row, true);
                if (!square.IsNone())
                    itemsList.Add(square);
            }

            return itemsList;
        }
        /// Get bunch of squares by column number
        public List<Square> GetColumnSquare(int col)
        {
            var itemsList = new List<Square>();
            for (var row = 0; row < levelData.maxRows; row++)
            {
                Square square = GetSquare(col, row, true);
                if (!square.IsNone())
                    itemsList.Add(square);
            }

            return itemsList;
        }
        /// Get bunch of items by row number
        public List<Item> GetRow(int row)
        {
            var itemsList = new List<Item>();
            for (var col = 0; col < levelData.maxCols; col++)
            {
                itemsList.Add(GetSquare(col, row, true).Item);
            }

            return itemsList.WhereNotNull().ToList();
        }
        /// Get bunch of items by column number
        public List<Item> GetColumn(int col)
        {
            var itemsList = new List<Item>();
            for (var row = 0; row < levelData.maxRows; row++)
            {
                itemsList.Add(GetSquare(col, row, true).Item);
            }

            return itemsList.WhereNotNull().ToList();
        }
        /// <summary>
        /// Get squares around the square
        /// </summary>
        /// <param name="square"></param>
        /// <returns></returns>
        public List<Square> GetSquaresAroundSquare(Square square)
        {
            var col = square.col;
            var row = square.row;
            var itemsList = new List<Square>();
            for (var r = row - 1; r <= row + 1; r++)
            {
                for (var c = col - 1; c <= col + 1; c++)
                {
                    itemsList.Add(GetSquare(c, r, true));
                }
            }

            return itemsList;
        }

        /// <summary>
        /// get 9 items around the square
        /// </summary>
        /// <param name="square"></param>
        /// <returns></returns>
        public List<Item> GetItemsAround9(Square square)
        {
            var itemsList = GetItemsAround8(square);
            itemsList.Add(square.Item);
            return itemsList;
        }

        /// get 8 items around the square
        public List<Item> GetItemsAround8(Square square)
        {
            var col = square.col;
            var row = square.row;
            var itemsList = new List<Item>();
            for (var r = row - 1; r <= row + 1; r++)
            {
                for (var c = col - 1; c <= col + 1; c++)
                {
                    var pos = GetSquare(c, r, true).transform.position;
                    RaycastHit2D[] result = new RaycastHit2D[3];
                    Physics2D.LinecastNonAlloc(square.transform.position, pos, result,1<< LayerMask.NameToLayer("Item"));
                    var hit = result.Where(i => i.transform).OrderBy(i => (i.transform.position - pos).magnitude).FirstOrDefault();
                    if(hit && hit.transform)
                        itemsList.Add(hit.transform.GetComponent<Item>());
                }
            }

            return itemsList;
        }

        //striped effect
        public void StripedShow(GameObject obj, bool horrizontal)
        {
            var effect = Instantiate(stripesEffect, obj.transform.position, Quaternion.identity);
            if (!horrizontal)
                effect.transform.Rotate(Vector3.back, 90);
            Destroy(effect, 1);
        }

        //popup score, to use - enable "Popup score" in editor
        public void ShowPopupScore(int value, Vector3 pos, int color)
        {
            // UpdateBar();
            if (showPopupScores)
            {
                var parent = GameObject.Find("CanvasScore").transform;
                var poptxt = Instantiate(popupScore, pos, Quaternion.identity);
                poptxt.transform.GetComponentInChildren<Text>().text = "" + value;
                if (color <= scoresColors.Length - 1)
                {
                    poptxt.transform.GetComponentInChildren<Text>().color = scoresColors[color];
                    poptxt.transform.GetComponentInChildren<Outline>().effectColor = scoresColorsOutline[color];
                }

                poptxt.transform.SetParent(parent);
                //   poptxt.transform.position += Vector3.right * 1;
                poptxt.transform.localScale = Vector3.one / 1.5f;
                Destroy(poptxt, 0.3f);
            }
        }

        /// <summary>
        /// check gained stars
        /// </summary>
        ///
        [SerializeField] public Sprite TestSprite;
        public void CheckStars()
        {
            if (Score >= levelData.star1 && stars <= 0)
            {
                stars = 1;
            }

            if (Score >= levelData.star2 && stars <= 1)
            {
                stars = 2;
            }

            if (Score >= levelData.star3 && stars <= 2)
            {
                stars = 3;
            }
        }

        /// <summary>
        /// load level from 
        /// </summary>
        /// <param name="currentLevel"></param>
        public void LoadLevel(int currentLevel)
        {
            levelLoaded = false;
            levelData = LoadingManager.LoadForPlay(currentLevel, levelData);
       
            if (gameStatus != GameState.Map)
            {
          
                foreach (var fieldData in levelData.fields)
                {
                    GameObject _field  = Instantiate(FieldBoardPrefab);
                    var fboard = _field.GetComponent<FieldBoard>();
                    fboard.fieldData = fieldData;
                    fboard.squaresArray = new Square[fieldData.maxCols * fieldData.maxRows];
                    fieldBoards.Add(fboard);
                }
            }
        }
    }
}
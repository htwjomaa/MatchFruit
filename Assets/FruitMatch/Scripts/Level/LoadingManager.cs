using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using FruitMatch.Scripts.Blocks;
using FruitMatch.Scripts.Core;
using FruitMatch.Scripts.System;
using FruitMatch.Scripts.Items;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace FruitMatch.Scripts.Level
{
    /// <summary>
    /// Loading level 
    /// </summary>
    public static class LoadingManager 
    {
        private static LevelData levelData;
        static string levelPath = "Assets/SweetSugar/Resources/Levels/";
        public static LevelConfig LevelConfig;
        public  static List<GameObject> allEnabledDots;
        public static Sprite[] loadedSprites;
        public static Sprite[] loadedMarmaladeSprites;
        public static Sprite[] loadedHorStriped;
        public static Sprite[] loadedVertStriped;
        public static GameObject[] GetDots(BoardDimensionsConfig boardDimensionsConfig, World world)
        {
            List<FruitType> enabledFruitTypes = new List<FruitType>();
            allEnabledDots = new List<GameObject>();
            for (int i = 0; i < boardDimensionsConfig.FruitsConfigParent.Length; i++)
            {
                if(boardDimensionsConfig.FruitsConfigParent[i].FruitEnabled)
                    enabledFruitTypes.Add(boardDimensionsConfig.FruitsConfigParent[i].FruitType);
            }

            for (int i = 0; i < enabledFruitTypes.Count; i++)
            {
                allEnabledDots.Add(world.GetFruitPrefab(enabledFruitTypes[i], Colors.AlleFarben)); ;  // <<<<<----CHANGE THIS TO THE RIGHT COLOR ONCE IMPLEMENTED
            }
          //  Debug.Log("DeboardDimensionsConfig.FruitsConfigParent.Length : ::::  " + boardDimensionsConfig.FruitsConfigParent.Length);

          //  Debug.Log("enabledFruitTypes : ::::  " + enabledFruitTypes );
//            Debug.Log("allEnabledDots : ::::  " + allEnabledDots );
            return allEnabledDots.ToArray();
        }
        private static Sprite TranslateDotsToRandomColors(Dot dot, Sprite[] sprites)
        {
            switch (dot.tag)
            {
                case "Green Dot":
                    return sprites[0];
                case "Dark Green Dot":
                    return sprites[1];
                case "Red Dot":
                    return sprites[2];;
                case "Orange Dot":
                    return sprites[3];
                case "Teal Dot":
                    return sprites[4];
                case "Indigo Dot":
                    return sprites[5];
                case "Yellow Dot":
                    return sprites[6];
                case "Salmon Dot":
                    return sprites[7];
            }

            return   LoadingHelper.THIS.Sprites[0];
        }
        public static GameObject[] allDotPrefabs;
        public static Sprite[] GetLoadedSprites(Sprite[] sprites)
        {
            List<Sprite> allSprites = new List<Sprite>();
            allDotPrefabs  = GetDots(Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs
                .LevelConfigs[LevelManager.THIS.currentLevel-1].BoardDimensionsConfig, Rl.world);
            for (int i = 0; i < allDotPrefabs.Length; i++)
            {
                allSprites.Add(TranslateDotsToRandomColors(allDotPrefabs[i]
                    .GetComponent<Dot>(),sprites));
            }

            return allSprites.ToArray();
        }

        private static SquareTypes BlockTranslater(TT tileType)
        {
            switch (tileType)
            {
                case TT.Normal:
                    return SquareTypes.EmptySquare;
                case TT.EmptyTile:
                    return SquareTypes.NONE;
                case TT.Undestroyable:
                    return SquareTypes.UndestrBlock;
                case TT.Jelly:
                    return SquareTypes.JellyBlock;
                //case TT.Lock:
                 //   return Sq
                case TT.Chest:
                    return SquareTypes.SugarSquare;
                case TT.LockedChest:
                    return SquareTypes.SugarSquare;
                case TT.Bubble:
                    return SquareTypes.ThrivingBlock;
                case TT.BigBlock:
                    return SquareTypes.BigBlock;
              //  case TT.Fruit:
                //    break;
               // case TT.Bomb:
               //     break;
              //  case TT.Ingredient:
                  
               // case TT.SameColorBomb:
                  //  break;
                default:
                    return SquareTypes.EmptySquare;
            }
        }
        private static ItemsTypes ItemTranslater(TT tileType)
        {
            switch (tileType)
            {
                case TT.Lock:
                    return ItemsTypes.PACKAGE;
                  case TT.Fruit:
                      return ItemsTypes.MARMALADE;
                 case TT.Bomb:
                     return ItemsTypes.VERTICAL_STRIPED;
                  case TT.Ingredient:
                    return ItemsTypes.INGREDIENT;
                 case TT.SameColorBomb:
                     return ItemsTypes.MULTICOLOR;
                default:
                    return ItemsTypes.NONE;
            }
        }

        private static Vector2Int DirectioEnumTranslater(Directions direction)
        {
            switch (direction)
            {
                case Directions.top:
                    return new Vector2Int(0, 1);
                case Directions.bottom:
                    return new Vector2Int(0, -1);
                case Directions.left:
                    return new Vector2Int(-1, 0);
                case Directions.right:
                    return new Vector2Int(1, 0);
                case Directions.none:
                    return new Vector2Int(0, 0);
                default:
                    return new Vector2Int(0, 0);
            }
        }
        private static void TranslateDirections(List<TileSettingConfig> tileSettingConfigs, ref LevelData levelData)
        {
            for (int i = 0; i < levelData.fields[0].levelSquares.Length; i++)
            {
              if(levelData.fields[0].levelSquares[i] != null) 
              {
                  
                  levelData.fields[0].levelSquares[i].direction = DirectioEnumTranslater(tileSettingConfigs[i].Direction);
                  levelData.fields[0].levelSquares[i].enterSquare = tileSettingConfigs[i].IsDirectionStart;
              }
            }
        }
        public static GameObject[] GetBlockPrefab(SquareTypes sqType, int layer=-1)
        {
            var list = new List<GameObject>();
            var item1 = Resources.Load("FruitMatch/Resources/Blocks/" + sqType) as GameObject;
            var layeredBlock = item1?.GetComponent<LayeredBlock>();
            if (layeredBlock != null)
            {
                int range = layer == -1 ? layeredBlock.layers.Length : layer+1;
                list.AddRange(layeredBlock.layers.Select(i => i.gameObject).Take(range));
            }
            if (list.Count() == 0 && item1 != null) list.Add(item1);
            return list.ToArray();
        }
        private static void TranslateBlocks(List<TileSettingConfig> tileSettingConfigs, ref LevelData levelData)
        {
            for (int i = 0; i < levelData.fields[0].levelSquares.Length; i++)
            {
                if(levelData.fields[0].levelSquares[i] != null)
                {
                    levelData.fields[0].levelSquares[i].block = SquareTypes.EmptySquare;
                    levelData.fields[0].levelSquares[i].blocks = new List<SquareTypeLayer>();
                    var sqrType  = BlockTranslater(tileSettingConfigs[i].TileType);
                
                    if (sqrType != SquareTypes.NONE)
                    {
                        levelData.fields[0].levelSquares[i].blocks.Add(new SquareTypeLayer(SquareTypes.EmptySquare));
                    }
                    else
                    {
                        levelData.fields[0].levelSquares[i].block = SquareTypes.NONE;
                        levelData.fields[0].levelSquares[i].obstacle = SquareTypes.NONE;
                        levelData.fields[0].levelSquares[i].item.ItemType = ItemsTypes.NONE;
                    }
                    levelData.fields[0].levelSquares[i].obstacle = sqrType;
                    if (sqrType != SquareTypes.EmptySquare && sqrType != SquareTypes.NONE)
                    {
                        levelData.fields[0].levelSquares[i].blocks.Add(new SquareTypeLayer(sqrType));
                    }
                    levelData.fields[0].levelSquares[i].item.ItemType = ItemTranslater(tileSettingConfigs[i].TileType);
                    if (levelData.fields[0].levelSquares[i].item.ItemType != ItemsTypes.NONE)
                    {
                        levelData.fields[0].levelSquares[i].item.Item = LoadingHelper.THIS.GetItemPrefab(levelData.fields[0].levelSquares[i].item.ItemType );
                        levelData.fields[0].levelSquares[i].item.SetColor(0, CurrentLevelToload-1);
                    };
                    levelData.fields[0].levelSquares[i].enterSquare = tileSettingConfigs[i].IsDirectionStart;
                    // if (levelData.fields[0].levelSquares[i].block == SquareTypes.EmptySquare)
                    // {
                    //     levelData.fields[0].levelSquares[i].blocks.Add(new SquareTypeLayer());
                    // }
                }
            }
        }
[Serializable]
        struct  TeleportTranslateStruct
        {
            public int TeleportFrom;
            public int TeleportTo;

            public TeleportTranslateStruct(int teleportFrom, int teleportTo)
            {
                TeleportFrom = teleportFrom;
                TeleportTo = teleportTo;
            }
        }
        private static void TranslateTeleports(List<TileSettingConfig> tileSettingConfigs, ref LevelData levelData)
        {

            List<TeleportTranslateStruct> linkBackList = new List<TeleportTranslateStruct>();

            for (int i = 0; i < levelData.fields[0].levelSquares.Length; i++)
            {
                levelData.fields[0].levelSquares[i].isEnterTeleport = false;
                levelData.fields[0].levelSquares[i].teleportCoordinatesLinked = new Vector2Int(-1, -1);
                levelData.fields[0].levelSquares[i].teleportCoordinatesLinkedBack = new Vector2Int(-1, -1);
            }

            for (int y = 0; y < 9; y++)
            { 
                for (int x = 0; x < 9; x++)
                {
                    levelData.fields[0].levelSquares[y+x].position = new Vector2Int(x, y);
                }

            }


           // levelData.fields[0].levelSquares[0].teleportCoordinatesLinked = new Vector2Int(5, 0);
            for (int i = 0; i < levelData.fields[0].levelSquares.Length; i++)
            {
                if (levelData.fields[0].levelSquares[i] != null && tileSettingConfigs[i].TeleportTarget > -1)
                    {
                        levelData.fields[0].levelSquares[i].isEnterTeleport = true;
                        int translatedTarget = TranslateTeleportTarget(tileSettingConfigs[i].TeleportTarget);
                        Debug.Log("TranslatedTarget: " + translatedTarget);
                        Debug.Log("TransformToXYCoords(translatedTarget): " + TransformToXYCoords(translatedTarget));
                        linkBackList.Add(new TeleportTranslateStruct(i, translatedTarget));
                        levelData.fields[0].levelSquares[i].teleportCoordinatesLinked = TransformToXYCoords(translatedTarget);
                    }
            }

              for (int i = 0; i < linkBackList.Count; i++)
              {
                  levelData.fields[0].levelSquares[linkBackList[i].TeleportTo].teleportCoordinatesLinkedBack =
                      TransformToXYCoords(linkBackList[i].TeleportFrom);
                  Debug.Log(   "TransformToXYCoords(linkBackList[i].TeleportFrom::: " +    TransformToXYCoords(linkBackList[i].TeleportFrom));
                  Debug.Log("LinkBackList: To: " + linkBackList[i].TeleportTo + " || From: " +
                            linkBackList[i].TeleportFrom);
                  
                  levelData.fields[0].levelSquares[linkBackList[i].TeleportTo].enterSquare = true;
                 
              }
              //levelData.fields[0].levelSquares[5].teleportCoordinatesLinkedBack = new Vector2Int(0, 0);
        }

        private static List<TileSettingConfig> TranslateToRightDownDimension(TileSettingConfig[]  rightUpSystem)
        {
            TileSettingConfig[] rightDownSystem = new TileSettingConfig[81];

            int cHeight = 8;
            int counter = 0;
            for (int i = 0; i < rightUpSystem.Length; i++)
            {
                rightDownSystem[i] = rightUpSystem[cHeight * 9 + counter];
                counter++;
                if (counter == 9)
                {
                    counter = 0;
                    cHeight--;

                }
            }
            return rightDownSystem.ToList();
        }

        private static Vector2Int TransformToXYCoords(int TeleportTarget)
        {
            int y = TeleportTarget / 9;
            int x = TeleportTarget - y*9;
            return new Vector2Int(x, y);
        }
        private static int TranslateTeleportTarget(sbyte TeleportTarget)
        {
            int cHeight = 8;
            int counter = 0;
            for (int i = 0; i < 81; i++)
            {
                if (TeleportTarget == cHeight * 9 + counter) return i;
                counter++;
                if (counter == 9)
                {
                    counter = 0;
                    cHeight--;

                }
            }

            return -1;
        }

        private static SquareBlocks[] CutOutBlocks(SquareBlocks[] sqrBlocks, int width, int height)
        {
             //return CutOutRows(sqrBlocks, width);
            sqrBlocks = CutOutColumns(sqrBlocks, height); 
           return CutOutRows(sqrBlocks, width);
        }
        private static SquareBlocks[] CutOutRows( SquareBlocks[] squareBlocks, int width)
        {
            List<SquareBlocks> squareBlocksList = new List<SquareBlocks>();
            int counter = 0;
            for (int i = 0; i < squareBlocks.Length; i++)
            {
                if (counter < width)
                {
                    squareBlocksList.Add(squareBlocks[i]);
                   
                }
                counter++;
                if(counter == 9) counter = 0;
            }
            
            return squareBlocksList.ToArray();
        }
        private static SquareBlocks[] CutOutColumns(SquareBlocks[] sqrBlocks, int height)
        {
            List<SquareBlocks> squareBlocksList = new List<SquareBlocks>();
            for (int i = (9-height)*9; i < sqrBlocks.Length; i++)
            {
                squareBlocksList.Add(sqrBlocks[i]);
            }

            return squareBlocksList.ToArray();
        }
        public static LevelData LoadForPlay(int currentLevel, LevelData levelData)
        {
            Rl.goalManager.TranslateGoals();
            LevelConfig = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[CurrentLevelToload-1];
            levelData = new LevelData(Application.isPlaying, CurrentLevelToload );
            levelData = LoadlLevel(CurrentLevelToload , levelData).DeepCopyForPlay(CurrentLevelToload );
            TranslateData();
            List<TileSettingConfig> tileSettingConfigs = TranslateToRightDownDimension(LevelConfig.TileSettingFieldConfigs[0].TileSettingConfigs.ToList().ToArray());
            TranslateDirections(tileSettingConfigs,ref levelData);
            TranslateTeleports(tileSettingConfigs, ref levelData);
            TranslateBlocks(tileSettingConfigs, ref levelData);
            levelData.LoadTargetObject();

            levelData.InitTargetObjects(true);
            Rl.adminLevelSettingsLookDev.SetBackgroundImage();

            for (int i = 0; i < levelData.fields.Count; i++)
            {
                levelData.fields[i].levelSquares = CutOutBlocks(levelData.fields[i].levelSquares, LevelConfig.BoardDimensionsConfig.Width, LevelConfig.BoardDimensionsConfig.Height);
            }
            void TranslateData()
            {
                for (int i = 0; i < levelData.fields.Count; i++)
                {
                    levelData.fields[i].maxCols = LevelConfig.BoardDimensionsConfig.Width;
                    levelData.fields[i].maxRows = LevelConfig.BoardDimensionsConfig.Height;
                    LoadingHelper.THIS.height = LevelConfig.BoardDimensionsConfig.Height;
                    LoadingHelper.THIS.width = LevelConfig.BoardDimensionsConfig.Width;
                    levelData.fields[i].noRegenLevel = false;
                }
                
            

                LIMIT limit = LIMIT.MOVES;
                GameType gameType = GameType.Moves;
                if (LevelConfig.BoardDimensionsConfig.GameTypeP1.GameType == GameType.Time)
                {
                    limit = LIMIT.TIME;
                    gameType = GameType.Time;
                }
                levelData.star1 =
                    GenericSettingsFunctions.GetConstvaluesMovesTime(LevelConfig.ScoreGoalsConfig.Star1Value,
                        gameType);
                levelData.star2 =
                    GenericSettingsFunctions.GetConstvaluesMovesTime(LevelConfig.ScoreGoalsConfig.Star2Value,
                        gameType);
                levelData.star3 =
                    GenericSettingsFunctions.GetConstvaluesMovesTime(LevelConfig.ScoreGoalsConfig.Star3Value,
                        gameType);
                levelData.limit = GenericSettingsFunctions.GetConstvaluesMovesTime(LevelConfig.BoardDimensionsConfig.GameTypeP1.CounterValue, gameType);
                
                levelData.limitType = limit;
                levelData.enableMarmalade = true;
                
                // levelData.Name = LevelConfig.LevelTextConfig[LevelManager.THIS.currentLevel-1].LevelName;
                loadedSprites = GetLoadedSprites( LoadingHelper.THIS.Sprites);
                loadedMarmaladeSprites = GetLoadedSprites(LoadingHelper.THIS.Marmalades);
                loadedHorStriped = GetLoadedSprites(LoadingHelper.THIS.HorStriped);
                loadedVertStriped = GetLoadedSprites(LoadingHelper.THIS.VertStriped);
                levelData.colorLimit = loadedSprites.Length;
                    
                LoadingHelper.THIS.loadedSpritesDebug = loadedSprites;
                LoadingHelper.THIS.loadedSpritesDebugMarmalade = loadedMarmaladeSprites;
                //levelData.fields[0].
                // GoalManager goalManager = Rl.goalManager;
                //  goalManager. TranslateGoals();
//                var n = goalManager._goalsP1Queue.Dequeue();

                // levelData.subTargetsContainers[0].extraObject = n[0].goalSprite;
                // Get the Prefab Asset root GameObject and its asset path.
                //   GameObject assetRoot = LoadingHelper.THIS.ItemPrefab;
                /*string assetPath = "Assets/FruitMatch/Resources/Items/Item.prefab";
                // Modify prefab contents and save it back to the Prefab Asset
         
                using (EditPrefabAssetScope editScope = new EditPrefabAssetScope(assetPath))
                {
                    for (int i = 0; i < loadedSprites.Length;i++)
                    {
                        editScope.prefabRoot.GetComponent<IColorableComponent>().Sprites[0].Sprites[i] = loadedSprites[i];
                    }

                   // editScope.prefabRoot.GetComponent<IColorableComponent>().Sprites[0].Sprites[n.Length] = n[^1];
                }*/
            }
            /*
             
            public static List<Vector3> InstaniteEverythingAtStart(int width, int height, float leftOffset, float rightOffset, float bottomOffset,
        float topOffset, GameObject tilePrefab, BackgroundTile[,] alltiles, ref List<BackGroundTileSideList> allSideBackGroundTiles,
        Sprite tileBackgroundBright, Sprite tileBackgroundDark,  GameObject[] dots, GameObject[,] allDots, ref BackgroundTile[,] allTiles, Transform boardTransform,
        bool bottomActive, bool leftActive, bool rightActive, bool topActive )
             */
            //LoadingManager.LoadSideDots(LoadingHelper.THIS.boardTransform);
            return levelData;
        }

        public static void SetSideDotActive(SideFruitsConfig sideFruitsConfig)
        {
            LoadingHelper.THIS.bottomActive= sideFruitsConfig.BottomActive;
            LoadingHelper.THIS.leftActive= sideFruitsConfig.LeftActive;
            LoadingHelper.THIS.rightActive= sideFruitsConfig.RightActive;
            LoadingHelper.THIS.topActive= sideFruitsConfig.TopActive;
        }
        public static void LoadSideDots()
        {
            BoardInstantiate.InstaniteEverythingAtStart(
                LoadingHelper.THIS.width,
                LoadingHelper.THIS.height,
                LoadingHelper.THIS.leftOffset,
                LoadingHelper.THIS.rightOffset,
                LoadingHelper.THIS.bottomOffset,
                LoadingHelper.THIS.topOffset,
                LoadingHelper.THIS.tilePrefab,
                LoadingHelper.THIS.alltiles,
                ref LoadingHelper.THIS.bgTiles,
                LoadingHelper.THIS.tileBackgroundBright,
                LoadingHelper.THIS.tileBackgroundDark,
                LoadingHelper.THIS.allDots,
                ref LoadingHelper.THIS.alltiles,
                LoadingHelper.THIS.boardTransform,
                LoadingHelper.THIS.bottomActive,
                LoadingHelper.THIS.leftActive,
                LoadingHelper.THIS.rightActive,
                LoadingHelper.THIS.topActive
            );
            float yWert = 0.35f;
            yWert -= (LoadingHelper.THIS.height * 0.4f);
            LoadingHelper.THIS.boardTransform.position = new Vector3(-3.3f, yWert , 0f);
            LoadingHelper.THIS.boardTransform.localScale = new Vector3(0.82f, 0.82f, 0.82f);
            
          LoadingHelper.THIS.SideDotScaler();
        }
        
        
#if UNITY_EDITOR
        public class EditPrefabAssetScope : IDisposable {
 
    public readonly string assetPath;
    public readonly GameObject prefabRoot;

            

    public EditPrefabAssetScope(string assetPath) {
        this.assetPath = assetPath;
        prefabRoot = PrefabUtility.LoadPrefabContents(assetPath);
    }

    public void Dispose() {
        PrefabUtility.SaveAsPrefabAsset(prefabRoot, assetPath);
        PrefabUtility.UnloadPrefabContents(prefabRoot);
    }

}
#endif
        public static int CurrentLevelToload = 0;
        public static LevelData LoadlLevel(int currentLevel, LevelData levelData)
        {
            levelData = ScriptableLevelManager.LoadLevel(CurrentLevelToload );
            levelData.CheckLayers();
            // LevelData.THIS = levelData;
            levelData.LoadTargetObject();
            // levelData.InitTargetObjects();

            return levelData;
        }


        public static int GetLastLevelNum()
        {
            return Resources.LoadAll<LevelContainer>("Levels").Length;
        }
    }
}
#if UNITY_EDITOR
public class EditPrefabAssetScope : IDisposable {
 
    public readonly string assetPath;
    public readonly GameObject prefabRoot;
 
    public EditPrefabAssetScope(string assetPath) {
        this.assetPath = assetPath;
        prefabRoot = PrefabUtility.LoadPrefabContents(assetPath);
    }
 
    public void Dispose() {
        PrefabUtility.SaveAsPrefabAsset(prefabRoot, assetPath);
        PrefabUtility.UnloadPrefabContents(prefabRoot);
    }
}
#endif

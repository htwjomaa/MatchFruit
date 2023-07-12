﻿using System;
using System.Collections.Generic;
using System.Linq;
 using FruitMatch.Scripts.Blocks;
 using FruitMatch.Scripts.Core;
 using FruitMatch.Scripts.Items;
 using FruitMatch.Scripts.TargetScripts.TargetEditor;
 using FruitMatch.Scripts.TargetScripts.TargetSystem;
 using FruitMatch.Scripts.System;
 using UnityEngine;
 using UnityEngine.Serialization;

 namespace FruitMatch.Scripts.Level
{
    /// <summary>
    /// Level data for level editor
    /// </summary>
    [Serializable]
    public class LevelData
    {
        public string Name;
        public static LevelData THIS;
        /// level number
        public int levelNum;
        private int hashCode;
        /// fields data
        public List<FieldData> fields = new List<FieldData>();
        /// target container keeps the object should be collected, its count, sprite, color
        [SerializeField] public TargetContainer target;
        /// target manager reference
        [SerializeField] public Target targetObject;
        public int targetIndex;

        // public static TargetContainer targetContainer;
        /// moves or time
        public LIMIT limitType;
       
        public int[] ingrCountTarget = new int[2];
        /// moves amount or seconds 
         public int limit = 25;
        public int Limit
        {
            get => limit;
            set
            {
                LevelManager.THIS.InvokeMoveMadeEvent();
                limit = value;
            }
        }


        /// color amount
        public int colorLimit = 5;
        /// score amount for reach 1 star
        public int star1 = 100;
        /// score amount for reach 2 stars
        public int star2 = 300;
        /// score amount for reach 3 stars
        public int star3 = 500;

        /// pre generate marmalade
        ///
        public bool enableVertBombs;

        public bool enableHorBombs;
        public bool enableSameColorBomb;
        public bool enablePackageBomb;
        public bool enableMarmalade;
        public int maxRows { get { return GetField().maxRows; } set { GetField().maxRows = value; } }
        public int maxCols { get { return GetField().maxCols; } set { GetField().maxCols = value; } }
        public int selectedTutorial;

        public int currentSublevelIndex;
        private TargetEditorScriptable targetEditorObject;
        private List<TargetContainer> targetEditorArray;
        /// target container keeps the object should be collected, its count, sprite, color
        public List<SubTargetContainer> subTargetsContainers = new List<SubTargetContainer>();

        public List<TargetCounter> TargetCounters;


        public FieldData GetField()
        {
            return fields[currentSublevelIndex];
        }

        public FieldData GetField(int index)
        {
            currentSublevelIndex = index;
            return fields[index];
        }

        public LevelData(bool isPlaying, int currentLevel)
        {
            hashCode = GetHashCode();
            levelNum = currentLevel;
            Debug.Log("Loaded " + hashCode);
            Name = "Level " + levelNum;
            THIS = this;
            LoadTargetObject();
            // if (isPlaying)
            //     targetEditorObject = LevelManager.This.targetEditorScriptable;
            // else
            //     targetEditorObject = AssetDatabase.LoadAssetAtPath("Assets/SweetSugar/Scriptable/TargetEditorScriptable.asset", typeof(TargetEditorScriptable)) as TargetEditorScriptable;

        }

        // public List<int> GetTargetContainerColor()
        // {
        //     
        // }
        public void LoadTargetObject()
        {
            targetEditorObject = Resources.Load("Levels/TargetEditorScriptable") as TargetEditorScriptable;
           // targetEditorObject.targets.Clear();
            //targetEditorObject.targets[0].defaultSprites[0].sprites[0].icon = LevelManager.THIS.TestSprite;ä
            for (int i = 0; i < targetEditorObject.targets[0].defaultSprites[0].sprites.Count; i++)
            {
                targetEditorObject.targets[0].defaultSprites[0].sprites[i].icon = LevelManager.THIS.TestSprite;
            }
         //  0].uiSprite = true;
       
//            Debug.Log("targetEditorObject.targets[0].defaultSprites[0].sprites.Count: " + targetEditorObject.targets[0].defaultSprites[0].sprites.Count);
            targetEditorArray = targetEditorObject.targets;

        }

        public Target GetTargetObject() => targetObject;

        public SquareBlocks GetBlock(int row, int col) => GetField().levelSquares[row * GetField().maxCols + col];

        public SquareBlocks GetBlock(Vector2Int vec) => GetBlock(vec.y, vec.x);

        public FieldData AddNewField()
        {
            var fieldData = new FieldData();
            fields.Add(fieldData);
            return fieldData;
        }

        public void RemoveField()
        {
            FieldData field = fields.Last();
            fields.Remove(field);
        }
        public Sprite[] GetTargetSprites()
        {
            return GetTargetContainersForUI().Where(i => i.extraObject && i.extraObject is Sprite).Select(i => (Sprite) i.extraObject).ToArray();
        }

        //Gets targets except Star, or only alone Star
        public TargetCounter[] GetTargetContainersForUI()
        {
            List<TargetCounter> list = TargetCounters;
            if (TargetCounters.Count > 1) list = TargetCounters.Where(i => !i.IsTargetStars()).ToList();
            return list.OrderBy(i=>i.count).ToArray();
        }

        public TargetCounter[] GetTargetCounters() => TargetCounters.ToArray();

        /// <summary>
        /// deprecated
        /// </summary>
        /// <returns></returns>
        public string[] GetTargetsNames() => targetEditorArray?.Select(i => i.name).ToArray();

        public TargetCounter[] GetTargetsByAction(CollectingTypes action) => TargetCounters.Where(i => i.collectingAction == action).ToArray();

        /// <summary>
        /// deprecated
        /// </summary>
        /// <returns></returns>
        public string[] GetSubTargetNames() => target.prefabs.Select(i => i.name).ToArray();

        public GameObject[] GetSubTargetPrefabs()
        {

            var layerBlockPrefabs = target.prefabs.Where(i => i.GetComponent<LayeredBlock>() != null).SelectMany(i => i.GetComponent<LayeredBlock>().layers).Select(i => i.gameObject).ToArray();
            var mergedList = layerBlockPrefabs.Concat(target.prefabs.Where(i => i.GetComponent<LayeredBlock>() == null)).ToArray();
            return mergedList;
        }
        public int GetTargetIndex()
        {
            var v = targetEditorArray.FindIndex(i => i.name == target.name);
            if (v < 0) { SetTarget(0); v = 0; }
            return v;
        }

        public TargetContainer GetTargetEditor() => targetEditorArray.Find(i => i.name == target.name);
        public TargetContainer GetTargetByNameEditor(string targetName) => targetEditorArray.Find(i => i.name == targetName);
        public bool IsTargetByNameExist(string targetName) => TargetCounters.Any(i => i.targetLevel.name == targetName);
        public bool IsTargetByActionExist(CollectingTypes action) => TargetCounters.Any(i => i.targetLevel.collectAction == action);
        public int GetTargetIndex(string targetName) => targetEditorArray.FindIndex(i => i.name == targetName);

        public TargetContainer GetFirstTarget(bool skipStars)
        {
            if (skipStars) return TargetCounters.Where(i => !i.IsTargetStars()).TryGetElement(0)?.targetLevel;
            return TargetCounters.First().targetLevel;
        }
        
        public void SetTargetFromArray()
        {
            SetTarget(targetEditorArray.FindIndex(x => x.name == target.name));
        }

        public void SetTarget(int index)
        {
            if (targetEditorObject == null || targetEditorArray == null) LoadTargetObject();
            subTargetsContainers.Clear();
            if (index < 0) index = 0;
            target = targetEditorArray[index];
            targetIndex = index;
            try
            {
                targetObject = (Target)Activator.CreateInstance(Type.GetType("Fruitmatch.Scripts.TargetScripts."+target.name));
                Debug.Log("create target " + targetObject);
                var subTargetPrefabs = GetSubTargetPrefabs();
                if(subTargetPrefabs.Length>1)
                {
                    foreach (var _target in subTargetPrefabs)
                    {
                        var component = _target.GetComponent<Item>();
                        Sprite extraObject = null;
                        if(component)
                        {
                            extraObject = component.sprRenderer.FirstOrDefault().sprite;
                        }
                        subTargetsContainers.Add(new SubTargetContainer(_target, 0, extraObject));
                    }
                }
                else if (subTargetPrefabs.Length > 0 && subTargetPrefabs[0].GetComponent<IColorableComponent>())
                {
                    foreach (var item in subTargetPrefabs[0].GetComponent<IColorableComponent>().GetSprites(levelNum))
                    {
                        subTargetsContainers.Add(new SubTargetContainer(subTargetPrefabs[0], 0, item));
                    }
                }
                else if (subTargetPrefabs.Length > 0)
                {
                    foreach (var _target in subTargetPrefabs)
                    {
                        var component = _target.GetComponent<Item>();
                        Sprite extraObject = null;
                        if(component)
                        {
                            extraObject = component.sprRenderer.FirstOrDefault().sprite;
                        }
                        subTargetsContainers.Add(new SubTargetContainer(_target, 0, extraObject));
                    }
                }
            }
            catch (Exception)
            {
                Debug.LogError("Check the target name or create class " + target.name);
            }

        }

        private TargetLevel GetTargetSprites(int level, TargetLevel targetLevel)
        {
//             level -= 1;
            List<ObjectiveSettings> objSettings = Rl.goalManager.listPhase1;
            //targetLevel.targets.Clear();
           // TargetList n = targetLevel.targets;
           
//             for (int i = 0; i < Rl.goalManager._goalsP1Queue.Count; i++)
//             {
//               //  targetLevel.targets[0].sprites[i].icon = n[i].goalSprite;
//
//               targetLevel.targets[0].sprites[i].icon = LevelManager.THIS.TestSprite;
//             }
           // targetLevel.targets[0].sprites[0].icon = LevelManager.THIS.TestSprite;
           for (int i = 0; i< targetLevel.targets.Count(); i++)
           {
             //  TargetObject p = new TargetObject();
              // p.targetType = new TargetType();
              // p.sprites.Add(new SpriteObject());
              targetLevel.targets[i].sprites[0].icon = Rl.world.GetGoalSprite(objSettings[0].PhaseGoalArray[i].GoalFruit);
               
           }
          // targetLevel.targets[0].sprites[0].icon =  Rl.world.GetGoalSprite(objSettings[0].PhaseGoalArray[0].GoalFruit);
            return targetLevel;
        }

        private TargetLevel TrimUnusedTargets(TargetLevel targetLevel)
        {
            for (int i = 0; i < targetLevel.targets.Count; i++)
            {
                if (targetLevel.targets[i].sprites[0].icon == Rl.world.GetGoalSprite(FruitType.Nothing))
                    targetLevel.targets.RemoveAt(i);
            }

            return targetLevel;
        }
        public void InitTargetObjects(LIMIT limitType, bool forPlay = false)
        {
            if(forPlay)
            {
                TargetLevel targetLevel = Resources.Load<TargetLevel>("Levels/Targets/TargetLevel1");
               targetLevel = GetTargetSprites(LoadingManager.CurrentLevelToload, targetLevel);
               targetLevel = TrimUnusedTargets(targetLevel);
               
            //   targetLevel.targets[0].sprites[0].icon = LevelManager.THIS.TestSprite;

         
               
                List<TargetCounter> _TargetCounters = targetLevel.targets.Select(i 
                    => new TargetCounter(i.targetType.GetTarget().prefabs.FirstOrDefault(), i.CountDrawer.count, i.sprites
                .Select(o => o.icon).ToArray(), -1, i.targetType.GetTarget(),i.NotFinishUntilMoveOut, i)).ToList();
                
                TargetCounters = _TargetCounters.ToList();//subTargetsContainers.ToArray();
                TargetCounters.RemoveAll(i => i.targetLevel.setCount == SetCount.Manually && i.count == 0);
                PhaseGoal[] Goals = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs.LevelConfigs[LoadingManager.CurrentLevelToload-1].GoalConfig.PhaseGoalsList[0].ObjectiveSettingsArray[0].PhaseGoalArray;

                
                if (GenericFunctions.IsSubstractiveState(limitType))
                {
                    for (int i = 0; i < _TargetCounters.Count; i++)
                    {
                        _TargetCounters[i].count = Mathf.Abs((int)Goals[i].CollectionAmount);
                    
                        _TargetCounters[i].MaxCount = Mathf.Abs((int)Goals[i].CollectionAmount);
                      
                        LevelManager.THIS.TargetCollectionStyle.Add(Goals[i].CollectionStyle);
                    }
                }
                else
                {
                    for (int i = 0; i < _TargetCounters.Count; i++)
                    {
                        _TargetCounters[i].count = Int32.MaxValue;
                        LevelManager.THIS.TargetCollectionStyle.Add(Goals[i].CollectionStyle);
                    }
                }
            
              
            }
            targetObject.subTargetContainers = subTargetsContainers.ToArray();
            if (targetObject.subTargetContainers.Length > 0)
                targetObject.InitTarget(this);
            else Debug.LogError( "set " + target.name + " more than 0" );
        }

        public void SetItemTarget(Item item)
        {
            foreach (var _subTarget in TargetCounters)
            {
                if (_subTarget.targetPrefab && item.CompareTag(_subTarget.targetPrefab.tag) && _subTarget.count > 0 && item.gameObject.GetComponent<TargetComponent>() == null)
                {
                    item.gameObject.AddComponent<TargetComponent>();
                }
            }
        }

        public void SetSquareTarget(GameObject gameObject, SquareTypes _sqType, GameObject prefabLink)
        {
            // if (_sqType.ToString().Contains(target.name))
            {
                var subTargetContainer = TargetCounters.FirstOrDefault(i => i.targetPrefab != null && _sqType.ToString() == i.targetPrefab.name);
                if (subTargetContainer != null)
                {
                    subTargetContainer.changeCount(1);
                    gameObject.AddComponent<TargetComponent>();
                }
            }
        }

        public string GetSaveString()
        {
            var str = "";
            foreach (var item in subTargetsContainers)
            {
                str += item.GetCount() + "/";
            }
            return str;
        }

        public LevelData DeepCopy(int level)
        {
            LoadTargetObject();
            var other = (LevelData)MemberwiseClone();
            other.hashCode = other.GetHashCode();
            other.levelNum = level;
            other.Name = "Level " + other.levelNum;
            other.fields = new List<FieldData>();
            for (var i = 0; i < fields.Count; i++)
            {
                other.fields.Add(fields[i].DeepCopy());
            }
            if (targetEditorArray.Count > 0)
                other.target = targetEditorArray.First(x => x.name == target.name);//target.DeepCopy();
            else
                other.target = target.DeepCopy();
            if (targetObject != null)
                other.targetObject = targetObject.DeepCopy();
            other.subTargetsContainers = new List<SubTargetContainer>();
            for (var i = 0; i < subTargetsContainers.Count; i++)
            {
                other.subTargetsContainers.Add(subTargetsContainers[i].DeepCopy());
            }

            other.targetObject = (Target)Activator.CreateInstance(Type.GetType("FruitMatch.Scripts.TargetScripts."+target.name));
            return other;
        }

        public LevelData DeepCopyForPlay(int level)
        {
            LevelData data = DeepCopy(level);
            THIS = data;
            return data;
        }

        public bool IsTotalTargetReached()
        {
            return TargetCounters.All(i => i.IsTotalTargetReached());
        }
        
        public bool IsTargetReachedSublevel()
        {
            return TargetCounters.All(i => i.IsTargetReachedSublevel());
        }

        public bool WaitForMoveOut() => TargetCounters.Any(i => i.NotFinishUntilMoveOut);

        public void CheckLayers()
        {
            foreach (var fieldData in fields) fieldData.ConvertToLayered();
        }
    }

    /// <summary>
    /// Field data contains field size and square array
    /// </summary>
    [Serializable]
    public class FieldData
    {
        public int maxRows;
        public int maxCols;
        public bool noRegenLevel;
        public bool noMatches; //no regenerate level if no matches possible
        public SquareBlocks[] levelSquares = new SquareBlocks[81];
        public int bombTimer = 15;
        public int layers;
        
        public FieldData DeepCopy()
        {
            var other = (FieldData)MemberwiseClone();
            other.levelSquares = new SquareBlocks[levelSquares.Length];
            for (var i = 0; i < levelSquares.Length; i++)
            {
                other.levelSquares[i] = levelSquares[i].DeepCopy();
            }
            
            return other;
        }

        public void ConvertToLayered()
        {
            if (layers != 0) return;
            foreach (var squareBlockse in levelSquares)
            {
                if (squareBlockse.blocks.Any() || squareBlockse.block == SquareTypes.NONE) continue;
                squareBlockse.blocks.Add(new SquareTypeLayer {squareType = SquareTypes.EmptySquare});
                AddLayers(squareBlockse, squareBlockse.block, squareBlockse.blockLayer);
                AddLayers(squareBlockse, squareBlockse.obstacle, squareBlockse.obstacleLayer);
            }
            layers = levelSquares.Max(i => i.blocks.Count);
        }

        private static void AddLayers(SquareBlocks squareBlockse, SquareTypes type, int layers)
        {
            if (type == SquareTypes.NONE || type == SquareTypes.EmptySquare) return;
            for (int i = 0; i < layers; i++) squareBlockse.blocks.Add(new SquareTypeLayer {squareType = type});
        }
    }

    /// <summary>
    /// Square blocks uses in editor
    /// </summary>
    [Serializable]
    public class SquareBlocks
    {
        public List<SquareTypeLayer> blocks = new List<SquareTypeLayer>();
        public SquareTypes block;
        public int blockLayer = 1;
        public SquareTypes obstacle;
        public int obstacleLayer = 1;
        public Vector2Int position;
        public Vector2 direction;
        public bool enterSquare;
        public bool isEnterTeleport;
        public Vector2Int teleportCoordinatesLinked = new Vector2Int(-1, -1);
        public Vector2Int teleportCoordinatesLinkedBack = new Vector2Int(-1, -1);
        public Rect guiRect;
        public ItemForEditor item;

        public SquareBlocks DeepCopy()
        {
            var other = (SquareBlocks)MemberwiseClone();
            return other;
        }

        public void MergeEmptySquares()
        {
            if (blocks.Last().squareType == SquareTypes.EmptySquare)
            {
                for (int i = blocks.Count - 1; i >= 1; i--)
                {
                    if (blocks[i].squareType != SquareTypes.EmptySquare) break;
                    else blocks.RemoveAt(i);
                }
            }
        }

        public void SortMergeBlocks()
        {
            var array = Resources.LoadAll<BindLayer>("Blocks");
            var list = array.OrderBy(i => i.order).ToList();
            var blocks1 = blocks.Where(i=>i.squareType != SquareTypes.NONE).Select(i => new {block = i, layered = list.Find(x => x.name == i.squareType.ToString()).GetComponent<LayeredBlock>()
                                                                                                                    != null});
            var b1 = blocks1.Where(i => i.layered == false).DistinctBy(i=>i.block.squareType);
            var b2 = blocks1.Where(i => i.layered);
            blocks = b1.Concat(b2).Select(i=>i.block).ToList();
            blocks.Sort((a,b) =>
            {
                if (list.FindIndex(i => i.name == a.squareType.ToString()) < list.FindIndex(i => i.name == b.squareType.ToString()))
                    return -1;
                else if (list.FindIndex(i => i.name == a.squareType.ToString()) == list.FindIndex(i => i.name == b.squareType.ToString()))
                    return 0;
                return 1;
            });
        }
        
    }


    [Serializable]
    public struct SquareTypeLayer
    {
        public SquareTypes squareType;
        //for big targets, not draw texture in editor
        public bool anotherSquare;
        public Vector2Int originalPos;
        public bool rotate;
        public Vector2Int size;

        public SquareTypeLayer(SquareTypes squareType, bool anotherSquare, Vector2Int originalPos, bool rotate, Vector2Int size)
        {
            this.squareType = squareType;
            this.anotherSquare = anotherSquare;
            this.originalPos = originalPos;
            this.rotate = rotate;
            this.size = size;
        }
        public SquareTypeLayer(SquareTypes squareType)
        {
            this.squareType = squareType;
            this.anotherSquare = false;
            this.originalPos  = new Vector2Int(0, 0);
            this.rotate = false;
            this.size = new Vector2Int(0, 0);
        }
    }

    /// <summary>
    /// Item for editor uses in editor
    /// </summary>
    [Serializable]
    public class ItemForEditor
    {
        public int Color;
        public ItemsTypes ItemType;
        public Texture2D Texture;
        public IColorableComponent colors;
        public IItemInterface ItemInterface;
        public GameObject Item;
        public bool EnableMarmaladeTargets;
        public Vector2Int[] TargetMarmaladePositions;
        public int order;
        public ItemForEditor DeepCopy()
        {
            var other = (ItemForEditor)MemberwiseClone();
            return other;
        }

        public void SetColor(int color, int currentLevel)
        {
            Color = color;
            if(colors && colors.GetSprites(currentLevel).Count() > color)
                Texture = colors.GetSprites(currentLevel)[color].texture;
        }
    }
}
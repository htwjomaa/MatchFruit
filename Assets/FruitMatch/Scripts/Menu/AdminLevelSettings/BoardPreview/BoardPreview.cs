using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class BoardPreview : MonoBehaviour
{
  [SerializeField] public Sprite tileBackgroundBright;
  [SerializeField] public Sprite tileBackgroundDark;
  [SerializeField] public GameObject tilePrefab;
  [SerializeField] public int offSet = 15;
  private BackgroundTile[,] allTiles;
  public RectTransform RectTransform;

  private int _lastClickedRow = int.MaxValue;
  private int _lastClickedColumn = int.MaxValue;
  public static Transform boardPreviewSideDot;
  [SerializeField] public List<GameObject> tiles;
  public delegate void DrawSideboards();
  public static event DrawSideboards DrawSideBoardsEvent;
  
  public delegate void SideDotBoardTransform();
  public static event SideDotBoardTransform SideDotBoardTransformEvent;

  public float distanceT1T2X = 311f;
  public void FadeIn() => Rl.fadeControllerSplashMenu.FadeInPreviewBoard(SplashMenu.PanelOnLeft);

  public void FadeOut() => Rl.fadeControllerSplashMenu.FadeOutPreviewBoard();
  public void MoveSide() => Rl.fadeControllerSplashMenu.PreviewBoardSideMove(SplashMenu.PanelOnLeft);
  
  [SerializeField] private GameObject Outline;

  private void Awake()
  {
    RectTransform = GetComponent<RectTransform>();
    ResetOutlineAroundOneTile();
  }

  private bool ClickedAgain(int column, int row)
  {
    bool mih = false || column == _lastClickedColumn && row == _lastClickedRow;

    if (mih)
    {
      _lastClickedColumn = int.MaxValue;
      _lastClickedRow = int.MaxValue;
    }
    else
    {
      _lastClickedColumn = column;
      _lastClickedRow = row;
    }
   
    return mih;
  }

  public void ResetLastClicked()
  {
    _lastClickedColumn = int.MaxValue;
    _lastClickedRow = int.MaxValue;
    boardPreviewSideDot = null;
  }
  public void DrawOutlineAroundOneTile(int row, int column, bool isSideDotBoard = false)
  {
    Transform parent = this.transform;
    if (isSideDotBoard)
    {
      SideDotBoardTransformEvent?.Invoke();
      if(boardPreviewSideDot != null) parent = boardPreviewSideDot;
    }

    if (isSideDotBoard)
    {
      switch (Rl.adminLevelSettingsSideDots.CurrentSideDotSettingButton.direction)
      {
        case Directions.left or Directions.right:
          if (column > (int)Rl.adminLevelSettingsBoard.BoardHeightSlider.value - 1)
          {
            _drawBoard = true;
            ResetLastClicked();
            return;
          }
          break;
        case Directions.bottom or Directions.top:
          if (column > (int)Rl.adminLevelSettingsBoard.BoardWidthSlider.value - 1)
          {
            _drawBoard = true;
            ResetLastClicked();
            return;
          }
          break;
      }
  
    }
    else
    {
      if (column > (int)Rl.adminLevelSettingsBoard.BoardWidthSlider.value - 1 ||
          row > (int)Rl.adminLevelSettingsBoard.BoardHeightSlider.value - 1)
      {
        _drawBoard = true;
        ResetLastClicked();
        return;
      }

    }
   
    if (ClickedAgain(column, row)) _drawBoard = true;
    else
    {
      Transform child;
      if (isSideDotBoard)
      {
        child = parent.GetChild(column);
      }
      else child = parent.GetChild(column + row * ((int)Rl.adminLevelSettingsBoard.BoardWidthSlider.value));

      Outline.transform.position = child.transform.position;
      _cashedChildTransform = child;
      DimmOthers(child, parent); 
      ShowChild( _cashedChildTransform);
    }
  }

  private Transform _cashedChildTransform;

  private void ShowChild(Transform activeChild)
  {
    activeChild.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
    if(activeChild.GetChild(0).GetComponent<Image>().sprite != null) activeChild.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 1f);
  
  }
  private void ResetAllColors()
  {
    ResetOutlineAroundOneTile();
    for (int i = 0; i < transform.childCount; i++)
      transform.GetChild(i).GetComponent<Image>().color = new Color(255, 255, 255, 1f);
  }
  private static void DimmOthers(Transform child, Transform parent)
  {
    for (int i = 0; i < parent.childCount; i++)
    {
      parent.GetChild(i).GetComponent<Image>().color = new Color(127, 127, 255, 0.1f);
        if(parent.GetChild(i).GetChild(0).GetComponent<Image>().sprite != null) parent.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(127, 127, 255, 0.1f);
    }
  }

  private void ResetOutlineAroundOneTile() => Outline.transform.position = new Vector3(9999, 9999, 9999);

  public void RemoveBoardPreviewListeners() => GenericSettingsFunctions.RemoveListeners(
    Rl.adminLevelSettingsBoard.BoardHeightSlider,
    Rl.adminLevelSettingsBoard.BoardWidthSlider);
  private bool _drawBoard;


  public void StartDrawBoard(float timer = 0.025f) => StartCoroutine(LateUpdateDraw_CO(timer));

  IEnumerator LateUpdateDraw_CO(float time)
  {
    yield return new WaitForSeconds(time);
    _drawBoard = true;
  }

  private void LateUpdate()
  {
    if (!_drawBoard) return;
    _drawBoard = false;
    DestroyAllBoardChildren();
    DrawGrid((int)Rl.adminLevelSettingsBoard.BoardWidthSlider.value, (int)Rl.adminLevelSettingsBoard.BoardHeightSlider.value);
    DrawSideBoardsEvent?.Invoke();
    KeepFocus();
  }

  public void DrawGrid(int width, int height)
   {
     ResetOutlineAroundOneTile();
     allTiles = new BackgroundTile[width, height];
     float scaleFactor = GetScaleFactor(width, height);
     int offSet = GetOffset(width, height);
     tiles?.Clear();
    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        //if (!blankSpaces[i, j])
       // {
          Vector2 tempPosition = new Vector2(i, j + offSet);
        BoardInstantiate.InstantiateTile(tempPosition , i, j, Directions.none, tilePrefab, ref allTiles,
          tileBackgroundBright, tileBackgroundDark, this.transform);
        ModifyToUI(allTiles[i, j].transform.gameObject, scaleFactor, i, j, offSet, ref tiles);
       
        //}
      }
      this.GetComponent<GridLayoutGroup>().constraintCount = width;
    }
    SetTileSprite(ref tiles, width, tileBackgroundBright, tileBackgroundDark);
    distanceT1T2X = Mathf.Abs(transform.GetChild(1).GetComponent<RectTransform>().localPosition.x - transform.GetChild(0).GetComponent<RectTransform>().localPosition.x);
    if (distanceT1T2X == 0) distanceT1T2X = 311f;
    InstantiateChilds(tiles, 0.86f);
    DrawChildSprites(width, height, Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs, Rl.adminLevelSettingsTiles.CurrentTileKind,
      tiles);
   }

  
  public void InstantiateChilds(List<GameObject> tiles, float childSize)
   {
     for (int i = 0; i < tiles.Count; i++)
     {
       Destroy(tiles[i].GetComponent<BackgroundTile>());
       
       GameObject child = new GameObject();
       child.AddComponent<Button>();
       child.AddComponent<BoardPreviewField>();
       child.name = tiles[i].name + " - Item";
       child.transform.position = tiles[i].transform.position;
       child.transform.parent = tiles[i].transform;
       child.AddComponent<Image>().color = new Color(0f,0f,0f, 0f );
       child.transform.localScale = new Vector3(3f,3.1f,3f);   //FIXED SIZE FOR NOW BECAUSE IT BUGS
      // child.transform.localScale = new Vector3(child.transform.localScale.x*childSize*1.02f,child.transform.localScale.y * childSize, child.transform.localScale.z * childSize);
     }
   }

  struct TileSettingsWithExtra
  {
    public TileSettingConfig TileSettingConfig;
    public sbyte TeleportFrom;

    public TileSettingsWithExtra(TileSettingConfig tileSettingConfig, sbyte teleportFrom)
    {
      TileSettingConfig = tileSettingConfig;
      TeleportFrom = teleportFrom;
    }
  }
  private void DrawChildSprites(int maxRow, int maxColumn, TileSettingConfig[] tileSettingConfigs, TKind tKind, List<GameObject> tiles)
   {
     // Weed out the stuff we don't draw   alternative Way: Get make a loop for the missing numbers and add all multipliers to an byte array and compare it to i
     int counterRow = 0;
     int counterColumn = 0;

     HashSet<int> teleportFromList = new HashSet<int>();

     for (int i = 0; i < tileSettingConfigs.Length; i++)
     {
       if (tileSettingConfigs[i].TeleportTarget != -1)
       {
         teleportFromList.Add(tileSettingConfigs[i].TeleportTarget);
       }
     }
     
     List<TileSettingsWithExtra> TileSettingsWithExtra = new List<TileSettingsWithExtra>();

     for (int i = 0; i < tileSettingConfigs.Length; i++)
     {
       sbyte tel = -128;
       if (teleportFromList.Contains(i))
       {
         tel = (sbyte)i;
       }
       TileSettingsWithExtra.Add(new TileSettingsWithExtra(tileSettingConfigs[i], tel));
     }
     
     List<TileSettingsWithExtra> helperList = new List<TileSettingsWithExtra>(81);
     List<TileSettingsWithExtra> cleanedList = new List<TileSettingsWithExtra>(81);
     
     

     Debug.Log("teleportFromList " + teleportFromList.Count);
     Debug.Log("---------------------");
     foreach (var VARIABLE in teleportFromList)
     {
       Debug.Log("TELEPORTFROM: " + VARIABLE);
     }
     //List<TeleportTuple> columnValid = new List<TeleportTuple>();

     List<int> columnIntValidTel = new List<int>();
     List<int> defaultMih = new List<int>();
     for (int i = 0; i <  TileSettingsWithExtra.Count; i++)
     {
       if (counterColumn > 8) counterColumn = 0;
       if (counterColumn < maxColumn)
       {
         helperList.Add(TileSettingsWithExtra[i]);
          // if (teleportFromList.Contains(i))
          // {
          //   columnIntValidTel.Add(128);
          //   defaultMih.Add(i);
          // }
          // else
          // {
          //   columnIntValidTel.Add(-128);
          // }
       }
       counterColumn++;
     }
     
     List<int> rowIntValidTel = new List<int>();
     for (int i = 0; i < helperList.Count; i++)
     {
       if (counterRow > 8) counterRow= 0;
       if (counterRow < maxRow)
       {
         cleanedList.Add(TileSettingsWithExtra[i]);
       }
       counterRow++;
     }

     counterRow = 0;
     //Debug.Log("columnIntValidTelC_: " + columnIntValidTel + "|| helperList.Count_: " + helperList.Count);
     // for (int i = 0; i < columnIntValidTel.Count; i++)
     // {
     //   if (counterRow > 8) counterRow= 0;
     //   if (counterRow < maxRow)
     //   {
     //     rowIntValidTel.Add(columnIntValidTel[i]);
     //   }
     //   counterRow++;
     // }

     switch (tKind)
     {
       case TKind.Tiles:
         for (int i = 0; i < cleanedList.Count; i++)
         {
           switch (cleanedList[i].TileSettingConfig.TileType)
           {
             case TT.Normal:
               break;
             
             default:
               tiles[i].transform.GetChild(0).GetComponent<Image>().sprite = Rl.adminLevelSettingsTiles.TTypeSprites[(int)cleanedList[i].TileSettingConfig.TileType];
               tiles[i].transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 1);
               break;
           }
         }
         break;
       
       case TKind.Vectors:
         for (int i = 0; i < cleanedList.Count; i++)
         {
           if (EmptyTilesForDirTel(cleanedList[i].TileSettingConfig.TileType))
           {
             tiles[i].transform.GetChild(0).GetComponent<Image>().sprite = Rl.adminLevelSettingsTiles.TTypeSprites[(int)cleanedList[i].TileSettingConfig.TileType];
             var newChild = Instantiate(tiles[i].transform.GetChild(0), tiles[i].transform.GetChild(0), true);
             newChild.GetComponent<Image>().sprite = Rl.adminLevelSettingsTiles.DirectionSprites[GetDirectionSprite(cleanedList[i].TileSettingConfig.Direction, cleanedList[i].TileSettingConfig.IsDirectionStart)];
             newChild.GetComponent<Image>().color = new Color(127, 127, 255, 0.14f);
           }
           else
           {
             tiles[i].transform.GetChild(0).GetComponent<Image>().sprite = Rl.adminLevelSettingsTiles.DirectionSprites[GetDirectionSprite(cleanedList[i].TileSettingConfig.Direction, cleanedList[i].TileSettingConfig.IsDirectionStart)];
           }
           tiles[i].transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 1);
         }
         break;

       case TKind.Teleports:
         for (int i = 0; i < cleanedList.Count; i++)
         {
           if (EmptyTilesForDirTel(cleanedList[i].TileSettingConfig.TileType))
           {
             //make empty tile
             tiles[i].transform.GetChild(0).GetComponent<Image>().sprite =
               Rl.adminLevelSettingsTiles.TTypeSprites[(int)cleanedList[i].TileSettingConfig.TileType];
             tiles[i].transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 1);
           }
           else
           {
             switch (cleanedList[i].TileSettingConfig.TeleportTarget)  //if it has a teleport target
             {
               case -1:
                 if (cleanedList[i].TeleportFrom != -128)
                 {
                   tiles[i].transform.GetChild(0).GetComponent<Image>().sprite =
                     Rl.adminLevelSettingsTiles.TeleportSprites[2];
                   tiles[i].transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 1);
                 }
                 break;

               default:
                
                   tiles[i].transform.GetChild(0).GetComponent<Image>().sprite = Rl.adminLevelSettingsTiles.TeleportSprites[1];
                   tiles[i].transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 1);
                   break;
             }
           
           }
          
         }
         break;
     }
   }

  private void DrawTeleportLine(int fromInt, GameObject currentObj)
  {
    
  }
   [Serializable]
   public struct TeleportTuple
   {
     public sbyte TeleportFrom;
     public sbyte TeleportTo;

     public TeleportTuple(sbyte teleportFrom, sbyte teleportTo)
     {
       TeleportFrom = teleportFrom;
       TeleportTo = teleportTo;
     }
   }
   public bool EmptyTilesForDirTel(TT TileType)
   {
     if (TileType == TT.EmptyTile) return true;
     return false;

   }
   public int GetDirectionSprite(Directions directions, bool isDirectionStart = false)
   {
     //For some reason the Sprites get drawn wrong and need correction
     if (!isDirectionStart)
     {
       switch (directions)
       {
         case Directions.top:
           return 0;
         case Directions.bottom:
           return 1;
         case Directions.left:
           return 2;
         case Directions.right:
           return 3;
       }
     }
     else
     {
       switch (directions)
       {
         case Directions.top:
           return 4;
         case Directions.bottom:
           return 5;
         case Directions.left:
           return 6;
         case Directions.right:
           return 7;
       }
     }
     return 0;
   }

   public static void SetTileSprite(ref List<GameObject> tiles, int width,Sprite tileBackgroundBright, Sprite tileBackgroundDark)
   {
     Queue<GameObject> tilesQueue = new Queue<GameObject>();

     foreach (var n in tiles) tilesQueue.Enqueue(n);
     
     bool _change = true;
     int counter = 0;
     
     for (int i = 0; i < tiles.Count; i++)
     {
       if (counter == width && width%2 == 0)
       {
         _change = !_change;
         counter = 0;
       }
       if (_change) tilesQueue.Dequeue().GetComponent<Image>().sprite = tileBackgroundBright;
       else tilesQueue.Dequeue().GetComponent<Image>().sprite = tileBackgroundDark;
       _change = !_change;
       counter++;
     }
   }


   public static void ModifyToUI(GameObject tile, float scaleFactor, int currentWidth, int currentHeight, int offset,ref List<GameObject> tiles)
   {
     tile.AddComponent<Image>();
     tile.GetComponent<Image>().sprite = tile.GetComponent<SpriteRenderer>().sprite;
     tile.AddComponent<Button>();
     tile.GetComponent<Button>().image = tile.GetComponent<Image>();
     Destroy(tile.GetComponent<SpriteRenderer>());
     //Destroy(tile.GetComponent<BackgroundTile>());
     tiles.Add(tile);
     
     tile.GetComponent<RectTransform>().sizeDelta = new Vector2(scaleFactor , scaleFactor );
     tile.GetComponent<RectTransform>().position = new Vector3(tile.GetComponent<RectTransform>().position.x+ (currentWidth*(scaleFactor + offset)), 
       tile.GetComponent<RectTransform>().position.y + (currentHeight*(scaleFactor + offset)), tile.GetComponent<RectTransform>().position.z);
   }

  [SerializeField] public List<PreviewBoardTileSettings> PreviewBoardTileSettingsList = new List<PreviewBoardTileSettings>();

  public float GetScaleFactor(int width, int height)
  {
    float scaleFactor = 0;
    for (int i = 0; i < PreviewBoardTileSettingsList.Count; i++)
    {
      if (height == PreviewBoardTileSettingsList[i].Height && width == PreviewBoardTileSettingsList[i].Width)
      {
        scaleFactor = PreviewBoardTileSettingsList[i].ScaleFactor;
        return scaleFactor;
      }
    }

    return scaleFactor;
  }

  public int GetOffset(int width, int height)
  {
    int offSet = 0;
    for (int i = 0; i < PreviewBoardTileSettingsList.Count; i++)
    {
      if (height == PreviewBoardTileSettingsList[i].Height && width == PreviewBoardTileSettingsList[i].Width)
      {
        offSet = PreviewBoardTileSettingsList[i].OffSet;
        return offSet;
      }
    }

    return offSet;
  }

  private Vector2 CalculateTravelDistance()
  {
    Vector3 thisPosition = this.gameObject.transform.position;
    Vector3 midPointTiles =  GenericSettingsFunctions.FindTheMidPoint(tiles.ToArray());
    float moveToHor = Mathf.Abs(midPointTiles.x - thisPosition.x);
    float moveToVert = Mathf.Abs(midPointTiles.y - thisPosition.y);

    return new Vector2(moveToHor, moveToVert);
  }

 [Button()] public void DestroyAllBoardChildren() =>  DestroyAllChildrenWithImageFirst(this.gameObject);
 
 public static void DestroyAllChildrenWithImageFirst(GameObject parent)
 {
   for (int i = 0; i < parent.transform.childCount; i++)
   {
    if(parent.transform.GetChild(i).GetComponent<Image>() !=null) Destroy(parent.transform.GetChild(i).GetComponent<Image>());
     Destroy(parent.transform.GetChild(i).gameObject);
   }
 }


 private int _cashedLastClickedRow;
 private int _cashedLastClickedColumn;
 public bool keepFocus;
 private void KeepFocus()
 {
   if (!keepFocus) return;
   keepFocus = false;

   _cashedLastClickedColumn = _lastClickedColumn;
   _cashedLastClickedRow = _lastClickedRow;
   ResetLastClicked();
  // if(boardPreviewSideDot != null) 
     //DrawOutlineAroundOneTile(_cashedLastClickedColumn, _cashedLastClickedRow, boardPreviewSideDot );
  // else  
   DrawOutlineAroundOneTile(_cashedLastClickedRow, _cashedLastClickedColumn);
 }
}

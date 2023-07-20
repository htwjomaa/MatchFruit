using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
public sealed class BoardPreviewSideDot : MonoBehaviour
{
  [SerializeField] public Directions direction;
   private BackgroundTile[,] _allTiles;
    private int _lastClickedRow = int.MaxValue;
    private int _lastClickedColumn = int.MaxValue;
    [SerializeField] private float _cashedXPos, _cashedYPos;
    [SerializeField] public List<GameObject> tiles;
    [SerializeField] private GameObject Outline;
    private bool _drawBoard;
    private void ResetOutlineAroundOneTile() => Outline.transform.position = new Vector3(9999, 9999, 9999);
    private void Start()
    {
      _cashedXPos = GetComponent<RectTransform>().localPosition.x;
      _cashedYPos = GetComponent<RectTransform>().localPosition.y;
      BoardPreview.DrawSideBoardsEvent += StartDrawBoard;
      BoardPreview.SideDotBoardTransformEvent += SetAsParent;
    }

    private void SetAsParent()
    {
      if (direction == GetDirectionFromByte(Rl.adminLevelSettingsSideDots.CurrentBar))
        BoardPreview.boardPreviewSideDot = transform;
    }

    private Directions GetDirectionFromByte(byte id)
    {
      switch (id)
      {
        case 0: return Directions.bottom;
        case 1: return Directions.top;
        case 2: return Directions.right;
        case 3: return Directions.left;
      }

      return Directions.none;
    }
    private void OnDestroy()
    {
      BoardPreview.DrawSideBoardsEvent -= StartDrawBoard;
      BoardPreview.SideDotBoardTransformEvent -= SetAsParent;
    }

    public void DrawGrid(int width, int height)
    {
   
      int cashWidth = width;
      int cashHeight = height;
      int missingWidth = 9-width;
      int missingHeight = 9 -height;
      
      if (direction is Directions.left or Directions.right) width = 1;
      else height = 1;
      switch (direction)
      {
        case Directions.left:
          if (!Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].RightActive) return;
          GetComponent<RectTransform>().localPosition= new Vector2(_cashedXPos - Rl.BoardPreview.distanceT1T2X*missingWidth/1.85f, _cashedYPos); 
          break;
        case Directions.right:
          if (!Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].LeftActive) return;
          GetComponent<RectTransform>().localPosition= new Vector2(_cashedXPos + Rl.BoardPreview.distanceT1T2X*missingWidth/1.85f, _cashedYPos); 
          break;
        case Directions.bottom:
          if (!Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].TopActive) return;
          GetComponent<RectTransform>().localPosition= new Vector2(_cashedXPos, _cashedYPos - Rl.BoardPreview.distanceT1T2X*missingHeight/3.2f); 
          break;
        case Directions.top:
          if (!Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].BottomActive) return;
          GetComponent<RectTransform>().localPosition= new Vector2(_cashedXPos, _cashedYPos + Rl.BoardPreview.distanceT1T2X*missingHeight/3.2f); 
          break;
      }
      ResetOutlineAroundOneTile();
     _allTiles = new BackgroundTile[width, height];
     float scaleFactor = Rl.BoardPreview.GetScaleFactor(width, height);
     int offSet = Rl.BoardPreview.GetOffset(width, height);
     tiles?.Clear();
    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
    
          Vector2 tempPosition = new Vector2(i, j + offSet);
        BoardInstantiate.InstantiateTile(tempPosition , i, j, Directions.none, Rl.BoardPreview.tilePrefab, ref _allTiles,
           Rl.BoardPreview.tileBackgroundDark, Rl.BoardPreview.tileBackgroundBright,transform);
        BoardPreview.ModifyToUI(_allTiles[i, j].transform.gameObject, scaleFactor, i, j, offSet, ref tiles);
      }
      GetComponent<GridLayoutGroup>().constraintCount = 9;
    }
    if (direction is Directions.bottom && missingHeight % 2 == 1 || direction is Directions.left && missingWidth % 2 == 1 )
      BoardPreview.SetTileSprite(ref tiles, width,  Rl.BoardPreview.tileBackgroundBright, Rl.BoardPreview.tileBackgroundDark);
    else BoardPreview.SetTileSprite(ref tiles, width,  Rl.BoardPreview.tileBackgroundDark, Rl.BoardPreview.tileBackgroundBright);
    Rl.BoardPreview.InstantiateChilds(tiles, 0.87f);
    DrawChildSprites(cashWidth , cashHeight, Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].SideFruitsSettings.ToArray(), direction,tiles);
   }

  private void DrawChildSprites(int maxRow, int maxColumn, SideFruitsSetting[] sideFruitSettingConfigs, Directions direction, List<GameObject> tiles)
   {
     // Weed out the stuff we don't draw   alternative Way: Get make a loop for the missing numbers and add all multipliers to an byte array and compare it to i
     switch (direction)
     {
       case Directions.bottom:
         sideFruitSettingConfigs  = sideFruitSettingConfigs[..9]; 
         break;
       case Directions.top:
         sideFruitSettingConfigs  = sideFruitSettingConfigs[9..18]; 
         break;
       case Directions.right:
         sideFruitSettingConfigs  = sideFruitSettingConfigs[18..27]; 
         break;
       case Directions.left:
         sideFruitSettingConfigs  = sideFruitSettingConfigs[27..]; 
         break;
     }
     
     sideFruitSettingConfigs = direction is Directions.bottom or Directions.top ? sideFruitSettingConfigs[0..maxRow] : sideFruitSettingConfigs[..maxColumn];
     for (int i = 0; i < sideFruitSettingConfigs.Length; i++)
     {
       switch (sideFruitSettingConfigs[i].SideDotTile)
       {
         case SideDotTile.Normal:
           break;

         default:
           tiles[i].transform.GetChild(0).GetComponent<Image>().sprite =
             Rl.adminLevelSettingsSideDots.SideDotSpriteList[(int)sideFruitSettingConfigs[i].SideDotTile];
           tiles[i].transform.GetChild(0).GetComponent<RectTransform>().localRotation = RotateSideDotChild(direction,
             tiles[i].transform.GetChild(0).GetComponent<RectTransform>().transform);
           tiles[i].transform.GetChild(0).GetComponent<RectTransform>().localScale = ScaleSideDotChild(direction,
             tiles[i].transform.GetChild(0).GetComponent<RectTransform>().transform);
         
    
           tiles[i].transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 1);
           break;
       }
     }
   }
  public static Quaternion RotateSideDotChild(Directions direction, Transform transform)
  {
    Quaternion newRotation = transform.rotation;

    switch (direction)
    {
      case Directions.bottom:
        newRotation.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y,
          -90);
        break;
        case Directions.top:
          newRotation.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y,
            -90);
          break;
    }
    
    return newRotation;
  }
  

  public static Vector3 ScaleSideDotChild(Directions direction, Transform transform)
  {
    if (direction is Directions.right or Directions.bottom) 
      return new Vector3(transform.localScale.x*-1, transform.localScale.y, transform.localScale.z);

    return transform.localScale;
  }
   [Button()] public void DestroyAllBoardChildren() =>  BoardPreview.DestroyAllChildrenWithImageFirst(this.gameObject);
 [Button]public void StartDrawBoard() => StartCoroutine(LateUpdateDraw_CO(0.025f));

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
 }
}
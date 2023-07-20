using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FruitMatch.Scripts.Blocks;
using FruitMatch.Scripts.Items;
using FruitMatch.Scripts.System.Pool;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

public sealed class BoardInstantiate : MonoBehaviour
{
    public static ItemsTypes TranslateSideDotTileToItem(SideDotTile sideDotTile)
    {
        switch (sideDotTile)
        {
            case SideDotTile.Normal:
                return ItemsTypes.NONE;
            case SideDotTile.EmptyTile:
                return ItemsTypes.NONE;
            case SideDotTile.Package:
                return ItemsTypes.PACKAGE;
            case SideDotTile.HorizontalBomb:
                return ItemsTypes.HORIZONTAL_STRIPED;
            case SideDotTile.VerticalBomb:
                return ItemsTypes.VERTICAL_STRIPED;
            case SideDotTile.SuchBombe:
                return ItemsTypes.MARMALADE;
            case SideDotTile.Jelly:
                return ItemsTypes.NONE;
            case SideDotTile.Lock:
                return ItemsTypes.NONE;
            case SideDotTile.Chest:
                return ItemsTypes.NONE;
            case SideDotTile.LockedChest:
                return ItemsTypes.NONE;
            case SideDotTile.Fruit:
                return ItemsTypes.NONE;
            case SideDotTile.Ingredient:
                return ItemsTypes.INGREDIENT;
            case SideDotTile.SameColorBomb:
                return ItemsTypes.MULTICOLOR;
            default:
                throw new ArgumentOutOfRangeException(nameof(sideDotTile), sideDotTile, null);
        }
    }
    
    public static void InstantiateLeftSideDots(int rowLength, float leftOffSet, GameObject tilePrefab, ref BackgroundTile[,] alltiles,ref List<BackGroundTileSideList> allSideBackGroundTiles,
        Sprite tileBackgroundBright, Sprite tileBackgroundDark, Transform boardTransform, SideFruitsSetting[] sideFruitsSetting)
    {
        
        for (int i = 0; i < rowLength; i++)
        {
            
            if (sideFruitsSetting[i].SideDotTile != SideDotTile.EmptyTile)
            {
                Vector2 tempPosition = new Vector2(0 - leftOffSet, i);
                InstantiateTile(tempPosition, -1, i, Directions.left, tilePrefab, ref alltiles,
                    ref allSideBackGroundTiles, tileBackgroundBright, tileBackgroundDark, boardTransform);
                InstantiateSideDot(tempPosition, -1, i, boardTransform, TranslateSideDotTileToItem(sideFruitsSetting[i].SideDotTile));
            }
        }
    }

    public static void InstantiateRightSideDots(int columnLength, int rowLength, float rightOffset, GameObject tilePrefab, ref BackgroundTile[,] alltiles, ref List<BackGroundTileSideList> allSideBackGroundTiles,
        Sprite tileBackgroundBright, Sprite tileBackgroundDark, Transform boardTransform, SideFruitsSetting[] sideFruitsSetting)
    {
        for (int i = 0; i < rowLength; i++)
        {
            if (sideFruitsSetting[i].SideDotTile != SideDotTile.EmptyTile)
            {

                Vector2 tempPosition = new Vector2(columnLength - 1 + rightOffset, i);
                InstantiateTile(tempPosition, columnLength, i, Directions.right, tilePrefab, ref alltiles,
                    ref allSideBackGroundTiles, tileBackgroundBright, tileBackgroundDark, boardTransform);
                InstantiateSideDot(tempPosition, columnLength, i, boardTransform, TranslateSideDotTileToItem(sideFruitsSetting[i].SideDotTile));

           }
        }
    }

    public static void InstantiateBottomSideDots(int columnLength, float bottomOffset, GameObject tilePrefab, ref BackgroundTile[,] alltiles, ref List<BackGroundTileSideList> allSideBackGroundTiles,
        Sprite tileBackgroundBright, Sprite tileBackgroundDark, Transform boardTransform, SideFruitsSetting[] sideFruitsSetting)
    {
        
        for (int i = 0; i < columnLength; i++)
        {
            if (sideFruitsSetting[i].SideDotTile != SideDotTile.EmptyTile)
            {
                Vector2 tempPosition = new Vector2(i, 0 - bottomOffset);
                InstantiateTile(tempPosition, i, -1, Directions.bottom, tilePrefab,  ref  alltiles,ref allSideBackGroundTiles, tileBackgroundBright, tileBackgroundDark, boardTransform);
                InstantiateSideDot(tempPosition, i, -1, boardTransform, TranslateSideDotTileToItem(sideFruitsSetting[i].SideDotTile));
            }
         
        }
    }
    public static void InstantiateTopSideDots(int columnLength, int rowLength,float topOffset, GameObject tilePrefab, ref List<BackGroundTileSideList> allSideBackGroundTiles,
        Sprite tileBackgroundBright, Sprite tileBackgroundDark, ref GameObject[,] allDots, ref BackgroundTile[,] allTiles, Transform boardTransform, SideFruitsSetting[] sideFruitsSetting)
    {
        ModifyTopSideDots(columnLength, rowLength, topOffset, DestroyOrInstantiate.instantiate, tilePrefab,  ref allTiles, ref allSideBackGroundTiles, tileBackgroundBright,
            tileBackgroundDark, ref  allDots, ref  allTiles, boardTransform, sideFruitsSetting);
    }

    public static void ModifyTopSideDots(int columnLength, int rowLength, float topOffset,
        DestroyOrInstantiate destroyOrInstantiate, GameObject tilePrefab,ref BackgroundTile[,] alltiles, ref List<BackGroundTileSideList> allSideBackGroundTiles,
        Sprite tileBackgroundBright, Sprite tileBackgroundDark, ref GameObject[,] allDots, ref BackgroundTile[,] allTiles,
        Transform boardTransform, SideFruitsSetting[] sideFruitsSetting)
    {
        for (int i = 0; i < columnLength; i++)
        {
          // if (sideFruitsSetting[i].IsActivate)
          //  {
                Vector2 tempPosition = new Vector2(i, rowLength - 1 + topOffset);
                if (destroyOrInstantiate == DestroyOrInstantiate.destroy)
                {
                    Destroy(allDots[i, rowLength]);
                    Destroy(allTiles[i, rowLength]);
                }
                else if (destroyOrInstantiate == DestroyOrInstantiate.instantiate)
                {
                    InstantiateTile(tempPosition, i, rowLength, Directions.top, tilePrefab, ref alltiles,
                        ref allSideBackGroundTiles, tileBackgroundBright, tileBackgroundDark, boardTransform);
                    InstantiateSideDot(tempPosition, i, rowLength, boardTransform, TranslateSideDotTileToItem(sideFruitsSetting[i].SideDotTile));
              //  }
            }
        }
    }

    public static void InstantiateTile(Vector2 tempPos, int column, int row, Directions direction, GameObject tilePrefab, ref BackgroundTile[,] alltiles,ref List<BackGroundTileSideList> allSideBackGroundTiles,
        Sprite tileBackgroundBright, Sprite tileBackgroundDark, Transform boardTransform)
    {
        
        GameObject backgroundTile = Instantiate(tilePrefab, tempPos, Quaternion.identity, boardTransform) as GameObject;
        
        if ((column + 1 + row + 1) % 2 == 1)
            backgroundTile.GetComponent<SpriteRenderer>().sprite = tileBackgroundBright;
        else
            backgroundTile.GetComponent<SpriteRenderer>().sprite = tileBackgroundDark;

        backgroundTile.name = "( X:" + column + ", Y:" + row + " )";
        //&& column > -1 && row > -1 && column < Rl.board.width-1 && row < Rl.board.height-1
       
        if(direction == Directions.none)   alltiles[column, row] = backgroundTile.GetComponent<BackgroundTile>();

        if (direction != Directions.none)
        {
           
            var sprtrndr = backgroundTile.GetComponent<SpriteRenderer>();
            sprtrndr.color = new Color(sprtrndr.color.r, sprtrndr.color.g, sprtrndr.color.b, sprtrndr.color.a * 0.9f);
            allSideBackGroundTiles[(int)direction].gameObjectList.Add(backgroundTile);
        }
    }
    public static void InstantiateTile(Vector2 tempPos, int column, int row, Directions direction, GameObject tilePrefab, ref BackgroundTile[,] alltiles,
        Sprite tileBackgroundBright, Sprite tileBackgroundDark, Transform boardTransform)
    {
        List<BackGroundTileSideList> allSideBackGroundTiles = new List<BackGroundTileSideList>();
        InstantiateTile(tempPos, column,row, direction, tilePrefab , ref alltiles, ref allSideBackGroundTiles,
             tileBackgroundBright,  tileBackgroundDark, boardTransform);
        allSideBackGroundTiles = null;

    }
    
    private static void InstantiateSideDot(Vector2 tempPos, int column, int row, Transform boardTransform, ItemsTypes itemType)
    {
     //   int dotToUse = Random.Range(0, dots.Length);
       // GameObject dot = Instantiate(dots[dotToUse], tempPos, Quaternion.identity);
       string itemTypeString = itemType.ToString();
       if (itemType == ItemsTypes.NONE)
       {
           itemTypeString = "Item";
       }
       GameObject dot = ObjectPooler.Instance.GetPooledObject(itemTypeString);


       switch (itemType)
       {
           case ItemsTypes.NONE:
               Item itemComponent = dot.GetComponent<Item>();
               itemComponent.GenColor();
               itemComponent.needFall = false;
               break;
           case ItemsTypes.VERTICAL_STRIPED:
               break;
           case ItemsTypes.HORIZONTAL_STRIPED:
               break;
           case ItemsTypes.PACKAGE:
               Item itemPackage = dot.GetComponent<ItemPackage>();
               itemPackage.GenColor();
               itemPackage.needFall = false;
               break;
           case ItemsTypes.MULTICOLOR:
               break;
           case ItemsTypes.INGREDIENT:
               break;
           case ItemsTypes.SPIRAL:
               break;
           case ItemsTypes.MARMALADE:
               break;
           case ItemsTypes.TimeBomb:
               break;
           default:
               throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null);
       }
  
  
        
        dot.transform.position = tempPos;
        dot.transform.parent = boardTransform;
       // dot.name = "( X:" + column + ", Y:" + row + " )";
      // dot.name = dot.GetComponent<ItemSimple>().
       // Destroy(dot.GetComponent<Dot>());
       dot.AddComponent<ThrivingBlock>();
       
        dot.AddComponent<SideDot>();
        dot.GetComponent<SideDot>().columnSideDot = column;
        dot.GetComponent<SideDot>().rowSideDot = row;
        SideDot.ItemEnableDisable(ref dot, false);
        //  Destroy(dot.GetComponent<SpriteRenderer>());

    }

    public static void CreateOffSetTiles(int columnLength, int rowLength, float leftOffSet, float rightOffset, float bottomOffset,
        float topOffset, GameObject tilePrefab, ref BackgroundTile[,] alltiles, ref List<BackGroundTileSideList> allSideBackGroundTiles,
        Sprite tileBackgroundBright, Sprite tileBackgroundDark, ref GameObject[,] allDots, ref BackgroundTile[,] allTiles, Transform boardTransform,
        bool bottomActive, bool leftActive, bool rightActive, bool topActive, List<SideFruitsSetting> sideFruitsSettings)
    { 
   
            //   0 - bottom | 1 - top | 2 - right | 3 - left
            var t = sideFruitsSettings.ToArray();
            SideFruitsSetting[] top = t[0..9];
            SideFruitsSetting[] bottom = t[9..18];
            SideFruitsSetting[] left = t[18..27];
            SideFruitsSetting[] right = t[27..36];
            
       
        if (bottomActive) InstantiateBottomSideDots(columnLength, bottomOffset, tilePrefab, ref alltiles, ref allSideBackGroundTiles,
                tileBackgroundBright, tileBackgroundDark, boardTransform, bottom);
        if (leftActive) InstantiateLeftSideDots(rowLength, leftOffSet, tilePrefab, ref alltiles, ref allSideBackGroundTiles,
                tileBackgroundBright, tileBackgroundDark, boardTransform, left);
        if (rightActive) InstantiateRightSideDots(columnLength, rowLength, rightOffset, tilePrefab, ref alltiles,
                ref allSideBackGroundTiles, tileBackgroundBright, tileBackgroundDark, boardTransform, right);
        if (topActive) InstantiateTopSideDots(columnLength, rowLength, topOffset, tilePrefab,
                ref allSideBackGroundTiles, tileBackgroundBright, tileBackgroundDark, ref allDots, ref allTiles,
                boardTransform, top);
    }

    public static List<Vector3> InstaniteEverythingAtStart(int width, int height, float leftOffset, float rightOffset, float bottomOffset,
        float topOffset, GameObject tilePrefab, BackgroundTile[,] alltiles, ref List<BackGroundTileSideList> allSideBackGroundTiles,
        Sprite tileBackgroundBright, Sprite tileBackgroundDark, GameObject[,] allDots, ref BackgroundTile[,] allTiles, Transform boardTransform,
        bool bottomActive, bool leftActive, bool rightActive, bool topActive, List<SideFruitsSetting> sideFruitsSettings)
    {
        List<Vector3> posBlankspaces = new List<Vector3>();
        /*for (int i = 0; i < width; i ++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j + offSet);
                if (!blankSpaces[i, j])
                {
                    
                    InstantiateTile(new Vector2(i,j),i,j,Directions.none, tilePrefab, ref alltiles, ref allSideBackGroundTiles, tileBackgroundBright, tileBackgroundDark, boardTransform);
                    

                    int dotToUse = Random.Range(0, dots.Length); 

                    int maxIterations = 0;

                    while (MatchHelper.MatchesAt(i, j, allDots,dots[dotToUse]) && maxIterations < 100)
                    {
                        dotToUse = Random.Range(0, dots.Length);
                        maxIterations++;
                        Debug.Log(maxIterations);
                    }

                    GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    dot.GetComponent<Dot>().row = j;
                    dot.GetComponent<Dot>().column = i;
                    dot.transform.parent = boardTransform;
                    dot.name = "( " + i + ", " + j + " )";
                    allDots[i, j] = dot;
                }
                else
                {
                    posBlankspaces.Add(tempPosition);
                }
            }
        }*/
          CreateOffSetTiles(width, height, leftOffset, rightOffset, bottomOffset, topOffset, tilePrefab, ref alltiles,ref allSideBackGroundTiles,
            tileBackgroundBright, tileBackgroundDark, ref allDots, ref allTiles, boardTransform, bottomActive, leftActive, rightActive, topActive, sideFruitsSettings);
          return posBlankspaces;
    }
}

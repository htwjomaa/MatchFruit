using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FruitMatch.Scripts.Items;
using FruitMatch.Scripts.System.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class BoardInstantiate : MonoBehaviour
{
    public static void InstantiateLeftSideDots(int rowLength, float leftOffSet, GameObject tilePrefab, ref BackgroundTile[,] alltiles,ref List<BackGroundTileSideList> allSideBackGroundTiles,
        Sprite tileBackgroundBright, Sprite tileBackgroundDark, Transform boardTransform, SideFruitsSetting[] sideFruitsSetting)
    {
        
        for (int i = 0; i < rowLength; i++)
        {
            if (sideFruitsSetting[i].IsActivate)
            {
                Vector2 tempPosition = new Vector2(0 - leftOffSet, i);
                InstantiateTile(tempPosition, -1, i, Directions.left, tilePrefab, ref alltiles,
                    ref allSideBackGroundTiles, tileBackgroundBright, tileBackgroundDark, boardTransform);
                InstantiateSideDot(tempPosition, -1, i, boardTransform);
            }
        }
    }

    public static void InstantiateRightSideDots(int columnLength, int rowLength, float rightOffset, GameObject tilePrefab, ref BackgroundTile[,] alltiles, ref List<BackGroundTileSideList> allSideBackGroundTiles,
        Sprite tileBackgroundBright, Sprite tileBackgroundDark, Transform boardTransform, SideFruitsSetting[] sideFruitsSetting)
    {
        for (int i = 0; i < rowLength; i++)
        {
            if (sideFruitsSetting[i].IsActivate)
            {

                Vector2 tempPosition = new Vector2(columnLength - 1 + rightOffset, i);
                InstantiateTile(tempPosition, columnLength, i, Directions.right, tilePrefab, ref alltiles,
                    ref allSideBackGroundTiles, tileBackgroundBright, tileBackgroundDark, boardTransform);
                InstantiateSideDot(tempPosition, columnLength, i, boardTransform);

            }
        }
    }

    public static void InstantiateBottomSideDots(int columnLength, float bottomOffset, GameObject tilePrefab, ref BackgroundTile[,] alltiles, ref List<BackGroundTileSideList> allSideBackGroundTiles,
        Sprite tileBackgroundBright, Sprite tileBackgroundDark, Transform boardTransform, SideFruitsSetting[] sideFruitsSetting)
    {
        
        for (int i = 0; i < columnLength; i++)
        {
            if (sideFruitsSetting[i].IsActivate)
            {
                Vector2 tempPosition = new Vector2(i, 0 - bottomOffset);
                InstantiateTile(tempPosition, i, -1, Directions.bottom, tilePrefab,  ref  alltiles,ref allSideBackGroundTiles, tileBackgroundBright, tileBackgroundDark, boardTransform);
                InstantiateSideDot(tempPosition, i, -1, boardTransform);
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
            if (sideFruitsSetting[i].IsActivate)
            {
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
                    InstantiateSideDot(tempPosition, i, rowLength, boardTransform);
                }
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
    public static Item GenItemSimple()
    {
        GameObject item = null;
        item = ObjectPooler.Instance.GetPooledObject("Item");
        item.transform.localScale = Vector2.one * 0.42f;
      
        IColorableComponent colorableComponent = item.GetComponent<IColorableComponent>();
        Item itemComponent = item.GetComponent<Item>();
        itemComponent.GenColor();
        itemComponent.needFall = false;
        return itemComponent;
    }

  
    private static void InstantiateSideDot(Vector2 tempPos, int column, int row, Transform boardTransform)
    {
     //   int dotToUse = Random.Range(0, dots.Length);
       // GameObject dot = Instantiate(dots[dotToUse], tempPos, Quaternion.identity);
        GameObject dot = ObjectPooler.Instance.GetPooledObject("Item");
        Item itemComponent = dot.GetComponent<Item>();
        itemComponent.GenColor();
        itemComponent.needFall = false;
        
        dot.transform.position = tempPos;
        dot.transform.parent = boardTransform;
       // dot.name = "( X:" + column + ", Y:" + row + " )";
      // dot.name = dot.GetComponent<ItemSimple>().
       // Destroy(dot.GetComponent<Dot>());
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
            SideFruitsSetting[] bottom = t[0..9];
            SideFruitsSetting[] top = t[9..18];
            SideFruitsSetting[] right = t[18..27];
            SideFruitsSetting[] left = t[27..36];
            
       
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

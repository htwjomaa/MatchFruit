using UnityEngine;

public sealed class LockTiles : MonoBehaviour
{
    public static void GenerateLockTiles(TileType[] boardLayout, ref BackgroundTile[,] lockTiles, GameObject lockTilePrefab)
    {
        //Look at all the tiles in the layout
        for (int i = 0; i < boardLayout.Length; i++)
        {
            //if a tile is a "Lock" tile
            if (boardLayout[i].tileKind == TileKind.Lock)
            {
                //Create a "Lock" tile at that position;
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(lockTilePrefab, tempPosition, Quaternion.identity);
                lockTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }
    
    public static void BindLockTilesToDot(int width, int height, BackgroundTile[,] lockTiles, GameObject [,] allDots)
    {
        for (int i = 0; i < width; i++)  //for every spot on the board. . . 
        {
            for (int j = 0; j < height; j++)
            {
                if (lockTiles[i, j] != null && allDots[i, j] != null && allDots[i, j].gameObject != null)
                {
                    lockTiles[i, j].AssosiatedGameObject = allDots[i, j].gameObject;
                }
                
            }
        }
        
        
        /*Debug.Log(allDots[0,0].name);
        int counter = 0;
        for (var index0 = 0; index0 < allDots.GetLength(0); index0++)
        for (var index1 = 0; index1 < allDots.GetLength(1); index1++)
        {
            if (allDots[index0, index1]!= null && allDots[index0, index1].gameObject != null)
            {
                if (lockTiles[index0, index1] != null && lockTiles[index0, index1].AssosiatedGameObject == null)
                    lockTiles[index0, index1].AssosiatedGameObject = allDots[index0, index1].gameObject;
                return;
            }
        }*/
        //Correct if 0-0 has lockTile, dont know why I have to do this
        //if (lockTiles[0, 0] != null && allDots[0, 0] != null && allDots[0, 0].gameObject != null)
       // {
           // lockTiles[0, 0].AssosiatedGameObject = allDots[0, 0].gameObject;
       // }
    }
    
    public static void CorrectBinding(int width, int height, BackgroundTile[,] lockTiles, GameObject [,] allDots)
    {
        
        for (int i = 0; i < width; i++)  //for every spot on the board. . . 
        {
            for (int j = 0; j < height; j++)
            {
                if (!lockTiles[i, j] && lockTiles[i, j].gameObject) continue;
                if(allDots[i, j]) lockTiles[i, j].AssosiatedGameObject = allDots[i, j].gameObject;
                else
                {
                    int newX = Random.Range(0, width);
                    int newY = Random.Range(0, height);

                    // use a while loop like in sime later
                    var adjacent = BoardUtil.CheckForAdjacent(newX, newY, width, height, allDots);
                    if (adjacent != Vector2.zero)
                    {

                        lockTiles[i, j].AssosiatedGameObject = allDots[i + (int)adjacent.x, j + (int)adjacent.y].gameObject;
                    }
                }
        }
    } 
    }
    public static void UpdateLockTiles(int width, int height, ref BackgroundTile[,] lockTiles)
    {
        BackgroundTile[,] lockTilesHelperList = new BackgroundTile[width,height];

        for (int i = 0; i < width; i++)  //for every spot on the board. . . 
        {
            for (int j = 0; j < height; j++)
            {
                lockTilesHelperList[i, j] = lockTiles[i, j];
            }
        }
        
        for (int i = 0; i < width; i++)  //for every spot on the board. . . 
        {
            for (int j = 0; j < height; j++)
            {
                if (lockTilesHelperList[i, j] != null && lockTilesHelperList[i, j].AssosiatedGameObject !=null && lockTilesHelperList[i, j].AssosiatedGameObject.GetComponent<Dot>() != null)
                {
            
                    int column = lockTilesHelperList[i, j].AssosiatedGameObject.GetComponent<Dot>().column;
                    int row = lockTilesHelperList[i, j].AssosiatedGameObject.GetComponent<Dot>().row;
                    if (column != i || row != j)
                    {
                        lockTiles[i, j] = null;
                        lockTiles[column, row] = lockTilesHelperList[i, j];
                    }
                }
                    
            }
        }
    }
    
    public static void MoveLockTiles(BackgroundTile[,] lockTiles)
    {
        foreach (BackgroundTile lockTile in lockTiles)
        {
            if(lockTile !=null && lockTile.AssosiatedGameObject != null)
            lockTile.transform.position = lockTile.AssosiatedGameObject.transform.position;
        }
      
    }
    
}
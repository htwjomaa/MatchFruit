using UnityEngine;

public sealed class Concrete : MonoBehaviour
{

    public static void MarkConcreteField(ref BackgroundTile[,] concreteFields, bool markBlank, ref bool[,] blankSpaces, ref GameObject[,] allDots)
    {
        for (var index0 = 0; index0 < concreteFields.GetLength(0); index0++)
        for (var index1 = 0; index1 < concreteFields.GetLength(1); index1++)
        {
            var n = concreteFields[index0, index1];
            if (n != null && n.GetComponent<BackgroundTile>() != null && n.GetComponent<BackgroundTile>().hitPoints < 1)
            {
                if (markBlank)
                {
                    if (allDots[index0, index1] != null && allDots[index0, index1].gameObject != null)
                    {
                        Destroy(allDots[index0,index1].gameObject);
                        allDots[index0, index1] = null;
                        blankSpaces[index0, index1] = true;
                    }
                }
                else if (!markBlank)
                {
                    blankSpaces[index0, index1] = false;
                }
            }
        }
    }
    public static void GenerateConcreteTiles(TileType[] boardLayout, ref BackgroundTile[,]  concreteTiles, GameObject concreteTilePrefab)
    {
        //Look at all the tiles in the layout
        for (int i = 0; i < boardLayout.Length; i++)
        {
            //if a tile is a "Lock" tile
            if (boardLayout[i].tileKind == TileKind.Chest)
            {
                //Create a "Lock" tile at that position;
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(concreteTilePrefab, tempPosition, Quaternion.identity);
                concreteTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }

    public static void DamageConcrete(int column, int row, int width, int height, ref BackgroundTile[,] concreteTiles)
    {
        if(column > 0)
        {
            if(concreteTiles[column - 1, row])
            {
                concreteTiles[column - 1, row].TakeDamage(1);
                if (concreteTiles[column - 1, row].hitPoints <= 0)
                {
                    concreteTiles[column - 1, row] = null;
                }
            }
        }
        if (column < width - 1)
        {
            if (concreteTiles[column + 1, row])
            {
                concreteTiles[column + 1, row].TakeDamage(1);
                if (concreteTiles[column + 1, row].hitPoints <= 0)
                {
                    concreteTiles[column + 1, row] = null;
                }
            }
        }
        if (row > 0)
        {
            if (concreteTiles[column, row - 1])
            {
                concreteTiles[column, row - 1].TakeDamage(1);
                if (concreteTiles[column, row - 1].hitPoints <= 0)
                {
                    concreteTiles[column, row - 1] = null;
                }
            }
        }
        if (row < height -1)
        {
            if (concreteTiles[column, row + 1])
            {
                concreteTiles[column, row + 1].TakeDamage(1);
                if (concreteTiles[column, row + 1].hitPoints <= 0)
                {
                    concreteTiles[column, row + 1] = null;
                }
            }
        }

       if(concreteTiles[column, row] != null && concreteTiles[column, row].hitPoints < 1) Rl.board.MarkConcrete(false);
    }
}

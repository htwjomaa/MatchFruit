using UnityEngine;

public sealed class Breakabletiles : MonoBehaviour
{
    public static void GenerateBreakableTiles(TileType[] boardLayout, ref BackgroundTile[,] breakableTiles, GameObject breakableTilePrefab)
    {
        for (int i = 0; i < boardLayout.Length; i++)
        {
            //if a tile is a jelly tile
            if (boardLayout[i].tileKind == TileKind.Breakable)
            {
                //create a jelly tile at that position
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(breakableTilePrefab, tempPosition, Quaternion.identity);
                breakableTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }
}

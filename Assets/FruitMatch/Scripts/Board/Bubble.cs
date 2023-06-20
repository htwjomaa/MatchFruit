using System.Collections.Generic;
using UnityEngine;

public sealed class Bubble : MonoBehaviour
{
    public static void StartBubbleOnRandomLocation( ref GameObject[,] allDots,
        ref BackgroundTile[,] bubbleTiles, ref BackgroundTile[,] lockTiles, ref BackgroundTile[,] concreteTiles,
        GameObject bubblePiecePrefab, bool makeBubble, string makeBubbleSound)
    {
        Vector2Int adjacent = BoardUtil.GetRandomDot(allDots);
   
        if (bubbleTiles[adjacent.x, adjacent.y] == null)
        {
            BubbleMaker(ref allDots,  0,0, adjacent, ref bubbleTiles,
                ref lockTiles, ref  concreteTiles, bubblePiecePrefab, ref makeBubble, makeBubbleSound);
        }
       
    }
    public static void CheckToMakeBubble(int width, int height, ref GameObject[,] allDots,
        ref BackgroundTile[,] bubbleTiles, ref BackgroundTile[,] lockTiles, ref BackgroundTile[,] concreteTiles,
        GameObject bubblePiecePrefab, bool makeBubble, string makeBubbleSound)
    {
        //Check the Bubble tiles array
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (bubbleTiles[i, j] != null && makeBubble)
                {
                    //Call another method to make a new bubble
                    MakeNewBubble(width, height, ref allDots, ref bubbleTiles, ref lockTiles, ref concreteTiles,
                        bubblePiecePrefab, makeBubbleSound);
                    return;
                }
            }
        }
    }

    private static void MakeNewBubble(int width, int height, ref GameObject[,] allDots, ref BackgroundTile[,] bubbleTiles,
        ref BackgroundTile[,] lockTiles, ref BackgroundTile[,] concreteTiles, GameObject bubblePiecePrefab,
        string makeBubbleSound)
    {
        bool bubble = false;
        int loops = 0;
        bool allowZero = false;
        while (!bubble && loops < 200)
        {
            int newX = Random.Range(0, width);
            int newY = Random.Range(0, height);
            if (bubbleTiles[newX, newY] != null)
            {
                Vector2Int adjacent = Vector2Int.zero;
                if (loops < 199) adjacent = BoardUtil.CheckForAdjacent(newX, newY, width, height, allDots);
                else
                {
                    adjacent = BoardUtil.GetRandomDot(allDots);
                    allowZero = true;
                    newX = 0;
                    newY = 0;
                }
                Debug.Log(adjacent);
                if (allowZero  || (adjacent != Vector2Int.zero && bubbleTiles[newX + adjacent.x, newY + adjacent.y] == null))
                {
                    BubbleMaker(ref allDots,  newX,newY, adjacent, ref bubbleTiles,
                         ref lockTiles, ref  concreteTiles, bubblePiecePrefab, ref bubble, makeBubbleSound);
                }
            }

            loops++;
            Debug.Log(loops);
        }
    }

    private static void BubbleMaker (ref GameObject[,] allDots, int newX, int newY, Vector2Int adjacent, ref BackgroundTile[,] bubbleTiles,
        ref BackgroundTile[,] lockTiles, ref BackgroundTile[,] concreteTiles, GameObject bubblePiecePrefab, ref bool makeBubble,
        string makeBubbleSound )
    {
        Destroy(allDots[newX + adjacent.x, newY + adjacent.y]);
        allDots[newX + adjacent.x, newY + adjacent.y] = null;
        Vector2 tempPosition = new Vector2(newX + adjacent.x, newY + adjacent.y);
        GameObject tile = Instantiate(bubblePiecePrefab, tempPosition, Quaternion.identity);
        bubbleTiles[newX + adjacent.x, newY + adjacent.y] = tile.GetComponent<BackgroundTile>();

        List<BackgroundTile[,]> backgroundTilesList = new List<BackgroundTile[,]>();

        backgroundTilesList.Add(lockTiles);
        backgroundTilesList.Add(concreteTiles);
        DestroyBackGroundTiles(newX, newY, adjacent, backgroundTilesList);

        Rl.GameManager.PlayAudio(makeBubbleSound, Random.Range(0, 4), Rl.settings.GetSFXVolume,
            Rl.effects.audioSource);
        makeBubble = true;
    }
 
    private static void DestroyBackGroundTiles(int newX, int newY, Vector2Int adjacent, List<BackgroundTile[,]> backgroundTilesList)
    {

        for (int i = 0; i < backgroundTilesList.Count; i++)
        {
            if (backgroundTilesList[i][newX + adjacent.x, newY + adjacent.y] != null && backgroundTilesList[i][newX + adjacent.x, newY + adjacent.y].transform.gameObject != null)
            {
                Destroy(backgroundTilesList[i][newX + adjacent.x, newY + adjacent.y].transform.gameObject);
                backgroundTilesList[i][newX + adjacent.x, newY + adjacent.y] = null;
            }
        }
    }

    public static void DamageBubble(int column, int row, int width, int height, ref BackgroundTile[,] bubbleTiles, ref bool makeBubble, string damageBubbleSound)
    {
        if (column > 0)
        {
            if (bubbleTiles[column - 1, row])
            {
                bubbleTiles[column - 1, row].TakeDamage(1);
                if (bubbleTiles[column - 1, row].hitPoints <= 0)
                {
                    bubbleTiles[column - 1, row] = null;
                }
                makeBubble = false;
            }
        }
        if (column < width - 1)
        {
            if (bubbleTiles[column + 1, row])
            {
                bubbleTiles[column + 1, row].TakeDamage(1);
                if (bubbleTiles[column + 1, row].hitPoints <= 0)
                {
                    bubbleTiles[column + 1, row] = null;
                }
                makeBubble = false;
            }
        }
        if (row > 0)
        {
            if (bubbleTiles[column, row - 1])
            {
                bubbleTiles[column, row - 1].TakeDamage(1);
                if (bubbleTiles[column, row - 1].hitPoints <= 0)
                {
                    bubbleTiles[column, row - 1] = null;
                }
                makeBubble = false;
            }
        }
        if (row < height - 1)
        {
            if (bubbleTiles[column, row + 1])
            {
                bubbleTiles[column, row + 1].TakeDamage(1);
                if (bubbleTiles[column, row + 1].hitPoints <= 0)
                {
                    bubbleTiles[column, row + 1] = null;
                }
                makeBubble = false;
            }
        }
        Rl.GameManager.PlayAudio(damageBubbleSound, Random.Range(0, 4), Rl.settings.GetSFXVolume,
            Rl.effects.audioSource);
    }
    
    public static void GenerateBubbleTiles(TileType[] boardLayout, ref BackgroundTile[,] bubbleTiles, GameObject bubblePiecePrefab)
    {
        //Look at all the tiles in the layout
        for (int i = 0; i < boardLayout.Length; i++)
        {
            //if a tile is a "Lock" tile
            if (boardLayout[i].tileKind == TileKind.Bubble)
            {
                //Create a "Lock" tile at that position;
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(bubblePiecePrefab, tempPosition, Quaternion.identity);
                bubbleTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }
}
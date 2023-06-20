using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DestroyOnBoard : MonoBehaviour
{
    public  void DestroyMatches(int width, int height, int streakValue, ref GameObject[,] allDots,
        ref BackgroundTile[,] breakableTiles, ref BackgroundTile[,] lockTiles, ref BackgroundTile[,] concreteTiles, ref BackgroundTile[,] slimeTiles, ref bool[,] blankSpaces,
        GameObject destroyParticle, World world, ref bool makeSlime, string matchFound, string damageSlimeSound,
        FindMatches findMatches, ref Dot currentDot, ref MatchType matchtype, RowDecreaser rowDecreaser)
    {
        //How many elements are in the matched pieces list from findmatches?
        if (findMatches.currentMatches.Count >= 4)
        {
            BombClass.CheckToMakeBombs(ref currentDot, findMatches ,ref matchtype);
        }
        findMatches.currentMatches.Clear();
        for (int i = 0; i < width; i ++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    DestroyOnBoard.DestroyMatchesAt(i, j,  width,  height, streakValue,  ref allDots,
                        ref breakableTiles, ref  lockTiles, ref  concreteTiles, ref  slimeTiles,
                        destroyParticle,  world, ref  makeSlime, matchFound, damageSlimeSound);
                }
            }
        }
       
        StartCoroutine(rowDecreaser.DecreaseRowCo(width,  height, 
            allDots, blankSpaces,  concreteTiles, slimeTiles));
    }
    
    public static void DestroyMatchesAt(int column, int row, int width, int height, int streakValue, ref GameObject[,] allDots,
        ref BackgroundTile[,] breakableTiles, ref BackgroundTile[,] lockTiles, ref BackgroundTile[,] concreteTiles, ref BackgroundTile[,] slimeTiles,
        GameObject destroyParticle, World world, ref bool makeSlime, string matchFoundAudio, string damageSlimeSound)
    {
        if (!allDots[column, row].GetComponent<Dot>().isMatched) return;
        
        //Does a tile need to break?
        if(breakableTiles[column, row]!=null)
        {   //if it does, give one damage.
            breakableTiles[column, row].TakeDamage(1);
            if(breakableTiles[column, row].hitPoints <= 0) breakableTiles[column, row] = null;
        }
        if (lockTiles[column, row] != null)
        {  //if it does, give one damage.
            lockTiles[column, row].TakeDamage(1);
            if (lockTiles[column, row].hitPoints <= 0) lockTiles[column, row] = null;
        }
        Concrete.DamageConcrete(column, row, width, height, ref concreteTiles);
        Bubble.DamageBubble(column, row, width, height, ref slimeTiles, ref makeSlime, damageSlimeSound);
        if(Rl.goalManager != null)
        {
            Rl.goalManager.CompareGoal(allDots[column, row].tag, allDots[column, row].GetComponent<Dot>().FruitColor);
            Rl.goalManager.UpdateGoals();
        }

        //Does the sound manager exist?
        Rl.GameManager.PlayAudio(matchFoundAudio, Random.Range(0,4), Rl.settings.GetSFXVolume, Rl.effects.audioSource);
        GameObject particle = Instantiate(destroyParticle, 
            allDots[column, row].transform.position, 
            Quaternion.identity);
        Destroy(particle, .5f);
        //allDots[column, row].GetComponent<Dot>().PopAnimation();
        Destroy(allDots[column, row]);
        Rl.ScoreManager.IncreaseScore(world.basePieceValue * streakValue);
        allDots[column, row] = null;
    }

    public static void DestroyLine(int width, int height, ref List<BackGroundTileSideList> allSideBackGroundTiles, Directions direction)
    {
        foreach(GameObject sideLineDot in GetSideDotsLine(width, height,direction)) 
            Destroy(sideLineDot);
        foreach(GameObject backGroundTile in allSideBackGroundTiles[(int)direction].gameObjectList)
            Destroy(backGroundTile);
       
        allSideBackGroundTiles[(int)direction].gameObjectList.Clear();
    }
    
    private static List<GameObject> GetSideDotsLine(int width, int height, Directions direction)
    {   // Finds out what the Dot on the sidelines on the opposite Side is
        List<GameObject> tempObjects = new List<GameObject>();
        foreach (SideDot sideDot in FindObjectsOfType<SideDot>())
        {
            if (direction == Directions.top && sideDot.rowSideDot == height) tempObjects.Add(sideDot.gameObject);
            if (direction == Directions.bottom && sideDot.rowSideDot == -1) tempObjects.Add(sideDot.gameObject);
            if (direction == Directions.left && sideDot.columnSideDot == -1 ) tempObjects.Add(sideDot.gameObject);
            if (direction == Directions.right && sideDot.columnSideDot == width) tempObjects.Add(sideDot.gameObject);
        }
        return tempObjects;
    }
}

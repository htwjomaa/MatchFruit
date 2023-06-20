using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombClass : MonoBehaviour
{
    public static void CheckToMakeBombs(ref Dot currentDot, FindMatches findMatches, ref MatchType matchType)
    {
        //How many objects are in findMatches currentMatches?
        if (findMatches.currentMatches.Count > 3)
        {
            //What type of match?
            MatchType typeOfMatch = MatchHelper.ColumnOrRow(findMatches, ref matchType);
            if (typeOfMatch.type == 1)
            {
                //Make a color bomb
                //is the current dot matched?
                if (currentDot != null && currentDot.isMatched && currentDot.tag == typeOfMatch.color)
                {
                    currentDot.isMatched = false;
                    currentDot.MakeColorBomb();
                }
                else
                {
                    if (currentDot.otherDot != null)
                    {
                        Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                        if (otherDot.isMatched && otherDot.tag == typeOfMatch.color)
                        {
                            otherDot.isMatched = false;
                            otherDot.MakeColorBomb();
                        }
                    }
                }
            }

            else if (typeOfMatch.type == 2)
            {
                //Make a adjacent bomb
                //is the current dot matched?
                if (currentDot != null && currentDot.isMatched && currentDot.tag == typeOfMatch.color)
                {
                    currentDot.isMatched = false;
                    currentDot.MakeAdjacentBomb();
                }
                else if (currentDot.otherDot != null)
                {
                    Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                    if (otherDot.isMatched && otherDot.tag == typeOfMatch.color)
                    {
                        otherDot.isMatched = false;
                        otherDot.MakeAdjacentBomb();
                    }
                }
            }
            else if (typeOfMatch.type == 3)
            {
                findMatches.CheckBombs(typeOfMatch);
            }
        }
    }
    
    public static void BombRow(int row, int width,ref BackgroundTile[,] concreteTiles)
    {
        for (int i = 0; i < width; i ++)
        {
            if(concreteTiles[i,row])
            {
                concreteTiles[i, row].TakeDamage(1);
                if (concreteTiles[i, row].hitPoints <= 0)
                {
                    concreteTiles[i, row] = null;
                }
            }
        }
    }
    public static void BombColumn(int column, int width, int height, ref BackgroundTile[,] concreteTiles)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (concreteTiles[i, j])
                {
                    concreteTiles[column, i].TakeDamage(1);
                    if (concreteTiles[column, i].hitPoints <= 0)
                    {
                        concreteTiles[column, i] = null;
                    }
                }
            }
        }
    }
}

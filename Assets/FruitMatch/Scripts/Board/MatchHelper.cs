using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class MatchHelper : MonoBehaviour
{
    public static MatchType ColumnOrRow(FindMatches findMatches, ref MatchType matchtype)
    {
        //Make a copy of the current matches
        List<GameObject> matchCopy = findMatches.currentMatches.ToList();

        matchtype.type = 0;
        matchtype.color = "";

        //Cycle through all of match Copy and decide if a bomb needs to be made
        for (int i = 0; i < matchCopy.Count; i ++)
        {
            //Store this dot
            Dot thisDot = matchCopy[i].GetComponent<Dot>();
            string color = matchCopy[i].tag;
            int columnMatch = 0;
            int rowMatch = 0;
            //Cycle through the rest of the pieces and compare
            for (int j = 0; j < matchCopy.Count; j ++)
            {
                //Store the next dot
                Dot nextDot = matchCopy[j].GetComponent<Dot>();
                if(nextDot == thisDot){
                    continue;
                }
                if(nextDot.column == thisDot.column && nextDot.tag == color)
                {
                    columnMatch++;
                }
                if (nextDot.row == thisDot.row && nextDot.tag == color)
                {
                    rowMatch++;
                }
            }
            // Return 3 if column or row match
            //Return 2 if adjacent
            //Return 1 if it's a color bomb
            if(columnMatch == 4 || rowMatch == 4)
            {
                matchtype.type = 1;
                matchtype.color = color;
                return matchtype;
            }
            else if(columnMatch == 2 && rowMatch == 2)
            {
                matchtype.type = 2;
                matchtype.color = color;
                return matchtype;
            }
            else if(columnMatch == 3 || rowMatch == 3)
            {
                matchtype.type = 3;
                matchtype.color = color;
                return matchtype;
            }
        }
        matchtype.type = 0;
        matchtype.color = "";
        return matchtype;
    }

    public static bool MatchesAt(int column, int row, GameObject[,] allDots, GameObject piece)
    {
        if(column > 1 && row > 1)
        {
            if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
            {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
            if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
            {
                if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
        }
        else if(column <= 1 || row <= 1)
        {
            if(row > 1){
                
                if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
                {
                    if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
            if (column > 1)
            {
                if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
                {
                    if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }


    public static bool MatchesOnBoard(int width, int height, FindMatches findMatches, GameObject[,] allDots)
    {
        SetAllMatchedToFalse(allDots);
        findMatches.FindAllMatches();
        for (int i = 0; i < width; i ++)
        {
            for (int j = 0; j < height; j ++)
            {
                if(allDots[i, j]!= null)
                {
                    if(allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public static bool CheckForMatches(int width, int height, GameObject[,] allDots)
    {
        for (int i = 0; i < width; i ++)
        {
            for (int j = 0; j < height; j ++)
            {
                if(allDots[i,j]!= null)
                {
                    //Make sure that one and two to the right are in the
                    //board
                    if (i < width - 2)
                    {
                        //Check if the dots to the right and two to the right exist
                        if (allDots[i + 1, j] != null && allDots[i + 2, j] != null)
                        {
                            if (allDots[i + 1, j].tag == allDots[i, j].tag
                                && allDots[i + 2, j].tag == allDots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                    if (j < height - 2)				
                    {
                        //Check if the dots above exist
                        if (allDots[i, j + 1] != null && allDots[i, j + 2] != null)
                        {
                            if (allDots[i, j + 1].tag == allDots[i, j].tag
                                && allDots[i, j + 2].tag == allDots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
    private static void SetAllMatchedToFalse(GameObject[,] allDots)
    {
        foreach (GameObject dot in allDots)
        {
            if (dot == null || dot.GetComponent<Dot>() == null) continue;
            dot.GetComponent<Dot>().isMatched = false;
        }
    }
}

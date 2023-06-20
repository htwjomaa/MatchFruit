using System.Collections;
using UnityEngine;

public sealed class RowDecreaser : MonoBehaviour
{
    public IEnumerator DecreaseRowCo(int width, int height, 
        GameObject[,] allDots, bool[,] blankSpaces, BackgroundTile[,] concreteTiles, BackgroundTile[,] slimeTiles)
    {
        //yield return new WaitForSeconds(.5f);
        for (int i = 0; i < width; i ++)
        {
            for (int j = 0; j < height; j ++)
            {
                //if the current spot isn't blank and is empty. . . 
                if(!blankSpaces[i,j] && allDots[i,j] == null && !concreteTiles[i,j] && !slimeTiles[i,j])
                {
                    //loop from the space above to the top of the column
                    for (int k = j + 1; k < height; k ++)
                    {
                        //if a dot is found. . .
                        if(allDots[i, k]!= null)
                        {
                            //move that dot to this empty space
                            allDots[i, k].GetComponent<Dot>().row = j;
                            //set that spot to be null
                            allDots[i, k] = null;
                            //break out of the loop;
                            break;
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(Rl.settings.GetRefillDelay * 0.1f);
        Debug.Log("Refilling the board");
        StartCoroutine(Rl.board.FillBoardCo());
    }
}

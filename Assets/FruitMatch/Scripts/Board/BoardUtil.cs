using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BoardUtil : MonoBehaviour
{
    public static Vector2Int CheckForAdjacent(int column, int row, int width, int height, GameObject[,] allDots)
    {
        if (column < width - 1 && column > 0 && row < height - 1 && row > 0)
        {
            if (allDots[column + 1, row] ) return Vector2Int.right;
            if (allDots[column - 1, row] ) return Vector2Int.left;
            if (allDots[column, row + 1] ) return Vector2Int.up;
            if (allDots[column, row - 1] ) return Vector2Int.down;
        }
   
        return Vector2Int.zero;
    }
    
    public static Vector2Int GetRandomDot(GameObject[,] allDots)
    {
        Debug.Log("getting random dot" );
        foreach (GameObject dot in allDots)
        {
            if (dot != null && dot.gameObject != null)
            {
                Debug.Log("Random Dot Column: " +  dot.GetComponent<Dot>().column + " Row"+ dot.GetComponent<Dot>().row );
                return new Vector2Int(dot.GetComponent<Dot>().column, dot.GetComponent<Dot>().row );
            }
        }

        return Vector2Int.zero;
    }
    public static TileKind CheckTileBelow(int column, int row)
    {
        foreach (TileType Item in Rl.board.boardLayout)
        {
            if (Item.x == column && Item.y == row)
                return Item.tileKind;
        }
        return TileKind.Normal;
    }
    
    public  bool SwitchAndCheck(int column, int row, int width, int height, ref GameObject[,] allDots, bool[,] blankSpaces ,Vector2 direction)
    {
        SwitchPieces(column, row, direction, ref allDots, blankSpaces);
        if (MatchHelper.CheckForMatches(width, height, allDots))
        {
            SwitchPieces(column, row, direction, ref allDots, blankSpaces);
            return true;
        }

        SwitchPieces(column, row, direction, ref allDots, blankSpaces);
        return false;
    }
    private static void SwitchPieces(int column, int row, Vector2 direction, ref GameObject[,] allDots, bool[,] blankSpaces)
    {
        //Take the second piece and save it in a holder
        if (!blankSpaces[column + (int)direction.x, row + (int)direction.y])
        {
            GameObject holder = allDots[column + (int)direction.x, row + (int)direction.y] as GameObject;
            //switching the first dot to be
            allDots[column + (int)direction.x, row + (int)direction.y] = allDots[column, row];
            //set the first dot to be the second dot
            allDots[column, row] = holder;
        }
    }

    public bool RefillBoard(int width, int height, float offSet, ref GameObject[,] allDots, GameObject[] dots,
        bool[,] blankSpaces, BackgroundTile[,] concreteTiles, BackgroundTile[,] slimeTiles)
    {
        /*for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null && !blankSpaces[i, j] && !concreteTiles[i, j] && !slimeTiles[i, j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int dotToUse = Random.Range(0, dots.Length);
                    int maxIterations = 0;

                    while (MatchHelper.MatchesAt(i, j, allDots, dots[dotToUse]) && maxIterations < 100)
                    {
                        maxIterations++;
                        dotToUse = Random.Range(0, dots.Length);
                    }

                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;
                }
            }
        }*/
        
        int counter = 9999;
       // while (counter > 0)
      //  {
        //Get highest Blocks
        Dictionary<int, int> highestDots = new Dictionary<int, int>();
        for (int i = 0; i < width; i++)
        {
            highestDots.Add(i, 0);
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (CheckIfViableSpace(allDots, blankSpaces, concreteTiles, slimeTiles, i,  j))
                {
                    int t = 0;
                    highestDots.TryGetValue(i, out t);
                    if (j > t) highestDots[i] = j;
                    counter = j;
                }
            }
        }

        // foreach (KeyValuePair<int, int> dotToBeInstantiated in highestDots)
        // {
        //     Debug.Log("PosX: " + dotToBeInstantiated.Key + " | PosY " +dotToBeInstantiated.Value);
        // }

        //Get the highest Dots
        bool hasInstantiated = false;

        StartCoroutine(InstantiateCo(allDots));
        IEnumerator InstantiateCo(GameObject[,] allDots)
        {
            foreach (KeyValuePair<int, int> dotToBeInstantiated in highestDots)
        {
            if (dotToBeInstantiated.Value > 0)
            {
                
                Vector2 tempPosition = new Vector2(dotToBeInstantiated.Key, dotToBeInstantiated.Value * offSet);
                
                    InstantiateNewDot(ref allDots, dots, dotToBeInstantiated.Key, dotToBeInstantiated.Value,
                        tempPosition );
                    yield return new WaitForSeconds(0.5f);
            }
        }
            
     
       // if (hasInstantiated) return false;
        
        foreach (KeyValuePair<int, int> dotWantsToFall in highestDots)
        {
            bool foundSomething = false;
            int counterSecurity = 0;
            Vector2Int newPos = new Vector2Int( dotWantsToFall.Key, dotWantsToFall.Value);

            StartCoroutine(MoveToNextPlaceCo(allDots));
            
            IEnumerator MoveToNextPlaceCo(GameObject[,] allDots)
            {
                foundSomething = CheckBelow(ref allDots, newPos.x,  newPos.y, Check.Below);
                
                if (foundSomething)
                {
                    newPos = new Vector2Int(newPos.x, newPos.y - 1);
                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(MoveToNextPlaceCo(allDots));
                    yield break;
                }
                
                foundSomething = CheckBelow(ref allDots, newPos.x, newPos.y, Check.LeftDown);
                if (foundSomething)
                {
                    newPos = new Vector2Int(newPos.x - 1, newPos.y - 1);
                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(MoveToNextPlaceCo(allDots));
                    yield break;
                }
                
                foundSomething = CheckBelow(ref allDots, newPos.x, newPos.y, Check.RightDown);
                if (foundSomething) 
                {
                    newPos =  new Vector2Int(newPos.x + 1, newPos.y - 1);
                    yield return 
                    new WaitForSeconds(0.1f);
                    StartCoroutine(MoveToNextPlaceCo(allDots));
                    yield break;
                }
            }
        }
        }
      
       

        bool CheckBelow(ref GameObject[,] allDots, int PosX, int PosY, Check check)
        {
            //bool false == found nothing
            //bool true == found something;
            int modPosY = -1;
            int modPosX = 0;
            switch (check)
            {
                case Check.LeftDown:
                    modPosX -= 1;
                    break;
                case Check.RightDown:
                    modPosX  += 1;
                    break;
            }
            if (CheckIfInBoardDimensions(width, height, PosX+modPosX, PosY+modPosY) && CheckIfViableSpace(allDots,
                    blankSpaces, concreteTiles, slimeTiles, PosX+modPosX, PosY+ modPosY))
            {
                Debug.LogWarning("Checking: ___::::: ____" +check.ToString());
                GameObject dot = allDots[PosX, PosY];
                dot.GetComponent<Dot>().column = PosX+modPosX;
                dot.GetComponent<Dot>().row = PosY+modPosY;
                allDots[PosX + modPosX, PosY + modPosY] = dot;
                allDots[PosX, PosY] = null;
                return true;
            }

            return false;
        }
        if (counter > 0) return true;
        return true;
    }
    public enum Check
    {
            Below,
            LeftDown,
            RightDown
    }
    private static void InstantiateNewDot(ref GameObject[,] allDots, GameObject dotObj, int PosX, int PosY, Vector3 worldPos)
    {
        GameObject piece = Instantiate(dotObj, worldPos, Quaternion.identity);
        allDots[PosX, PosY] = piece;
     
        piece.GetComponent<Dot>().column = PosX;
        piece.GetComponent<Dot>().row = PosY;
    }
    private static void InstantiateNewDot(ref GameObject[,] allDots, GameObject[] dots, int PosX, int PosY, Vector3 worldPos)
    {
        int dotToUse = Random.Range(0, dots.Length);
        int maxIterations = 0;

        while (MatchHelper.MatchesAt(PosX, PosY, allDots, dots[dotToUse]) && maxIterations < 100)
        {
            maxIterations++;
            dotToUse = Random.Range(0, dots.Length);
        }

        InstantiateNewDot(ref allDots, dots[dotToUse], PosX, PosY, worldPos);
    }

    private static bool CheckIfInBoardDimensions(int width, int height, int PosX, int PosY)
    {
        if (PosX > width - 1 || PosX < 0 || PosY > height - 1 || PosY < 0) return false;
        return true;
    }
    private static bool CheckIfViableSpace(GameObject[,] allDots, bool[,] blankSpaces, BackgroundTile[,] concreteTiles, BackgroundTile[,] slimeTiles, int PosX, int PosY)
    {
        if (allDots[PosX, PosY] == null && !blankSpaces[PosX, PosY] && !concreteTiles[PosX, PosY] &&
            !slimeTiles[PosX, PosY])
            return true;
        return false;
    }
}

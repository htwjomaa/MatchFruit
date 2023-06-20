using System.Collections.Generic;
using System.Linq;
using FruitMatch.Scripts.Blocks;
using UnityEngine;

public sealed class SideDotUtil : MonoBehaviour
{
    public static void DestroyDotComponents(ref List<DotObject> needDotComponent, ref GameObject destroyablelastDot)
    {
        List<DotObject> helperList = needDotComponent.ToList();
        foreach (DotObject dotObject in helperList)
        {
            Destroy(dotObject.DotGameObject.GetComponent<Dot>());
        }
        if(destroyablelastDot && destroyablelastDot.GetComponent<Dot>() ) Destroy(destroyablelastDot.GetComponent<Dot>());
    }
    
    public static Directions CheckDirection(int column, int row, int width, int height)
    { //Determines which direction the flow should go
        if (column == -1) return Directions.right;
        if (column == width) return Directions.left;
        if (row == -1) return Directions.top;
        if (row == height) return Directions.bottom;

        return Directions.none;
    }
    public static Directions CheckModDirection(int modX, int modY)
    { //Determines which direction the flow should go
        if (modX == -1) return Directions.left;
        if (modX == 1) return Directions.right;
        if (modY == -1) return Directions.bottom;
        if (modY == 1) return Directions.top;

        return Directions.none;
    }
    public static Vector2Int GetXYFromNormalXYOBJ(GameObject square, GameObject[,] alldots)
    {
        int xPos = 0, yPos = 0;
        for (int x = 0; x < alldots.GetLength(0); x++)
        for (int y = 0; y < alldots.GetLength(1); y++)
        {
            if (alldots[x, y] == square)
            {
                xPos = x; yPos = y;
            }
        }

        return new Vector2Int(xPos, yPos);
    }

    public static  Vector2Int ModifyDirectionColRow(int column, int row, Directions direction)
    {
        switch (direction)
        {
            case Directions.bottom: row -= 1; break;
            case Directions.top: row += 1; break;
            case Directions.left: column -= 1; break;
            case Directions.right: column += 1; break;
        }

        return new Vector2Int(column, row);
    }
    public static List<GameObject> CheckForBlankSpaces(int column, int row, Directions direction, GameObject[,] allDots, ref List<GameObject> wayPoints)
    {
        Vector2Int modifedColRow = ModifyDirectionColRow(column, row, direction);
        column = modifedColRow.x;
        row = modifedColRow.y;
        
        if (column < 0 || row < 0 || column > allDots.GetLength(0) - 1 || row > allDots.GetLength(1)-1)
            return wayPoints;
        
        wayPoints.Add(allDots[column, row]);
        if (allDots[column, row].GetComponent<SpriteRenderer>().enabled &&
            allDots[column, row].GetComponent<Square>() && 
            allDots[column, row].GetComponent<Square>().item != null)
        {
            return wayPoints;
        }
        
        return CheckForBlankSpaces(column, row, direction, allDots, ref wayPoints);
    }
    public static Vector2Int CheckForBlankSpaces(int column, int row, int nextColumnMod, int nextRowMod,
        ref GameObject[,] allDots, ref List<GameObject> SquareObjs, ref bool onlyDot)
    {
        
        if (allDots[column + nextColumnMod, row + nextRowMod].transform.GetComponent<SpriteRenderer>().enabled &&
            allDots[column + nextColumnMod, row + nextRowMod].GetComponent<Square>() != null &&
            allDots[column + nextColumnMod, row + nextRowMod].GetComponent<Square>().item != null)
        {
            return new Vector2Int(nextColumnMod, nextRowMod);
        }
        
    if (nextColumnMod < 0) nextColumnMod -= 1 ;
        else if (nextColumnMod > 0) nextColumnMod += 1 ;
        if (nextRowMod < 0) nextRowMod -= 1;
        else if (nextRowMod > 0) nextRowMod += 1;
        
        if (column + nextColumnMod == LoadingHelper.THIS.width || row + nextRowMod == LoadingHelper.THIS.height ||
            column + nextColumnMod == -1 || row + nextRowMod == -1)
        {
            onlyDot = true;
            Debug.Log("Returning -9999");
            return new Vector2Int(-9999, -9999);
        }
        SquareObjs.Add(allDots[column + nextColumnMod, row + nextRowMod]);
        return CheckForBlankSpaces(column, row, nextColumnMod, nextRowMod, ref allDots, ref SquareObjs, ref onlyDot);
    }

    public static Vector2 ModifiedModScreens(float modScreenDstX, float modScreenDstY, Directions direction)
    {  //modifies it work
        float modScreenDst = Mathf.Abs(modScreenDstX) + Mathf.Abs(modScreenDstY);
        switch (direction)
        {
            case Directions.left:
                return new Vector2(modScreenDst* (-1), 0);

            case Directions.right:
                return new Vector2(modScreenDst, 0);
            case Directions.top:
                return new Vector2(0, modScreenDst);

            case Directions.bottom:
                return new Vector2(0, modScreenDst*(-1));
        }

        return Vector2.zero;
    }
}

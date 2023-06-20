using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed  class SideDotFirstDot : MonoBehaviour
{
    public static GameObject RecurseUntilFirstDot(int width, int height,int minWidth, int minHeight, int column, int row, Directions direction, ref GameObject[,] allDots, GameObject lastGameObjectMoveNextDot)
    {
        GameObject newSideDot = SideDotLastDot.GetLastDotObj(width, height,  minWidth, minHeight, column, row, direction,ref allDots, lastGameObjectMoveNextDot);
        if (newSideDot != null) return newSideDot;
    
        switch (direction)
        {
            case Directions.top:
                return SideDotLastDot.GetLastDotObj(width, height-1,  minWidth, minHeight,column, row, direction, ref allDots, lastGameObjectMoveNextDot);
    
            case Directions.bottom:
                return SideDotLastDot.GetLastDotObj(width, height, minWidth, minHeight+1,column, row, direction, ref allDots, lastGameObjectMoveNextDot);
 
            case Directions.left:
                return SideDotLastDot.GetLastDotObj(width, height,  minWidth+1, minHeight,column, row, direction, ref allDots, lastGameObjectMoveNextDot);
  
            case Directions.right:
                return SideDotLastDot.GetLastDotObj(width-1, height, minWidth, minHeight, column, row, direction, ref allDots, lastGameObjectMoveNextDot);
        
            default: newSideDot = null;
                break;
        }

        return newSideDot;
    }
    
}

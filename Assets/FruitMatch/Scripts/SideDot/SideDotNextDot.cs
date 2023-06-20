using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FruitMatch.Scripts.Blocks;
using UnityEngine;

public sealed class SideDotNextDot : MonoBehaviour
{

    public static void MoveToNextDot(GameObject firstDotAimSquare, Directions direction, GameObject[,] alldots, ref GameObject lastDotSqrObject, ref Queue<MoveTowardsTarget> helperQueue, ref List<DotObject> needDotComponent)
    {
        int xPos = 0, yPos = 0;
        for (int x = 0; x < alldots.GetLength(0); x++)
        for (int y = 0; y < alldots.GetLength(1); y++)
        {
            if (alldots[x, y] == firstDotAimSquare)
            {
                xPos = x; yPos = y;
            }
        }
       
        switch (direction)
      {
          case Directions.bottom:
              for (int i = yPos; i > -1; i--) Animation(xPos, i, direction, alldots, ref lastDotSqrObject,ref helperQueue, ref needDotComponent);
              break;
          case Directions.top:
              for (int i = yPos; i <alldots.GetLength(1); i++) Animation(xPos, i, direction, alldots, ref lastDotSqrObject, ref helperQueue, ref needDotComponent);
              break;
          case Directions.left:
              for (int i = xPos; i > -1; i--) Animation(i, yPos, direction, alldots, ref lastDotSqrObject, ref helperQueue, ref needDotComponent);
              break;
          case Directions.right:
              for (int i = xPos; i <alldots.GetLength(0); i++)Animation(i, yPos, direction, alldots, ref lastDotSqrObject, ref helperQueue, ref needDotComponent);
              break;
      }

        if(lastDotSqrObject != null && lastDotSqrObject.GetComponent<Square>().item.gameObject != null && helperQueue.Count > 0)helperQueue = RemoveLastDot(helperQueue, lastDotSqrObject.GetComponent<Square>().item.gameObject);
        if(lastDotSqrObject  != null && lastDotSqrObject.GetComponent<Square>().item.gameObject != null && needDotComponent.Count > 0)   needDotComponent = RemoveLastDot(needDotComponent,lastDotSqrObject.GetComponent<Square>().item.gameObject  );
    }
    private static  Queue<MoveTowardsTarget> RemoveLastDot( Queue<MoveTowardsTarget> queToModifiy, GameObject objToRemove)
    {
        List<MoveTowardsTarget> toList = queToModifiy.ToList();
        Queue<MoveTowardsTarget> newQueue = new Queue<MoveTowardsTarget>();
        for (int i = 0; i < toList.Count; i++)
        {
            if (toList[i].objectToMove != objToRemove)
                newQueue.Enqueue(toList[i]);
        }
        return newQueue;
    }
    private static  List<DotObject> RemoveLastDot( List<DotObject> queToModifiy, GameObject objToRemove)
    {
        List<DotObject> toList = queToModifiy;
        Queue<DotObject> newQueue = new Queue<DotObject>();
        for (int i = 0; i < toList.Count; i++)
        {
            if (toList[i].DotGameObject != objToRemove)
                newQueue.Enqueue(toList[i]);
        }
        return newQueue.ToList();
    }
    private static void Animation(int column, int row, Directions direction, GameObject[,] allDots, ref GameObject lastDotSqrObject, ref Queue<MoveTowardsTarget> helperQueue, ref List<DotObject> needDotComponent)
    {
        if(allDots[column,row] == null || !allDots[column,row].GetComponent<SpriteRenderer>().enabled || allDots[column, row].GetComponent<Square>().item == null) return;
        
        List<GameObject> wayPoints = new List<GameObject>();
        wayPoints?.Clear();
          wayPoints.Add(allDots[column,row]);
          
        SideDotUtil.CheckForBlankSpaces(column, row, direction, allDots, ref wayPoints);
        // if (wayPoints[0].GetComponent<Square>().item == null)
        // {
        //     wayPoints = RemoveFromList(ref wayPoints);
        // }
        int counter = 0;
        int index = 0;
        for (int i = 0; i < wayPoints.Count; i++)
        {
            
            if (wayPoints[i].GetComponent<SpriteRenderer>().enabled && wayPoints[i].GetComponent<Square>().item != null)
            {
                lastDotSqrObject = wayPoints[i];
                counter++;
            }

            if (counter == 2)
            {
                index = i;
            }
        }
     //   Debug.Log("WPOINTS COUNT: " + wayPoints.Count);
      //  Debug.Log(" --------------");

        if (wayPoints.Count == 0) return;
        int nextSquare = 0;
      
   
        Vector2Int colRow = SideDotUtil.GetXYFromNormalXYOBJ(wayPoints[index], allDots);

        
        
        Debug.Log("wayPoints[0].GetComponent<Square>().name::: :" + wayPoints[nextSquare].GetComponent<Square>().name);
          needDotComponent.Add(new DotObject(wayPoints[nextSquare].GetComponent<Square>().item.gameObject, allDots[colRow.x,colRow.y]));
           helperQueue.Enqueue(new MoveTowardsTarget(wayPoints[nextSquare].GetComponent<Square>().item.gameObject, wayPoints, Rl.settings.GetAnimationSpeedLinePush));

        // if(allDots[colRow.x,colRow.y] != null && wayPoints[0].GetComponent<Square>().item != null) needDotComponent.Add(new DotObject(wayPoints[0].GetComponent<Square>().item.gameObject, allDots[colRow.x,colRow.y]));
        // if( wayPoints[0].GetComponent<Square>().item != null) helperQueue.Enqueue(new MoveTowardsTarget(wayPoints[0].GetComponent<Square>().item.gameObject, wayPoints, Rl.settings.GetAnimationSpeedLinePush));
        // else
        // { //null the next
        //     Vector2Int modifiedCR = SideDotUtil.ModifyDirectionColRow(column, row, direction);
        //     Debug.Log("modifiedCR: " + modifiedCR.x + " | " + modifiedCR.y);
        //     allDots[modifiedCR.x, modifiedCR.y].GetComponent<Square>().item = null;
        // }
    }

    private static List<GameObject> RemoveFromList(ref List<GameObject> wayPoints)
    {
        for (int i = 0; i < wayPoints.Count; i++)
        {
            if (wayPoints[i].GetComponent<Square>().item == null)
            {
                wayPoints.RemoveAt(i);
                return RemoveFromList(ref wayPoints);
            }
        }

        return wayPoints;
    }
}
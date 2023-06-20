using System.Collections.Generic;
using FruitMatch.Scripts.Blocks;
using Unity.Mathematics;
using UnityEngine;
public sealed class SideDotLastDot : MonoBehaviour
{
    public static void  MoveLastDot(GameObject lastDotObject, ref GameObject otherSideDot, GameObject[,] allDots, Directions direction, ref List<SideDotComponent> needSideDotComponents, ref Queue<MoveTowardsTarget> helperQueue)
    {
        if (lastDotObject == null) return;
        GameObject itemObj = lastDotObject.GetComponent<Square>().item.gameObject;
        SideDot.ItemEnableDisable(ref itemObj, false);
        
        if (otherSideDot == null)
        {
            otherSideDot = itemObj;
            return;
        }
        
       // itemObj.transform.position = otherSideDot.transform.position;
        SideDot.ItemEnableDisable(ref lastDotObject, false);
        
        List<GameObject> wayPoints = new List<GameObject>();
        Vector2Int pos = SideDotUtil.GetXYFromNormalXYOBJ(lastDotObject, allDots);
        SideDotUtil.CheckForBlankSpaces( pos.x, pos.y, direction, allDots, ref wayPoints);
        GameObject dummyObj = new GameObject();
       
        dummyObj   = Instantiate(dummyObj, otherSideDot.transform.position, quaternion.identity);
        dummyObj.AddComponent<DummyObj>();
        wayPoints.Add(dummyObj.gameObject );
        
   
        helperQueue.Enqueue(new MoveTowardsTarget(itemObj, wayPoints,  Rl.settings.GetAnimationSpeedLinePush));
        needSideDotComponents.Add(new SideDotComponent(itemObj, otherSideDot.GetComponent<SideDot>().columnSideDot, otherSideDot.GetComponent<SideDot>().rowSideDot, false, SideDotType.moveLine));
    }
    public static GameObject GetLastDotObj(int width, int height, int minWidth, int minHeight, int column, int row, Directions direction, ref GameObject[,] allDots, GameObject lastGameObjectMoveNextDot)
    {
        switch (direction)
        {
            case Directions.right:
                if(allDots[width - 1, row])
                    return allDots[width - 1, row];
                return lastGameObjectMoveNextDot;
            case Directions.top:
                if(allDots[column, height - 1]) 
                    return allDots[column, height - 1];
                return lastGameObjectMoveNextDot;
            case Directions.left:
                if(allDots[minWidth, row])
                    return allDots[minWidth, row];
                return lastGameObjectMoveNextDot;
            case Directions.bottom: 
                if( allDots[column, minHeight])
                    return allDots[column, minHeight];
                return lastGameObjectMoveNextDot;
        }
        return null;
    }
}
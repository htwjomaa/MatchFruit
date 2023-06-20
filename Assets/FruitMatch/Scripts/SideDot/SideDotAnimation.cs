using System.Collections;
using System.Collections.Generic;
using FruitMatch.Scripts.Blocks;
using FruitMatch.Scripts.Items;
using UnityEngine;
using DG.Tweening;
public sealed class SideDotAnimation : MonoBehaviour
{
    public static void AnimateAllDots(ref Queue<MoveTowardsTarget> moveTowardsTargets)
    {
        //Animates the Dots movement by emptying a queue. If movement is not finished yet, it gets added to a new queue which gets reassigned at the end
        Queue<MoveTowardsTarget> helperQueueForIteration = new Queue<MoveTowardsTarget>(); //helperQueue because you can't modify the queue you iterating trough
        foreach (MoveTowardsTarget dot in moveTowardsTargets)
        {
            if (MathLibrary.CalculateDistance(dot.objectToMove, dot.wayPoints[0].gameObject) >
                0.03f) //Is it close enough to "Snap" to place? No then lerp
            {

                //dot.objectToMove.transform.DOMove(dot.wayPoints[0].transform.position, 0.5f, false);
                // dot.objectToMove.transform.position = Vector3.Slerp(
                //     dot.objectToMove.transform.position,
                //     dot.wayPoints[0].transform.position,
                //     dot.lerpFactor
                // );
               // LeanTween.moveLocal(dot.objectToMove, dot.wayPoints[0].transform.position, 0.5f);
                dot.objectToMove.transform.position = 
                    
                    Vector3.Slerp(
                    dot.objectToMove.transform.position,
                    dot.wayPoints[0].transform.position,
                    dot.lerpFactor*85 *Time.deltaTime + ((LoadingHelper.THIS.width*LoadingHelper.THIS.height* dot.wayPoints.Count)*0.00015f)
                );
                helperQueueForIteration.Enqueue(new MoveTowardsTarget(dot.objectToMove, dot.wayPoints, dot.lerpFactor));
            }
            else
            {
                dot.objectToMove.transform.position = dot.wayPoints[0].transform.position;
                if (dot.wayPoints.Count > 0)
                {
                    dot.wayPoints.Remove(dot.wayPoints[0]);
                }

                if (dot.wayPoints.Count > 0)
                    helperQueueForIteration.Enqueue(new MoveTowardsTarget(dot.objectToMove, dot.wayPoints,
                        dot.lerpFactor));
            }

            moveTowardsTargets = helperQueueForIteration;
        }
    }

    public static void ModifySquareItemRef(GameObject item, GameObject targetSquare)
    {
        if(targetSquare == null) return;
        Debug.Log("ITEM: " + item.name + " || SQUARE: COLUMN:" + targetSquare.GetComponent<Square>().col +" ROW:" + targetSquare.GetComponent<Square>().row);
     //   targetSquare.GetComponent<Square>().item = item.GetComponent<ItemSimple>();
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using FruitMatch.Scripts.Blocks;
using FruitMatch.Scripts.Items;
using UnityEngine;
using UnityEngine.UI;

public sealed class SideDotBugSecurity : MonoBehaviour
{
     // ---- BugSecurity methods ---- //
     public static void ConvertToRightQueue(ref Queue<MoveTowardsTarget> collectedMovePositions, ref Queue<MoveTowardsTarget> helperQueue, ref Queue<MoveTowardsTarget> rightQueue)
   { //Helpermethod to avoid an accidently Update Tick
       collectedMovePositions = new Queue<MoveTowardsTarget>();
       while (helperQueue.Count > 0) rightQueue.Enqueue(
           helperQueue.Dequeue());
       helperQueue = null;
   }
     
     public static void ChangeSquareRefs(ref List<DotObject> dotComponentToAdd)
     {//Change enabled and disabled
         for (int i = 0; i < dotComponentToAdd.Count; i++)
         {
             if (dotComponentToAdd[i].DotGameObject == null) continue;
            // if (dotComponentToAdd.Count - 1 == i)
           //  {
                 GameObject n = dotComponentToAdd[i].DotGameObject;
                 dotComponentToAdd[i] = new DotObject(n, dotComponentToAdd[i].TargetSquare);
                 //     }

             if (dotComponentToAdd[i].TargetSquare != null)
             {
                 if (dotComponentToAdd[i].DotGameObject.GetComponent<ItemSimple>() != null)
                 {
                     dotComponentToAdd[i].DotGameObject.GetComponent<ItemSimple>().square = dotComponentToAdd[i].TargetSquare.GetComponent<Square>();
                     dotComponentToAdd[i].TargetSquare.GetComponent<Square>().item = dotComponentToAdd[i].DotGameObject.GetComponent<ItemSimple>();
                 }

                 if (dotComponentToAdd[i].DotGameObject.GetComponent<ItemStriped>() != null)
                 {
                     dotComponentToAdd[i].DotGameObject.GetComponent<ItemStriped>().square = dotComponentToAdd[i].TargetSquare.GetComponent<Square>();
                     dotComponentToAdd[i].TargetSquare.GetComponent<Square>().item = dotComponentToAdd[i].DotGameObject.GetComponent<ItemStriped>();
                 }
                 if (dotComponentToAdd[i].DotGameObject.GetComponent<ItemMarmalade>() != null)
                 {
                     dotComponentToAdd[i].DotGameObject.GetComponent<ItemMarmalade>().square = dotComponentToAdd[i].TargetSquare.GetComponent<Square>();
                     dotComponentToAdd[i].TargetSquare.GetComponent<Square>().item = dotComponentToAdd[i].DotGameObject.GetComponent<ItemMarmalade>();
                 }
                 
                 if (dotComponentToAdd[i].DotGameObject.GetComponent<ItemPackage>() != null)
                 {
                     dotComponentToAdd[i].DotGameObject.GetComponent<ItemPackage>().square = dotComponentToAdd[i].TargetSquare.GetComponent<Square>();
                     dotComponentToAdd[i].TargetSquare.GetComponent<Square>().item = dotComponentToAdd[i].DotGameObject.GetComponent<ItemPackage>();
                 }
                 if (dotComponentToAdd[i].DotGameObject.GetComponent<ItemIngredient>() != null)
                 {
                     dotComponentToAdd[i].DotGameObject.GetComponent<ItemIngredient>().square = dotComponentToAdd[i].TargetSquare.GetComponent<Square>();
                     dotComponentToAdd[i].TargetSquare.GetComponent<Square>().item = dotComponentToAdd[i].DotGameObject.GetComponent<ItemIngredient>();
                 }
                 if (dotComponentToAdd[i].DotGameObject.GetComponent<ItemSpiral>() != null)
                 {
                     dotComponentToAdd[i].DotGameObject.GetComponent<ItemSpiral>().square = dotComponentToAdd[i].TargetSquare.GetComponent<Square>();
                     dotComponentToAdd[i].TargetSquare.GetComponent<Square>().item = dotComponentToAdd[i].DotGameObject.GetComponent<ItemSpiral>();
                 }
                 if (dotComponentToAdd[i].DotGameObject.GetComponent<ItemMulticolor>() != null)
                 {
                     dotComponentToAdd[i].DotGameObject.GetComponent<ItemMulticolor>().square = dotComponentToAdd[i].TargetSquare.GetComponent<Square>();
                     dotComponentToAdd[i].TargetSquare.GetComponent<Square>().item = dotComponentToAdd[i].DotGameObject.GetComponent<ItemMulticolor>();
                 }
                 // if (dotComponentToAdd[i].DotGameObject.GetComponent<Item>() != null)
                 // {
                 //     dotComponentToAdd[i].DotGameObject.GetComponent<Item>().square = dotComponentToAdd[i].TargetSquare.GetComponent<Square>();
                 //     dotComponentToAdd[i].TargetSquare.GetComponent<Square>().item = dotComponentToAdd[i].DotGameObject.GetComponent<Item>();
                 // }
             }
             SideDot.ItemEnableDisable( ref n, true);      
         }
     }
     
    public static void ReportIamWrong(int column, int row)
    {
        if (column < 0) Debug.Log("COLUMN BROKEN " + column);
        if (row < 0) Debug.Log("ROW BROKEN " + row);

        if (column > LoadingHelper.THIS.width - 1) Debug.Log("COLUMN BROKEN " + column);
        if (row > LoadingHelper.THIS.height - 1) Debug.Log("ROW BROKEN " + row);
    }
    
}

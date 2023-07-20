using System.Collections;
using System.Collections.Generic;
using FruitMatch.Scripts.Blocks;
using Unity.Mathematics;
using UnityEngine;

public class SideDotOtherSideDot : MonoBehaviour
{
    
    public static GameObject GetOtherSideDot(int minWidth, int minHeight, int columnSideDot, int rowSideDot, ref Directions direction, ref GameObject[,] allDots, SideDotType sideDotType, ref GameObject lastGameObjectMoveNextDot)
    { // Finds out what the Dot on the sidelines on the opposite Side is

        int width = allDots.GetLength(0);
        int height = allDots.GetLength(1);
        switch (sideDotType)
        {
            case SideDotType.turnA:
                foreach (SideDot sideDot in FindObjectsOfType<SideDot>())
                {
                    if (direction == Directions.right && sideDot.columnSideDot == 0 && sideDot.rowSideDot == -1 )
                    {
                        direction = Directions.bottom;
                        return  sideDot.gameObject;  //right and down
                    }
                    if (direction == Directions.left && sideDot.columnSideDot == width-1 && sideDot.rowSideDot == -1) //left and down
                    {
                        direction = Directions.bottom;
                        return sideDot.gameObject;
                    }
                    if (direction == Directions.top && sideDot.columnSideDot == -1 && sideDot.rowSideDot == 0) // top and left
                    {
                        direction = Directions.left;
                        return sideDot.gameObject;
                    }
                    if (direction == Directions.bottom && sideDot.columnSideDot == -1 && sideDot.rowSideDot == height-1) //bottom and left
                    {
                        direction = Directions.left;
                        return sideDot.gameObject; 
                    }
                }
                break;
            case SideDotType.turnB:
                foreach (SideDot sideDot in FindObjectsOfType<SideDot>())
                {
                    if (direction == Directions.right && sideDot.columnSideDot == 0 && sideDot.rowSideDot == height)   //right and up
                    {        
                        direction = Directions.top;
                        return  sideDot.gameObject; 
                    }
                    if (direction == Directions.left && sideDot.columnSideDot == width - 1 && sideDot.rowSideDot == height) //left and up
                    {  
                        direction = Directions.top;
                        return sideDot.gameObject; 
                    }

                    if (direction == Directions.top && sideDot.columnSideDot == width && sideDot.rowSideDot == 0) //top and right
                    {
                        direction = Directions.right;
                        return sideDot.gameObject; 
                    }
                    
                    if (direction == Directions.bottom && sideDot.columnSideDot == width && sideDot.rowSideDot == height - 1) //bottom and right
                    {
                        direction = Directions.right;
                        return sideDot.gameObject; 
                    }
                }
                break;
            case SideDotType.moveLine:
                foreach (SideDot sideDot in FindObjectsOfType<SideDot>())
                {  
                    if (direction == Directions.right && sideDot.rowSideDot == rowSideDot && sideDot.columnSideDot == width) return sideDot.gameObject; // right to
                    if (direction == Directions.left && sideDot.rowSideDot == rowSideDot && sideDot.columnSideDot == -1) return  sideDot.gameObject; //left to right
                    if (direction == Directions.top && sideDot.columnSideDot == columnSideDot && sideDot.rowSideDot == height) return  sideDot.gameObject; //bottom to top
                    if (direction == Directions.bottom && sideDot.columnSideDot == columnSideDot && sideDot.rowSideDot == -1) return sideDot.gameObject; //top to bottom
                }
                break;
        }

         //if null. This is a nullcheck if the other SideObject does not exist
        switch (sideDotType)
        {
            case SideDotType.turnA:
                if (direction is Directions.left)
                {
                    direction = Directions.bottom;
                    return NoOtherSideDotFound(width, height,  minWidth, minHeight,width-1, height, direction, ref allDots, ref lastGameObjectMoveNextDot);
                }

                if (direction is Directions.right)
                {
                    direction = Directions.bottom;
                    return NoOtherSideDotFound(width, height, minWidth, minHeight,0, height, direction, ref allDots, ref lastGameObjectMoveNextDot);
                }

                if (direction is Directions.top)
                {
                    direction = Directions.left;
                    return NoOtherSideDotFound(width, height,  minWidth, minHeight,width, 0, direction, ref allDots, ref lastGameObjectMoveNextDot);
                }

                if (direction is Directions.bottom)
                {
                    direction = Directions.left;
                    return NoOtherSideDotFound(width, height, minWidth, minHeight,width, height-1, direction, ref allDots, ref lastGameObjectMoveNextDot);
                }
                break;
            

            case SideDotType.turnB:
                if (direction is Directions.right)
                {
                    direction = Directions.top;
                    return NoOtherSideDotFound(width, height, minWidth, minHeight,0, -1, direction, ref allDots, ref lastGameObjectMoveNextDot);
                }

                if (direction is Directions.left)
                {
                    direction = Directions.top;
                    return NoOtherSideDotFound(width, height,  minWidth, minHeight,width-1, -1, direction, ref allDots, ref lastGameObjectMoveNextDot);
                }

                if (direction is Directions.top)
                {
                    direction = Directions.right;
                    return NoOtherSideDotFound(width, height,  minWidth, minHeight,-1, 0, direction, ref allDots,ref  lastGameObjectMoveNextDot);
                }

                if (direction is Directions.bottom)
                {
                    direction = Directions.right;
                    return NoOtherSideDotFound(width, height, minWidth, minHeight,-1, height-1, direction, ref allDots, ref lastGameObjectMoveNextDot);
                }
                break;

            case SideDotType.moveLine:   //column und row statt columnd sideDot?
                if (direction == Directions.right) return NoOtherSideDotFound(width, height, minWidth, minHeight,columnSideDot, rowSideDot, Directions.right, ref allDots, ref lastGameObjectMoveNextDot);
                if (direction == Directions.left) return NoOtherSideDotFound(width, height,  minWidth, minHeight,columnSideDot, rowSideDot, Directions.left, ref allDots, ref lastGameObjectMoveNextDot);
                if (direction == Directions.top) return NoOtherSideDotFound(width, height, minWidth, minHeight,columnSideDot, rowSideDot, Directions.top, ref allDots, ref lastGameObjectMoveNextDot);
                if (direction == Directions.bottom) return NoOtherSideDotFound(width, height,  minWidth, minHeight,columnSideDot, rowSideDot, Directions.bottom, ref allDots, ref lastGameObjectMoveNextDot);
                break;
        }
        return null;
    }
    
    private static GameObject NoOtherSideDotFound(int width, int height,int minWidth, int minHeight,int column, int row, Directions direction, ref GameObject[,] allDots, ref GameObject lastGameObjectMoveNextDot)
    {
        //GameObject newSideDot= SideDotFirstDot.RecurseUntilFirstDot(width, height,  minWidth, minHeight,column, row, direction,ref allDots, lastGameObjectMoveNextDot);
  
       // Debug.Log("Not Other Side Dot found");
       // if(newSideDot != null) newSideDot.AddComponent<SideDot>();
     //  lastGameObjectMoveNextDot.AddComponent<SideDot>();
     return null;
    }
    public static void MoveOtherSideDot(GameObject otherSideDot, int width, int height, int column, int row, SideDotType sideDotType, Directions direction, Transform transform,
        ref Queue<MoveTowardsTarget> helperQueue, ref Queue<OtherSideDotPos> otherSideDots, ref List<SideDotComponent> needSideDotComponent)
    { //Moves or adds to a moving Queue and modifies its variables to the the Dot in the Sidelines that we clicked in (the firstSideDot)
        // if (otherSideDot == null) return;   // nullcheck because of slime

      
        OtherSideDotAnimation(otherSideDot, width, height, column, row, sideDotType, direction, transform, ref helperQueue, ref  otherSideDots);
        //otherSideDot.transform.position = transform.position;

        sideDotType = SideDotType.moveLine;
        bool hasSideDot = !(otherSideDot.GetComponent<SideDot>() == null);
        if (hasSideDot) sideDotType = otherSideDot.GetComponent<SideDot>().sideDotType;
        needSideDotComponent.Add(new SideDotComponent(otherSideDot, column, row, hasSideDot, sideDotType));
        if (hasSideDot)
        {
            Destroy(otherSideDot.GetComponent<SideDot>().iconObj);
        }

        SideDot.OtherSideDotForPrepapre = otherSideDot;
        // otherSideDot.AddComponent<SideDot>();
        // otherSideDot.GetComponent<SideDot>().columnSideDot = column;
        // otherSideDot.GetComponent<SideDot>().rowSideDot = row;
        //otherSideDot.GetComponent<SideDot>().SetIconIdentifier();
    }


    private static void OtherSideDotAnimation(GameObject otherSideDot, int width, int height, int column, int row, SideDotType sideDotType, Directions direction, Transform transform,
        ref Queue<MoveTowardsTarget> helperQueue, ref Queue<OtherSideDotPos> otherSideDots)
    {   //Using a Queue for later if I want to move multiple Dots at once
        
        //Later  I might need to do a null Check   --- yap xD
        //if (otherSideDot == null) return;  // nullcheck because of slime
       // float calcDistFirstAndLastSideDot =  MathLibrary.CalculateDistance(otherSideDot.transform.position, transform.position);
        //Guarding if something goes crazy wrong
       // if (calcDistFirstAndLastSideDot < 0.3f) calcDistFirstAndLastSideDot = 0.3f;
       // else if(calcDistFirstAndLastSideDot > 20f) calcDistFirstAndLastSideDot = 20f;
        //The Off-Set Distance gets Calc here
       // float modScreenDst = calcDistFirstAndLastSideDot / 1.5f,
        float modScreenDst = 2f,
            modScreenDstX = 0, modScreenDstY = 0;
       
        switch (direction)
        {
            case Directions.right: 
                modScreenDstX = modScreenDst;
                break;
            case Directions.left: 
                modScreenDstX = modScreenDst*-1;
                break;
            case Directions.top: 
                modScreenDstY = modScreenDst;
                break;
            case Directions.bottom:
                modScreenDstY = modScreenDst*-1;
                break;
        }
        OtherSideDotAnimationHelperMethod(otherSideDot, width, height,column, row, modScreenDstX, modScreenDstY, sideDotType, transform, ref helperQueue, ref otherSideDots);
    }

    private static void OtherSideDotAnimationHelperMethod(GameObject otherSideDot, int width, int height, int column, int row, float modScreenDstX, float modScreenDstY, SideDotType sideDotType,
        Transform transform, ref Queue<MoveTowardsTarget> helperQueue, ref Queue<OtherSideDotPos> otherSideDots)
    {   //Helperqueue for security reason. OtherSideDots is needed because it needs to teleport after the first animation

        if (otherSideDot == null)
        {
            return;
            //  for (int i = 0; i < LoadingHelper.THIS.FieldBoard.squaresArray.Length; i++)
            // {
            //    if (LoadingHelper.THIS.FieldBoard.squaresArray[i].item == null)
            //         otherSideDot = LoadingHelper.THIS.FieldBoard.squaresArray[i].item.gameObject;
            //    }

        }
        if (otherSideDot.GetComponent<Square>() && otherSideDot.GetComponent<Square>().item != null)
        {
            otherSideDot = otherSideDot.GetComponent<Square>().item.gameObject;
        }
        
        List<GameObject> dummyList1 = new List<GameObject>();
        GameObject dummyObj1 = new GameObject();
        Vector3 pos = new Vector3(otherSideDot.transform.position.x + modScreenDstX,
            otherSideDot.transform.position.y + modScreenDstY, otherSideDot.transform.position.z);
        dummyObj1   = Instantiate(dummyObj1, pos, quaternion.identity);
        dummyObj1.AddComponent<DummyObj>();
        dummyList1.Add(dummyObj1.gameObject );
        
         helperQueue.Enqueue(new MoveTowardsTarget(
             otherSideDot, 
             dummyList1,
             Rl.settings.GetAnimationSpeedLinePush));


        if (sideDotType is SideDotType.turnA or SideDotType.turnB)
        {
            Vector2 rightSide = SideDotUtil.ModifiedModScreens(modScreenDstX, modScreenDstY,SideDotUtil.CheckDirection(width, height,column, row));
            modScreenDstX = rightSide.x;
            modScreenDstY = rightSide.y;
        }

        List<GameObject> dummyList = new List<GameObject>();
        GameObject dummyObj = new GameObject();
            dummyObj   = Instantiate(dummyObj, transform.position, quaternion.identity);
            dummyObj.AddComponent<DummyObj>();
            dummyList.Add(dummyObj.gameObject );
        otherSideDots.Enqueue(new OtherSideDotPos(new Vector3(transform.position.x-modScreenDstX, transform.position.y -modScreenDstY, transform.position.z), new MoveTowardsTarget(otherSideDot,dummyList,
            Rl.settings.GetAnimationSpeedLinePush)));
    }
}

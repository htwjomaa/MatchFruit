using UnityEngine;

public sealed class DeadLock 
{
    public static bool IsDeadlocked(int width, int height, ref GameObject[,] allDots, bool[,] blankSpaces , BoardUtil boardUtil)
 {
     for (int i = 0; i < width; i++)
     {
         for (int j = 0; j < height; j++)
         {
             if (allDots[i, j] != null)
             {
                 if (i < width - 1)
                 {
                     if (boardUtil.SwitchAndCheck(i, j, width,  height, ref allDots, blankSpaces, Vector2.right)) return false;
                 }
 
                 if (j < height - 1)
                 {
                     if (boardUtil.SwitchAndCheck(i, j, width,  height, ref allDots, blankSpaces,Vector2.up)) return false;
                 }
             }
         }
     }
     return true;
 }
}

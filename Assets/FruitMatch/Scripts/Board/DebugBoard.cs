using System.Collections.Generic;
using UnityEngine;

public sealed class DebugBoard : MonoBehaviour
{
  public bool GameStartBool = true;
  public static void AddIterationDebug(int iterationCount, ref Dictionary<int, int> iterationsDebug)
  {
    if (iterationCount > 0)
    {
      if (iterationsDebug.ContainsKey(iterationCount))
        iterationsDebug[iterationCount]++;
      else  iterationsDebug.Add(iterationCount, 1);
    }
  }

  public static void ReportIterationDebug(Dictionary<int, int> iterationsDebug)
  {
    foreach(KeyValuePair<int, int> reportMe in iterationsDebug)
      Debug.Log(reportMe.Key + " Iterations were needed " + reportMe.Value + "x times");
  }

  public void InvokeCheckBrokenElements()
  {
    InvokeRepeating("CheckForNotWorkingDots", 1.5f, 0.2f);
    Invoke("SetGameStartBoolFalse", 1.1f);
  }
  private void CheckForNotWorkingDots()
  {
    foreach (Dot dot in FindObjectsOfType<Dot>())
    {
      bool destroyme = true;
        
      foreach (GameObject allDotsDot in Rl.board.AllDots)
      {
        if (dot.gameObject == allDotsDot) destroyme = false;
      }
      if(destroyme) Destroy(dot.gameObject);
    }
  }
  
  private void SetGameStartBoolFalse() => GameStartBool = false;
}

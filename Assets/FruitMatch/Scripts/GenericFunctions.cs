using FruitMatch.Scripts.Core;
using UnityEngine;

public class GenericFunctions : MonoBehaviour
{
  public static bool IsSubstractiveState(LIMIT limitType)
  {
    switch (limitType)
    {
      case LIMIT.MOVES:
        return true;
      case LIMIT.TIME:
        return true;
      case LIMIT.AVOID:
        return false;
      default:
        return true;
    }
  }
}

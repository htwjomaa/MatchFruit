//---------------- CurveManager-------------------//
// In another project it handles much more but for now it can only 
// enable and disable the pathfinding gizmos
//----------------                    -------------------//
using UnityEngine;
using NaughtyAttributes;

public sealed class CurveManager : MonoBehaviour
{
#if UNITY_EDITOR
     [ShowNonSerializedField]    public static bool drawPathfindingInEditMode = true;
     [ShowNonSerializedField]    public static bool updatePathfinder;

     [Button("Toogle Pathfinding in Editor")]
     private void TogglePathfinding()
     {
          drawPathfindingInEditMode = !drawPathfindingInEditMode;
          if (drawPathfindingInEditMode) Debug.Log("Pathfinding now show up in the Editor");
          else Debug.Log("Pathfinding no longer show up in the Editor");
     }
#endif
}




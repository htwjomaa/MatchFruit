//----------------   RenderBounds Gizmos-----------------//
// Visualizer for the Render Bounds. Needs to be attached to a GameObject
// e.g. on the Gamemanager
//----------------                    -------------------//

using UnityEngine;
[ExecuteAlways]
public sealed class RenderBoundsGizmos : MonoBehaviour
{
    #if UNITY_EDITOR
    public void  OnDrawGizmos()
    {
        if (RenderBounds.RenderBoundsList != null && RenderBounds.RenderBoundsList.Count == 0) return;
        Gizmos.color = Color.magenta;
        Mesh m;
        foreach (MeshFilter mf in RenderBounds.RenderBoundsList)
        {
            if (mf.sharedMesh != null)
            {
            Gizmos.matrix = mf.transform.localToWorldMatrix;
            m = mf.sharedMesh;
                Gizmos.DrawWireCube(m.bounds.center, m.bounds.size);
            }
        }
    }
#endif
}


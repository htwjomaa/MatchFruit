#if UNITY_EDITOR
//----------------   RenderBounds -------------------//
// This class does:
// 1. visualize the Render Bounds
// 2. change the Render Bounds
// 3. Reset the Render Bounds
// It offers different selection methods, e.g. ignoring Tags
//----------------                    -------------------//
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public sealed class RenderBounds : EditorWindow
{
    public static readonly HashSet<MeshFilter> RenderBoundsList = new HashSet<MeshFilter>();
    private  static readonly HashSet<MeshFilter> HasAllBoundsToModify = new HashSet<MeshFilter>();
    private const float LeftBoundMult = 0;
    private const float RightBoundMult = 11;
    private static float _multiplicationValue = 1;
    private string ignoreTag = "";
    private static int tagNumber;
    [UnityEditor.MenuItem("FruitMatch/ RenderBounds")]
    public static void ShowWindow()
    {
        GetWindow(typeof(RenderBounds));
    }
    private void OnGUI()
    {

        GUILayout.Label("Change Tag that you want to ignore or draw Only");
        tagNumber = (EditorGUILayout.IntSlider(tagNumber, 0, UnityEditorInternal.InternalEditorUtility.tags.Length-1));
        GUILayout.Space(10);
      
        GUILayout.Label("Draw Selected");
        if (GUILayout.Button("Draw Bounds on Selected")) AddSelectedToList();
        if (GUILayout.Button("Draw Bounds Ignore Tag: " + UnityEditorInternal.InternalEditorUtility.tags[tagNumber])) drawSelectedIgnoreTag();
        if (GUILayout.Button("Draw Bounds Only Tag: " + UnityEditorInternal.InternalEditorUtility.tags[tagNumber]))
            drawSelectedOnlyWithTag();
        GUILayout.Space(18);
        GUILayout.Label("Utility");
        if (GUILayout.Button("Clear Bounds")) {RenderBoundsList.Clear(); Debug.Log("Everything cleared!!");}
        if (GUILayout.Button("Change Bounds Size")) changeBoundsSize();
        _multiplicationValue = EditorGUILayout.Slider("Multiplicationvalue", _multiplicationValue, LeftBoundMult, RightBoundMult);
        GUILayout.Space(4);
        if (GUILayout.Button("Recalculate Bounds")) recalculateBoundsSize();
        GUILayout.Space(18);
        GUILayout.Label("Draw All Scene Objects");
        if (GUILayout.Button("Draw All")) drawAll();
        if (GUILayout.Button("Draw All Ignore Tag: " + UnityEditorInternal.InternalEditorUtility.tags[tagNumber])) drawAllIgnoreTag();
        if (GUILayout.Button("Draw All Only with Tag: " + UnityEditorInternal.InternalEditorUtility.tags[tagNumber])) drawAllOnlyWithTag();
    }
    
    void AddSelectedToList()
    {
        RenderBoundsList.Clear();
        foreach (GameObject showMe in Selection.gameObjects)
        {
            if (showMe.transform.GetComponent<MeshFilter>() != null) RenderBoundsList.Add(showMe.transform.GetComponent<MeshFilter>());
            if (showMe.transform.childCount > 0) foreach (MeshFilter mf in showMe.transform.GetComponentsInChildren<MeshFilter>()) RenderBoundsList.Add(mf);
        }
        Debug.Log("You see the Render bounds of : " + RenderBoundsList.Count + " Meshfilters");
    }
    void drawSelectedIgnoreTag()
    {
        RenderBoundsList.Clear();
        foreach (GameObject showMe in Selection.gameObjects)
        {
            if (!showMe.CompareTag(UnityEditorInternal.InternalEditorUtility.tags[tagNumber]))
            {
                if (showMe.transform.GetComponent<MeshFilter>() != null)
                    RenderBoundsList.Add(showMe.transform.GetComponent<MeshFilter>());
                if (showMe.transform.childCount > 0) foreach (MeshFilter mf in showMe.transform.GetComponentsInChildren<MeshFilter>()) RenderBoundsList.Add(mf);
            }
        }
        Debug.Log("You see the Render bounds of : " + RenderBoundsList.Count + " Meshfilters");
    }
    void drawSelectedOnlyWithTag()
    {
        RenderBoundsList.Clear();
        foreach (GameObject showMe in Selection.gameObjects)
        {
            if (showMe.CompareTag(UnityEditorInternal.InternalEditorUtility.tags[tagNumber]))
            {
                if (showMe.transform.GetComponent<MeshFilter>() != null)
                    RenderBoundsList.Add(showMe.transform.GetComponent<MeshFilter>());
                if (showMe.transform.childCount > 0) foreach (MeshFilter mf in showMe.transform.GetComponentsInChildren<MeshFilter>()) RenderBoundsList.Add(mf);
            }
        }
        Debug.Log("You see the Render bounds of : " + RenderBoundsList.Count + " Meshfilters");
    }
    void drawAll()
    {
        for (ushort j = 0; j < UnityEngine.SceneManagement.SceneManager.sceneCount; j++)
        {
            foreach (GameObject allParentObjects in UnityEngine.SceneManagement.SceneManager.GetSceneAt(j)
                .GetRootGameObjects())
            {
                foreach (MeshFilter mf in allParentObjects.GetComponentsInChildren<MeshFilter>())
                {
                    if (mf != null) RenderBoundsList.Add(mf);
                }
            }
            Debug.Log("You see the Render bounds of : " + RenderBoundsList.Count + " Meshfilters");
        }
    }
    void drawAllOnlyWithTag()
    {
        for (ushort j = 0; j < UnityEngine.SceneManagement.SceneManager.sceneCount; j++)
        {
            foreach (GameObject allParentObjects in UnityEngine.SceneManagement.SceneManager.GetSceneAt(j)
                .GetRootGameObjects())
            {
                foreach (MeshFilter mf in allParentObjects.GetComponentsInChildren<MeshFilter>())
                {
                    if (mf != null && mf.CompareTag(UnityEditorInternal.InternalEditorUtility.tags[tagNumber])) RenderBoundsList.Add(mf);
                }
            }
            Debug.Log("You see the Render bounds of : " + RenderBoundsList.Count + " Meshfilters");
        }
    }
    void drawAllIgnoreTag()
    {
        for (ushort j = 0; j < UnityEngine.SceneManagement.SceneManager.sceneCount; j++)
        {
            foreach (GameObject allParentObjects in UnityEngine.SceneManagement.SceneManager.GetSceneAt(j)
                .GetRootGameObjects())
            {
                foreach (MeshFilter mf in allParentObjects.GetComponentsInChildren<MeshFilter>())
                    {
                        if (mf != null && !mf.CompareTag(UnityEditorInternal.InternalEditorUtility.tags[tagNumber])) RenderBoundsList.Add(mf);
                    }
            }
            Debug.Log("You see the Render bounds of : " + RenderBoundsList.Count + " Meshfilters");
        }
    }
    void changeBoundsSize()
    {
        HashSet<Mesh> uniqueSharedMeshes = new HashSet<Mesh>();

        foreach (MeshFilter mf in RenderBoundsList) 
            uniqueSharedMeshes.Add(mf.sharedMesh);

        List <Mesh> uniqueSharedMeshesList = uniqueSharedMeshes.ToList();
        
        if (Selection.gameObjects.Length == 0)
        {
            drawAllIgnoreTag();
            foreach (Mesh mesh in uniqueSharedMeshesList)
            {
                mesh.RecalculateBounds();
                mesh.bounds = new Bounds(mesh.bounds.center, new Vector3(mesh.bounds.size.x * _multiplicationValue, mesh.bounds.size.y, mesh.bounds.size.z * _multiplicationValue));
            }
        }
        
        foreach (GameObject showMe in Selection.gameObjects)
        {
            if (showMe.CompareTag(ignoreTag)) foreach (MeshFilter mf in showMe.transform.GetComponentsInChildren<MeshFilter>()) HasAllBoundsToModify.Add(mf);
            if (!showMe.CompareTag(ignoreTag))
            {
                if (showMe.transform.GetComponent<MeshFilter>() != null) RenderBoundsList.Add(showMe.transform.GetComponent<MeshFilter>());
                if (showMe.transform.childCount > 0) foreach (MeshFilter mf in showMe.transform.GetComponentsInChildren<MeshFilter>()) HasAllBoundsToModify.Add(mf);
            }
        }
        Debug.Log("You see the Render bounds of : " + HasAllBoundsToModify.Count + " Meshfilters");
        foreach (MeshFilter mf in HasAllBoundsToModify)
        {
            mf.sharedMesh.RecalculateBounds();
            mf.sharedMesh.bounds = new Bounds(mf.sharedMesh.bounds.center, mf.sharedMesh.bounds.size * _multiplicationValue);
        }
    }
    void recalculateBoundsSize()
    {
        HashSet<Mesh> uniqueSharedMeshes = new HashSet<Mesh>();

        foreach (MeshFilter mf in RenderBoundsList)
        {
            uniqueSharedMeshes.Add(mf.sharedMesh);
        }

        List <Mesh> uniqueSharedMeshesList = uniqueSharedMeshes.ToList();
        
        if (Selection.gameObjects.Length == 0)
        {
            drawAllIgnoreTag();
            foreach (Mesh mesh in uniqueSharedMeshesList) 
                mesh.RecalculateBounds();
        }
        
        foreach (GameObject showMe in Selection.gameObjects)
        {
            if (showMe.CompareTag(UnityEditorInternal.InternalEditorUtility.tags[tagNumber])) 
                foreach (MeshFilter mf in showMe.transform.GetComponentsInChildren<MeshFilter>()) HasAllBoundsToModify.Add(mf);
            if (!showMe.CompareTag(UnityEditorInternal.InternalEditorUtility.tags[tagNumber]))
            {
                if (showMe.transform.GetComponent<MeshFilter>() != null) RenderBoundsList.Add(showMe.transform.GetComponent<MeshFilter>());
                if (showMe.transform.childCount > 0) foreach (MeshFilter mf in showMe.transform.GetComponentsInChildren<MeshFilter>()) HasAllBoundsToModify.Add(mf);
            }
        }
        Debug.Log("Recalculating the Render bounds of : " + RenderBoundsList.Count + " Meshfilters");
        foreach (MeshFilter mf in HasAllBoundsToModify) mf.sharedMesh.RecalculateBounds();
    }
}
#endif

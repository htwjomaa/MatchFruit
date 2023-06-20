using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraScalar : MonoBehaviour
{

    [SerializeField] private GameObject backgroundImage;

  

    public void MinMaxScalar(List<Vector3> posBlankspaces, LayoutConfig layoutConfig)
    {
        
       // BoardDimensionsConfig brd
        //brd.
        //Get Information
        int width = Rl.board.width;
        int height = Rl.board.height;
        int dimension = width > height ? (int)(width * height * 1.65f): height * height;
        float zoomOutSetting = MathLibrary.Remap(0, 100, 0, max-dimension, layoutConfig.ZoomOut);
        dimension += (int)zoomOutSetting;
        Debug.LogWarning("dimension: ____" + dimension);
        float leftPadding = MathLibrary.Remap(0, 100, 0, 2, layoutConfig.LeftPadding);
        float rightPadding = MathLibrary.Remap(0, 100, 0, 2, layoutConfig.RightPadding);
        float topPadding = MathLibrary.Remap(0, 100, 0, 1f, layoutConfig.TopPadding);
        float bottomPadding = MathLibrary.Remap(0, 100, 0, 1f, layoutConfig.BottomPadding);

        dimension += (int)Math.Abs(layoutConfig.LeftPadding- layoutConfig.RightPadding) > (int)Math.Abs(layoutConfig.BottomPadding - layoutConfig.TopPadding)
            ? (int)Math.Abs(leftPadding - rightPadding) * 15
            : (int)Math.Abs(bottomPadding - topPadding) * 15;

        var cam = GetComponent<Camera>();
        //InterpolateNewValues
        
        float cameraSize = MathLibrary.Remap(min, max, minCamerSize, maxCameraSize, dimension);
        float backgroundScale = MathLibrary.Remap(min, max, minBackgroundScale, maxBackgroundScale, dimension);
   
        Vector3 BackgroundPos = MathLibrary.Remap(min, max, minBackgroundPos ,maxBackgroundPos, dimension);

        //Vector3 CameraPos = MathLibrary.Remap(min, max, minCameraPos ,maxCameraPos, dimension);
        
        //Set new Values 

        cam.orthographicSize = cameraSize;
       // cam.transform.position = CameraPos;

        List<GameObject> allDotObjects = new List<GameObject>();
        foreach (BackgroundTile dot in FindObjectsOfType<BackgroundTile>())
        {
            allDotObjects.Add(dot.gameObject);
        }

        Vector3 midPointPerDotObjects = MathLibrary.FindTheMidPoint(posBlankspaces, allDotObjects.ToArray());
        cam.transform.position = new Vector3(midPointPerDotObjects.x, midPointPerDotObjects.y, cam.transform.position.z);
        
        
        
        backgroundImage.transform.localScale = new Vector3(backgroundScale, backgroundScale, backgroundScale);
        BackgroundPos = new Vector3(BackgroundPos.x + cam.transform.position.x, BackgroundPos.y + cam.transform.position.y, BackgroundPos.z);
        backgroundImage.transform.localPosition = BackgroundPos;


       

        backgroundImage.transform.localPosition = new Vector3(backgroundImage.transform.localPosition.x - leftPadding + rightPadding,
            backgroundImage.transform.localPosition.y + topPadding - bottomPadding, backgroundImage.transform.localPosition.z);
        
        cam.transform.position = new Vector3(cam.transform.position.x - leftPadding + rightPadding,
            cam.transform.position.y + topPadding - bottomPadding, cam.transform.position.z);
        Debug.Log("width: " + width + " height: " + height + " dimension: " + dimension + " cameraSize: " + cameraSize + " backgroundScale: " + backgroundScale +
                  " BackgroundPos: " + BackgroundPos);
    }

    private int min = 9;
    private int max = 120;
    //private Vector3 minCameraPos = new (1.18f, 1.72f, -53.7f);
   // private Vector3 maxCameraPos = new (7.16f, 3.84f, -53.7f);
    
    private float minCamerSize = 3.5f;
    private float maxCameraSize = 8f;
    
    private Vector3 minBackgroundPos = new (1.7f, -1f, 18f);
    private Vector3 maxBackgroundPos = new (2.33f, 0f, 18f);
  
    
    private float minBackgroundScale = 1.14f;
    private float maxBackgroundScale = 2.6f;
    
    
    /*private Board board;
    public float cameraOffset;
    public float aspectRatio = 0.625f;
    public float padding = 2;
    
	void Start ()
    {
        board = FindObjectOfType<Board>();
        if(board!= null)
        {
            RepositionCamera(board.width - 1, board.height - 1);
        }
	}

    void RepositionCamera(float x, float y){
        Vector3 tempPosition = new Vector3(x/2, y/2, cameraOffset);
        transform.position = tempPosition;
        if (board.width >= board.height)
        {
            Camera.main.orthographicSize = (board.width / 2 + padding) / aspectRatio;
        }
        else
        {
            Camera.main.orthographicSize = board.height / 2 + padding;
        }
    }*/
}
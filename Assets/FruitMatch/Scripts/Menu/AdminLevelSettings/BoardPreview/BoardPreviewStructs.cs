using System;
using UnityEngine;

[Serializable]
public struct PreviewBoardTileSettings
{
    [Range(3,15)] public int Width;
    [Range(3,15)] public int Height;
    [Range(-60,300)] public int OffSet;
    [Range(25,250)]public float ScaleFactor;

    public PreviewBoardTileSettings(int width, int height, int offSet, float scaleFactor)
    {
        Width = width;
        Height = height;
        OffSet = offSet;
        ScaleFactor = scaleFactor;
    }
}

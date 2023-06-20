using System;
[Serializable]
public struct IpadResolution
{
    public string[] Ipads;
    public int Width;
    public int Height;

    public IpadResolution(string[] ipads, int width, int height)
    {
        Ipads = ipads;
        Width = width;
       Height = height;
    }
}
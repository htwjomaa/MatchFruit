using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public sealed class WorldLine
{
    public WorldNode PrevWorldNode;
    public List<Vector3> ControlPoints;
    public WorldNode NextWorldNode;
    
    [Range(3,40)]public byte Smoothing;
    public Sprite DeactivedLineSprite;
    public Sprite ActivatedLineSprite;

    public WorldLine(WorldNode prevWorldNode,List<Vector3> controlPoints, WorldNode nextWorldNode, byte smoothing, Sprite deactivedLineSprite, Sprite activatedLineSprite)
    {   
        if (smoothing < 7) smoothing = 7;
        if (smoothing > 40) smoothing = 40;
        PrevWorldNode = prevWorldNode;
        ControlPoints = controlPoints;
        NextWorldNode = nextWorldNode;
        Smoothing = smoothing;
        DeactivedLineSprite = deactivedLineSprite;
        ActivatedLineSprite = activatedLineSprite;
     
    }
}
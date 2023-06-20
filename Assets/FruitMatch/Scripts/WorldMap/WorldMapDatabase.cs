using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
[CreateAssetMenu]
public class WorldMapDatabase : ScriptableObject
{
    public List<GraphicCategories> NodeGraphics = new List<GraphicCategories>();
    public List<GraphicCategories> PathGraphics = new List<GraphicCategories>();
    public List<GraphicCategories> PropGraphics = new List<GraphicCategories>();
}


[Serializable]
public struct GraphicCategories
{
    public D Dimension;
    public List<PropCategories> PropCategories;
}

[Serializable]
public struct PropCategories
{
    public PropCategory PropCategory;
    public List<Spritearray> SpriteArray;
}
[Serializable]
public struct Spritearray
{
public string SpriteID;
public List<Sprite> Sprites;

}

[Serializable]
public enum D
{
    TwoD,
    ThreeD,
}

[Serializable]
public enum PropCategory
{
    Haus,
    Stein,
    Wald
}
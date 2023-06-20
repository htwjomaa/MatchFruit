using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public sealed class BlankGoal
{
    public int numberNeeded;
    public int numberCollected;
    public CollectionStyle collectionStyle;
    public Sprite goalSprite;
    public FruitType fruitType;
    public Colors fruitColors;
    public BlankGoal(int numberNeeded, int numberCollected, CollectionStyle collectionStyle, Sprite goalSprite, FruitType fruitType, Colors fruitColors)
    {
        this.numberNeeded = numberNeeded;
        this.numberCollected = numberCollected;
        this.collectionStyle = collectionStyle;
        this.goalSprite = goalSprite;
        this.fruitType = fruitType;
        this.fruitColors = fruitColors;
    }

    public BlankGoal()
    {
        numberNeeded = 30;
        numberCollected = 0;
        collectionStyle = CollectionStyle.Destroy;
        goalSprite = goalSprite;
        fruitType = FruitType.DunkelRoteFrucht;
        fruitColors = Colors.AlleFarben;
    }
}
[Serializable]
public struct MatchType
{
    public int type;
    public string color;

    public MatchType(int type, string color)
    {
        this.type = type;
        this.color = color;
    }
}
[Serializable] public  struct SideDotStat
{
    public GameObject targetSquare;
    public int column;
    public int row;

    public SideDotStat(GameObject targetSquare, int column, int row)
    {
        this.targetSquare = targetSquare;
        this.column = column;
        this.row = row;
    }
}
[Serializable] public struct MoveTowardsTarget
{
    public GameObject objectToMove;
    public List<GameObject> wayPoints;
    public float lerpFactor;

    public MoveTowardsTarget(GameObject objectToMove, List<GameObject> wayPoints, float lerpFactor)
    {
        this.objectToMove = objectToMove;
        this.wayPoints = wayPoints;
        this.lerpFactor = lerpFactor;
    }
}
[Serializable] public struct OtherSideDotPos
{
    public Vector3 teleportPos;
    public MoveTowardsTarget moveTowardsTarget;
    
    public OtherSideDotPos(Vector3 teleportPos, MoveTowardsTarget moveTowardsTarget)
    {
        this.teleportPos = teleportPos;
        this.moveTowardsTarget = moveTowardsTarget;
        
    }
}
[Serializable] public struct BackGroundTileSideList
{
    public List<GameObject> gameObjectList;
    public BackGroundTileSideList(List<GameObject> gameObjectList) => this.gameObjectList = gameObjectList;
}


[Serializable] public struct DotObject
{
    public GameObject DotGameObject;
    public GameObject TargetSquare;

    public DotObject(GameObject dotGameObject, GameObject targetSquare)
    {
        DotGameObject = dotGameObject;
        TargetSquare = targetSquare;
    }
}

[Serializable]
public struct TileType
{
    public int x;
    public int y;
    public TileKind tileKind;
}

[Serializable]
public struct PrefabLookUp
{
    public string Tag;
    public GameObject DefaultGameObject;
    public List<FruitPrefab> AllColorPrefabs;

    public PrefabLookUp(string tag, GameObject defaultGameObject, List<FruitPrefab> allColorPrefabs)
    {
        Tag = tag;
        DefaultGameObject = defaultGameObject;
        AllColorPrefabs = allColorPrefabs;
    }
}

[Serializable]
public struct FruitPrefab
{
    public GameObject Prefab;
    public Colors Color;

    public FruitPrefab(GameObject prefab, Colors color)
    {
        Prefab = prefab;
        Color = color;
    }
}
[Serializable]
public struct Sequence
{
    public FruitType FruitType;
    public Colors Color;
    public bool positive;
    public int counterValue;

    public Sequence(FruitType fruitType, Colors color, bool positive, int counterValue)
    {
        FruitType = fruitType;
        Color = color;
        this.positive = positive;
        this.counterValue = counterValue;
    }
}





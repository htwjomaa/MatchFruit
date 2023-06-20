//These are the structs to record a safety node

using UnityEngine;

public struct ReplayGameInformation
{
    public double UniqueIdentifier;
    public byte Level;
    public bool[] StarsPrevReached;
}
public struct ReplayGoals
{
    public float Score;
    public float[] CurrentGoalPoints;
    public float[] ReachGoalPoints;
    public FruitType[] GoalTypes;
}
public struct ReplaySideDots
{
    public byte[,] LeftDots;
    public byte[,] RightDots;
    public byte[,] TopDots;
    public byte[,] BottomDots;
}
public struct ReplaySideDotsType
{
    public FruitType[] LeftDotsType;
    public FruitType[] RightDotsType;
    public FruitType[] TopDotsType;
    public FruitType[] BottomDotsType;
}

public struct isReplaySwipeBack
{
    
}


public class IsReplayFunctions : MonoBehaviour
{
    public char seperator = '°';
    public string isReplayString = "";
    
}
using System;
using UnityEngine;

[Serializable]
public struct PhaseGoals
{
    private PhaseNumber Phase;
    public ObjectiveSettings[] ObjectiveSettingsArray;

    public PhaseGoals(PhaseNumber phase, ObjectiveSettings[] objectiveSettingsArray)
    {
        Phase = phase;
        ObjectiveSettingsArray = objectiveSettingsArray;
    }
}

[Serializable]
public struct ObjectiveSettings
{
    public PhaseGoal[] PhaseGoalArray;
    public ObjectiveNumber ObjectiveNumber;
    public bool Enabled;
    public bool AllowSimilar;
    public bool GoalOnly;
    public bool Additive;

    public ObjectiveSettings(PhaseGoal[] phaseGoalArray, ObjectiveNumber objectiveNumber, bool enabled, bool allowSimilar, bool goalOnly, bool additive)
    {
        PhaseGoalArray = phaseGoalArray;
        ObjectiveNumber = objectiveNumber;
        Enabled = enabled;
        AllowSimilar = allowSimilar;
        GoalOnly = goalOnly;
        Additive = additive;
    }
}

[Serializable]
public struct PhaseGoal
{
    public FruitType GoalFruit;
    public Colors GoalColor;
    public GoalNumber GoalNumber;
    [Range(0, 500)] public uint CollectionAmount;
    public CollectionStyle CollectionStyle;

    public PhaseGoal(FruitType goalFruit, Colors goalColor, GoalNumber goalNumber, uint collectionAmount, CollectionStyle collectionStyle)
    {
        GoalFruit = goalFruit;
        GoalColor = goalColor;
        GoalNumber = goalNumber;
        CollectionAmount = collectionAmount;
        CollectionStyle = collectionStyle;
    }
}
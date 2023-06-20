using System;

[Serializable]
public enum Directions { top, bottom, left, right, none }
[Serializable]
public enum DestroyOrInstantiate {destroy, instantiate}
public enum SideDotType{moveLine, turnA, turnB}

[Serializable]
public enum TileKind { Breakable, Blank, Lock, Chest, Bubble, Normal }

[Serializable]
public enum FruitType
{
    DunkelGrüneFrucht,
    HellGrüneFrucht,
    DunkelRoteFrucht,
    HellRoteFrucht,
    GelbeFrucht,
    OrangeFrucht,
    BlaueFrucht,
    LilaFrucht,
    AlleFrüchte,
    Jelly,
    Lock,
    Bubble,
    Truhe,
    Nothing
}

[Serializable]
public enum Colors
{
    AlleFarben,
    DunkelGrün ,
    HellGrüne,
    DunkelRot,
    HellRot,
    Gelb,
    Orange,
    Blaue,
    Lila
}
[Serializable]
public enum MatchingStyle
{
    InRow,
    Quad,
    Diagonally,
    OnlyVertical,
    OnlyHorizontal,
}
[Serializable]
public enum MatchingGoal
{
    SameColor,
    SameFruit,
    NotSameColor,
    NotSameFruit
}
[Serializable]
public enum Star
{
    Star1,
    Star2,
    Star3,
    Trophy
}


using System;
[Serializable]
public enum Row
{
    Both,
    Horizontal,
    Vertical
}

[Serializable]
public enum Diagonal
{
    Both,
    LeftUp,
    RightUp
}

[Serializable]
public enum Pattern
{
    Block,
    Corners,
    Cross,
    X_Pattern,
    L_Pattern,
    T_Pattern
}

[Serializable]
public enum IsMatchStyle
{
    IsRow,
    IsDiagonal,
    IsPattern1,
    IsPattern2
}
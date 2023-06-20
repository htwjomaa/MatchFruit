using System;
[Serializable]
public enum TT
{
    Normal,
    EmptyTile,
    Undestroyable,
    Jelly,
    Lock,
    Chest,
    LockedChest,
    Bubble,
    BigBlock,
    Fruit,
    Bomb,
    Ingredient,
    SameColorBomb
}

[Serializable]
public enum TKind
{
    Tiles,
    Vectors,
    Teleports
}
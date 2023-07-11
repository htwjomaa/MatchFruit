using System;
[Serializable]
public enum MatchStyle
{
    row, diagonal, block, x, cross, moves, rain
}
[Serializable]
public enum Bomb
{
    Horizontal, Vertical, Search, Package, Color, AllBombs
}
using UnityEngine;
public sealed class BlankSpaces : MonoBehaviour
{
    public static void GenerateBlankSpaces(TileType[] boardLayout, ref bool[,] blankSpaces)
    {
        for (int i = 0; i < boardLayout.Length; i++)
        {
            if (boardLayout[i].tileKind == TileKind.Blank)
            {
                blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
            }
        }
    }
}


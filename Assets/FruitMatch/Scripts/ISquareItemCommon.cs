using UnityEngine;

namespace FruitMatch.Scripts
{
    public interface ISquareItemCommon
    {
        bool IsBottom();
        Sprite GetSprite();
        SpriteRenderer GetSpriteRenderer();
        SpriteRenderer[] GetSpriteRenderers();
    }
}
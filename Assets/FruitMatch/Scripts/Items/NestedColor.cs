using FruitMatch.Scripts.Core;
using UnityEngine;

namespace FruitMatch.Scripts.Items
{
    /// <summary>
    /// Nested color for marmalade
    /// </summary>
    public class NestedColor : MonoBehaviour, IColorChangable/* , IColorEditor */
    {
        public Sprite[] Sprites;
        public Sprite randomEditorSprite;

        private void Start()
        {
            Sprites = GetComponentInParent<IColorableComponent>().GetSprites(LevelManager.THIS.currentLevel);
        }
        // Sprite[] IColorEditor.Sprites
        // {
        //     get
        //     {
        //         return Sprites;
        //     }
        // }

        // Sprite IColorEditor.randomEditorSprite
        // {
        //     get
        //     {
        //         return randomEditorSprite;
        //     }
        // }

        public void OnColorChanged(int color)
        {
          if(color < Sprites.Length)  GetComponent<SpriteRenderer>().sprite = Sprites[color];
        }
    }
}

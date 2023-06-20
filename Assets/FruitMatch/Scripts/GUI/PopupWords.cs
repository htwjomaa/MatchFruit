using FruitMatch.Scripts.Core;
using UnityEngine;

namespace FruitMatch.Scripts.GUI
{
    /// <summary>
    /// Greetings words for a combo
    /// </summary>
    public class PopupWords : MonoBehaviour
    {
        private void Update() => transform.position = LevelManager.THIS.field.GetPosition();
    }
}
using TMPro;
using UnityEngine;

namespace FruitMatch.Scripts.GUI
{
    /// <summary>
    /// Time message in the Lifeshop
    /// </summary>
    public class LifeTimerDouble : MonoBehaviour
    {
        public TextMeshProUGUI textSource;
        public TextMeshProUGUI textDest;
        void Update()
        {
            textDest.text = "+1" + LocalisationSystem.GetLocalisedString("life_after") + textSource.text;
        }
    }
}
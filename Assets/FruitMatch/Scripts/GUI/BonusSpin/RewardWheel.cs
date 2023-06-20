using FruitMatch.Scripts.GUI.Boost;
using UnityEngine;

namespace FruitMatch.Scripts.GUI.BonusSpin
{
    /// <summary>
    /// Reward on the wheel
    /// </summary>
    public class RewardWheel : MonoBehaviour
    {
        public RewardScriptable reward;
        public BoostType type;
        public int count;
        public string description;
        public int descriptionLocalizationRefrence;
        public string GetDescription()
        {
            return "";
            //LocalizationManager.GetText(descriptionLocalizationRefrence, description);
        }

    }
}

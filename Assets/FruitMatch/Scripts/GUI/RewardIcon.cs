﻿using FruitMatch.Scripts.GUI.BonusSpin;
using FruitMatch.Scripts.GUI.Boost;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FruitMatch.Scripts.GUI
{
    /// <summary>
    /// Reward icon for the Reward popup
    /// </summary>
    public class RewardIcon : MonoBehaviour
    {
        public Sprite[] sprites;
        public Image icon;
        public Transform iconHolder;
        public TextMeshProUGUI text;
        public TextMeshProUGUI rewardName;

        private void Awake()
        {
          //  Destroy(text.GetComponent<LocalizeText>());
          //  Destroy(rewardName.GetComponent<LocalizeText>());
        }

        /// <summary>
        /// Sets Wheel reward
        /// </summary>
        /// <param name="reward">reward object</param>
        public void SetWheelReward(RewardWheel reward)
        {
            foreach (Transform item in iconHolder)
            {
                Destroy(item.gameObject);
            }
            var g = Instantiate(reward.gameObject, Vector2.zero, Quaternion.identity, iconHolder);
            g.transform.localPosition = Vector3.zero;
            g.transform.localScale = Vector3.one * 2;
            icon = g.GetComponent<Image>();
            if (reward.type == BoostType.None)
            {
                text.text = LocalisationSystem.GetLocalisedString("you_got_coins");
                rewardName.text = reward.GetDescription();
            }
            else
            {
                text.text = LocalisationSystem.GetLocalisedString("you_got_the_boost");
                rewardName.text = reward.GetDescription();
            }

        }

        /// <summary>
        /// Set icon
        /// </summary>
        /// <param name="i"></param>
        public void SetIconSprite(int i)
        {
            icon.sprite = sprites[i];
            if (i == 0)
            {
                text.text = LocalisationSystem.GetLocalisedString("you_got_coins");
                rewardName.text = LocalisationSystem.GetLocalisedString("coins");
            }
            else if (i == 1)
            {
                text.text = LocalisationSystem.GetLocalisedString("you_got_life");
                rewardName.text = LocalisationSystem.GetLocalisedString("life");
            }
        }
    }
}

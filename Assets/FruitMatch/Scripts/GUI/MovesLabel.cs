using System;
using System.Collections;
using System.Collections.Generic;
using FruitMatch.Scripts.Core;
using TMPro;
using UnityEngine;

namespace FruitMatch.Scripts.GUI
{
    /// <summary>
    /// Moves / Time label in the game
    /// </summary>
    public class MovesLabel : MonoBehaviour
    {
        // Use this for initialization
        void OnEnable()
        {
            if(LevelManager.THIS?.levelData == null || !LevelManager.THIS.levelLoaded)
                LevelManager.OnLevelLoaded += Reset;
            else 
                Reset();

            StartCoroutine(ResetLabelCo());
        }

        void OnDisable()
        {
            LevelManager.OnLevelLoaded -= Reset;
        }

        private IEnumerator ResetLabelCo()
        {
            yield return new WaitForSeconds(0.2f);
            Reset();
        }
    void Reset()
    {
        if (LevelManager.THIS != null && LevelManager.THIS.levelLoaded)
        {
            switch (LevelManager.THIS.levelData.limitType)
            {
                case LIMIT.MOVES:
                    GetComponent<TextMeshProUGUI>().text = LocalisationSystem.GetLocalisedString("MOVES");
                    break;
                case LIMIT.TIME:
                    GetComponent<TextMeshProUGUI>().text = LocalisationSystem.GetLocalisedString("TIME");
                    break;
                case LIMIT.AVOID:
                    GetComponent<TextMeshProUGUI>().text = LocalisationSystem.GetLocalisedString("AVOID");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }

    }
}

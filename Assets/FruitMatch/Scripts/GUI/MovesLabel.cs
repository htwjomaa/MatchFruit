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
        }

        void OnDisable()
        {
            LevelManager.OnLevelLoaded -= Reset;
        }


    void Reset()
    {
        if (LevelManager.THIS != null && LevelManager.THIS.levelLoaded)
        {
            if (LevelManager.THIS.levelData.limitType == LIMIT.MOVES)
                GetComponent<TextMeshProUGUI>().text = LocalisationSystem.GetLocalisedString("MOVES");
            else
                GetComponent<TextMeshProUGUI>().text = LocalisationSystem.GetLocalisedString("TIME");
        }
    }

    }
}

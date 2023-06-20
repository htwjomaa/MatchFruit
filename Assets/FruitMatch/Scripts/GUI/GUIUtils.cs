using FruitMatch.Scripts.Core;
using FruitMatch.Scripts.System;
using UnityEngine;

namespace FruitMatch.Scripts.GUI
{
    /// <summary>
    /// Spends a life after game started or offers to buy a life
    /// </summary>
    public class GUIUtils : MonoBehaviour
    {
        public DebugSettings DebugSettings;

        public static GUIUtils THIS;

        private void Start()
        {
            DebugSettings = Resources.Load<DebugSettings>("Scriptable/DebugSettings");
            if (!Equals(THIS, this)) THIS = this;
        }

        public void StartGame()
        {
          //  if (InitScript.lifes > 0 || DebugSettings.AI)
           // {
           //     InitScript.Instance.SpendLife(1);
                LevelManager.THIS.gameStatus = GameState.PrepareGame;
          //  }
           // else
           // {
           //     BuyLifeShop();
           // }

        }

        public void BuyLifeShop()
        {

            if (InitScript.lifes < InitScript.Instance.CapOfLife)
                MenuReference.THIS.LiveShop.gameObject.SetActive(true);

        }
    }
}
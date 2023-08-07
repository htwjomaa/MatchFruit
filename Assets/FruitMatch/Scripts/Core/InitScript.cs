using System;
using System.Collections;
using FruitMatch.Scripts.GUI;
using FruitMatch.Scripts.GUI.Boost;
using FruitMatch.Scripts.Level;
using FruitMatch.Scripts.MapScripts;
using FruitMatch.Scripts.System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FruitMatch.Scripts.Core
{
    /// <summary>
    /// class for main system variables, ads control and in-app purchasing
    /// </summary>
    public class InitScript : MonoBehaviour
    {
        public static InitScript Instance;

        ///life gaining timer
        public static float RestLifeTimer;

        ///date of exit for life timer
        public static string DateOfExit;
        
        ///amount of life
        public static int lifes = 999999;

        //EDITOR: max amount of life
        public int CapOfLife = 9999999;

        //EDITOR: time for restList life
        public float TotalTimeForRestLifeHours;

        //EDITOR: time for restList life
        public float TotalTimeForRestLifeMin = 15;

        //EDITOR: time for restList life
        public float TotalTimeForRestLifeSec = 60;

        //amount of coins
        public static int Gems;

        
        //EDITOR: should player lose a life for every passed level
        public bool losingLifeEveryGame;

        //daily reward popup reference
        public GameObject DailyMenu;

        // Use this for initialization
        void Awake()
        {
            Application.targetFrameRate = 60;
            Instance = this;
            DebugLogKeeper.Init();
        }

        public void ShowGemsReward(int amount)
        {
            var reward = MenuReference.THIS.Reward.GetComponent<RewardIcon>();
            reward.SetIconSprite(0);
            reward.gameObject.SetActive(true);
            AddGems(amount);
        }
        

        public void AddGems(int count)
        {
            Gems += count;
            //save
        }

        public void SpendGems(int count)
        {
            Gems -= count;
            //save
        }


        public void RestoreLifes()
        {
            lifes = CapOfLife;
        }

        public void AddLife(int count)
        {
            lifes += count;
        }

        public int GetLife()
        {
            return 999999;
        }

        public void BuyBoost(BoostType boostType, int price, int count)
        {
            PlayerPrefs.SetInt("" + boostType, PlayerPrefs.GetInt("" + boostType) + count);
            PlayerPrefs.Save();
#if PLAYFAB || GAMESPARKS
            NetworkManager.dataManager.SetBoosterData();
#endif
        }

        public void SpendBoost(BoostType boostType)
        {
           // PlayerPrefs.SetInt("" + boostType, PlayerPrefs.GetInt("" + boostType) - 1);
           // PlayerPrefs.Save();
        }
        

        public void OnLevelClicked(object sender, LevelReachedEventArgs args)
        {
            if (EventSystem.current.IsPointerOverGameObject(-1))
                return;
            if (!GameObject.Find("CanvasGlobal").transform.Find("MenuPlay").gameObject.activeSelf &&
                !GameObject.Find("CanvasGlobal").transform.Find("GemsShop").gameObject.activeSelf &&
                !GameObject.Find("CanvasGlobal").transform.Find("LiveShop").gameObject.activeSelf)
            {
              //  SoundBase.Instance.PlayOneShot(SoundBase.Instance.click);
                OpenMenuPlay(args.Number);
            }
        }

        public static void OpenMenuPlay(int num)
        {
            PlayerPrefs.SetInt("OpenLevel", num);
            PlayerPrefs.Save();
            LevelManager.THIS.MenuPlayEvent();
            LevelManager.THIS.LoadLevel();
            CrosssceneData.openNextLevel = false;
            MenuReference.THIS.MenuPlay.gameObject.SetActive(true);
        }

        void OnEnable()
        {
            LevelsMap.LevelSelected += OnLevelClicked;
        }

        void OnDisable()
        {
            LevelsMap.LevelSelected -= OnLevelClicked;
        }
        public void delayedCall(float sec, Action action)
        {
            StartCoroutine(DelayedCallCor(sec, action));
        }

        IEnumerator DelayedCallCor(float sec, Action action)
        {
            yield return new WaitForSeconds(sec);
            action?.Invoke();
        }
    }

    /// moves or time is level limit type
    public enum LIMIT
    {
        MOVES,
        TIME,
        AVOID
    }
}

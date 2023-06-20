using System;
using System.Collections;
using System.Collections.Generic;
using FruitMatch.Scripts.Core;
using FruitMatch.Scripts.GUI.Boost;
using FruitMatch.Scripts.Level;
using FruitMatch.Scripts.MapScripts.StaticMap.Editor;
using FruitMatch.Scripts.System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif
namespace FruitMatch.Scripts.GUI
{
    /// <summary>
    /// Popups animation event manager
    /// </summary>
    public class AnimationEventManager : MonoBehaviour
    {
        public bool PlayOnEnable = true;
        bool WaitForPickupFriends;

        bool WaitForAksFriends;
        Dictionary<string, string> parameters;

        void OnEnable()
        {
            if (PlayOnEnable)
            {
                //            SoundBase.Instance.PlayOneShot(SoundBase.Instance.swish[0]);

            }
            if (name == "MenuPlay")
            {

            }

            if (name == "PrePlay")
            {
                // GameObject
            }
            if (name == "PreFailed")
            {
//            SoundBase.Instance.PlayOneShot(SoundBase.Instance.gameOver[0]);
                transform.Find("Banner/Buttons/Buy").GetComponent<Button>().interactable = true;

                GetComponent<Animation>().Play();
            }

            if (name == "Settings" || name == "MenuPause")
            {
                if (PlayerPrefs.GetInt("Sound") < 1)
                {
                    transform.Find("Sound/Sound/SoundOff").gameObject.SetActive(true);
                    transform.Find("Sound/Sound").GetComponent<Image>().enabled = false;
                }
                else
                {
                    transform.Find("Sound/Sound/SoundOff").gameObject.SetActive(false);
                    transform.Find("Sound/Sound").GetComponent<Image>().enabled = true;
                }

                if (PlayerPrefs.GetInt("Music") < 1)
                {
                    transform.Find("Music/Music/MusicOff").gameObject.SetActive(true);
                    transform.Find("Music/Music").GetComponent<Image>().enabled = false;
                }
                else
                {
                    transform.Find("Music/Music/MusicOff").gameObject.SetActive(false);
                    transform.Find("Music/Music").GetComponent<Image>().enabled = true;
                }

            }

            if (name == "GemsShop")
            {
                // var tr = GetComponent<SweetSugarPacks>().packs;
                // for (var i = 0; i < LevelManager.THIS.gemsProducts.Count; i++)
                // {
                //     var item = tr[i];
                //     item.Find("Count").GetComponent<TextMeshProUGUI>().text = "" + LevelManager.THIS.gemsProducts[i].count;
                //     item.Find("Buy/Price").GetComponent<TextMeshProUGUI>().text = "" + LevelManager.THIS.gemsProducts[i].price;
                // }
            }
            if (name == "MenuComplete")
            {
                for (var i = 1; i <= 3; i++)
                {
                    transform.Find("Image").Find("Star" + i).gameObject.SetActive(false);
                }

            }

            var videoButton = transform.Find("Image/Video");
            if (videoButton == null) videoButton = transform.Find("Banner/Buttons/Video");
            if (videoButton != null)
            {
            }
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (name == "MenuPlay" || name == "Settings" || name == "BoostInfo" || name == "GemsShop" || name == "LiveShop" || name == "BoostShop" || name == "Reward")
                    CloseMenu();
            }
        }

        /// <summary>
        /// show rewarded ads
        /// </summary>

        /// <summary>
        /// Open rate store
        /// </summary>
        public void GoRate()
        {

#if UNITY_ANDROID
       // Application.OpenURL(InitScript.Instance.RateURL);
#elif UNITY_IOS
      //  Application.OpenURL(InitScript.Instance.RateURLIOS);
#endif
            PlayerPrefs.SetInt("Rated", 1);
            PlayerPrefs.Save();
            CloseMenu();
        }

        void OnDisable()
        {
            if (transform.Find("Image/Video") != null)
            {
                transform.Find("Image/Video").gameObject.SetActive(true);
            }

            //if( PlayOnEnable )
            //{
            //    if( !GetComponent<SequencePlayer>().sequenceArray[0].isPlaying )
            //        GetComponent<SequencePlayer>().sequenceArray[0].Play
            //}
        }

        /// <summary>
        /// Event on finish animation
        /// </summary>
        public void OnFinished()
        {
            if (name == "MenuComplete")
            {
                StartCoroutine(MenuComplete());
                StartCoroutine(MenuCompleteScoring());
            }
            if (name == "MenuPlay")
            {
                //            InitScript.Instance.currentTarget = InitScript.Instance.targets[PlayerPrefs.GetInt( "OpenLevel" )];
                transform.Find("Image/Boosters/Boost1").GetComponent<BoostIcon>().InitBoost();
                transform.Find("Image/Boosters/Boost2").GetComponent<BoostIcon>().InitBoost();
                transform.Find("Image/Boosters/Boost3").GetComponent<BoostIcon>().InitBoost();

            }
            if (name == "MenuPause")
            {
                if (LevelManager.THIS.gameStatus == GameState.Playing)
                    LevelManager.THIS.gameStatus = GameState.Pause;
            }

            if (name == "MenuFailed")
            {
                if (LevelManager.Score < LevelManager.THIS.levelData.star1)
                {
                    TargetCheck(false, 2);
                }
                else
                {
                    TargetCheck(true, 2);
                }

            }
            if (name == "PrePlay")
            {
                CloseMenu();
                LevelManager.THIS.gameStatus = GameState.Tutorial;
                if (LevelManager.THIS.levelData.limitType == LIMIT.TIME)
                {
                    Rl.GameManager.PlayAudio(Rl.soundStrings.TimeOut,
                        Random.Range(0, 5),  true,Rl.settings.GetSFXVolume, Rl.effects.audioSource);
                
                }

            }
            if (name == "PreFailed")
            {
                transform.Find("Banner/Buttons/Video").gameObject.SetActive(false);
                CloseMenu();
            }

            if (name.Contains("gratzWord"))
                gameObject.SetActive(false);
            if (name == "NoMoreMatches")
                gameObject.SetActive(false);
            if (name == "failed")
                gameObject.transform.parent.gameObject.SetActive(false);
            // if (name == "CompleteLabel")
            //     gameObject.SetActive(false);

        }

        void TargetCheck(bool check, int n = 1)
        {
            var TargetCheck = transform.Find("Image/TargetCheck" + n);
            var TargetUnCheck = transform.Find("Image/TargetUnCheck" + n);
            TargetCheck.gameObject.SetActive(check);
            TargetUnCheck.gameObject.SetActive(!check);
        }

        /// <summary>
        /// Shows rewarded ad button in Prefailed popup 
        /// </summary>
        [UsedImplicitly]
        public void WaitForGiveUp()
        {
            if (name == "PreFailed" && LevelManager.THIS.gameStatus != GameState.Playing)
            {
                GetComponent<Animation>()["bannerFailed"].speed = 0;

            }
        }

        /// <summary>
        /// Complete popup animation
        /// </summary>
        IEnumerator MenuComplete()
        {
            for (var i = 1; i <= LevelManager.THIS.stars; i++)
            {
                //  SoundBase.Instance.audio.PlayOneShot( SoundBase.Instance.scoringStar );
                transform.Find("Image").Find("Star" + i).gameObject.SetActive(true);
                Rl.GameManager.PlayAudio(Rl.soundStrings.Stars[i - 1], Random.Range(0,5),true, Rl.settings.GetSFXVolume, Rl.effects.audioSource);
                yield return new WaitForSeconds(0.5f);
            }
        }

        /// <summary>
        /// Complete popup animation
        /// </summary>
        IEnumerator MenuCompleteScoring()
        {
            var scores = transform.Find("Image").Find("Score").GetComponent<TextMeshProUGUI>();
            for (var i = 0; i <= LevelManager.Score; i += 500)
            {
                scores.text = "" + i;
                // SoundBase.Instance.audio.PlayOneShot( SoundBase.Instance.scoring );
                yield return new WaitForSeconds(0.00001f);
            }
            scores.text = "" + LevelManager.Score;
        }

        /// <summary>
        /// SHows info popup
        /// </summary>
        public void Info()
        {
            MenuReference.THIS.Tutorials.gameObject.SetActive(false);
            MenuReference.THIS.Tutorials.gameObject.SetActive(true);
            OpneMenu(gameObject);
        }
        
        public void OpneMenu(GameObject menu)
        {
            if (menu.activeSelf)
                menu.SetActive(false);
            else
                menu.SetActive(true);
        }
        
        public void CloseMenu()
        {
            if (gameObject.name == "MenuPreGameOver")
            {
                ShowGameOver();
            }
            if (gameObject.name == "MenuComplete")
            {
//            LevelManager.THIS.gameStatus = GameState.Map;
                PlayerPrefs.SetInt("OpenLevel", LevelManager.THIS.currentLevel + 1);
                CrosssceneData.openNextLevel = true;
                SceneManager.LoadScene(Resources.Load<MapSwitcher>("Scriptable/MapSwitcher").GetSceneName());
            }
            if (gameObject.name == "MenuFailed")
            {
                LevelManager.THIS.gameStatus = GameState.Map;
            }

            if (SceneManager.GetActiveScene().name == "game")
            {
                if (LevelManager.THIS.gameStatus == GameState.Pause)
                {
                    LevelManager.THIS.gameStatus = GameState.WaitAfterClose;

                }
            }

            if (gameObject.name == "Settings" && LevelManager.GetGameStatus() != GameState.Map)
            {
                BackToMap();
            }
            else if (gameObject.name == "Settings" && LevelManager.GetGameStatus() == GameState.Map)
                SceneManager.LoadScene("main");

            //        SoundBase.Instance.PlayOneShot(SoundBase.Instance.swish[1]);

            gameObject.SetActive(false);
        }

        public void SwishSound()
        {
            
            Rl.GameManager.PlayAudio(Rl.soundStrings.Swish[1], Random.Range(0,5),true, Rl.settings.GetSFXVolume, Rl.effects.audioSource);
        }

        public void ShowInfo()
        {
            GameObject.Find("CanvasGlobal").transform.Find("BoostInfo").gameObject.SetActive(true);

        }

        public void Play()
        {
          //  SoundBase.Instance.PlayOneShot(SoundBase.Instance.click);
            if (gameObject.name == "MenuPreGameOver")
            {
                if (InitScript.Gems >= 12)
                {
                    InitScript.Instance.SpendGems(12);
                    //                LevelData.LimitAmount += 12;
                    LevelManager.THIS.gameStatus = GameState.WaitAfterClose;
                    gameObject.SetActive(false);

                }
                else
                {
                    BuyGems();
                }
            }
            else if (gameObject.name == "MenuFailed")
            {
                LevelManager.THIS.gameStatus = GameState.Map;
            }
            else if (gameObject.name == "MenuPlay")
            {
                GUIUtils.THIS.StartGame();
                CloseMenu();
            }
            else if (gameObject.name == "MenuPause")
            {
                CloseMenu();
                LevelManager.THIS.gameStatus = GameState.Playing;
            }
        }

        public void PlayTutorial()
        {
            LevelManager.THIS.gameStatus = GameState.Playing;
            CloseMenu();
        }

        public void BackToMap()
        {
            Time.timeScale = 1;
            LevelManager.THIS.gameStatus = GameState.GameOver;
             CloseMenu();
            gameObject.SetActive(false);
            LevelManager.THIS.gameStatus = GameState.Map;
            SceneManager.LoadScene(Resources.Load<MapSwitcher>("Scriptable/MapSwitcher").GetSceneName());
        }

        public void Next()
        {
            Rl.GameManager.PlayLimitSound(Rl.soundStrings.Click,
                Random.Range(0, 5),  Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);

            CloseMenu();
        }

        [UsedImplicitly]
        public void Again()
        {
            Rl.GameManager.PlayLimitSound(Rl.soundStrings.Click,
                Random.Range(0, 5),  Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
            GameObject gm = new GameObject();
            gm.AddComponent<RestartLevel>();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }

        public void BuyGems()
        {

            Rl.GameManager.PlayLimitSound(Rl.soundStrings.Click,
                Random.Range(0, 5),  Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
            MenuReference.THIS.GemsShop.gameObject.SetActive(true);
        }

        [UsedImplicitly]
        public void Buy(GameObject pack)
        {
            CloseMenu();

        }

        public void BuyLifeShop()
        {

           // SoundBase.Instance.PlayOneShot(SoundBase.Instance.click);
            if (InitScript.lifes < InitScript.Instance.CapOfLife)
                MenuReference.THIS.LiveShop.gameObject.SetActive(true);

        }

        public void BuyLife(GameObject button)
        {
           // SoundBase.Instance.PlayOneShot(SoundBase.Instance.click);
            if (InitScript.Gems >= int.Parse(button.transform.Find("Price").GetComponent<TextMeshProUGUI>().text))
            {
                InitScript.Instance.SpendGems(int.Parse(button.transform.Find("Price").GetComponent<TextMeshProUGUI>().text));
                InitScript.Instance.RestoreLifes();
                CloseMenu();
            }
            else
            {
                MenuReference.THIS.GemsShop.gameObject.SetActive(true);
            }

        }

        public void BuyFailed(GameObject button)
        {
//        if (GetComponent<Animation>()["bannerFailed"].speed == 0)
            {
                if (InitScript.Gems >= LevelManager.THIS.FailedCost)
                {
                    InitScript.Instance.SpendGems(LevelManager.THIS.FailedCost);
                    button.GetComponent<Button>().interactable = false;
                    GoOnFailed();
                    GetComponent<Animation>()["bannerFailed"].speed = 1;  
                }
                else
                {
                    MenuReference.THIS.GemsShop.gameObject.SetActive(true);
                }
            }
        }

        public void GoOnFailed()
        {
            GetComponent<PreFailed>().Continue();
        }

        [UsedImplicitly]
        public void GiveUp()
        {
            GetComponent<PreFailed>().Close();
        }

        void ShowGameOver()
        {
            Rl.GameManager.PlayAudio(Rl.soundStrings.GameOver[1], Random.Range(0,5),true, Rl.settings.GetSFXVolume, Rl.effects.audioSource);
            GameObject.Find("Canvas").transform.Find("MenuGameOver").gameObject.SetActive(true);
            gameObject.SetActive(false);

        }

        #region boosts

        public void BuyBoost(BoostType boostType, int price, int count, Action callback)
        {
          //  SoundBase.Instance.PlayOneShot(SoundBase.Instance.click);
            if (InitScript.Gems >= price)
            {
                InitScript.Instance.SpendGems(price);
                InitScript.Instance.BuyBoost(boostType, price, count);
                callback?.Invoke();
                //InitScript.Instance.SpendBoost(boostType);
                CloseMenu();
            }
            else
            {
                BuyGems();
            }
        }
        #endregion
    }
}

using System;
using System.Collections;
using System.Linq;
using FruitMatch.Scripts.Core;
using FruitMatch.Scripts.Level;
using FruitMatch.Scripts.TargetScripts.TargetSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace FruitMatch.Scripts.GUI
{
    /// <summary>
    /// various GUi counters
    /// </summary>
    public class Counter_ : MonoBehaviour
    {
        TextMeshProUGUI txt;
        private float lastTime;
        bool alert;
        public bool isLabel;
        private LevelData _thisLevelData;

        IEnumerator HideOnStartCo(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            HideText();
        }

        private void HideText()
        {

            int value = 0;
            Int32.TryParse(txt.text, out value);

            if (value == Int32.MaxValue)
            {
                txt.alpha = 0f;
            }
        }
        public LevelData ThisLevelData
        {
            get
            {
                if (_thisLevelData == null) _thisLevelData = LevelData.THIS;
                return _thisLevelData;
            }
            set => _thisLevelData = value;
        }

        // Use this for initialization
        void Awake()
        {
            // txt = GetComponent<Text>();
            txt = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            ThisLevelData = LevelManager.THIS.levelData;
            StartCoroutine(HideOnStartCo(0.2f));
        }

        void OnEnable()
        {
            lastTime = 0;
            UpdateText();
            alert = false; StartCoroutine(UpdateRare());
            if (name == "Limit") StartCoroutine(TimeTick());
        }

        // Update is called once per frame
        IEnumerator UpdateRare()
        {
            while (true)
            {
                if (txt == null) continue;

                UpdateText();
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void UpdateText()
        {
            if (name == "Score")
            {
                txt.text = "" + LevelManager.Score;
            }

            if (name == "BestScore")
            {
                txt.text = "Best score:" + PlayerPrefs.GetInt("Score" + PlayerPrefs.GetInt("OpenLevel"));
            }

            if (name == "Limit" && ThisLevelData != null)
            {
                switch (ThisLevelData.limitType)
                {
                    case LIMIT.MOVES:
                        txt.text = "" + Mathf.Clamp(ThisLevelData.limit, 0, ThisLevelData.limit);
                        txt.transform.localScale = Vector3.one;
                        if (ThisLevelData.limit <= 5)
                        {
                            txt.color = new Color(255f / 255f, 132f / 255, 222f / 255);
                            txt.outlineColor = Color.white;
                            if (!alert)
                            {
                                alert = true;
//                            SoundBase.Instance.PlayOneShot(SoundBase.Instance.alert);
                            }
                        }
                        else
                        {
                            alert = false;
                            txt.color = Color.white;
                            // txt.GetComponent<Outline>().effectColor = new Color(148f / 255f, 61f / 255f, 95f / 255f);
                        }
                        break;
                    case LIMIT.TIME:
                        var minutes = Mathf.FloorToInt(ThisLevelData.limit / 60F);
                        var seconds = Mathf.FloorToInt(ThisLevelData.limit - minutes * 60);
                        txt.text = "" + $"{minutes:00}:{seconds:00}";
                        txt.transform.localScale = Vector3.one * 0.68f;
                        txt.fontSize = 80;
                        if (ThisLevelData.limit <= 5 && LevelManager.THIS.gameStatus == GameState.Playing)
                        {
                            // txt.color = new Color(216f / 255f, 0, 0);
                            // txt.outlineColor = Color.white;
                            if (lastTime + 5 < Time.time)
                            {
                                lastTime = Time.time;
                                Rl.GameManager.PlayAudio(Rl.soundStrings.TimeOut,
                                    Random.Range(0, 5),  true,Rl.settings.GetSFXVolume, Rl.effects.audioSource);
                            }
                        }
                        else
                        {
                            txt.color = Color.white;
                            txt.outlineColor = new Color(148f / 255f, 61f / 255f, 95f / 255f);
                        }
                        break;
                    case LIMIT.AVOID:

                        if (!isLabel)
                        {
                            txt.text = "" + Mathf.Clamp(ThisLevelData.limit, 0, ThisLevelData.limit);
                            txt.transform.localScale = Vector3.one;
                            if (ThisLevelData.limit >= 5)
                            {
                                txt.color = new Color(255f / 255f, 132f / 255, 222f / 255);
                                txt.outlineColor = Color.white;
                                if (!alert)
                                {
                                    alert = true;
//                            SoundBase.Instance.PlayOneShot(SoundBase.Instance.alert);
                                }
                            }
                            else
                            {
                                alert = false;
                                txt.color = Color.white;
                                // txt.GetComponent<Outline>().effectColor = new Color(148f / 255f, 61f / 255f, 95f / 255f);
                            }
                        }

                        else
                        {
                            txt.text = "";
                            txt.color = new Color(0, 0, 0, 0);
                            txt.fontSize = 0;
                        }
                 
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
            }

            if (name == "Lifes")
            {
                txt.text = "" + InitScript.Instance?.GetLife();
            }

            if (name == "FailedCount")
            {
                if (ThisLevelData.limitType == LIMIT.MOVES)
                    txt.text = "+" + LevelManager.THIS.ExtraFailedMoves;
                else
                    txt.text = "+" + LevelManager.THIS.ExtraFailedSecs;
            }

       
            if (name == "FailedDescription")
            {
                txt.text = "" + LevelData.THIS.GetTargetCounters().First(i => !i.IsTotalTargetReached()).targetLevel.GetFailedDescription();
            }

            if (name == "TargetScore")
            {
                txt.text = "" + ThisLevelData.star1;
            }

            if (name == "Level")
            {
                txt.text = "" + PlayerPrefs.GetInt("OpenLevel");
            }

            // if (name == "TargetDescription1")
            // {
            //     txt.text = "" + LevelData.THIS.GetTargetContainersForUI().First().targetLevel.GetDescription();
            // }
        }

        IEnumerator TimeTick()
        {
            while (true)
            {
                if (LevelManager.THIS.gameStatus == GameState.Playing)
                {
                    if (_thisLevelData.limitType == LIMIT.TIME)
                    {
                        _thisLevelData.limit--;
                        if (!LevelManager.THIS.DragBlocked)
                            LevelManager.THIS.CheckWinLose();
                    }
                }
                if (LevelManager.THIS.gameStatus == GameState.Map)
                    yield break;
                yield return new WaitForSeconds(1);
            }
        }
    }
}

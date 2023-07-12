using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FruitMatch.Scripts.Core;
using FruitMatch.Scripts.GUI;
using FruitMatch.Scripts.Items;
using FruitMatch.Scripts.Level;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace FruitMatch.Scripts.TargetScripts.TargetSystem
{
    /// <summary>
    /// target icon
    /// </summary>
    public class TargetGUI : MonoBehaviour
    {
        public Image image;
        public TextMeshProUGUI text;
        public GameObject check;
        public GameObject uncheck;
        public int color;
        public TargetCounter subTargetContainer;
        public Image HideTargetIcon;
        public TextMeshProUGUI GoalContraint;
        public bool IsIngameTarget;
        public Image BorderImage;
        [FormerlySerializedAs("Frame")] public Image frame;
        [SerializeField] public CollectionStyle collectionStyle;
        private string _goalColor = "#63FF4F";
        private string _avoidColor = "#FB226B";
        private string _emptyColor = "#FFFFFF";


        private Vector3 _borderLocalSize = new Vector3(1.063f, 1.063f, 1.063f);
        private string _goalBorderColor = "#ADFF97";
        private string _avoidBorderColor = "#FF4643";
        private string _emptyBorderColor = "#FFFFFF";
        private int _maxCount = 0;
        private void Awake()
        {
            if(HideTargetIcon != null) HideTargetIcon.color = new Color(HideTargetIcon.color.r, HideTargetIcon.color.g, HideTargetIcon.color.b, 0f);
        }
        
        public Sprite[] allIngameSprites;
        

        private Color ChangeToCollectionStyleColor(CollectionStyle collectionStyle)
        {
          
            Color color;
          
            switch (collectionStyle)
            {
                case CollectionStyle.Nothing:
                    color = new Color(255, 255, 255, 1);
                    break;
                case CollectionStyle.Destroy:
                    ColorUtility.TryParseHtmlString(_goalColor, out color);
                    break;
                case CollectionStyle.Avoid:
                    ColorUtility.TryParseHtmlString(_avoidColor, out color);
                    break;
                case CollectionStyle.Empty:
                    ColorUtility.TryParseHtmlString(_emptyColor, out color);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(collectionStyle), collectionStyle, null);
            }

            color = new Color(color.r, color.g, color.b, 1);
            return color;
        }
        private Color ChangeToCollectionStyleBorderColor(CollectionStyle collectionStyle)
        {
          
            Color color;
          
            switch (collectionStyle)
            {
                case CollectionStyle.Nothing:
                    color = new Color(255, 255, 255, 1);
                    break;
                case CollectionStyle.Destroy:
                    ColorUtility.TryParseHtmlString(_goalBorderColor, out color);
                    break;
                case CollectionStyle.Avoid:
                    ColorUtility.TryParseHtmlString(_avoidBorderColor, out color);
                    break;
                case CollectionStyle.Empty:
                    ColorUtility.TryParseHtmlString(_emptyBorderColor, out color);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(collectionStyle), collectionStyle, null);
            }

            color = new Color(color.r, color.g, color.b, 0.94f);
            return color;
        }
        IEnumerator FadeIconsInCO(float waitInBetweenFramesSec)
        {
            if (image.color.a > 0.95f)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
                GoalContraint.color = new Color(GoalContraint.color.r, GoalContraint.color.g, GoalContraint.color.b, 1);
                if(BorderImage != null) BorderImage.color = new Color(BorderImage.color.r, BorderImage.color.g, BorderImage.color.b,
                    0.94f);
                if(frame != null)   frame.color = new Color(frame.color.r, frame.color.g, frame.color.b, 0.94f);
                yield break;
            } 
            yield return new WaitForSeconds(waitInBetweenFramesSec);
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a+0.05f);
        if(frame != null)frame.color = new Color(frame.color.r, frame.color.g, frame.color.b, frame.color.a+0.055f);
        if(BorderImage != null) BorderImage.color= new Color(BorderImage.color.r, BorderImage.color.g, BorderImage.color.b, BorderImage.color.a + 0.055f);
            GoalContraint.color = new Color(GoalContraint.color.r, GoalContraint.color.g, GoalContraint.color.b, GoalContraint.color.a+0.05f);
            
           StartCoroutine(FadeIconsInCO(waitInBetweenFramesSec));
        }
        
        private string GetTargetCollectionStyleText(CollectionStyle collectionStyle)
        {
       
            switch (collectionStyle)
            {
                case CollectionStyle.Nothing:
                    return "";
                    break;
                case CollectionStyle.Destroy:
                    return LocalisationSystem.GetLocalisedValue("GOAL");
                    break;
                case CollectionStyle.Avoid:
                    return LocalisationSystem.GetLocalisedValue("MEIDE");
                    break;
                case CollectionStyle.Empty:
                    return LocalisationSystem.GetLocalisedValue("EMPTY");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(collectionStyle), collectionStyle, null);
            }
        }

        IEnumerator ChangeCollectionStyleText_CO(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            
            SetGoalConstraintText();
            if(frame != null) SetFrameImage();

            Int32.TryParse(text.text, out _maxCount);
        }

        private void SetFrameImage()
        {
            CollectionStyle collectionStyle = LevelManager.THIS.TargetCollectionStyle[
                SiblingIndex];

            for (int i = 0; i < Rl.world.CollectionStyleSet.Count; i++)
            {
                if (Rl.world.CollectionStyleSet[i].CollectionStyle == collectionStyle)
                {
                    frame.sprite = Rl.world.CollectionStyleSet[i].CollenctionStyleImage;
                    return;
                }
             
            }
        }

        public int SiblingIndex;
       [NaughtyAttributes.Button()] private void SetGoalConstraintText()
       {
           if (GoalContraint != null)
           {
               GoalContraint.text = 
                   GetTargetCollectionStyleText(
                       LevelManager.THIS.TargetCollectionStyle[
                           SiblingIndex]);
               
           }

           collectionStyle = LevelManager.THIS.TargetCollectionStyle[
               SiblingIndex];
           
       }
        [NaughtyAttributes.Button()] private void GetColor()
        {

           allIngameSprites = LoadingHelper.THIS.FieldParent.GetComponentInChildren<IColorableComponent>().Sprites[0].Sprites;
            for (int i = 0; i < allIngameSprites.Length; i++)
            {
                if (image.sprite == allIngameSprites[i])
                {
                    color = i;
                    if(LoadingHelper.THIS.TargetSequence.Count <3) LoadingHelper.THIS.TargetSequence.Add(i);
                    return;
                }
            }
        }

        
        private void InvokeFadeIn() =>  StartCoroutine(FadeIconsInCO(0.09f));
        private void Start()
        {
            if(HideTargetIcon != null)   TargetSettings.ShowItemEvent += ShowIcon;
            GetColor();
            if (BorderImage != null)
            {
                BorderImage.sprite = image.sprite;
                BorderImage.GetComponent<RectTransform>().transform.localScale = _borderLocalSize;
            }
            if(GoalContraint != null)    GoalContraint.color = ChangeToCollectionStyleColor(LevelManager.THIS.TargetCollectionStyle[
                SiblingIndex]);
            if(BorderImage != null) BorderImage.color = ChangeToCollectionStyleBorderColor(LevelManager.THIS.TargetCollectionStyle[
                SiblingIndex]);
            StartCoroutine(ChangeCollectionStyleText_CO(0.05f));

            if (IsIngameTarget)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
                if(frame != null) frame.color = new Color(frame.color.r, frame.color.g, frame.color.b, 0);
                if(BorderImage != null) BorderImage.color = new Color(BorderImage.color.r, BorderImage.color.g, BorderImage.color.b, 0);
                GoalContraint.color = new Color(GoalContraint.color.r, GoalContraint.color.g, GoalContraint.color.b, 0);
                Invoke(nameof(InvokeFadeIn), 3.3f);
            }
        
        }
        private void OnDestroy()
        {
            LevelManager.MoveMadeEvent -= MoveMade;
            if(HideTargetIcon != null) TargetSettings.ShowItemEvent -= ShowIcon;
        }
        IEnumerator HideIconsCo(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _cashedCount = subTargetContainer.GetCount(true);
          if(HideTargetIcon != null)  HideTargetIcon.color = new Color(HideTargetIcon.color.r, HideTargetIcon.color.g, HideTargetIcon.color.b, 1f);
         // if (frame != null) frame.color = new Color(frame.color.r, frame.color.g, frame.color.b, 0f);
            yield return null;
        }
          IEnumerator ShowIconCo()
        {
            HideTargetIcon.color = new Color(HideTargetIcon.color.r, HideTargetIcon.color.g, HideTargetIcon.color.b, 0f);
            if (frame != null) frame.color = new Color(frame.color.r, frame.color.g, frame.color.b, 0.94f);
            if(BorderImage != null) BorderImage.color = new Color(BorderImage.color.r, BorderImage.color.g, BorderImage.color.b, 0.94f);
            yield return new WaitForSeconds(3.5f);
           // if (frame != null) frame.color = new Color(frame.color.r, frame.color.g, frame.color.b, 0f);
            HideTargetIcon.color = new Color(HideTargetIcon.color.r, HideTargetIcon.color.g, HideTargetIcon.color.b, 1f);
            yield return null;
        }

          public void ShowIcon()
          {
              GenericSettingsFunctions.SmallJumpAnimation();
              StopCoroutine(ShowIconCo());
              StartCoroutine(ShowIconCo());
          }
        public void SetSprite(Sprite spr)
        {
            image.sprite = spr;
        }

        void OnEnable()
        {
            // if (LevelData.THIS?.target.name == "Stars")
            // {
            //     gameObject.SetActive(false);
            //     return;
            // }
            check.SetActive(false);
            uncheck.SetActive(false);
            // UpdateText();
            StartCoroutine(Check());
            LevelManager.MoveMadeEvent += MoveMade;
            StartCoroutine(HideIconsCo(9f));
        }

        private void OnDisable()
        {
            LevelManager.MoveMadeEvent -= MoveMade;
        }

        private void Update()
        {
            if(subTargetContainer != null)
                UpdateText();
        }

        private int _counterAvoid = 0;
        [SerializeField]private int _cashedCount = 0;
        private bool preventDoubleClick = true;
        public bool PreventDoubleClick
        {
            get
            {
                if (preventDoubleClick)
                {
                    preventDoubleClick = false;
                    StartCoroutine(ResetPreventDoubleClickCo(0.3f));
                    return true;
                }
                return false;
            }
        }
        IEnumerator ResetPreventDoubleClickCo(float sec)
        {
            yield return new WaitForSeconds(sec);
            preventDoubleClick = true;
            yield return null;
        }
        private void MoveMade()
        {
       
            if(collectionStyle == CollectionStyle.Avoid) StartCoroutine(MoveMadeCo(0.05f));
        }

        IEnumerator MoveMadeCo(float seconds)
        {

            yield return new WaitForSeconds(seconds);
            if (LevelManager.GetGameStatus() == GameState.RegenLevel || LevelManager.THIS.findMatchesStarted || !LoadingHelper.THIS.sideDotBool)
            {
                StartCoroutine(MoveMadeCo(seconds));
            }
            else
            {
                MoveMadeFunction();
            }
      
        }

        /*private void FlashText()
        {
            StartCoroutine(FlashTextCo((0.03f)));
          
        }

        private IEnumerator FlashTextCo(float secondsBetween)
        {
            Debug.Log("frame.color.r" + frame.color.r);
            if (frame.color.r < 5)
            {
                frame.color = new Color(frame.color.r, frame.color.g, frame.color.b, frame.color.a);
                StartCoroutine(FlashTextUp(secondsBetween));
                yield break;
            }
            
            yield return new WaitForSeconds(secondsBetween);
            
            frame.color = new Color(frame.color.r-1, frame.color.g-1, frame.color.b-1, frame.color.a);
            StartCoroutine(FlashTextCo(secondsBetween));
        }
        private IEnumerator FlashTextUp(float secondsBetween)
        {
            if (frame.color.r > 200)
            {
                frame.color = new Color(frame.color.r, frame.color.g, frame.color.b, frame.color.a);
                yield break;
            }
            yield return new WaitForSeconds(secondsBetween);
            
            frame.color = new Color(frame.color.r+1, frame.color.g+1, frame.color.b+1, frame.color.a);
            StartCoroutine(FlashTextCo(secondsBetween));
        }*/
        private void MoveMadeFunction()
        {
            if (collectionStyle == CollectionStyle.Avoid)
            {
                int compareCount = subTargetContainer.GetCount(true);
                if (_cashedCount != compareCount)
                {
                    _cashedCount = compareCount;
                 //   FlashText();
                }

                else
                {
                    if (preventDoubleClick)
                    {
                        _counterAvoid++;
                        GenericSettingsFunctions.SmallJumpAnimation(frame.transform);
                        GenericSettingsFunctions.SmallJumpAnimation(GoalContraint.transform);
                        GenericSettingsFunctions.SmallJumpAnimation(text.transform);
                        if(HideTargetIcon != null) GenericSettingsFunctions.SmallJumpAnimation(HideTargetIcon.transform);
                        Rl.GameManager.PlayAudio(Rl.soundStrings.AvoidedSuccessful, UnityEngine.Random.Range(0,5), true, Rl.settings.GetSFXVolume, Rl.effects.audioSource);
                    }
                    text.text = _counterAvoid.ToString();
                }
            }
        }
        private void UpdateText()
        {
        
            if(collectionStyle != CollectionStyle.Avoid)
            {
            
                text.text = ChangeNumbersToCollectionStyle(collectionStyle, subTargetContainer.GetCount(true));
            }
        
        }

        private string ChangeNumbersToCollectionStyle(CollectionStyle collectionStyle, int count)
        {
            switch (collectionStyle)
            {
                case CollectionStyle.Nothing:
                    return count.ToString();
                case CollectionStyle.Destroy:
                    return count.ToString();
                case CollectionStyle.Avoid:
                   int displayCount = Mathf.Abs(_maxCount - count);
                    return displayCount.ToString();
                  return count.ToString();
                    break;
                case CollectionStyle.Empty:
                    return count.ToString();
                default:
                    throw new ArgumentOutOfRangeException(nameof(collectionStyle), collectionStyle, null);
            }

            return count.ToString();
        }

        IEnumerator Check()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.2f);
                if (text.text == "0")
                {
                    check.SetActive(true);
                    uncheck.SetActive(false);
                    text.GetComponent<TextMeshProUGUI>().enabled = false;
                }
                else if (LevelManager.THIS.gameStatus == GameState.PreFailed || LevelManager.THIS.gameStatus == GameState.GameOver)
                {
                    check.SetActive(false);
                    uncheck.SetActive(true);
                    text.GetComponent<TextMeshProUGUI>().enabled = false;

                }
                else
                {
                    check.SetActive(false);
                    uncheck.SetActive(false);
                    text.GetComponent<TextMeshProUGUI>().enabled = true;

                }
            }
        }
        
        public static Vector2 GetTargetGUIPosition(string SpriteName)
        {
            if (!GenericFunctions.IsSubstractiveState())
                LevelManager.avoidedLateUpdate = true;
            var pos = Vector2.zero;
            var list = FindObjectsOfType(typeof(TargetGUI)) as TargetGUI[];
            foreach (var item in list)
            {
                if (item.image.GetComponent<Image>().sprite.name == SpriteName && item.gameObject.activeSelf)
                    return item.transform.position;
            }
            if (list.Length > 0) pos = list[0].transform.position;
            return pos;
        }
        
        public static Vector2 GetTargetGUIPosition(int color)
        {
            var pos = Vector2.zero;
            var list = FindObjectsOfType(typeof(TargetGUI)) as TargetGUI[];
            foreach (var item in list)
            {
                if (item.color == color && item.gameObject.activeSelf)
                    return item.transform.position;
            }
            if (list.Length > 0) pos = list[0].transform.position;
            return pos;
        }

        public void BindTargetGUI(TargetCounter subTarget)
        {
            subTargetContainer = subTarget;
            subTargetContainer.BindGUI(this);
            if (subTargetContainer.extraObject != null) color = subTargetContainer.color;
            text.text = subTargetContainer.GetCount().ToString();
        }
    }
}
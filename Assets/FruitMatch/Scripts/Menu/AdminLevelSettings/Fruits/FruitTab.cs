using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

[Serializable]

public sealed  class FruitTab: MonoBehaviour
{
    [Header("Text Overlay")]
    [SerializeField] private List<string> TabInfoTextList;
    [SerializeField] private TextMeshProUGUI TabInfoText;
    [Space]
    public List<FruitButton> tabButtons;
    public FruitButton selectedTab;
    public List<GameObject> GameObjectsToSwap;
    [SerializeField] private string buttonsDeactiveColor = "#B1BAFF";
    [SerializeField] private float buttonsDeactiveAlpha = 0.1f;

    [Space]
    [Header("AnimationDisplaySettings")]
    [SerializeField]  List<AnimationGameObjects> animationObjects = new List<AnimationGameObjects>();
    
    private IEnumerator ItemsAnimation(List<GameObject> items, bool animate, float delaytoNextItem, float fadeTime, Ease easetype, bool fadeAlpha, float fadealphatime,
        float fadeAlphaRoughness, float snapToAimPercent, float lerpMultiplier)
    {
        if(!animate && !fadeAlpha) yield break;
        
        List<float> alphas = new List<float>();
        List<List<Image>> imagesList = new List<List<Image>>();
      
        for (var i = 0; i < items.Count; i++)
        {
          if(animate) items[i].transform.localScale = Vector3.zero;

          if (fadeAlpha)
          {
              imagesList.Add(items[i].GetComponentsInChildren<Image>().ToList());
              for (var index = 0; index <  imagesList[i].Count; index++)
              {
                  alphas.Add(imagesList[i][index].color.a);
                  imagesList[i][index].GetComponent<Image>().color= new Color( imagesList[i][index].color.r,  imagesList[i][index].color.g,  imagesList[i][index].color.b, 0f);
              }
          }
        }

        for (var i = 0; i < items.Count; i++)
        {
          if(animate)  items[i].transform.DOScale(1f, fadeTime).SetEase(easetype);
          if (fadeAlpha)
          {
              for (var index = 0; index < imagesList[i].Count; index++)
              {
                  StartCoroutine(FadeAlpha(imagesList[i][index], alphas[i], fadealphatime, fadeAlphaRoughness,
                      snapToAimPercent, lerpMultiplier));
              }
          }

          if(animate) yield return new WaitForSeconds(delaytoNextItem);
          else yield break;
        }
    }

    private IEnumerator FadeAlpha(Image image, float aimAlpha, float fadeSpeed, float fadeRoughness, float snapToAimPercent, float lerpAdder)
    {
    
        if (image.color.a > aimAlpha*snapToAimPercent)
      {
          image.color = new Color( image.color.r, image.color.g, image.color.b, 1f);
          yield break;
     }
        
        float lerpedValue= Mathf.SmoothDamp(image.color.a, aimAlpha, ref lerpAdder, fadeRoughness * Time.deltaTime );
        //Debug.Log("name: " + image.transform.name + " | currentAlpha: " + image.color.a + " | aimAlpha: " + aimAlpha);
        image.color = new Color( image.color.r, image.color.g, image.color.b, lerpedValue);
        
       yield return new WaitForSeconds(fadeSpeed);
       lerpAdder += lerpAdder;
        StartCoroutine(FadeAlpha(image, aimAlpha, fadeSpeed, fadeRoughness,snapToAimPercent, lerpAdder));
    }
    public void Subscribe(FruitButton button)
    {
        if (tabButtons == null) tabButtons = new List<FruitButton>();
        tabButtons.Add(button);
    }
    public void OnTabEnter(FruitButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab || !button.ParentTabExist()) ;
        //button.backGround.sprite = tabHover;
    }
    public void OnTabExit(FruitButton button) => ResetTabs();
    public void OnTabSelected(FruitButton button)
    {
        if (selectedTab != null) selectedTab.DeSelect();
        selectedTab = button;
        selectedTab.Select();
        
        ResetTabs();
        button.backGround.color = GenericSettingsFunctions.ResetColorAlmostFull();
        button.transform.parent.GetComponent<Image>().color = GenericSettingsFunctions.ResetColorAlmostFull();
        Rl.GameManager.PlayAudio(Rl.soundStrings.TabButtonSound , Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource); 
        int index = button.transform.parent.GetSiblingIndex();
        TabInfoText.text = TabInfoTextList[index];
        for (int i = 0; i < GameObjectsToSwap.Count; i++)
        {
            if (i == index) GameObjectsToSwap[i].SetActive(true);
            else GameObjectsToSwap[i].SetActive(false);
        }

        if (animationObjects[index].PerSetting.Count <= 0) return;
        StartCoroutine(ItemsAnimation(animationObjects[index].PerSetting,animationObjects[index].Animate, animationObjects[index].DelaytoNextItem, animationObjects[index].FadeTime,
            animationObjects[index].EaseType, animationObjects[index].FadeAlpha, animationObjects[index].CoroutineCallsWaitForSeconds, animationObjects[index].FadeAlphaRoughess,
            animationObjects[index].SnapToAimPercent, animationObjects[index].LerpAdder));
    }
    
    public void ResetTabs()
    {
        Color color;
        ColorUtility.TryParseHtmlString(buttonsDeactiveColor, out color);
        for (int i = 0; i <  tabButtons.Count; i++)
        {
            tabButtons[i].GetComponent<Image>().color = color;
            tabButtons[i].GetComponent<Image>().color = new Color(
                tabButtons[i].GetComponent<Image>().color.r, tabButtons[i].GetComponent<Image>().color.g,
                tabButtons[i].GetComponent<Image>().color.b, buttonsDeactiveAlpha);
        }

        if (selectedTab != null)
        {
            selectedTab .backGround.color = GenericSettingsFunctions.ResetColorAlmostFull();
            selectedTab .transform.parent.GetComponent<Image>().color = GenericSettingsFunctions.ResetColorAlmostFull();
        }
    }
}
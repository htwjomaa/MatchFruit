using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Newtonsoft.Json.Serialization;

[RequireComponent(typeof(UnityEngine.UIElements.Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;
    public Image backGround;
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;
    [SerializeField] private TabGroupState tabGroupState;
    [SerializeField] private bool allowToggle = false;

    void Start() => backGround = GetComponent<Image>();
    public void OnPointerEnter(PointerEventData eventData) => tabGroup.OnTabEnter(CheckForChildren());
    private bool callFromWithIn;
    public void OnPointerClick(PointerEventData eventData)
    {
      //  GenericSettingsFunctions.SmallJumpAnimation(0.3f, transform);
      // if(SplashMenu.TabGroupState == tabGroupState)
      if (!toggle) tabGroup.OnTabSelected(CheckForChildren());
      else
      {
          callFromWithIn = true;
          ScaleDown(this.GetComponent<UnityEngine.UI.Image>());
      }
      
     if(allowToggle) toggle = !toggle;
     callFromWithIn = false;
    }
    

    public void ScaleDown(UnityEngine.UI.Image image)
    {
        if(!callFromWithIn)toggle = false;
        for (int i = 0; i < tabGroup.animationObjects.Count; i++)
        {
            foreach (var n in tabGroup.animationObjects[i].PerSetting)
                n.transform.DOScale(0f,0.3f).SetEase(Ease.InQuart);
        }

        this.GetComponent<UnityEngine.UI.Image>().sprite = image.sprite;

        // Invoke(nameof(SecurityScaleDown), 1f);
    }
    // public void ScaleUp()
    // {
    //     for (int i = 0; i < tabGroup.animationObjects.Count; i++)
    //     {
    //         foreach (var n in tabGroup.animationObjects[i].PerSetting)
    //             n.transform.DOScale(1f,1f).SetEase(Ease.Linear);
    //     }
    //     
    // }


    private void SecurityScaleDown()
    {
        for (int i = 0; i < tabGroup.animationObjects.Count; i++)
        {
            foreach (var n in tabGroup.animationObjects[i].PerSetting)
            {
                if(n.transform.localScale != Vector3.zero)ScaleDown(GetComponent<UnityEngine.UI.Image>());
            }
        }
    }
    
    public void FruitTab(UnityEngine.UI.Image image)
    {
        GetComponent<UnityEngine.UI.Image>().sprite = image.sprite;
            for (int i = 0; i < tabGroup.animationObjects.Count; i++)
            {
                foreach (GameObject gameObject1 in tabGroup.animationObjects[i].PerSetting)
                {
                    if (gameObject1 != null)
                    {
                        foreach (GameObject o in gameObject1.GetComponent<TabButton>().tabGroup.GameObjectsToSwap) o.SetActive(false);
                        gameObject1.GetComponent<TabButton>().tabGroup.selectedTab = null;
                        break;
                    }
                }
            }
    }
    public void ChangeTabGroupState()
    {
        SplashMenu.TabGroupState = tabGroupState;
    }
    public void  ClickThisButtonFromOtherScript()
    {
        if(SplashMenu.TabGroupState == tabGroupState) 
            
        Invoke(nameof(delayedInvoke), 0.005f);
        DeactivateObject();
    }

    [SerializeField]private GameObject objToDeactivate;
  
    private void delayedInvoke()
    {
        tabGroup.ClickNoSound = true;
        tabGroup.OnTabSelected(CheckForChildren());

    }

    public void DeactivateObject()
    {
        if (objToDeactivate != null)
        {
            StartCoroutine(DeactiveObject_CO(0.05f));
        }
    }

    IEnumerator DeactiveObject_CO(float waitForSec)
    {
       yield return new WaitForSeconds(waitForSec);
       objToDeactivate.SetActive(false);
    }
    public bool ParentTabExist()
    {
        if (this.transform.parent.GetComponent<TabButton>()) return true;

        return false;
    }

    public TabButton CheckForChildren()
    {
        if (transform.childCount == 0) return this;
        return transform.GetChild(0).GetComponent<TabButton>();
    }
    
    public TabButton CheckForParent()
    {
        if (transform.childCount == 0) return transform.parent.GetComponent<TabButton>();
        return null;
    }
    public void OnPointerExit(PointerEventData eventData) => tabGroup.OnTabExit(this);
    public void Select()
    {
        if (onTabSelected != null) 
            onTabSelected.Invoke();
    }
    public void DeSelect()
    {
        if (onTabDeselected != null)
            onTabDeselected.Invoke();
    }

    [SerializeField] private bool toggle = false;
}
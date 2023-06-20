using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UnityEngine.UIElements.Image))]
public class FruitButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public FruitTab tabGroup;
    public Image backGround;
    public UnityEvent onFruitTabSelected;
    public UnityEvent onFruitDeselected;
    
    void Start()
    {
        backGround = GetComponent<Image>();
        tabGroup.Subscribe(this);
    }
    public void OnPointerEnter(PointerEventData eventData) => tabGroup.OnTabEnter(CheckForChildren());

    public void OnPointerClick(PointerEventData eventData)
    {
        
        tabGroup.OnTabSelected(CheckForChildren());
    }

    public void  ClickThisButtonFromOtherScript() => tabGroup.OnTabSelected(CheckForChildren());

    public bool ParentTabExist()
    {
        if (this.transform.parent.GetComponent<TabButton>()) return true;

        return false;
    }
    private FruitButton CheckForChildren()
    {
        if (transform.childCount == 0) return this;
        return transform.GetChild(0).GetComponent<FruitButton >();
    }
    public void OnPointerExit(PointerEventData eventData) => tabGroup.OnTabExit(this);
    public void Select()
    {
        if (onFruitTabSelected != null) 
            onFruitTabSelected .Invoke();
    }
    public void DeSelect()
    {
        if (onFruitDeselected!= null)
            onFruitDeselected.Invoke();
    }
}
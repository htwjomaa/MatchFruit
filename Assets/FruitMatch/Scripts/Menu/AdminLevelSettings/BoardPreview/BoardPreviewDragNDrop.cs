using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public sealed class BoardPreviewDragNDrop : MonoBehaviour
{
    [ReadOnly] [SerializeField] private BoardPreviewItem currentSelectedItem;
    [ReadOnly] [SerializeField] private ItemCategory currentItemCategory = ItemCategory.All;
    [ReadOnly] public  float _targetValue =0.88f;
    [ReadOnly] public float _spacingCategoryButton =2.5f; 
    [ReadOnly] public float _spacingItemButton = 160f;
    [ReadOnly] public float _firstExtraSpacingItem = 35f;
    [ReadOnly] public float _firstItemPos = 1000f;
    [ReadOnly] float _firstExtraSpacing = 50f;
    public sbyte teleportFrom = -128;
    [ReadOnly] public float _heightSpacingItems = 175f;
    [SerializeField] public float  t =0.01f;
    [SerializeField] public float snapDifference = 0.05f;
    [SerializeField] public float waitSec = 0.05f;
    
     [SerializeField] public float moveBackDuration = 0.3f;
     [SerializeField] public RectTransform TopBarSmall;

     [SerializeField] public Ease CategoryEase = Ease.OutBounce;
     [SerializeField] public Ease ItemEase = Ease.OutBounce;
     [FormerlySerializedAs("BoardPreviewCategoryButton")] [SerializeField]
     
    public List<BoardPreviewCategoryButton> allCategoryButtons = new List<BoardPreviewCategoryButton>();
    public ItemCategory CurrentItemCategory
    {
        get => currentItemCategory;
        set
        {
            currentItemCategory = value;
            CategorySelected();
        }
    }
    private BoardPreviewItem currentSpecialSelected;
    public BoardPreviewItem CurrentSpecialSelected
    {
        get => currentSpecialSelected;
        set
        {
            currentSpecialSelected = value;
            SpecialSelected();
        }
    }
    private void GetSpacingValues()
    {
        //So it scales with all Screens
        int width = Screen.width;
        int refWidth = 2732;

        int height = Screen.height;
        int refHeight = 2048;
        
        float targetValueMax = 0.88f;
        float spacingCategoryButtonMax = 178;
        float spacingItemButtonMax = 175;
        float firstExtraSpacingItemMax = 60;
        float firstExtraSpacingMax = 101;
        float firstItemPosMax = 1060;
        float heightSpacingItemsMax = 180;

        _targetValue = MathLibrary.Remap(0, refWidth, 0, targetValueMax, width );
        _spacingCategoryButton = MathLibrary.Remap(0, refWidth, 0, spacingCategoryButtonMax, width );
        _spacingItemButton = MathLibrary.Remap(0, refWidth, 0, spacingItemButtonMax, width );
        _firstExtraSpacingItem  = MathLibrary.Remap(0, refWidth, 0, firstExtraSpacingItemMax, width );
        _firstExtraSpacing= MathLibrary.Remap(0, refWidth, 0, firstExtraSpacingMax, width );
        _firstItemPos = MathLibrary.Remap(0, refWidth, 0, firstItemPosMax, width );
        _heightSpacingItems = MathLibrary.Remap(0, refHeight, 0, heightSpacingItemsMax, height);
    }
    public void ToggleTeleportTarget(sbyte fieldNumber)
    {    // using -128 as a bool because -128 doesnt get used anyway and an extra bool is something more to keep track off
        // also -1  means no teleport but this is relevant somewhere else
        if (teleportFrom == -128) teleportFrom= fieldNumber;
        else teleportFrom = -128;
    }
    public BoardPreviewItem CurrentSelectedItem
    {
        get => currentSelectedItem;
        set
        {
            currentSelectedItem = value;
            ItemSelected();
        }
    }
    private bool preventDoubleClick = true;
    public bool PreventDoubleClick
    {
        get
        {
            if (preventDoubleClick)
            {
                preventDoubleClick = false;
                StartCoroutine(ResetPreventDoubleClickCo(0.5f));
                return true;
            }
            return false;
        }
    }
    private bool categoryMoveFinished = true;
    public bool categoryMovFin;
    public bool CategoryMoveFinished
    {
        get
        {
            if (categoryMoveFinished)
            {
                categoryMoveFinished = false;
                StartCoroutine(ResetCategoryMovedCo(0.25f));
                return true;
            }
            return false;
        }
    }
    IEnumerator ResetCategoryMovedCo(float sec)
    {
        yield return new WaitForSeconds(sec);
        categoryMoveFinished = true;
        categoryMovFin = true;
        yield return null;
    }
    IEnumerator ResetPreventDoubleClickCo(float sec)
    {
        yield return new WaitForSeconds(sec);
        preventDoubleClick = true;
        yield return null;
    }
    public delegate void NewItemSelected ();
    public event NewItemSelected NewItemSelectedEvent;
    
    public delegate void NewSpecialSelected ();
    public event NewSpecialSelected NewSpecialSelectedEvent;
    
    public delegate void NewCategorySelected ();
    public event NewCategorySelected NewCategorySelectedEvent;
    
    public delegate void MoveOutItems();
    public event MoveOutItems MoveOutItemsEvent;
    
    public delegate void MoveInItems();
    public event MoveInItems MoveInItemsEvent;

    public void InvokeMoveInItemsEvent() => MoveInItemsEvent?.Invoke();
    public void InvokeNewCategorySelectedEvent() => NewCategorySelectedEvent?.Invoke();
    private void ItemSelected()
    {
        NewItemSelectedEvent?.Invoke();
    }
    private void SpecialSelected()
    {
        NewSpecialSelectedEvent?.Invoke();
    }
    private void CategorySelected()
    {
        MoveIn();
        StartCoroutine(CheckIfMoveFinishedCo());
    }

    IEnumerator CheckIfMoveFinishedCo()
    {
        yield return new WaitForSeconds(0.04f);
        if (categoryMovFin)
        {
           
            MoveOutItemsEvent?.Invoke();
            MoveTopBarSmallOut();
            SetTkind();
            yield break;
        }

    
        StartCoroutine(CheckIfMoveFinishedCo());
    }

    public void SetTkind()
    {
        switch (CurrentItemCategory)
        {
            case ItemCategory.Overview:
                Rl.adminLevelSettingsTiles.CurrentTileKind = TKind.Tiles;
                break;
            case ItemCategory.All:
                Rl.adminLevelSettingsTiles.CurrentTileKind = TKind.Tiles;
                break;
            case ItemCategory.Object:
                Rl.adminLevelSettingsTiles.CurrentTileKind = TKind.Tiles;
                break;
            case ItemCategory.Arrow:
                Rl.adminLevelSettingsTiles.CurrentTileKind = TKind.Vectors;
                break;
            case ItemCategory.Teleport:
                Rl.adminLevelSettingsTiles.CurrentTileKind = TKind.Teleports;
                break;
            case ItemCategory.Fruit:
                Rl.adminLevelSettingsTiles.CurrentTileKind = TKind.Tiles;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Rl.adminLevelSettingsTiles.InvokeLoadFieldsEvent();
    }
    public TKind GetTkind(ItemCategory itemCategory)
    {
        switch (itemCategory)
        {
            case ItemCategory.Overview: return TKind.Tiles;
            case ItemCategory.All: return TKind.Tiles;
            case ItemCategory.Object: return TKind.Tiles;
            case ItemCategory.Arrow: return TKind.Vectors;
            case ItemCategory.Teleport: return  TKind.Teleports;
            case ItemCategory.Fruit: return  TKind.Tiles;
            default: throw new ArgumentOutOfRangeException();
        }
    }
    public void IsFieldValid()
    {
        if (CurrentSelectedItem == null) return;
        if (GetTkind(CurrentSelectedItem.ItemCategory) != Rl.adminLevelSettingsTiles.CurrentTileKind)
        {
            Rl.adminLevelSettingsTiles.CurrentTileKind = GetTkind(CurrentSelectedItem.ItemCategory);
            Rl.adminLevelSettingsTiles.InvokeLoadFieldsEvent();
        }
    }
   [SerializeField] private Vector3 _cashedTopBarSmallPos;
    private void Awake()
    {
        _cashedTopBarSmallPos = TopBarSmall.transform.localPosition;
    }

    private void MoveTopBarSmallOut() 
        => TopBarSmall.transform.DOLocalMove(new Vector3(_cashedTopBarSmallPos.x, _cashedTopBarSmallPos.y+350,_cashedTopBarSmallPos.z), 0.25f, true).SetEase(CategoryEase) ;
    private void MoveTopBarSmallIn() 
        => TopBarSmall.transform.DOLocalMove(_cashedTopBarSmallPos, 0.25f, true) ;

    public bool toggleMoveOut = true;
    public void ClickBigButton()
    {
        if (!PreventDoubleClick) return;
        GetSpacingValues();
        MoveInItemsEvent?.Invoke();
        CurrentSelectedItem = null;
        CurrentSpecialSelected = null;
        //CurrentItemCategory = ItemCategory.Overview;
        if (toggleMoveOut) MoveOut();
        else MoveIn();
        GenericSettingsFunctions.SmallShake(transform.parent.transform);
    }
    [Button()] private void MoveIn()
    {
        bool activate = CategoryMoveFinished; //dont remove it is needed
        categoryMovFin = false;
        NewCategorySelectedEvent?.Invoke();
        toggleMoveOut = true;
        MoveTopBarSmallIn();
    }
    [Button()]private void MoveOut()
    {
        toggleMoveOut = false;
        bool activate = CategoryMoveFinished; //dont remove it is needed
        categoryMovFin = false;
        Vector3 pos = GetComponent<RectTransform>().position;
        pos = new Vector3(pos.x + _firstExtraSpacing, pos.y,pos.z);
        List<Image> allImages = new List<Image>();
        for (int i = 0; i < allCategoryButtons.Count; i++)
        {
            Vector3 newPos = new Vector3(pos.x + (i+1)*_spacingCategoryButton, pos.y, pos.z);
            allCategoryButtons[i].transform.parent.GetComponent<RectTransform>().transform.DOMove(newPos, 0.25f).SetEase(CategoryEase);
            allImages.Add(allCategoryButtons[i].GetComponent<Image>());
            allImages.Add(allCategoryButtons[i].transform.parent.GetComponent<Image>());
        }

        TransitionAlphas(_targetValue, t, snapDifference, waitSec, allImages.ToArray());
    }
    public void TransitionAlphas(float targetValue, float t, float snapDifference, float waitSec, params Image[] images)
    {
        StopCoroutine(TransitionAlphasCO(targetValue, t, snapDifference, waitSec, images));
        StartCoroutine(TransitionAlphasCO(targetValue, t, snapDifference, waitSec, images));
    }
    private IEnumerator TransitionAlphasCO(float targetValue, float stepValue, float snapValue, float waitSec, params Image[] images)
    {
        bool targetValueReached = false;
        for (int i = 0; i < images.Length; i++)
        {
            float a = Mathf.Lerp(images[i].color.a, targetValue, stepValue);
            if ((Mathf.Abs(a - targetValue)) > snapValue)
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, a);
            else
            {
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, targetValue);
                targetValueReached = true;
            }
        }
        if (targetValueReached) yield break;
        
        yield return new WaitForSeconds(waitSec);
        StartCoroutine(TransitionAlphasCO(targetValue, stepValue,snapValue,waitSec,images));
    }
}
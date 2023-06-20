using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
public sealed class BoardPreviewItem : MonoBehaviour
{
    [SerializeField] private Image bgImageActive;
    [SerializeField] private Image bgImageInactive;
    [SerializeField] private Button itemImage;
    [SerializeField] public ItemCategory ItemCategory = ItemCategory.Object;
    [SerializeField] public string Identifier = "";
    private Color _defaultColor = new (255, 255, 255, 1);
    private float _defaultAlpha = 0.95f;
    private float _buttonsDeActiveAlpha = 0.25f;
    private string _buttonsDeActiveColor = "#B1BAFF";
    
    IEnumerator ResetPreventDoubleClickCo(float sec)
    {
        yield return new WaitForSeconds(sec);
        preventDoubleClick = true;
        yield return null;
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
    public void ClickResetArrow()
    {
        if (!PreventDoubleClick) return;
        switch (Rl.adminLevelSettingsTiles.CurrentTileKind)
        {
            case TKind.Tiles:
                break;
            case TKind.Vectors:
                for (int i = 0;
                     i < Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs.Length;
                     i++)
                {
                    Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[i].Direction =
                        Directions.bottom;
                    Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[i]
                        .IsDirectionStart = false;
                }
                for (int i = ((int)Rl.adminLevelSettingsBoard.BoardHeightSlider.value-1) * 9; i < 81; i++)
                    Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[i]
                        .IsDirectionStart = true;
                break;
            case TKind.Teleports:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
         Rl.BoardPreview.StartDrawBoard(0.155f);
        GenericSettingsFunctions.SmallShake(0.155f, 1, transform);
        if(FieldState.CurrentSection == CopySection.TileSettings) Rl.adminLevelSettingsTiles.InvokeShowItem();
    }
    //private float _buttonsDeActiveAlpha = 0.125f;
    [SerializeField] private bool isSpecial;
    void Start()
    {
       if(!isSpecial) Rl.boardPreviewDragNDrop.NewItemSelectedEvent += ItemSelected;
       else if(Identifier != "reset")Rl.boardPreviewDragNDrop.NewSpecialSelectedEvent+= SpecialSelected;
        
        Rl.boardPreviewDragNDrop.MoveOutItemsEvent += MoveOut;
        Rl.boardPreviewDragNDrop.MoveInItemsEvent += MoveIn;
    }
    private void OnDestroy()
    {
        if(!isSpecial) Rl.boardPreviewDragNDrop.NewItemSelectedEvent -= ItemSelected;
        else if(Identifier != "reset")Rl.boardPreviewDragNDrop.NewSpecialSelectedEvent -= SpecialSelected;
        
        Rl.boardPreviewDragNDrop.MoveOutItemsEvent -= MoveOut;
        Rl.boardPreviewDragNDrop.MoveInItemsEvent -= MoveIn;
    }
    public void ClickOnItem()
    {
        if (!isSpecial)
            {
                Rl.boardPreviewDragNDrop.IsFieldValid();
                if (Rl.boardPreviewDragNDrop.CurrentSelectedItem == this) return; 
                Rl.boardPreviewDragNDrop.CurrentSelectedItem = this;
            }
         else if(Identifier != "reset")
            {
                if (Rl.boardPreviewDragNDrop.CurrentSpecialSelected == this) return; 
                Rl.boardPreviewDragNDrop.CurrentSpecialSelected  = this;
            }
    }
    private void MoveOut()
    {
        switch (Rl.boardPreviewDragNDrop.CurrentItemCategory)
        {
            case ItemCategory.Overview:
                break;
            case ItemCategory.All:
                break;
            case ItemCategory.Object:
                MoveOutObjects();
                break;
            case ItemCategory.Arrow:
                MoveOutArrows();
                break;
            case ItemCategory.Teleport:
                MoveOutTeleports();
                break;
            case ItemCategory.Fruit:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private Vector3 modifiyPos(ref int index, Vector3 pos)
    {
        if (index == 10)
        {
            pos = new Vector3(pos.x-Rl.boardPreviewDragNDrop._firstItemPos, pos.y,pos.z);
        }
        if (index > 4)
        {
            pos = new Vector3(pos.x, pos.y+Rl.boardPreviewDragNDrop._heightSpacingItems,pos.z);
            index -= 5;
        }
        return new Vector3(pos.x + Rl.boardPreviewDragNDrop._firstExtraSpacingItem, pos.y,pos.z);
    }
    [Button] private void MoveOutTeleports()
    {
        ActivateAll(_defaultColor, _defaultAlpha);
        if (ItemCategory != Rl.boardPreviewDragNDrop.CurrentItemCategory) return;
        Rl.boardPreviewDragNDrop.toggleMoveOut = false;
        bool activate = Rl.boardPreviewDragNDrop.CategoryMoveFinished;
        Rl.boardPreviewDragNDrop.categoryMovFin = false;
        Vector3 pos = Rl.boardPreviewDragNDrop.GetComponent<RectTransform>().position;
        int index = bgImageInactive.transform.parent.transform.GetSiblingIndex();
        Debug.Log("SiblingIndex: " + index);
        pos = modifiyPos(ref index, pos);
        List<Image> allImages = new List<Image>();
      
        Vector3 newPos = new Vector3(pos.x + (index+1)*Rl.boardPreviewDragNDrop._spacingItemButton, pos.y,pos.z);
          
        allImages.Add(bgImageActive);
        allImages.Add(bgImageInactive);
        allImages.Add(itemImage.image);

        for(int i = 0; i < allImages.Count; i++)
            allImages[i].transform.parent.GetComponent<RectTransform>().transform.DOMove(newPos, 0.38f).SetEase(Rl.boardPreviewDragNDrop.ItemEase);
            
        Rl.boardPreviewDragNDrop.TransitionAlphas(
            Rl.boardPreviewDragNDrop._targetValue,
            Rl.boardPreviewDragNDrop.t,
            Rl.boardPreviewDragNDrop.snapDifference,
            Rl.boardPreviewDragNDrop.waitSec,
            allImages.ToArray());
        
    }
    [Button] private void MoveOutIems()
    {
        ActivateAll(_defaultColor, _defaultAlpha);
        if (ItemCategory != Rl.boardPreviewDragNDrop.CurrentItemCategory) return;
        Rl.boardPreviewDragNDrop.toggleMoveOut = false;
        bool activate = Rl.boardPreviewDragNDrop.CategoryMoveFinished;
        Rl.boardPreviewDragNDrop.categoryMovFin = false;
        Vector3 pos = Rl.boardPreviewDragNDrop.GetComponent<RectTransform>().position;
        int index = bgImageInactive.transform.parent.transform.GetSiblingIndex();
        Debug.Log("SiblingIndex: " + index);
        pos = modifiyPos(ref index, pos);
        List<Image> allImages = new List<Image>();
      
        Vector3 newPos = new Vector3(pos.x + (index+1)*Rl.boardPreviewDragNDrop._spacingItemButton, pos.y,pos.z);
          
        allImages.Add(bgImageActive);
        allImages.Add(bgImageInactive);
        allImages.Add(itemImage.image);

        for(int i = 0; i < allImages.Count; i++)
            allImages[i].transform.parent.GetComponent<RectTransform>().transform.DOMove(newPos, 0.38f).SetEase(Rl.boardPreviewDragNDrop.ItemEase);
            
        Rl.boardPreviewDragNDrop.TransitionAlphas(
            Rl.boardPreviewDragNDrop._targetValue,
            Rl.boardPreviewDragNDrop.t,
            Rl.boardPreviewDragNDrop.snapDifference,
            Rl.boardPreviewDragNDrop.waitSec,
            allImages.ToArray());
    }
    [Button] private void MoveOutObjects()
    {
        ActivateAll(_defaultColor, _defaultAlpha);
        if (ItemCategory != Rl.boardPreviewDragNDrop.CurrentItemCategory) return;
        Rl.boardPreviewDragNDrop.toggleMoveOut = false;
        bool activate = Rl.boardPreviewDragNDrop.CategoryMoveFinished;
        Rl.boardPreviewDragNDrop.categoryMovFin = false;
        Vector3 pos = Rl.boardPreviewDragNDrop.GetComponent<RectTransform>().position;
        int index = bgImageInactive.transform.parent.transform.GetSiblingIndex();
        Debug.Log("SiblingIndex: " + index);
        pos = modifiyPos(ref index, pos);
        List<Image> allImages = new List<Image>();
      
        Vector3 newPos = new Vector3(pos.x + (index+1)*Rl.boardPreviewDragNDrop._spacingItemButton, pos.y,pos.z);
          
        allImages.Add(bgImageActive);
        allImages.Add(bgImageInactive);
        allImages.Add(itemImage.image);

        for(int i = 0; i < allImages.Count; i++)
            allImages[i].transform.parent.GetComponent<RectTransform>().transform.DOMove(newPos, 0.38f).SetEase(Rl.boardPreviewDragNDrop.ItemEase);
            
        Rl.boardPreviewDragNDrop.TransitionAlphas(
            Rl.boardPreviewDragNDrop._targetValue,
            Rl.boardPreviewDragNDrop.t,
            Rl.boardPreviewDragNDrop.snapDifference,
            Rl.boardPreviewDragNDrop.waitSec,
            allImages.ToArray());
        
        Invoke(nameof(SetToOneObject), 0.39f);
    }
    
    [Button] private void MoveOutArrows()
    {
        ActivateAll(_defaultColor, _defaultAlpha);
        if (ItemCategory != Rl.boardPreviewDragNDrop.CurrentItemCategory) return;
        Rl.boardPreviewDragNDrop.toggleMoveOut = false;
        bool activate = Rl.boardPreviewDragNDrop.CategoryMoveFinished;
        Rl.boardPreviewDragNDrop.categoryMovFin = false;
        Vector3 pos = Rl.boardPreviewDragNDrop.GetComponent<RectTransform>().position;
        int index = bgImageInactive.transform.parent.transform.GetSiblingIndex();
        pos = modifiyPos(ref index, pos);
        List<Image> allImages = new List<Image>();
      
            Vector3 newPos = new Vector3(pos.x + (index+1)*Rl.boardPreviewDragNDrop._spacingItemButton, pos.y,pos.z);
          
            allImages.Add(bgImageActive);
            allImages.Add(bgImageInactive);
            allImages.Add(itemImage.image);

            for(int i = 0; i < allImages.Count; i++)
                allImages[i].transform.parent.GetComponent<RectTransform>().transform.DOMove(newPos, 0.38f).SetEase(Rl.boardPreviewDragNDrop.ItemEase);
            
        Rl.boardPreviewDragNDrop.TransitionAlphas(
            Rl.boardPreviewDragNDrop._targetValue,
            Rl.boardPreviewDragNDrop.t,
            Rl.boardPreviewDragNDrop.snapDifference,
            Rl.boardPreviewDragNDrop.waitSec,
            allImages.ToArray());
        
        Invoke(nameof(SetToOneArrow), 0.39f);
    }

    public void ClickResetAllObjects()
    {
        for (int i = 0; i < Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs.Length;i++)
        {
            Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[i].TileType =
                TT.Normal;
        }
        Rl.boardPreviewDragNDrop.CurrentSpecialSelected = this; 
        Rl.boardPreviewDragNDrop.CurrentSelectedItem = null; 
        Rl.BoardPreview.StartDrawBoard();
    }
    
    public void ClickResetAllTeleport()
    {
        for (int i = 0; i < Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs.Length;i++)
        {
            Rl.saveClipBoard.TileSettingFieldConfigs[FieldState.CurrentField].TileSettingConfigs[i].TeleportTarget = -1;
        }
        Rl.boardPreviewDragNDrop.CurrentSpecialSelected = this; 
        Rl.boardPreviewDragNDrop.CurrentSelectedItem = null; 
        Rl.BoardPreview.StartDrawBoard();
    }
    private void SetToOneArrow()
    {
        if (Identifier == "one" && Rl.adminLevelSettingsTiles.CurrentTileKind == TKind.Vectors) Rl.boardPreviewDragNDrop.CurrentSpecialSelected = this;
    }
    private void SetToOneObject()
    {
        if (Identifier == "one" && Rl.adminLevelSettingsTiles.CurrentTileKind == TKind.Tiles) Rl.boardPreviewDragNDrop.CurrentSpecialSelected = this;
    }
    private void MoveIn()
    {
        if (Rl.boardPreviewDragNDrop.transform.position == itemImage.transform.parent.transform.position) return;
   
        List<Image> allImages = new List<Image>();
        allImages.Add(bgImageActive);
        allImages.Add(bgImageInactive);
        allImages.Add(itemImage.image);


        for (int i = 0; i < allImages.Count; i++)
        {
            allImages[i].transform.DOMove(Rl.boardPreviewDragNDrop.transform.position, 0.3f);
        }
        Rl.boardPreviewDragNDrop.TransitionAlphas(
            0, 
            Rl.boardPreviewDragNDrop.t,  
            Rl.boardPreviewDragNDrop.snapDifference,
            Rl.boardPreviewDragNDrop.waitSec,
            allImages.ToArray()
            );
    }
    private void SpecialSelected()
    {
        if(Rl.boardPreviewDragNDrop.CurrentSpecialSelected == this) ActivateAll(_defaultColor, _defaultAlpha);
        else DeactivateAll(_buttonsDeActiveColor, _buttonsDeActiveAlpha);
    }
    private void ItemSelected()
    {
        if(Rl.boardPreviewDragNDrop.CurrentSelectedItem == this) ActivateAll(_defaultColor, Rl.boardPreviewDragNDrop._targetValue);
        else DeactivateAll(_buttonsDeActiveColor, _buttonsDeActiveAlpha);
    }
    private void ActivateAll(Color defaultColor, float defaultAlpha)
    {
        itemImage.image.color = GenericSettingsFunctions.SetColorAndAlpha(defaultColor, defaultAlpha);
        bgImageActive.color = GenericSettingsFunctions.SetColorAndAlpha(defaultColor, defaultAlpha);
        bgImageInactive.color = GenericSettingsFunctions.SetColorAndAlpha(defaultColor, defaultAlpha);
    }
    private void DeactivateAll(string hexCode, float buttonsDeActiveAlpha)
    {
        Color color;
        ColorUtility.TryParseHtmlString(hexCode, out color);
    
        itemImage.image.color = GenericSettingsFunctions.SetColorAndAlpha(color, buttonsDeActiveAlpha);
        bgImageActive.color = GenericSettingsFunctions.SetColorAndAlpha(color, buttonsDeActiveAlpha);
        bgImageInactive.color = GenericSettingsFunctions.SetColorAndAlpha(color, buttonsDeActiveAlpha);
    }
}
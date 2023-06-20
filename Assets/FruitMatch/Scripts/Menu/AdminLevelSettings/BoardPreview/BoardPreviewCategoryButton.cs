using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class BoardPreviewCategoryButton : MonoBehaviour
{
    [SerializeField] public ItemCategory ItemCategory = ItemCategory.Object;
    [SerializeField] private Button itemImage;
    private bool preventDoubleClick = true;
    public bool PreventDoubleClick
    {
        get
        {
            if (preventDoubleClick)
            {
                preventDoubleClick = false;
                StartCoroutine(ResetPreventDoubleClickCo(0.75f));
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

    private int _siblingIndex;
    private void Start()
    {
        Rl.boardPreviewDragNDrop.NewCategorySelectedEvent += MoveBackToButton;
        _siblingIndex = FindSiblingIndex();
    }

    private void OnDestroy()
    {
        Rl.boardPreviewDragNDrop.NewCategorySelectedEvent -= MoveBackToButton;
    }

    private int FindSiblingIndex()
    {
        for (int i = 0; i < Rl.boardPreviewDragNDrop.allCategoryButtons.Count; i++)
        {
            if (Rl.boardPreviewDragNDrop.allCategoryButtons[i] == this)
                return i;
        }

        return 1;
    }
    private void MoveBackToButton()
    {
        if (Rl.boardPreviewDragNDrop.transform.position == itemImage.transform.position) return;
        if (_siblingIndex is 1 or 0)
        {
            itemImage.transform.parent.transform.DOMove(Rl.boardPreviewDragNDrop.transform.position, 2 *Rl.boardPreviewDragNDrop.moveBackDuration);
        }
        else
        {
            itemImage.transform.parent.transform.DOMove(Rl.boardPreviewDragNDrop.transform.position, _siblingIndex *Rl.boardPreviewDragNDrop.moveBackDuration);
        }

        List<Image> allImages = new List<Image>();
        allImages.Add(itemImage.image);
        allImages.Add(itemImage.transform.parent.GetComponent<Image>());
        Rl.boardPreviewDragNDrop.TransitionAlphas(
            0, 
            Rl.boardPreviewDragNDrop.t,  
            Rl.boardPreviewDragNDrop.snapDifference,
            Rl.boardPreviewDragNDrop.waitSec,
            allImages.ToArray()
        );

    }
    public void ClickOnCategory()
    {
      
        if (PreventDoubleClick)
        {
            Rl.boardPreviewDragNDrop.CurrentItemCategory = ItemCategory;
            GenericSettingsFunctions.SmallShake( itemImage.transform.parent.transform);
        }
    }
    
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using FruitMatch.Scripts.Core;
using FruitMatch.Scripts.Level;
using UnityEngine;
using UnityEngine.UI;

public class SequencePreviewGUI : MonoBehaviour
{
    
    public Image image;
    public Image HideTargetIcon;
    public Image BorderImage;
    public Image frame;

    public int GUINumber;
    // Start is called before the first frame update
    private void Awake()
    {
        if(HideTargetIcon != null) HideTargetIcon.color = new Color(HideTargetIcon.color.r, HideTargetIcon.color.g, HideTargetIcon.color.b, 0f);
    }

    private void Start()
    {
       LevelManager.SequencePreviewGUILoadTrigger +=LoadSequenceImage;
    }

    private void OnDestroy()
    {
        LevelManager.SequencePreviewGUILoadTrigger  -=LoadSequenceImage;
    }

    public void LoadSequenceImage()
    {
        image.sprite = LoadingHelper.THIS.loadedSpritesDebug[LevelManager.THIS.MatchSequence[GUINumber]];
        
    }
}

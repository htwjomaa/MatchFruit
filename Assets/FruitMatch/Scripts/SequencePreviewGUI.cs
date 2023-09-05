using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequencePreviewGUI : MonoBehaviour
{
    public Image image;
    public GameObject check;
    public GameObject uncheck;
    public Image HideTargetIcon;
    public Image BorderImage;
    public Image frame;
    // Start is called before the first frame update
    private void Awake()
    {
        if(HideTargetIcon != null) HideTargetIcon.color = new Color(HideTargetIcon.color.r, HideTargetIcon.color.g, HideTargetIcon.color.b, 0f);
    }
}

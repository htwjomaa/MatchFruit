using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Aufgaben : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goalsText;
    
    // Start is called before the first frame update
    void Start()
    {
        goalsText.color = new Color(goalsText.color.r, goalsText.color.g, goalsText.color.b, 0);
        Invoke(nameof(InvokeFadeIn), 2.5f);
    }
    private void InvokeFadeIn() =>  StartCoroutine(FadeIconsInCO(0.07f));
    // Update is called once per frame
    IEnumerator FadeIconsInCO(float waitInBetweenFramesSec)
    {
        if (goalsText.color.a > 0.95f)
        {
            goalsText.color = new Color(goalsText.color.r, goalsText.color.g, goalsText.color.b, 1);
            yield break;
        } 
        yield return new WaitForSeconds(waitInBetweenFramesSec);
        goalsText.color = new Color(goalsText.color.r, goalsText.color.g, goalsText.color.b, goalsText.color.a+0.05f);
        
        StartCoroutine(FadeIconsInCO(waitInBetweenFramesSec));
    }
}

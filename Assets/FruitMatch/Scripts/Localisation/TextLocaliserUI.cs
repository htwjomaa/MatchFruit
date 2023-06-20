using System;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocaliserUI : MonoBehaviour
{
    private TextMeshProUGUI textfield;
    public LocalisedString[] LocalisedStrings;
    
    void Start()
    {
        LocalisationSystem.AllTextLocaliserUis.Add(this);
        textfield = GetComponent<TextMeshProUGUI>();
        foreach(LocalisedString localisedString in LocalisedStrings)
        textfield.text = localisedString.value;

       // textfield.font = Rl.localiserManager.currentFont;
    }

    private void OnDestroy()
    {
        LocalisationSystem.AllTextLocaliserUis.Remove(this);
    }
}

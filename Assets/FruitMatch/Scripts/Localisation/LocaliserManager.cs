using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class LocaliserManager : ScriptableObject
{
  public TMP_FontAsset currentFont;

  [Button] private void ChangeAllLocaliserFontsToCurrentFont()
  {
    foreach (TextLocaliserUI textLocaliserUI in   Resources.FindObjectsOfTypeAll<TextLocaliserUI>())
      textLocaliserUI.GetComponent<TextMeshProUGUI>().font = currentFont;
  }

  private void OnDestroy()
  {
    LocaliserMonos.Clear();
  }

  public List<LocaliserMono> LocaliserMonos = new List<LocaliserMono>();
  
  public void StartLanguageCheck()
  {
    for (int index = 0; index < LocaliserMonos.Count; index++)
    {
    
      if (  LocaliserMonos[index] == null) LocaliserMonos.Remove(  LocaliserMonos[index]);
      else CheckIfActiveLanguageForColor(  LocaliserMonos[index].language,   LocaliserMonos[index].GetComponent<Image>());
    }
  }

  public void CheckIfActiveLanguageForColor(Language language, Image image)
  {
    if (language != LocalisationSystem.CurrentLanguage)
    {
      image.color = new Color(0.19f, 0.25f, 0.3f, 0.55f);
    }
         
    else
    {
      image.color = new Color(255, 255, 255, 1);
    }
  }
}

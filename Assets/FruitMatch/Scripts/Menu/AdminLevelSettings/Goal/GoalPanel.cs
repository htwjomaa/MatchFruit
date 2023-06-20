using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class GoalPanel : MonoBehaviour
{
   public Image thisImage;
   public Sprite thisSprite;
   public TextMeshProUGUI  thisText;
   public RectTransform thisTextRect;
   public string thisString;

   private void Start() => Setup();
   void Setup()
   {
      thisImage.sprite = thisSprite;
      thisText.text = thisString;
   }
}
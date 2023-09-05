using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class AdminLevelSingleGoalSetting : MonoBehaviour
{
    [SerializeField] private GoalNumber goalNumber; 
    [SerializeField] private bool isFruit;
    [SerializeField] private bool isCollectionStyle;
    private TextMeshProUGUI _enumText; 
    private TextMeshProUGUI _sibling;
    private Image _uiImage;
    
   private void Awake()
   {
       _enumText = GetComponent<TextMeshProUGUI>();
       _sibling = GetSibling();
       _uiImage = GetImage();
   }

   private void Start()
   {
   AdminLevelSettingsGoalConfig.LoadCurrentObjective += MatchSpriteToEntry;
   }

   private void OnDestroy()
   {
       AdminLevelSettingsGoalConfig.LoadCurrentObjective -= MatchSpriteToEntry;
   }

   public void ClickNextFruitOrColor()
       => Rl.adminLevelSettingsGoalConfig.ClickNextFruitOrColor(goalNumber, isFruit, ref _enumText, ref _sibling );
   public void LoadSameFruitOrColor()
       => Rl.adminLevelSettingsGoalConfig.LoadSame(goalNumber, isFruit, ref _enumText, ref _sibling );
   
   
   
   
   public void ClickNextCollectionStyle()
       => Rl.adminLevelSettingsGoalConfig.ClickNextCollectionStyle(goalNumber, ref _enumText);
   public void LoadSameCollectionStyle()
       => Rl.adminLevelSettingsGoalConfig.LoadSame(goalNumber, ref _enumText);

   private  TextMeshProUGUI GetSibling()
   {
       int siblingIndex = transform.GetSiblingIndex();
       for (int i = 0; i < transform.parent.childCount; i++)
       {
           if(i != siblingIndex &&  transform.parent.GetChild(i).GetComponent<TextMeshProUGUI>() != null)
               return transform.parent.GetChild(i).GetComponent<TextMeshProUGUI>();
       }
       return null;
   }

   private void MatchSpriteToEntry()
   {
       if(!isCollectionStyle)Rl.adminLevelSettingsGoalConfig.LoadCurrentGoalSetting(goalNumber, isFruit, ref _enumText, ref _sibling);
       else Rl.adminLevelSettingsGoalConfig.LoadCurrentGoalSetting(goalNumber, ref _enumText);
   }

   private Image GetImage()
   {
       Image uiImage = null;
       for (int i = 0; i < transform.parent.childCount; i++)
       {
           if (transform.parent.GetChild(i).GetComponent<Image>() != null)
               uiImage = transform.parent.GetChild(i).GetComponent<Image>();
       }

       return uiImage;
   }

   public void SetObjectiveToNothing()
   {
       if(!AdminLevelSettingsGoalConfig.IsObjectiveSetting)
       Rl.adminLevelSettingsGoalConfig.SetToNothingButton(goalNumber, ref _enumText, ref _sibling);
       
       else  Rl.adminLevelSettingsGoalConfig.ResetSliderToZero(goalNumber);
   }
}
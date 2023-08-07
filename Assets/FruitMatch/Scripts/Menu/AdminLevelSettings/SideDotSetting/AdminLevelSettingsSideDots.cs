using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
// ----------------------
//   0 - bottom | 1 - top | 2 - right | 3 - left
//------------------------
public class AdminLevelSettingsSideDots : MonoBehaviour
{
   [SerializeField] public Switch enableBarButton;
   public byte currentNumberField;
   public byte currentSetting;
   private byte currentBar = 0;
[SerializeField] private SideDotSettingButton FirstSideDotSettingButton;

   public byte CurrentBar
   {
      get => currentBar;
      set
      {
         currentBar = value;
         LoadFromDiskEvent?.Invoke();
      }
   }

   public void InvokeFirstSideDotSettingButton()
   {
      StartCoroutine(FirstSideDotSettingButton_CO(0.00002f));
   }

   IEnumerator FirstSideDotSettingButton_CO(float waitForSec)
   {
      yield return new WaitForSeconds(waitForSec);
      FirstSideDotSettingButton.ClickButton();
   }
   private SideDotSettingButton currentSideDotSettingButton;
   public SideDotSettingButton fallBackDotSettingButton;
   public List<Sprite> SideDotSpriteList = new();
   public SideDotSettingButton CurrentSideDotSettingButton
   {
      get
      {
         if (currentSideDotSettingButton == null)
         {
            currentSideDotSettingButton = fallBackDotSettingButton;
            CurrenSideDotSettingButtonChanged();
         }
          
         return currentSideDotSettingButton;
      }
      set
      {
         currentSideDotSettingButton = value;
         CurrenSideDotSettingButtonChanged();
      }
   }

   public int Currentcolumn { get; set; }

   public delegate void LoadInformation();
   public static event LoadInformation LoadInformationEvent;
   public delegate void SideDotSettingButtonChanged();
   public static event SideDotSettingButtonChanged SideDotSettingButtonChangedEvent;

   public delegate void SideDotLoadTileSettings();
   public static event SideDotLoadTileSettings SideDotLoadTileSettingsEvent;
   public delegate void ResetSettings();

   public static event ResetSettings ResetSetting;
   
   public delegate void LoadFromDisk();
   public static event LoadFromDisk LoadFromDiskEvent;

   public static void InvokeLoadFromDisk() => LoadFromDiskEvent?.Invoke();
   [SerializeField] private TextMeshProUGUI barText;
   private void CurrenSideDotSettingButtonChanged()
   {
      SideDotSettingButtonChangedEvent?.Invoke();
      switch (CurrentSideDotSettingButton.direction)
      {
         case Directions.bottom:
            CurrentBar = 0;
            barText.text = LocalisationSystem.GetLocalisedString("top_bar");
            break;
         case Directions.top:
            CurrentBar = 1;
            barText.text = LocalisationSystem.GetLocalisedString("bottom_bar");
            break;
         case Directions.right:
            CurrentBar = 2;
            barText.text = LocalisationSystem.GetLocalisedString("left_bar");
            break;
         case Directions.left:
            CurrentBar = 3;
            barText.text = LocalisationSystem.GetLocalisedString("right_bar");
            break;
      }

      SwitchBar(true);
      //Set switch Value to right value
   }
   
   public void ClickSwitchBar(bool on)
   {
      SwitchEnabledBar(on, false, true);
      Rl.BoardPreview.StartDrawBoard();
      LoadInformationEvent?.Invoke();
   }

   private void ResizeTileConfig(ref SideFruitsConfig[] sideFruitsConfig)
   {
      for (int i = 0; i < sideFruitsConfig.Length; i++)
      {

         if (sideFruitsConfig[i].SideFruitsSettings == null || sideFruitsConfig[i].SideFruitsSettings.Count != 36)
         {
            SideFruitsSetting[] helperArray = sideFruitsConfig[i].SideFruitsSettings?.ToArray();
            Array.Resize(ref helperArray, 36);
            sideFruitsConfig[i].SideFruitsSettings = helperArray.ToList();
         }
      
      }
   }
   public void LoadSideDotSettings(SideFruitsFieldConfig  sideFruitsFieldConfig)
   {
      SideFruitsFieldConfig n = (SideFruitsFieldConfig)GenericSettingsFunctions.GetDeepCopy(sideFruitsFieldConfig);
      if(n.SideFruitsConfig == null || n.SideFruitsConfig.Length != SaveFileLevelConfigs.Fields)
         Array.Resize(ref n.SideFruitsConfig, SaveFileLevelConfigs.Fields);

      ResizeTileConfig(ref n.SideFruitsConfig);
      LoadBoardActive(n);
      Rl.saveClipBoard.SideFruitsFieldConfigs = n;
      SwitchBar(true);
      CurrenSideDotSettingButtonChanged();
   }
   public SideDotTile ClipBoardToTiles(byte column)
   {
      return Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField]
         .SideFruitsSettings[GetID(column)].SideDotTile;
   }

   private void SwitchBar(bool playNoSound)
   {
      switch (CurrentSideDotSettingButton.direction)
      {
         case Directions.top:
            SwitchEnabledBar(Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].BottomActive, playNoSound, false);
            break;
         case Directions.bottom:
            SwitchEnabledBar(Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].TopActive, playNoSound, false);
            break;
         case Directions.right:
            SwitchEnabledBar(Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].LeftActive, playNoSound, false);
            break;
         case Directions.left:
            SwitchEnabledBar(Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].RightActive , playNoSound, false);
            break;
      }
   }

   private void LoadBoardActive(SideFruitsFieldConfig  SideFruitsFieldConfig)
   {
      Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].LeftActive = SideFruitsFieldConfig.SideFruitsConfig[FieldState.CurrentField].LeftActive;
      Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].RightActive = SideFruitsFieldConfig.SideFruitsConfig[FieldState.CurrentField].RightActive;
      Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].TopActive = SideFruitsFieldConfig.SideFruitsConfig[FieldState.CurrentField].TopActive;
      Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].BottomActive = SideFruitsFieldConfig.SideFruitsConfig[FieldState.CurrentField].BottomActive;
   }

   public SideFruitsFieldConfig  SaveSideDotSettings() => (SideFruitsFieldConfig)GenericSettingsFunctions.GetDeepCopy(Rl.saveClipBoard.SideFruitsFieldConfigs);

   private void SwitchEnabledBar(bool on, bool playNoSound, bool animation)
   {
      switch (CurrentSideDotSettingButton.direction)
      {
         case Directions.top:
            Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].BottomActive = on;
            break;
         case Directions.bottom:
            Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].TopActive = on;
            break;
         case Directions.right:
            Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].LeftActive = on;
            break;
         case Directions.left:
            Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].RightActive = on;
            break;
      }
      enableBarButton.SwitchButton(on, playNoSound, animation);
   }

   private int GetID(byte id)
   {
      return id +CurrentBar*9;
   }
   public SideDotTile GetSideDotTile(byte id, Load load = Load.Only)
   {
      int findID = GetID(id);
      var sideDotTile = Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].SideFruitsSettings[findID].SideDotTile;
      switch (load)
      {
         case Load.Prev:
            sideDotTile = PrevSideDotTile((int)sideDotTile);
            break;
         case Load.Next:
            sideDotTile = NextSideDotTile((int)sideDotTile);
            break;
      }

      Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].SideFruitsSettings[findID] = new SideFruitsSetting
      (
         sideDotTile,
         Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].SideFruitsSettings[findID].IsActivate,
         Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].SideFruitsSettings[findID].ActivatesAfterTimeOrMoves,
         Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[FieldState.CurrentField].SideFruitsSettings[findID] .SideDotTypeSettings
      );
      return sideDotTile;
   }
   private SideDotTile NextSideDotTile(int sideDotTile)
   {
      sideDotTile += 1;
      SideDotTile nextSideDotTile = SideDotTile.Normal;
      if (sideDotTile > EnumCountSideDotTile() - 1) sideDotTile = 0;

      int counter = 0;
      foreach (SideDotTile sideDotTileEnum in Enum.GetValues(typeof(SideDotTile)))
      {
         if (sideDotTile == counter) nextSideDotTile = sideDotTileEnum;
         counter++;
      }
      return nextSideDotTile;
   }
   private SideDotTile PrevSideDotTile(int sideDotTile)
   {
      sideDotTile -= 1;
      SideDotTile prevSideDotTile = SideDotTile.Normal;
      if (sideDotTile < 0) sideDotTile = EnumCountSideDotTile() - 1;

      int counter = 0;
      foreach (SideDotTile SideDotTileEnum in Enum.GetValues(typeof(SideDotTile)))
      {
         if (sideDotTile == counter) prevSideDotTile = SideDotTileEnum;
         counter++;
      }
      return prevSideDotTile;
   }
   private static int EnumCountSideDotTile() => GenericSettingsFunctions.EnumCount(typeof(SideDotTile));
   
   public void ClickIcon()
   {
      ClickSection();
      Invoke(nameof(ClickSection), 0.01f);
   }

   private void ClickSection()
   {
      LoadFromDiskEvent?.Invoke();
      Rl.BoardPreview.ResetLastClicked();
      Rl.BoardPreview.StartDrawBoard();
   }

   public void CopyField(byte fieldOne, byte fieldTwo)
   {
      Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[fieldTwo] =
         (SideFruitsConfig)GenericSettingsFunctions.GetDeepCopy(Rl.saveClipBoard.SideFruitsFieldConfigs.SideFruitsConfig[fieldOne]);
      LoadCurrentField();
   }

   public void LoadCurrentField()
   {
      ResetSetting?.Invoke();
      LoadInformationEvent?.Invoke();
      SwitchBar(true);
      SideDotSettingButtonChangedEvent?.Invoke();
      SideDotLoadTileSettingsEvent?.Invoke();
      Rl.BoardPreview.ResetLastClicked();
      Rl.BoardPreview.StartDrawBoard();
   }


}
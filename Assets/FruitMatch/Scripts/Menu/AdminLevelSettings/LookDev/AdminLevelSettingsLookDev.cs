using System;
using System.Collections.Generic;
using FruitMatch.Scripts.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public sealed class AdminLevelSettingsLookDev : MonoBehaviour
{
  [SerializeField] private Image CurrentlevelImage;
  [SerializeField] private TextMeshProUGUI enumLevelCategory;
  [SerializeField] private TextMeshProUGUI BGImageName;
  [SerializeField] private TextMeshProUGUI CurrentLevelCounter;
  [SerializeField] private TextMeshProUGUI MaxLevelCounter;
  [SerializeField] private Switch BorderSwitch;
  private int _currentImagePointer = 0;
  private BgImageSet _currentBgImageSet;
  [SerializeField] private GameObject BackgroundImage;
  private void Start() => InitializeDictionary();
  public GraphicConfig SaveBorderGraphicSetting()
  {
    return new GraphicConfig(
      Rl.saveClipBoard.BorderGraphic,
      Rl.saveClipBoard.LevelCategory,
      Rl.saveClipBoard.BGName,
      Rl.saveClipBoard.WholeCategory
    );
  }
  public void ClickOnBorderSwitch(bool on) => ClickBorderSwitch(on, false, true);
  private void ClickBorderSwitch(bool on, bool playNoSound, bool animation)
  {
    BorderSwitch.SwitchButton(on, playNoSound, animation);
    Rl.saveClipBoard.BorderGraphic = on;
  }
  public void ClickGetNextLevel()
  {
    CurrentlevelImage.sprite = GetNextImageFromBgImage(_currentBgImageSet, ref _currentImagePointer);
    GenericSettingsFunctions.SmallJumpAnimation(CurrentlevelImage.transform, BGImageName.transform,  CurrentLevelCounter.transform);
  }

  public void ClickGetPrevLevel()
  {
    CurrentlevelImage.sprite = GetPrevImageFromBgImage(_currentBgImageSet, ref _currentImagePointer);
    GenericSettingsFunctions.SmallJumpAnimation(CurrentlevelImage.transform, BGImageName.transform,  CurrentLevelCounter.transform);
  }
  
  public void ClickNextLevelCategory()
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.NextRowSound, Random.Range(2,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        LoadLevelCategory(Load.Next);
    }
  public void ClickPrevLevelCategory()
  {
    Rl.GameManager.PlayAudio(Rl.soundStrings.NextRowSound, Random.Range(0,2), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    LoadLevelCategory(Load.Prev);
  }

 
  public void LoadFromDisk(GraphicConfig graphicConfig)
  {
    Rl.saveClipBoard.BorderGraphic = graphicConfig.AllowBorderGraphic;
    Rl.saveClipBoard.LevelCategory = graphicConfig.LevelCategory;
    Rl.saveClipBoard.WholeCategory = graphicConfig.WholeCategory;
    Rl.saveClipBoard.BGName = graphicConfig.BGName;
    LoadLevelCategory(Load.Only, graphicConfig.BGName);
    ClickBorderSwitch(Rl.saveClipBoard.BorderGraphic, true, false);
  }

  public void SetBackgroundImage()
  {
    GraphicConfig graphicConfig = Rl.saveFileLevelConfigManagement.AllSaveFileLevelConfigs
      .LevelConfigs[LevelManager.THIS.currentLevel-1].GraphicConfig;

    for (int i = 0; i < Rl.world.allBgImageSetDictionaries[graphicConfig.LevelCategory].BgImageList.Count; i++)
      if (Rl.world.allBgImageSetDictionaries[graphicConfig.LevelCategory].BgImageList[i].BGName == graphicConfig.BGName)
        BackgroundImage.GetComponent<Image>().sprite = Rl.world.allBgImageSetDictionaries[graphicConfig.LevelCategory].BgImageList[i].BGImage;
  }
  public void LoadLevelCategory(Load load, string BGImageName = null)
    {
      switch (load)
      {
        case Load.Next:
          Rl.saveClipBoard.LevelCategory = GetNextLevelCategory((int)Rl.saveClipBoard.LevelCategory);
          break;
        case Load.Prev:
          Rl.saveClipBoard.LevelCategory = GetPrevLevelCategory((int)Rl.saveClipBoard.LevelCategory);
          break;
        case Load.Only:
          break;
      }

      SetCurrentBgImageSet(Rl.saveClipBoard.LevelCategory, BGImageName);
    }

  private int GetEntry(LevelCategory levelCategory, string BGImagName)
  {
    for (int i = 0; i < Rl.world.allBgImageSetDictionaries[levelCategory].BgImageList.Count; i++)
      if (Rl.world.allBgImageSetDictionaries[levelCategory].BgImageList[i].BGName == BGImagName)
        return i;
    return 0;
  }
  private void SetCurrentBgImageSet(LevelCategory levelCategory, string BGImagName = null)
  {
    BgImageSet imageSet = GetListFromDictionaryEntry(levelCategory);
    if (imageSet.BgImageList.Count > 0) _currentBgImageSet = imageSet;
    enumLevelCategory.text = levelCategory.ToString();
    MaxLevelCounter.text = imageSet.BgImageList.Count.ToString();

    int entry = 0;
    if (BGImagName != null) entry = GetEntry(levelCategory, BGImagName);
    
      _currentImagePointer = entry ;
      CurrentlevelImage.sprite = imageSet.BgImageList[entry].BGImage;
      BGImageName.text = imageSet.BgImageList[entry].BGName;
      CurrentLevelCounter.text = (entry +1).ToString();
 
   
    GenericSettingsFunctions.SmallJumpAnimation(CurrentlevelImage.transform, BGImageName.transform,  CurrentLevelCounter.transform, MaxLevelCounter.transform, enumLevelCategory.transform);
  }
  private Sprite GetCurrentImageFromBgImage(BgImageSet currentBgImageSet, ref int currentCounter)
  {
    if (currentCounter > currentBgImageSet.BgImageList.Count-1) currentCounter = 0;
    else if (currentCounter < 0) currentCounter = currentBgImageSet.BgImageList.Count-1;

    Rl.saveClipBoard.BGName = currentBgImageSet.BgImageList[currentCounter].BGName;
    BGImageName.text = Rl.saveClipBoard.BGName;
    int displayCounter = currentCounter;
    displayCounter++;
    CurrentLevelCounter.text = displayCounter.ToString();
    return currentBgImageSet.BgImageList[currentCounter].BGImage;
  }
  private Sprite GetNextImageFromBgImage(BgImageSet currentBgImageSet, ref int currentCounter)
  {
    currentCounter++;
    return GetCurrentImageFromBgImage(currentBgImageSet, ref currentCounter);
  }
  private Sprite GetPrevImageFromBgImage(BgImageSet currentBgImageSet, ref int currentCounter)
  {
    currentCounter--;
    return GetCurrentImageFromBgImage(currentBgImageSet, ref currentCounter);
  }
  
  private void InitializeDictionary()
  {
    Rl.world.allBgImageSetDictionaries = new Dictionary<LevelCategory, BgImageSet>();
    
    for (int i = 0; i < EnumCountLevelCategories(); i++)
    {
      LevelCategory levelCategory = GetLevelCategory(i);
      Rl.world.allBgImageSetDictionaries.Add(levelCategory, GetCategoryList(levelCategory));
    }
    SetCurrentBgImageSet(Rl.saveClipBoard.LevelCategory);
  }

  private BgImageSet GetListFromDictionaryEntry(LevelCategory levelCategory)
  {
    Rl.world.allBgImageSetDictionaries.TryGetValue(levelCategory, out var bgImageSet);
    return bgImageSet;
  }
    
  private BgImageSet GetNextListFromDictionaryEntry(LevelCategory currentLevelCategory)
  {
    currentLevelCategory = (int)currentLevelCategory + 1 > EnumCountLevelCategories() - 1 ?
      GetLevelCategory(0) : GetLevelCategory((int)currentLevelCategory +1 );
    return GetListFromDictionaryEntry(currentLevelCategory);
  }

  private BgImageSet GetPrevListFromDictionaryEntry(LevelCategory currentLevelCategory)
  {
    currentLevelCategory = (int)currentLevelCategory -1 < 0 ?
      GetLevelCategory(EnumCountLevelCategories() - 1) : GetLevelCategory((int)currentLevelCategory -1 );
    return GetListFromDictionaryEntry(currentLevelCategory);
  }

  private static int EnumCountLevelCategories() => GenericSettingsFunctions.EnumCount(typeof(LevelCategory));
  
  private LevelCategory GetNextLevelCategory(int levelCategory) 
  {
    levelCategory++;
    if (levelCategory > EnumCountLevelCategories() - 1) levelCategory = 0;
    return GetLevelCategory(levelCategory);
  }
  private LevelCategory GetPrevLevelCategory(int levelCategory)
  {
    levelCategory--;
    if (levelCategory < 0) levelCategory = EnumCountLevelCategories() - 1;
    return GetLevelCategory(levelCategory);
  }
  private static LevelCategory GetLevelCategory(int levelCategory)
  {
    LevelCategory levelCategoryEnum = LevelCategory.Misc;
        
    int counter = 0;
    foreach (LevelCategory type in Enum.GetValues(typeof(LevelCategory)))
    {
      if (levelCategory == counter) levelCategoryEnum = type;
      counter++;
    }

    return levelCategoryEnum;
  }
  private BgImageSet GetCategoryList(LevelCategory levelCategory)
  {
    BgImageSet imageSetToReturn = new BgImageSet();
    for (int i = 0; i < Rl.world.allBGImageSets.Count; i++)
    {
      if (levelCategory == Rl.world.allBGImageSets[i].LevelCategory)
      {
        imageSetToReturn = Rl.world.allBGImageSets[i];
        return imageSetToReturn;
      }
    }
    return imageSetToReturn;
  }
}

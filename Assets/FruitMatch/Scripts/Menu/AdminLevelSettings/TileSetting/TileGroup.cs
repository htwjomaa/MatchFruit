using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TileGroup : MonoBehaviour
{
  [SerializeField] private Button prevButton;
  [SerializeField] private Button nextButton;
  [SerializeField] private Image image;
  [SerializeField] private Image bgImage;
  [SerializeField] private Image bgFieldImage;
  [SerializeField] private TextMeshProUGUI numberField;
  [SerializeField] private Button settingsButton;
  [SerializeField] private TextMeshProUGUI label;
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
  private string _buttonsDeactiveColor = "#B1BAFF";
  private string _buttonsDeactiveColorSetting = "F300BB";
  private float _buttonsDeactiveAlpha = 0.125f;
  private float _cashedPrevNextAlpha = 0.85f;
  private float _cashedImageAlpha = 1f;
  private float _cashedSettingsButtonAlpha = 0.9f;
  private float _cashedNumberFieldAlpha = 0.9f;
  private float _cashedLabelAlpha = 1f;
  private float _cashedBgImageAlpha = 1f;
  private float _cashedBgImageFieldAlpha = 1f;
  
  private Color _cashedImageColor;
  private Color _cashedPrevNextColor;
  private Color _cashedSettingsButtonColor;
  private Color _cashedNumberFieldColor;
  private Color _cashedLabelColor;
  private Color _cashedBgImageColor;
  private Color _cashedBgFieldImageColor;
  
  public TT tileType = TT.Normal;
  public Directions direction = Directions.bottom;
  public sbyte TeleportTarget = -1;
  [HideInInspector] public byte _cashedNumberField;
  private void Awake() => CashEverything();
  private void CashEverything()
  {
    //cash fieldNumber
    int result;
    int.TryParse(numberField.text, out result);
    _cashedNumberField = (byte)(result - 1);
    
    //cash Alphas
    _cashedPrevNextAlpha = prevButton.image.color.a;
    _cashedImageAlpha = image.color.a;
    _cashedSettingsButtonAlpha = settingsButton.image.color.a;
    _cashedNumberFieldAlpha = numberField.color.a;
    _cashedLabelAlpha = label.color.a;
    _cashedBgImageAlpha = bgImage.color.a;
    _cashedBgImageFieldAlpha = bgFieldImage.color.a;
    //cash Colors
    _cashedImageColor = image.color;
    _cashedPrevNextColor = prevButton.image.color;
    _cashedSettingsButtonColor = settingsButton.image.color;
    _cashedNumberFieldColor = numberField.color;
    _cashedLabelColor = label.color;
    _cashedBgImageColor = bgImage.color;
    _cashedBgFieldImageColor = bgFieldImage.color;
  }
  private void Start()
  {
    AdminLevelSettingsTiles.LoadCurrentSetting += ResetTabs;
    AdminLevelSettingsTiles.ResetSetting += GrayOut;
    AdminLevelSettingsTiles.LoadFieldsEvent+= LoadField;
    AdminLevelSettingsTiles.LoadFieldsEvent+= GrayOut;
    AdminLevelSettingsTiles.LoadCurrentRow += GrayOut;
    AdminLevelSettingsTiles.LoadCurrentRow += LoadInformation;
    AdminLevelSettingsTiles.LoadFromDiskEvent += LoadInformation;
    AdminLevelSettingsTiles.LoadInformationEvent += LoadInformation;
    AdminLevelSettingsTiles.ShowItem += ShowItem;
    AdminLevelSettingsTiles.TeleportFromEvent +=  LoadTeleportFrom;
  }
  private void OnDestroy()
  {
    AdminLevelSettingsTiles.LoadCurrentSetting -= ResetTabs;
    AdminLevelSettingsTiles.ResetSetting -= GrayOut;
    AdminLevelSettingsTiles.LoadFieldsEvent -= LoadField;
    AdminLevelSettingsTiles.LoadFieldsEvent -= GrayOut;
    AdminLevelSettingsTiles.LoadCurrentRow -= GrayOut;
    AdminLevelSettingsTiles.LoadCurrentRow -= LoadInformation;
    AdminLevelSettingsTiles.LoadFromDiskEvent -= LoadInformation;

    AdminLevelSettingsTiles.LoadInformationEvent -= LoadInformation;
    AdminLevelSettingsTiles.ShowItem -= ShowItem;
    AdminLevelSettingsTiles.TeleportFromEvent -= LoadTeleportFrom;
    
  }

  private void LoadTeleportFrom()
  {
    if (Rl.adminLevelSettingsTiles.CurrentTileKind != TKind.Teleports) return;
    if (Rl.adminLevelSettingsTiles.validTargets.ContainsKey(_cashedNumberField))
    {
      if (Rl.adminLevelSettingsTiles.IsAlsoTeleportFrom(_cashedNumberField))
      {
        image.sprite = Rl.adminLevelSettingsTiles.TeleportSprites[3];
        label.text = "MultiTarget";
      }
      else
      {
        image.sprite = Rl.adminLevelSettingsTiles.TeleportSprites[2];
      
        int number = Rl.adminLevelSettingsTiles.validTargets[_cashedNumberField];
        int row = Mathf.FloorToInt((float)number / 9) + 1;
        int column = number / row;
      
        label.text = "From - R: " + row + " | C: " + column;
      }
    }
  }
  private void LoadInformation()
  {
    Rl.adminLevelSettingsTiles.ClipBoardToTiles(_cashedNumberField, ref tileType, ref direction, ref TeleportTarget);
    Rl.adminLevelSettingsTiles.ClickHandler(_cashedNumberField, ref tileType, ref direction, ref TeleportTarget, label, image);
  }
  private void LoadField()
  {
    Rl.adminLevelSettingsTiles.ClickHandler(_cashedNumberField, ref tileType, ref direction, ref TeleportTarget, label, image);
  }
  private void ResetTabs()
  {
    if (_cashedNumberField == Rl.adminLevelSettingsTiles.CurrentSetting) ActivateAll();
    else DeactivateAll(_buttonsDeactiveColorSetting);
  }
  public void ClickNextTile()
  {
    Rl.adminLevelSettingsTiles.ClickHandler(_cashedNumberField, ref tileType, ref direction, ref TeleportTarget, label, image, Load.Next);
   // Rl.BoardPreview.keepFocus = true;
    FeedbackHandler(nextButton);
  }
  
  public void ShowItem()
  {
    if (Rl.adminLevelSettingsTiles.CurrentColumn != _cashedNumberField) return;
    if(PreventDoubleClick) GenericSettingsFunctions.SmallShake(bgImage.transform);
  }
  public void ClickPicture()
  {
    AdminLevelSettingsTiles.ResetSettingsInvoke();
    if(PreventDoubleClick) GenericSettingsFunctions.SmallShake(bgImage.transform);
    Rl.BoardPreview.DrawOutlineAroundOneTile(Rl.adminLevelSettingsTiles.CurrentRow, _cashedNumberField);
  }
  private void GrayOut() => GrayOutNotAvailable(
    (int)Rl.adminLevelSettingsBoard.BoardHeightSlider.value - 1, (int)Rl.adminLevelSettingsBoard.BoardWidthSlider.value - 1);
  private void GrayOutNotAvailable(int maxRow, int maxColumn)
  {
    if (Rl.adminLevelSettingsTiles.CurrentRow > maxRow || _cashedNumberField > maxColumn)
      DeactivateAll(_buttonsDeactiveColor);
    else ActivateAll();
  }
  public void ClickPrevTile()
  {
    Rl.adminLevelSettingsTiles.ClickHandler(_cashedNumberField, ref tileType, ref direction, ref TeleportTarget, label, image, Load.Prev);
   //Rl.BoardPreview.keepFocus = true;
    FeedbackHandler(prevButton);
  }
  private void FeedbackHandler(Button button)
  {
    GenericSettingsFunctions.SmallJumpAnimation(button.transform);
    Rl.GameManager.PlayAudio(Rl.soundStrings.Click, Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    Rl.BoardPreview.StartDrawBoard();
  }
  public void ClickSetting()
  {
    if (!PreventDoubleClick) return;
    GenericSettingsFunctions.SmallJumpAnimation(transform);
    GenericSettingsFunctions.SmallShake(settingsButton.transform);
    Rl.adminLevelSettingsTiles.CurrentSetting = _cashedNumberField;
  }
  private void ActivateAll()
  {
    settingsButton.image.color = GenericSettingsFunctions.SetColorAndAlpha(_cashedSettingsButtonColor, _cashedSettingsButtonAlpha);
    image.color = GenericSettingsFunctions.SetColorAndAlpha(_cashedImageColor, _cashedImageAlpha);
    prevButton.image.color = GenericSettingsFunctions.SetColorAndAlpha(_cashedPrevNextColor, _cashedPrevNextAlpha);
    nextButton.image.color = GenericSettingsFunctions.SetColorAndAlpha(_cashedPrevNextColor, _cashedPrevNextAlpha);
    numberField.color = GenericSettingsFunctions.SetColorAndAlpha(_cashedNumberFieldColor, _cashedNumberFieldAlpha);
    label.color = GenericSettingsFunctions.SetColorAndAlpha(_cashedLabelColor, _cashedLabelAlpha);
    bgImage.color = GenericSettingsFunctions.SetColorAndAlpha(_cashedBgImageColor, _cashedBgImageAlpha);
    bgFieldImage.color = GenericSettingsFunctions.SetColorAndAlpha(_cashedBgFieldImageColor, _cashedBgImageFieldAlpha);
    
  }
  private void DeactivateAll(string hexCode)
  {
    Color color;
    ColorUtility.TryParseHtmlString(hexCode, out color);
    
    settingsButton.image.color = GenericSettingsFunctions.SetColorAndAlpha(color, _buttonsDeactiveAlpha);
    image.color = GenericSettingsFunctions.SetColorAndAlpha(color, _buttonsDeactiveAlpha);
    prevButton.image.color  = GenericSettingsFunctions.SetColorAndAlpha(color, _buttonsDeactiveAlpha);
    nextButton.image.color  = GenericSettingsFunctions.SetColorAndAlpha(color, _buttonsDeactiveAlpha);
    numberField.color = GenericSettingsFunctions.SetColorAndAlpha(color, _buttonsDeactiveAlpha);
    label.color = GenericSettingsFunctions.SetColorAndAlpha(color, _buttonsDeactiveAlpha);
    bgImage.color = GenericSettingsFunctions.SetColorAndAlpha(color, _buttonsDeactiveAlpha);
    bgFieldImage.color = GenericSettingsFunctions.SetColorAndAlpha(color, _buttonsDeactiveAlpha);
  }
}
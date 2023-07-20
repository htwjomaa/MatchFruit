using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class TileGroupSideDots : MonoBehaviour
{
  [SerializeField] private Button prevButton;
  [SerializeField] private Button nextButton;
  [SerializeField] private Image image;
  [SerializeField] private Image bgImage;
  [SerializeField] private Image bgFieldImage;
  [SerializeField] private TextMeshProUGUI numberField;
  [SerializeField] private Button settingsButton;
  [SerializeField] private TextMeshProUGUI label;
  [SerializeField] private SideDotTile sideDotTile = SideDotTile.Normal;

  private string _buttonsDeactiveColor = "#B1BAFF";
  private float _buttonsDeactiveAlpha = 0.125f;
  private float _cashedPrevNextAlpha = 0.85f;
  private float _cashedImageAlpha = 1f;
  private float _cashedSettingsButtonAlpha = 0.9f;
  private float _cashedNumberFieldAlpha = 0.9f;
  private float _cashedLabelAlpha = 1f;
  private float _cashedBgImageAlpha = 1f;
  private float _cashedBgImageFieldAlpha = 1f;
  private Vector3 _cashedLocalScale;
  [HideInInspector] public byte _cashedNumberField;
  private Color _cashedImageColor;
  private Color _cashedPrevNextColor;
  private Color _cashedSettingsButtonColor;
  private Color _cashedNumberFieldColor;
  private Color _cashedLabelColor;
  private Color _cashedBgImageColor;
  private Color _cashedBgFieldImageColor;
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
    _cashedLocalScale = image.transform.localScale;
  }

  private void Start()
  {
    AdminLevelSettingsSideDots.SideDotLoadTileSettingsEvent += ResetTabs;
    AdminLevelSettingsSideDots.ResetSetting += ActivateAll;
    AdminLevelSettingsSideDots.LoadFromDiskEvent += LoadInformation;
    AdminLevelSettingsSideDots.SideDotLoadTileSettingsEvent += GrayOut;
    AdminLevelSettingsSideDots.LoadFromDiskEvent  += GrayOut;
    
    AdminLevelSettingsSideDots.LoadInformationEvent += LoadInformation;
    AdminLevelSettingsSideDots.LoadInformationEvent += GrayOut;
  }

  private void OnDestroy()
  {
    AdminLevelSettingsSideDots.SideDotLoadTileSettingsEvent -= ResetTabs;
    AdminLevelSettingsSideDots.ResetSetting -= ActivateAll;
    AdminLevelSettingsSideDots.LoadFromDiskEvent -= LoadInformation;
    AdminLevelSettingsSideDots.SideDotLoadTileSettingsEvent -= GrayOut;
    AdminLevelSettingsSideDots.LoadFromDiskEvent  -= GrayOut;


    AdminLevelSettingsSideDots.LoadInformationEvent -= LoadInformation;
    AdminLevelSettingsSideDots.LoadInformationEvent -= GrayOut;
  }

  private void ResetTabs()
  {
    if (_cashedNumberField == Rl.adminLevelSettingsSideDots.currentNumberField) ActivateAll();
    else DeactivateAll(_buttonsDeactiveColor);
  }
  private void LoadInformation()
  {
    sideDotTile = Rl.adminLevelSettingsSideDots.ClipBoardToTiles(_cashedNumberField);
    Click(Rl.adminLevelSettingsSideDots.GetSideDotTile(_cashedNumberField, Load.Only));
  }
  public void ClickNextTile()
  {
    if(!Rl.adminLevelSettingsSideDots.enableBarButton.GetEnabledStatus()) return;
    Click(Rl.adminLevelSettingsSideDots.GetSideDotTile(_cashedNumberField, Load.Next));
    FeedbackHandler(nextButton);
  }
  public void ClickPicture()
  {
    if(!Rl.adminLevelSettingsSideDots.enableBarButton.GetEnabledStatus()) return;
    //AdminLevelSettingsTiles.ResetSettingsInvoke();
    if(PreventDoubleClick) GenericSettingsFunctions.SmallShake(bgImage.transform);
    Rl.BoardPreview.DrawOutlineAroundOneTile(Rl.adminLevelSettingsSideDots.CurrentBar, _cashedNumberField, true);
  }
  public void ClickPrevTile()
  {
    if(!Rl.adminLevelSettingsSideDots.enableBarButton.GetEnabledStatus()) return;
    Click(Rl.adminLevelSettingsSideDots.GetSideDotTile(_cashedNumberField, Load.Prev));
    FeedbackHandler(prevButton);
  }

  public void ShowItem()
  {
    if (Rl.adminLevelSettingsSideDots.Currentcolumn != _cashedNumberField) return;
    if(PreventDoubleClick) GenericSettingsFunctions.SmallShake(bgImage.transform);
  }
  private void Click(SideDotTile tile)
  {
    label.text = LocalisationSystem.GetLocalisedString(tile.ToString());
  
      image.sprite = Rl.adminLevelSettingsSideDots.SideDotSpriteList[(int)tile];
      SetImageRoation(ref image, tile );
  }

  private void SetImageRoation(ref Image image, SideDotTile tile)
  {
    Directions direction = Rl.adminLevelSettingsSideDots.CurrentSideDotSettingButton.direction;
  
    image.transform.localScale = _cashedLocalScale;
    if (direction is Directions.left)
    {
      image.transform.localRotation = Quaternion.identity;
      return;
    }
    if (tile is SideDotTile.Normal or SideDotTile.EmptyTile) return;
    image.transform.localScale =
      BoardPreviewSideDot.ScaleSideDotChild(direction,
        image.transform);
    image.transform.rotation =
      BoardPreviewSideDot.RotateSideDotChild(direction,
        image.transform);
  }
  private void FeedbackHandler(Button button)
  {
    GenericSettingsFunctions.SmallJumpAnimation(button.transform);
    Rl.GameManager.PlayAudio(Rl.soundStrings.Click, Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    Rl.BoardPreview.StartDrawBoard();
  }
public void ClickSetting()
  {
    if(!Rl.adminLevelSettingsSideDots.enableBarButton.GetEnabledStatus()) return;
    GenericSettingsFunctions.SmallJumpAnimation(transform);
    GenericSettingsFunctions.SmallShake(settingsButton.transform);
    Rl.adminLevelSettingsSideDots.currentSetting = _cashedNumberField;
  }

private void GrayOut() => GrayOutNotAvailable(
  (int)Rl.adminLevelSettingsBoard.BoardHeightSlider.value - 1, (int)Rl.adminLevelSettingsBoard.BoardWidthSlider.value - 1);
private void GrayOutNotAvailable(int maxRow, int maxColumn)
{
  if (!Rl.adminLevelSettingsSideDots.enableBarButton.GetEnabledStatus())
  {
    DeactivateAll(_buttonsDeactiveColor);
    return;
  }
  
  switch (Rl.adminLevelSettingsSideDots.CurrentSideDotSettingButton.direction)
    {
      case Directions.left or Directions.right:
        if (_cashedNumberField > maxRow) DeactivateAll(_buttonsDeactiveColor);
        else ActivateAll();
        break;
      case Directions.bottom or Directions.top:
        if (_cashedNumberField > maxColumn) DeactivateAll(_buttonsDeactiveColor);
        else ActivateAll();
        break;
    }

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
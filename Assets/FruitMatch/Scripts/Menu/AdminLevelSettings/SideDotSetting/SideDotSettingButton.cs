using UnityEngine;
using UnityEngine.UI;
public class SideDotSettingButton : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image bgImage;
    private string _buttonsDeactiveColor = "#B1BAFF";
    private float _buttonsDeactiveAlpha = 0.125f;
    private float _cashedImageAlpha = 1f;
    private Color _cashedImageColor;
    
    private float _cashedBgImageAlpha = 1f;
    private Color _cashedBgImageColor;
    [SerializeField] public Directions direction;
    private void OnDestroy() => AdminLevelSettingsSideDots.SideDotSettingButtonChangedEvent -= ResetTabs;
    private void Awake()
    {   AdminLevelSettingsSideDots.SideDotSettingButtonChangedEvent += ResetTabs;
        _cashedImageAlpha = image.color.a;
        _cashedImageColor = image.color;
        _cashedBgImageAlpha = bgImage.color.a;
        _cashedBgImageColor = bgImage.color;
    }
    public void ClickButton()
    {
        GenericSettingsFunctions.SmallJumpAnimation(transform);
        Rl.adminLevelSettingsSideDots.CurrentSideDotSettingButton = this;
        Rl.BoardPreview.StartDrawBoard();
        AdminLevelSettingsSideDots.InvokeLoadFromDisk();
    }
    
    private void ResetTabs()
    {
        if (this == Rl.adminLevelSettingsSideDots.CurrentSideDotSettingButton) ActivateAll();
        else DeactivateAll();
    }
    private void ActivateAll()
    {
        image.color = GenericSettingsFunctions.SetColorAndAlpha(_cashedImageColor, _cashedImageAlpha);
        bgImage.color = GenericSettingsFunctions.SetColorAndAlpha(_cashedBgImageColor, _cashedBgImageAlpha);
    }
    private void DeactivateAll()
    {
        Color color;
        ColorUtility.TryParseHtmlString(_buttonsDeactiveColor, out color);
        image.color = GenericSettingsFunctions.SetColorAndAlpha(color, _buttonsDeactiveAlpha);
        bgImage.color = GenericSettingsFunctions.SetColorAndAlpha(color, _buttonsDeactiveAlpha);
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdminLevelSettingsTilesRow : MonoBehaviour
{
    [SerializeField] public byte row;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image backGroundButtonImage;
    [SerializeField] private Image buttonImage;
    private string _buttonsDeactiveColor = "#B1BAFF";
  private float _buttonsDeactiveAlpha = 0.3f;
    
    public void ClickOnRow()
    {
        GenericSettingsFunctions.SmallJumpAnimation(transform);
        if (Rl.adminLevelSettingsTiles.CurrentRow == row) return;
        Rl.adminLevelSettingsTiles.CurrentRow = row;
    }

    private void Awake()
    {
        int number = 1;
        int.TryParse(text.text, out number);
        row = (byte)(number - 1);
    }
    
    private void Start()
    {
        AdminLevelSettingsTiles.LoadCurrentRow += ResetTabs;
        Invoke(nameof(ResetTabs), 0.3f);
    }

    private void OnDestroy() => AdminLevelSettingsTiles.LoadCurrentRow -= ResetTabs;

    private void ResetTabs()
    {
        Color color;
        ColorUtility.TryParseHtmlString(_buttonsDeactiveColor, out color);
        if (row == Rl.adminLevelSettingsTiles.CurrentRow)
        {

            buttonImage.color = GenericSettingsFunctions.ResetColorAlmostFull();
            backGroundButtonImage.color = GenericSettingsFunctions.ResetColorAlmostFull();
            text.color = GenericSettingsFunctions.ResetColorAlmostFull();

        }
        else
        {
            buttonImage.color = GenericSettingsFunctions.SetColorAndAlpha(color, _buttonsDeactiveAlpha);
            backGroundButtonImage.color = GenericSettingsFunctions.SetColorAndAlpha(color, _buttonsDeactiveAlpha);
            text.color = GenericSettingsFunctions.SetColorAndAlpha(color, _buttonsDeactiveAlpha);
        }
    }
}

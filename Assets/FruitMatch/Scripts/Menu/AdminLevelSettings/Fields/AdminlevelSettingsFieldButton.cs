using UnityEngine;
using UnityEngine.UI;

public sealed class AdminlevelSettingsFieldButton : MonoBehaviour
{
 [SerializeField] private byte fieldNumber;
 [SerializeField] private Image text;
 [SerializeField] private Image backgroundImage ;
 private string _buttonsDeactiveColor = "#B1BAFF";
 private float _buttonsDeactiveAlpha = 0.2f;
 
 private float _cashedTextAlpha = 0.9f;
 private float _cashedBackgroundImageAlpha = 0.9f;

 private Color _cashedTextColor;
 private Color _cashedBackgroundImageColor;
 
 public void ClickOnField()
 {
  GenericSettingsFunctions.SmallJumpAnimation(transform);
  if (FieldState.CurrentField  == fieldNumber) return;
  FieldState.CurrentField = fieldNumber;
  Debug.Log("CurrentField :" + (fieldNumber+1).ToString());
  FieldState.InvokeLoadFieldsEvent();
 }

 private void Awake()
 {
  _cashedTextAlpha = text.color.a;
  _cashedTextColor = text.color;
  _cashedTextColor = text.color;
  _cashedBackgroundImageColor = backgroundImage.color;
 }

 private void Start()
 {
  FieldState.LoadFieldsEvent += ResetTabs;
  Invoke(nameof(ResetTabs), 0.3f);
 }

 private void OnDestroy() => FieldState.LoadFieldsEvent -= ResetTabs;

 private void ResetTabs()
 {
  Color color;
  ColorUtility.TryParseHtmlString(_buttonsDeactiveColor, out color);
  if (fieldNumber == FieldState.CurrentField)
  {
   backgroundImage.color =  GenericSettingsFunctions.SetColorAndAlpha(_cashedBackgroundImageColor, _cashedBackgroundImageAlpha);
   text.color =  GenericSettingsFunctions.SetColorAndAlpha(_cashedTextColor, _cashedTextAlpha);
  }
  
  else
  {
   backgroundImage.color = GenericSettingsFunctions.SetColorAndAlpha(color, _buttonsDeactiveAlpha);
   text.color = GenericSettingsFunctions.SetColorAndAlpha(color, _buttonsDeactiveAlpha);
  }
 }
}

using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[RequireComponent(typeof(Slider))]
public class SliderSwitcher : MonoBehaviour
{
    private Slider _slider;
    private float _midvalue;
    private float _maxValue;
    private GameObject _fillImage;
    private Sprite _orginalSprite;
    [SerializeField] private Sprite negativeSprite;
 
    void Awake()
    {
        _slider = GetComponent<Slider>();
        _maxValue = _slider.maxValue;
        _midvalue = _slider.maxValue / 2;
      //  _fillImage = _slider.fillRect;
       // _slider.fillRect.GetComponent<Image>().
       _orginalSprite = _slider.fillRect.GetComponent<Image>().sprite;
    }

  

    [NaughtyAttributes.Button()]
    public void Changed()
    {
        if (_slider == null) return;
        _slider.onValueChanged.Invoke(_slider.value);
    }
    private void StartSliderUpdate()
    {
        var tempSliderValue = _slider.value;
        _slider.value = _slider.maxValue;
        _slider.value = tempSliderValue;
    }

    public void UpdateSliderDirection()
    {
        if (_slider == null) return;
        if (_slider.value >=  _midvalue)
        {
            _slider.fillRect.GetComponent<Image>().sprite = _orginalSprite; 
            _slider.fillRect.anchorMin = new Vector2(_midvalue, 0);
            _slider.fillRect.anchorMax = new Vector2(_slider.handleRect.anchorMin.x, _maxValue);
        } 
        
        else if (_slider.value <  _midvalue)
        {
            _slider.fillRect.GetComponent<Image>().sprite = negativeSprite;
            _slider.fillRect.anchorMin = new Vector2(_slider.handleRect.anchorMin.x, 0);
            _slider.fillRect.anchorMax = new Vector2(_midvalue, _maxValue );
        }
    }
}
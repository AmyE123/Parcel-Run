using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HueSlider : MonoBehaviour
{
    [SerializeField]
    private Image _hueTexture;

    [SerializeField]
    private Image _valueTexture;

    [SerializeField]
    private Slider _hueSlider;
    
    [SerializeField]
    private Slider _valueSlider;

    [SerializeField]
    private Image _outputImage;

    // Start is called before the first frame update
    void Start()
    {
        OnSliderUpdated();
    }

    public void OnSliderUpdated()
    {
        _valueTexture.color = Color.HSVToRGB(_hueSlider.value, 1, 1);
        _hueTexture.color = new Color(_valueSlider.value, _valueSlider.value, _valueSlider.value);

        _outputImage.color = Color.HSVToRGB(_hueSlider.value, 0.7f, _valueSlider.value);
    }
}

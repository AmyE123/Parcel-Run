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
    private Image _valueBackground;

    [SerializeField]
    private Slider _hueSlider;
    
    [SerializeField]
    private Slider _valueSlider;

    [SerializeField]
    private Image _outputImage;

    private CharacterCustomizor _customiser;

    // Start is called before the first frame update
    void Start()
    {
        _customiser = FindObjectOfType<CharacterCustomizor>();
        OnSliderUpdated();
    }

    public void SetColor(Color col)
    {
        Color.RGBToHSV(col, out float hue, out float sat, out float value);

        _hueSlider.value = hue;

        if (value < 1)
        {
            _valueSlider.value = value / 2f;
        }
        else
        {
            _valueSlider.value = 1 - (sat / 2f);
        }
    }

    public void OnSliderUpdated()
    {
        float hue = _hueSlider.value;
        float val = Mathf.Clamp01(_valueSlider.value * 2);
        float sat = 1 - Mathf.Clamp01((_valueSlider.value - 0.5f) * 2);

        _valueTexture.color = Color.HSVToRGB(hue, 1, 1);
        _valueBackground.color = Color.HSVToRGB(hue, 1, 1);
        _hueTexture.color = new Color(val, val, val);

        Color newCol = Color.HSVToRGB(hue, sat, val);
        _outputImage.color = newCol;
        _customiser.ColorChanged(newCol);
    }
}

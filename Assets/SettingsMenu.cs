using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    SettingsData _data;

    [SerializeField]
    private Slider _sliderMusic;

    [SerializeField]
    private Slider _sliderSFX;
    
    [SerializeField]
    private Slider _sliderHealth;

    [SerializeField]
    private TMP_Text _healthText;

    public void SfxSliderChanged()
    {

    }

    public void MusicSliderChanged()
    {

    }

    public void HealthSliderChanged()
    {
        _healthText.text = _data.difficulties[Mathf.RoundToInt(_sliderHealth.value)].name;
    }

    public void ApplyDataToUI()
    {
        _sliderMusic.value = _data.musicVolume;
        _sliderSFX.value = _data.soundVolume;
        _sliderHealth.value = _data.chosenDifficulty;
    }

    public void GrabDataFromUI()
    {
        _data.musicVolume = _sliderMusic.value;
        _data.soundVolume = _sliderSFX.value;
        _data.chosenDifficulty = Mathf.RoundToInt(_sliderHealth.value);
    }
    
    public void SaveSettings()
    {
        GrabDataFromUI();
        _data.SaveToPlayerPrefs();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyArea { Skin, Eyes, Brows, Shirt, Legs, Mouth, None };

public class CharacterCustomizer : MonoBehaviour
{
    [System.Serializable]
    public class CustomItem
    {
        [HideInInspector]
        public string name;
        public Transform sceneObject;
        public Sprite icon;
        public BodyArea area;
    }

    [SerializeField]
    private List<CustomItem> _items;

    [SerializeField]
    private SelectBodypartGrid _grid;

    [SerializeField]
    private Material _skinMat;

    [SerializeField]
    private Material _shirtMat;
    
    [SerializeField]
    private Material _legsMat;

    [SerializeField]
    private SlidingMenu _colorPickerPanel;

    [SerializeField]
    private SavedCharacter _saveData;

    [SerializeField]
    private PlayerVisualApply _editorPlayer;

    [SerializeField]
    private PlayerVisualApply _titlePlayer;

    private Material _currentMat;

    private BodyArea _selectedArea = BodyArea.None;

    private HueSlider _hueSlider;

    public void ButtonPressBodyArea(int areaInt)
    {
        BodyArea area = (BodyArea) areaInt;

        if (_selectedArea == area)
        {
            _grid.SetItems(_items, BodyArea.None);
            _currentMat = null;
            _selectedArea = BodyArea.None;
            _colorPickerPanel.MakeDisappear();
        }
        else
        {
            if (area == BodyArea.Skin)
                _currentMat = _skinMat;
            else if (area == BodyArea.Shirt)
                _currentMat = _shirtMat;
            else if (area == BodyArea.Legs)
                _currentMat = _legsMat;
            else
                _currentMat = null;

            if (_currentMat != null)
            {
                _hueSlider.SetColor(_currentMat.color);
                _colorPickerPanel.MakeAppear();
            }
            else
            {
                _colorPickerPanel.MakeDisappear();
            }

            _selectedArea = area;
            _grid.SetItems(_items, area);
        }
    }

    public void SelectionMade(CustomItem chosenItem)
    {
        foreach (var itm in _items)
        {
            if (itm.area != chosenItem.area)
                continue;

            if (itm.sceneObject == null)
                continue;

            itm.sceneObject.gameObject.SetActive(chosenItem.sceneObject == itm.sceneObject);
        }
    }

    void OnValidate()
    {
        foreach (var itm in _items)
        {
            if (itm.sceneObject)
                itm.name = itm.sceneObject.name + $" ({itm.area})";
            else
                itm.name = "Nothing" + $" ({itm.area})";
        }
    }

    public void ColorChanged(Color col)
    {
        if (_currentMat == null)
            return;

        _currentMat.color = col;
    }

    // Start is called before the first frame update
    void Start()
    {
        _hueSlider = FindObjectOfType<HueSlider>();
        _saveData.LoadFromPlayerPrefs();
    }

    public void Activate()
    {
        _editorPlayer.SetVisuals(_saveData);
    }

    private void ApplyChangesToSaveData()
    {
        _saveData.legsColor = _legsMat.color;
        _saveData.skinColor = _skinMat.color;
        _saveData.shirtColor = _shirtMat.color;

        _saveData.eyeName = "";
        _saveData.browName = "";
        _saveData.mouthName = "";
        _saveData.shirtName = "";
        _saveData.legsName = "";

        foreach (var itm in _items)
        {
            if (itm.sceneObject == null || itm.sceneObject.gameObject.activeSelf == false)
                continue;
            
            if (itm.area == BodyArea.Shirt)
                _saveData.shirtName = itm.sceneObject.name;
            if (itm.area == BodyArea.Mouth)
                _saveData.mouthName = itm.sceneObject.name;
            if (itm.area == BodyArea.Eyes)
                _saveData.eyeName = itm.sceneObject.name;
            if (itm.area == BodyArea.Brows)
                _saveData.browName = itm.sceneObject.name;
            if (itm.area == BodyArea.Legs)
                _saveData.legsName = itm.sceneObject.name;
        }

        _saveData.SaveToPlayerPrefs();
    }

    public void Finish()
    {
        ApplyChangesToSaveData();
        _titlePlayer.SetVisuals(_saveData);

        _grid.SetItems(_items, BodyArea.None);
        _currentMat = null;
        _selectedArea = BodyArea.None;
    }
}

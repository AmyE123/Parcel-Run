using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyArea { Skin, Eyes, Brows, Shirt, Legs, Mouth, None };

public class CharacterCustomizor : MonoBehaviour
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

    public void ButtonPressBodyArea(int areaInt)
    {
        BodyArea area = (BodyArea) areaInt;
        _grid.SetItems(_items, area);
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

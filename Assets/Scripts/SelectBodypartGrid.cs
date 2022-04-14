using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBodypartGrid : MonoBehaviour
{
    [SerializeField]
    private GameObject _itemPrefab;

    private List<SelectBodypartItem> _spawnedObjects = new List<SelectBodypartItem>();

    public void SetItems(List<CharacterCustomizer.CustomItem> items, BodyArea bodyArea)
    {
        foreach(var obj in _spawnedObjects)
            obj.DestroySelf();

        _spawnedObjects = new List<SelectBodypartItem>();

        int i=0;

        foreach (var itm in items)
        {
            if (itm.area != bodyArea)
                continue;
            
            GameObject newObj = Instantiate(_itemPrefab, transform);
            SelectBodypartItem newItem = newObj.GetComponent<SelectBodypartItem>();
            newItem.Init(itm, i);

            _spawnedObjects.Add(newItem);
            i++;
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

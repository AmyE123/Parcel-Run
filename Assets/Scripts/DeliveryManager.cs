using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField]
    private DeliveryHouse[] _allHouses;

    [SerializeField]
    private Postbox[] _allPostboxes;

    private List<DeliveryHouse> _nextHouseList;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int) System.DateTime.Now.Ticks);
        _allHouses = FindObjectsOfType<DeliveryHouse>();
        _allPostboxes = FindObjectsOfType<Postbox>();

        SpawnNewPackagesWhereNeeded();
    }

    private DeliveryHouse GetNextHouse()
    {
        if (_nextHouseList == null || _nextHouseList.Count == 0)
        {
            _nextHouseList = new List<DeliveryHouse>(_allHouses);
            Shuffle(_nextHouseList);
        }

        DeliveryHouse next = _nextHouseList[0];
        _nextHouseList.RemoveAt(0);
        return next;
    }

    public void DeliveryMade()
    {
        foreach (Postbox box in _allPostboxes)
            box.DeliveryMade();

        SpawnNewPackagesWhereNeeded();
    }

    void SpawnNewPackagesWhereNeeded()
    {
        foreach (Postbox box in _allPostboxes)
        {
            if (box.IsReadyToSpawn == false)
                continue;

            box.SpawnPackage(GetNextHouse());
        }
    }

    public static void Shuffle<T>(IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = Random.Range(0, n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}

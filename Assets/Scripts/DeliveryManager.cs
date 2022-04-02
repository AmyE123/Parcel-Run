using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField]
    private DeliveryHouse[] _allHouses;

    [SerializeField]
    private Postbox[] _allPostboxes;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int) System.DateTime.Now.Ticks);
        _allHouses = FindObjectsOfType<DeliveryHouse>();
        _allPostboxes = FindObjectsOfType<Postbox>();

        SpawnNewPackagesWhereNeeded();

        StartCoroutine(GameLoop());
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

            box.SpawnPackage(_allHouses[0]);
        }
    }


    IEnumerator GameLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Postbox : MonoBehaviour
{
    [SerializeField]
    private Transform[] _spawnPoints;

    [SerializeField]
    private int _deliveriesBeforeRespawn = 2;

    [SerializeField]
    private int _currentDeliveryCooldown = 0;

    [SerializeField]
    private GameObject _packagePrefab;

    private List<Transform> _takenSpawnPoints = new List<Transform>();

    public bool IsReadyToSpawn => _currentDeliveryCooldown <= 0 && HasFreeSpawnSlots;

    public bool HasFreeSpawnSlots => _takenSpawnPoints.Count < _spawnPoints.Length;

    public void DeliveryMade()
    {
        if (HasFreeSpawnSlots)
            _currentDeliveryCooldown -= 1;
    }

    public void SpawnPackage(DeliveryHouse destination)
    {
        if (HasFreeSpawnSlots == false)
            return;

        int offset = Random.Range(0, 99);

        for (int i=0; i<_spawnPoints.Length; i++)
        {
            int randomIndex = (i + offset) % _spawnPoints.Length;

            if (_takenSpawnPoints.Contains(_spawnPoints[randomIndex]))
                continue;
            
            Debug.Log($"Spawning something in slot {randomIndex}");
            
            GameObject newObj = Instantiate(_packagePrefab, _spawnPoints[randomIndex].position, Quaternion.identity);
            PackagePickup pickup = newObj.GetComponent<PackagePickup>();
            
            pickup.SetHouse(destination);
            _takenSpawnPoints.Add(_spawnPoints[randomIndex]);
            pickup.OnPickup = () => OnPackagePickedUp(_spawnPoints[randomIndex]);
            break;
        }
        _currentDeliveryCooldown = _deliveriesBeforeRespawn;
    }

    private void OnPackagePickedUp(Transform spawnPoint)
    {
        if (_takenSpawnPoints.Contains(spawnPoint))
        {
            _takenSpawnPoints.Remove(spawnPoint);
            Debug.Log($"{spawnPoint.name} is now free!");
        }
    }
}

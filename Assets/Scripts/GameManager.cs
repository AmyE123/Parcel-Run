using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Phase Information")]
    [SerializeField] private Phase[] _gamePhases;
    [SerializeField] private int _phaseIdx = 0;
    [SerializeField] private int _deliveriesThisPhase;
    [SerializeField] private int _deliveriesTotal;

    [Header("Spawn Information")]
    [SerializeField] private SpawnPoint _northEnemySpawnPoint;
    [SerializeField] private SpawnPoint _eastEnemySpawnPoint;
    [SerializeField] private SpawnPoint _southEnemySpawnPoint;
    [SerializeField] private SpawnPoint _westEnemySpawnPoint;

    [SerializeField] private Player _player;
    [SerializeField] private List<Enemy> _allEnemies;

    public Phase CurrentGamePhase
    {
        get
        {
            if (_gamePhases.Length == 0 || _phaseIdx >= _gamePhases.Length)
                return null;

            return _gamePhases[_phaseIdx];
        }
    }

    private void Start()
    {
        StartCoroutine(CheckForNextPhaseLoop());
    }

    private IEnumerator CheckForNextPhaseLoop()
    {
        while (true)
        {
            Phase currentPhase = CurrentGamePhase;

            if (currentPhase == null)
            {
                yield return new WaitForSeconds(1);
                continue;
            }

            bool hasReachedRequirement = _deliveriesThisPhase >= currentPhase.deliveryCountToPass;

            if (hasReachedRequirement)
            {
                yield return new WaitForSeconds(3);
                StartNextPhase();
            }

            yield return new WaitForSeconds(1);
        }
    }

    private void StartNextPhase()
    {
        _phaseIdx ++;

        Phase currentPhase = CurrentGamePhase;
        _deliveriesThisPhase = 0;

        if (currentPhase == null)
            return;

        CheckForEnemiesToSpawn();
    }

    public void DeliveryMade()
    {
        _deliveriesThisPhase += 1;
        _deliveriesTotal += 1;

        CheckForEnemiesToSpawn();
    }

    void CheckForEnemiesToSpawn()
    {
        Phase currentPhase = CurrentGamePhase;

        if (currentPhase == null)
            return;

        foreach (var enemyDrop in currentPhase.enemyDrops)
        {
            if (enemyDrop.packageRequirement == _deliveriesThisPhase)
            {
                SpawnEnemies(enemyDrop);
            }
        }  
    }

    void RunCinematicCamera()
    {

    }

    private void SpawnEnemies(Phase.EnemyDrop enemyDropInfo)
    {
        SpawnPoint spawn = GetSpawnPoint(enemyDropInfo.point);

        if (spawn == null)
        {
            Debug.LogError($"No spawn point set for {enemyDropInfo.point}!");
            return;
        }

        if (enemyDropInfo.prefabToSpawn == null)
        {
            Debug.LogError($"Cannot peform enemy drop without a prefab!");
            return;
        }
    
        for (int i=0; i<enemyDropInfo.numberToDrop; i++)
        {
            GameObject newObj = Instantiate(enemyDropInfo.prefabToSpawn, spawn.GetRandomPointInBounds(), transform.rotation);
            _allEnemies.Add(newObj.GetComponent<Enemy>());
        }
    }

    private SpawnPoint GetSpawnPoint(Phase.SpawnPoint point)
    {
        if (point == Phase.SpawnPoint.North)
            return _northEnemySpawnPoint;

        if (point == Phase.SpawnPoint.East)
            return _northEnemySpawnPoint;

        if (point == Phase.SpawnPoint.South)
            return _northEnemySpawnPoint;

        if (point == Phase.SpawnPoint.West)
            return _northEnemySpawnPoint;

        return null;
    }
}

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

    private float _timePlayed;

    public int DeliveriesTotal => _deliveriesTotal;

    public int TotalSeconds => Mathf.RoundToInt(_timePlayed);

    public Phase CurrentGamePhase
    {
        get
        {
            if (_gamePhases.Length == 0 || _phaseIdx >= _gamePhases.Length)
                return null;

            return _gamePhases[_phaseIdx];
        }
    }

    public Phase CurrentOrLastGamePhase
    {
        get
        {
            if (_gamePhases.Length == 0)
                return null;
            
            if (_phaseIdx >= _gamePhases.Length)
                return _gamePhases[_gamePhases.Length-1];

            return _gamePhases[_phaseIdx];
        }
    }

    void Update()
    {
        _timePlayed += Time.deltaTime;
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
                yield return new WaitForSeconds(0.5f);
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

        if (currentPhase.cutscenePrefab != null)
        {
            FindObjectOfType<CinematicBars>()?.TransitionIn(() =>
            {
                Instantiate(currentPhase.cutscenePrefab);
            });
        }
        else
        {
            CheckForEnemiesToSpawn();
        }

        foreach (var enemy in _allEnemies)
        {
            enemy.SyncBalanceInfo(currentPhase.balanceInfo);
        }
    }

    public void DeliveryMade()
    {
        _deliveriesThisPhase += 1;
        _deliveriesTotal += 1;

        CheckForEnemiesToSpawn();
    }

    public void CheckForEnemiesToSpawn()
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

        FindObjectOfType<ArrivalNotice>().AddToQueue(enemyDropInfo);
    
        for (int i=0; i<enemyDropInfo.numberToDrop; i++)
        {
            GameObject newObj = Instantiate(enemyDropInfo.prefabToSpawn, spawn.GetRandomPointInBounds(), transform.rotation);
            Enemy newEnemy = newObj.GetComponent<Enemy>();
            newEnemy.SyncBalanceInfo(CurrentOrLastGamePhase.balanceInfo);

            _allEnemies.Add(newEnemy);
        }
    }

    private SpawnPoint GetSpawnPoint(Phase.SpawnPoint point)
    {
        if (point == Phase.SpawnPoint.North)
            return _northEnemySpawnPoint;

        if (point == Phase.SpawnPoint.East)
            return _eastEnemySpawnPoint;

        if (point == Phase.SpawnPoint.South)
            return _southEnemySpawnPoint;

        if (point == Phase.SpawnPoint.West)
            return _westEnemySpawnPoint;

        return null;
    }
}

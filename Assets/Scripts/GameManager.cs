using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const float CINEMATICTIMER = 4;

    public Phase[] GamePhases;
    public Transform EnemySpawnPointTransform;
    public SpawnPoint EnemySpawnPoint;

    public GameObject Player;

    public List<GameObject> ActiveEnemies;

    public int PhaseIdx = 0;

    public float Timer;
    public float CinematicTimer;

    public int ParcelsDelivered;

    public GameObject CinematicCamera;

    private void Start()
    {
        Timer = GamePhases[PhaseIdx].timeBeforeNextPopulation;
        CinematicTimer = CINEMATICTIMER;
    }

    private void Update()
    {        
        StartPhase(PhaseIdx);
        if (ParcelsDelivered == GamePhases[PhaseIdx].deliveryCountToPass)
        {
            foreach (GameObject g in ActiveEnemies)
            {
                Destroy(g);
            }
            ActiveEnemies.Clear();

            CinematicTimer = CINEMATICTIMER;
            PhaseIdx++;
        }
    }

    void StartPhase(int PhaseIdx)
    {
        var phase = GamePhases[PhaseIdx];
        RunCinematicCamera();

        if (ActiveEnemies.Count < phase.minEnemyCount)
        {
            for (int i = 0; i < phase.minEnemyCount; i++)
            {
                ActiveEnemies.Add(SpawnEnemy(phase.enemyPrefab));
            }         
        }

        if (ActiveEnemies.Count < phase.maxEnemyCount)
        {            
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                ActiveEnemies.Add(SpawnEnemy(phase.enemyPrefab));
                Timer = phase.timeBeforeNextPopulation;
            }           
        }               
    }

    void RunCinematicCamera()
    {
        CinematicCamera.SetActive(true);
        CinematicTimer -= Time.deltaTime;

        if (CinematicTimer <= 0)
        {
            CinematicCamera.SetActive(false);
            CinematicTimer = 0;
        }
    }

    GameObject SpawnEnemy(GameObject Enemy)
    {
        return Instantiate(Enemy, EnemySpawnPoint.GetRandomPointInBounds(), transform.rotation);
    }
}

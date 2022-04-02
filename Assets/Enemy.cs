using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyCalculatePath enemyCalculatePath;

    void Update()
    {
        if (enemyCalculatePath.HasEnemyCaughtPlayer())
        {
            Debug.Log("Caught player!");
        }
    }
}

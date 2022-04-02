using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField]
    private PersonMovement movement;

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private float enemySpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = targetTransform.position - transform.position;
        direction.Normalize();
        movement.SetDesiredDirection(direction * enemySpeed);
    }
}

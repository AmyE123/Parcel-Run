using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Player _chasingPlayer;

    [SerializeField]
    private EnemyCalculatePath _pathCalculator;

    [SerializeField]
    private PersonMovement _movement;

    [SerializeField]
    private float _diveDistance;

    private Vector3 _playerLastSeen;

    void Start()
    {
        StartCoroutine(EnemyAIRoutine());
    }

    private bool IsWithinDivingDistance()
    {
        Vector3 diff = _chasingPlayer.transform.position - transform.position;
        diff.y = 0;
        
        return diff.magnitude <= _diveDistance;
    }

    IEnumerator EnemyAIRoutine()
    {
        while (true)
        {
            if (_chasingPlayer == null)
            {
                yield return null;;
                continue;
            }

            _pathCalculator.ChasePlayer(_chasingPlayer.transform.position);

            if (IsWithinDivingDistance())
            {
                _movement.StartDive(_chasingPlayer.transform.position);

                while (_movement.IsDiving)
                    yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }
    }

    public void PlayerSeen(Player player)
    {
        if (_chasingPlayer != null)   
            return;

        _chasingPlayer = player;
    }
}

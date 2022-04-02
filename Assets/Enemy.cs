using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Player _nearbyPlayer;

    [SerializeField]
    private EnemyCalculatePath _pathCalculator;

    [SerializeField]
    private PersonMovement _movement;

    [SerializeField]
    private float _diveDistance;

    [SerializeField]
    private float _visionRange = 20f;

    [SerializeField]
    private GameObject _exclaimPrefab;

    [SerializeField]
    private GameObject _questionPrefab;

    private Vector3 _playerLastSeen;
    private bool _inChaseMode;

    void Start()
    {
        StartCoroutine(EnemyAIRoutine());
    }

    private bool IsWithinDivingDistance()
    {
        if (_nearbyPlayer == null)
            return false;

        Vector3 diff = _nearbyPlayer.transform.position - transform.position;
        diff.y = 0;
        
        return diff.magnitude <= _diveDistance;
    }

    IEnumerator EnemyAIRoutine()
    {
        while (true)
        {
            if (_inChaseMode)
            {
                if (_nearbyPlayer != null && HasLineOfSightToPlayer())
                {
                    _playerLastSeen = _nearbyPlayer.transform.position;
                }

                _pathCalculator.ChasePlayer(_playerLastSeen);

                if (IsWithinDivingDistance())
                {
                    _movement.StartDive(_nearbyPlayer.transform.position);
                    _pathCalculator.ClearPath();
                    _movement.SetDesiredDirection(Vector3.zero);

                    while (_movement.IsDiving)
                        yield return new WaitForSeconds(0.1f);

                    if (_nearbyPlayer != null && HasLineOfSightToPlayer())
                    {
                        _playerLastSeen = _nearbyPlayer.transform.position;
                        _inChaseMode = true;
                    }
                    else
                    {
                        _inChaseMode = false;
                        SpawnQuestion();
                    }
                }
                
                if (_pathCalculator.ReachedEndOfPath())
                {
                    _pathCalculator.ClearPath();
                    _inChaseMode = false;
                    _movement.SetDesiredDirection(Vector3.zero);
                    SpawnQuestion();
                }
            }

            // If the player is hiding behind something, they might be nearby but still can't be seen
            else if ( _nearbyPlayer != null)
            {
                if (HasLineOfSightToPlayer())
                {
                    _playerLastSeen = _nearbyPlayer.transform.position;
                    _inChaseMode = true;
                    SpawnExclaimation();
                }
            }

            if (_inChaseMode == false)
            {
                _movement.SetDesiredDirection(Vector3.zero);
            }

            yield return null;
        }
    }

    bool HasLineOfSightToPlayer()
    {
        Vector3 vecToTarget = _nearbyPlayer.transform.position - transform.position;

        if (Physics.Raycast(transform.position, vecToTarget.normalized, out RaycastHit hit, _visionRange))
        {
            if (hit.collider.tag == "Player")
            {
                Debug.DrawLine(transform.position, hit.point, Color.green);
                return true;
            }
            else
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
                return false;
            }
        }

        Debug.DrawLine(transform.position, transform.position + (vecToTarget.normalized * _visionRange), Color.red);
        return false;
    }

    public void PlayerSeen(Player player)
    {
        if (_nearbyPlayer != null)   
            return;

        _nearbyPlayer = player;

        if (HasLineOfSightToPlayer())
        {
            _inChaseMode = true;
            SpawnExclaimation();
        }
    }

    private void SpawnExclaimation()
    {
        W2C.InstantiateAs<IconBurst>(_exclaimPrefab).Init(transform.position + Vector3.up);
    }

    private void SpawnQuestion()
    {
        W2C.InstantiateAs<IconBurst>(_questionPrefab).Init(transform.position + Vector3.up);
    }

    public void PlayerLeft(Player player)
    {
        if (_nearbyPlayer == player)
            _nearbyPlayer  = null;
    }
}

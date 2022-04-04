using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected Player _nearbyPlayer;

    [SerializeField]
    protected EnemyCalculatePath _pathCalculator;

    [SerializeField]
    protected PersonMovement _movement;

    [SerializeField]
    protected float _visionRange = 20f;

    [SerializeField]
    private SphereCollider _detectZone;

    [SerializeField]
    private GameObject _exclaimPrefab;

    [SerializeField]
    private GameObject _questionPrefab;

    [SerializeField]
    private LayerMask _lineOfSightLayers;

    protected Vector3 _playerLastSeen;
    protected bool _inChaseMode;

    public bool LineOfSightOverride;
    public bool ChaseModeOverride;

    public virtual void SyncBalanceInfo(Phase.GameBalance info)
    {

    }

    protected void SetDetectionRadius(float radius)
    {
        _detectZone.radius = radius;
    }

    void Start()
    {
        StartCoroutine(EnemyAIRoutine());
    }

    protected virtual bool IsCloseEnoughForAction()
    {
        return false;
    }

    protected virtual bool IsDoingAction()
    {
        return false;
    }

    protected virtual void DoCloseAction()
    {

    }

    private IEnumerator EnemyAIRoutine()
    {
        while (true)
        {
            if (GameCutscene.IsPlaying)
            {
                _movement.SetDesiredDirection(Vector3.zero);
                yield return new WaitForSeconds(0.2f);
                continue;
            }

            if (_inChaseMode || ChaseModeOverride)
            {
                if (_nearbyPlayer != null && HasLineOfSightToPlayer())
                {
                    _playerLastSeen = _nearbyPlayer.transform.position;
                }

                _pathCalculator.ChasePlayer(_playerLastSeen);

                if (IsCloseEnoughForAction())
                {
                    DoCloseAction();
                    _pathCalculator.ClearPath();
                    _movement.SetDesiredDirection(Vector3.zero);

                    while (IsDoingAction())
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

    protected bool HasLineOfSightToPlayer()
    {
        if (LineOfSightOverride)
        {
            return true;
        }

        Vector3 vecToTarget = _nearbyPlayer.transform.position - transform.position;

        if (Physics.Raycast(transform.position, vecToTarget.normalized, out RaycastHit hit, _visionRange, _lineOfSightLayers))
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

        if (_inChaseMode == false && HasLineOfSightToPlayer())
        {
            _inChaseMode = true;
            SpawnExclaimation();
        }
    }

    protected void SpawnExclaimation()
    {
        W2C.InstantiateAs<IconBurst>(_exclaimPrefab).Init(transform.position + Vector3.up);
    }

    protected void SpawnQuestion()
    {
        W2C.InstantiateAs<IconBurst>(_questionPrefab).Init(transform.position + Vector3.up);
    }

    public void PlayerLeft(Player player)
    {
        if (_nearbyPlayer == player)
            _nearbyPlayer  = null;
    }
}

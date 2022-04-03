using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceEnemy : Enemy
{
    [SerializeField]
    private float _diveDistance = 3;

    [SerializeField]
    private int _tackleDamage = 8;

    protected override bool IsCloseEnoughForAction()
    {
        return IsWithinDivingDistance();
    }

    protected override bool IsDoingAction()
    {
        return _movement.IsDiving;
    }

    protected override void DoCloseAction()
    {
        _movement.StartDive( _nearbyPlayer.transform.position);
    }

    private bool IsWithinDivingDistance()
    {
        if (_nearbyPlayer == null)
            return false;

        Vector3 diff = _nearbyPlayer.transform.position - transform.position;
        diff.y = 0;
        
        return diff.magnitude <= _diveDistance;
    } 

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag != "Player")
            return;

        if (_movement.IsMidDive == false)
            return;

        other.collider.GetComponent<Player>().TakeDamage(_tackleDamage);
    }

    
    public override void SyncBalanceInfo(Phase.GameBalance info)
    {
        SetDetectionRadius(info.policeDetectRadius);
        _visionRange = info.policeVisionRange;
        _movement.SetTopSpeed(info.policeTopSpeed, info.policeAcceleration);
        _movement.SetDiveSpeeds(info.policeDiveSpeed, info.policeDiveRecoveryTime);
        _diveDistance = info.policeDiveDistance;
    }
}

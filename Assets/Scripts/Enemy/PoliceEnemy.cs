using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceEnemy : Enemy
{
    [SerializeField]
    private float _diveDistance = 3;

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
}

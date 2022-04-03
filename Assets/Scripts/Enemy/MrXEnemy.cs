using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrXEnemy : Enemy
{
    [SerializeField]
    private float _diveDistance = 3;

    [SerializeField]
    private int _tackleDamage = 10;

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
        _movement.StartDive(_nearbyPlayer.transform.position);
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
        //SetDetectionRadius(info.mrxDetectRadius);
        //_visionRange = info.mrxVisionRange;
        //_movement.SetTopSpeed(info.mrxTopSpeed, info.mrxAcceleration);
        //_movement.SetDiveSpeeds(info.mrxDiveSpeed, info.mrxDiveRecoveryTime);
        //_diveDistance = info.mrxDiveDistance;
    }
}

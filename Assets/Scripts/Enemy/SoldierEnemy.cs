using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierEnemy : Enemy
{
    private enum ShootStage { NotShooting, Aiming, Locked, Shooting };

    [SerializeField]
    private float _shootDistance = 3;

    [SerializeField, Tooltip("How long to spend lining up the shot")]
    private float _aimTime = 2;

    [SerializeField, Tooltip("How long to wait after lining up the shot before shooting")]
    private float _holdTime = 1;

    [SerializeField, Tooltip("How long before returning to idle after shooting")]
    private float _waitAfterShootTime = 1;

    [SerializeField]
    private Transform _gunFrontTransform;

    [SerializeField]
    private PlayerAnimations _anim;

    [SerializeField]
    private GunAimVisuals _gunAimVisuals;

    [SerializeField]
    private int _gunDamage = 15;

    [SerializeField]
    private ParticleSystem _shootParticles;

    [SerializeField]
    private float _shootForce = 5f;

    private Transform _aimTarget;
    private Vector3 _directionToTarget;
    private ShootStage _currentShootStage;
    
    protected override bool IsCloseEnoughForAction()
    {
        return IsWithinShootingDistance();
    }

    protected override bool IsDoingAction()
    {
        return _movement.IsDoingAction;
    }

    protected override void DoCloseAction()
    {
        _movement.StartDoingAction();
        StartCoroutine(ShootRoutine());
    }

    private bool IsWithinShootingDistance()
    {
        if (_nearbyPlayer == null)
            return false;

        Vector3 diff = _nearbyPlayer.transform.position - transform.position;
        diff.y = 0;
        
        return diff.magnitude <= _shootDistance;
    }

    private IEnumerator ShootRoutine()
    {
        _currentShootStage = ShootStage.Aiming;
        _anim.StartAimingGun();
        _aimTarget = _nearbyPlayer.transform;
        yield return new WaitForSeconds(_aimTime);

        _currentShootStage = ShootStage.Locked;
        _aimTarget = null;

        yield return new WaitForSeconds(_holdTime);

        _currentShootStage = ShootStage.Shooting;
        
        Vector3 hitPosition = GetHitAimPosition(out GameObject hitObj);
        _gunAimVisuals.TakeShot(_gunFrontTransform.position, hitPosition);

        _shootParticles.transform.position = _gunFrontTransform.position;
        _shootParticles.transform.rotation = _gunFrontTransform.rotation;

        _shootParticles.Emit(32);
        _movement.ApplyForce(_gunFrontTransform.forward * -_shootForce);

        if (hitObj != null && hitObj.tag == "Player")
        {
            hitObj.GetComponent<Player>()?.TakeDamage(_gunDamage);
        }

        yield return new WaitForSeconds(_waitAfterShootTime);

        _movement.StopDoingAction();
        _anim.StopAimingGun();
    }

    void Update()
    {
        if (_currentShootStage == ShootStage.NotShooting)
            return;

        if (_aimTarget != null)
        {
            _directionToTarget = (_aimTarget.transform.position - transform.position).normalized;
        }

        Vector3 horzVec = _directionToTarget;
        horzVec.y = 0;

        float angle = Vector3.Angle(horzVec, _directionToTarget);

        if (_directionToTarget.y < 0)
            angle = -angle;

        _anim.SetGunAimAngle(angle);
        
        transform.rotation = Quaternion.LookRotation(horzVec.normalized);

        Vector3 hitPosition = GetHitAimPosition(out GameObject hitObj);

        if (_currentShootStage == ShootStage.Aiming)
            _gunAimVisuals.SetAiming(_gunFrontTransform.position, hitPosition);

        // The locked animation doesn't look great so just stay aiming
        if (_currentShootStage == ShootStage.Locked)
            _gunAimVisuals.SetAiming(_gunFrontTransform.position, hitPosition);
    }

    private float ShootRange => 256;

    Vector3 GetHitAimPosition(out GameObject hitObj)
    {
        hitObj = null;

        if (Physics.Raycast(_gunFrontTransform.position, _directionToTarget.normalized, out RaycastHit hit, ShootRange))
        {
            hitObj = hit.collider.gameObject;
            return hit.point;
        }

        Vector3 farPoint = _gunFrontTransform.position + (_directionToTarget * ShootRange);
        return farPoint;
    }
}

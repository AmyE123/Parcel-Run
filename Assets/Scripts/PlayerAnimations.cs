using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public bool IsRunning = false;
    //public float AnimationPlaybackSpeed = 1;

    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private PersonMovement _playerMove;
    [SerializeField] private AnimationCurve _runMapping;
    [SerializeField] private float _runSpeedMultiplier = 1f;
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SyncVars();   
    }

    void SyncVars()
    {
        float vel = _rigidBody.velocity.magnitude;

        //_anim.speed = AnimationPlaybackSpeed;
        _anim.SetFloat("moveSpeed", Mathf.Max(vel * _runSpeedMultiplier, 0.1f));
        _anim.SetBool("isRunning", _playerMove.IsGrounded && vel > 0.1f);
        _anim.SetBool("isGrounded", _playerMove.IsGrounded);
        _anim.SetFloat("runAnimation", _runMapping.Evaluate(_rigidBody.velocity.magnitude));
    }

    public void DoJump()
    {
        _anim.ResetTrigger("jumpTrigger");
        _anim.SetTrigger("jumpTrigger");
    }

    public void DoAirJump()
    {
        _anim.ResetTrigger("airJumpTrigger");
        _anim.SetTrigger("airJumpTrigger");
    }

    public void StartDive(float recoverDelay)
    {
        _anim.ResetTrigger("diveStart");
        _anim.SetTrigger("diveStart");

        Invoke(nameof(RecoverFromDive), recoverDelay);
    }

    public void RecoverFromDive()
    {
        _anim.ResetTrigger("diveRecover");
        _anim.SetTrigger("diveRecover");
    }

    public void DiveStopEvent()
    {
        _playerMove.StopDiving();
    }

    public void DiveRecoveredEvent()
    {
        _playerMove.RecoveredFromDiving();
    }

    public void StartAimingGun()
    {
        _anim.ResetTrigger("aimGunTrigger");
        _anim.SetTrigger("aimGunTrigger");
    }

    public void StopAimingGun()
    {
        _anim.ResetTrigger("stopAimGunTrigger");
        _anim.SetTrigger("stopAimGunTrigger");
    }

    public void SetGunAimAngle(float angle)
    {
        angle = Mathf.Clamp(angle, -30, 30);
        _anim.SetFloat("gunAimAngle", angle);
    }
}

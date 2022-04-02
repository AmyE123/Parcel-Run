using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public bool isRunning = false;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private PlayerMove _playerMove;
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
}

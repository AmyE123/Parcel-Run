using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MovementInfo;

public class PersonMovement : MonoBehaviour
{
    [SerializeField] JumpInfo _jump;
    [SerializeField] GroundInfo _ground;
	[SerializeField] MoveInfo _move;

    [Space(8)]
	[SerializeField] Collider _collider;
    [SerializeField] PlayerAnimations _anim;
    
    [SerializeField] float _diveSpeed;
    [SerializeField] float _diveRecoverTime;

    Rigidbody _rb;

	public bool IsGrounded => _ground.groundContactCount > 0;

    public bool IsDiving => _isDiving;

    public bool IsDoingAction => _isDoingAction;

    public Vector3 DesiredVelocity => _move.desiredVelocity;

    public float MaxSpeed => _move.maxSpeed;

    public Vector3 ActualVelocity => _rb.velocity;

    bool _isDoingAction;
    bool _isDiving;
    float _currentDiveSpeed;

    public void StartDoingAction()
    {
        _isDoingAction = true;
    }

    public void StopDoingAction()
    {
        _isDoingAction = false;
    }

    public void SetTopSpeed(float maxSpd, float maxAcc)
    {
        _move.maxSpeed = maxSpd;
        _move.maxAcceleration = maxAcc;
    }

    public void SetDiveSpeeds(float diveSpd, float diveRecover)
    {
        _diveSpeed = diveSpd;
        _diveRecoverTime = diveRecover;
    }

    public void StartDive(Vector3 targetPosition)
    {
        if (_isDiving)
            return;

        if (targetPosition.magnitude > 0)
        {
            Vector3 dir = (targetPosition - transform.position);
            dir.y = 0;
            dir.Normalize();

            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
            

        _anim.StartDive(_diveRecoverTime);
        _isDiving = true;
        _currentDiveSpeed = _diveSpeed;
    }

    public void StopDiving()
    {
        _currentDiveSpeed = 0;
    }

    public void RecoveredFromDiving()
    {
        _isDiving = false;
    }

    public void SetDesiredDirection(Vector3 direction)
    {
        if (_isDiving || _isDoingAction)
            return;

        _move.desiredVelocity = direction * _move.maxSpeed;

        direction.y = 0;
    
        if (direction.magnitude > 0)
            HandleFacingDirection(direction.normalized);
    }

    public void SetJumpRequested(bool isRequested)
    {
        if (_isDiving || _isDoingAction)
            return;

        _jump.isRequested |= isRequested;
    }

	void OnValidate () 
    {
		_ground.minGroundDotProduct = Mathf.Cos(_ground.maxSlopeAngle * Mathf.Deg2Rad);
	}

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        OnValidate();
    }

    void ClearState () 
    {
		_ground.groundContactCount = 0;
		_ground.contactNormal = Vector3.zero;
        _ground.wallContactCount = 0;
        _ground.wallNormal = Vector3.zero;
	}

    void HandleFacingDirection(Vector3 desiredDirection)
    {
        if (desiredDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 19);
        }
    }

    void FixedUpdate()
    {
        UpdateState();
        AdjustVelocity();

        if (_jump.isRequested) 
			Jump();

        _rb.velocity = _move.velocity;
        ClearState();
    }


    void Jump()
    {
        _jump.isRequested = false;

        float jumpSpeed = _jump.Speed;

        if (IsGrounded)
        {
            float alignedSpeed = Vector3.Dot(_move.velocity, _ground.contactNormal);
            if (alignedSpeed > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
            }

            _move.velocity += _ground.contactNormal  * jumpSpeed;
            SetHasJump(true);
            _anim.DoJump();
        }
        else if (_jump.hasExtraJump)
        {
            float boostAlignment = Vector3.Dot(ActualVelocity.normalized, transform.forward);
            boostAlignment = Mathf.Clamp01(boostAlignment);

            _move.velocity *= boostAlignment;

            _jump.hasExtraJump = false;
            _move.velocity += transform.forward * _jump.extraJumpSpeed;
            _move.velocity += Vector3.up * _jump.extraJumpLift;
            _anim.DoAirJump();
        }

        _jump.stepsSinceLastJump = 0;
    }

    void SetHasJump(bool hasit)
    {
        _jump.hasExtraJump = hasit;
    }

    Vector3 ProjectOnContactPlane (Vector3 vector) 
    {
		return vector - _ground.contactNormal * Vector3.Dot(vector, _ground.contactNormal);
	}

    void AdjustVelocity () 
    {
        if (_isDiving || _isDoingAction)
        {
            float yVel = _move.velocity.y;
            _move.velocity = transform.forward * _currentDiveSpeed;
            _move.velocity.y = yVel;
            return;
        }

		Vector3 xAxis = ProjectOnContactPlane(Vector3.right);
		Vector3 zAxis = ProjectOnContactPlane(Vector3.forward);

        float currentX = Vector3.Dot(_move.velocity, xAxis);
		float currentZ = Vector3.Dot(_move.velocity, zAxis);

		float acceleration = IsGrounded ? _move.maxAcceleration : _move.maxAirAcceleration;
		float maxSpeedChange = acceleration * Time.deltaTime;

        Vector2 currentVel = new Vector2(currentX, currentZ);
        Vector2 desiredVel = new Vector2(_move.desiredVelocity.x, _move.desiredVelocity.z);

        Vector2 newVel = Vector2.MoveTowards(currentVel, desiredVel, maxSpeedChange);

        _move.velocity += xAxis * (newVel.x - currentX) + zAxis * (newVel.y - currentZ);
	}

    void UpdateState () 
    {
        _ground.stepsSinceLastGrounded += 1;
        _jump.stepsSinceLastJump += 1;
		_move.velocity = _rb.velocity;

		if (IsGrounded || SnapToGround() || CheckSteepContacts()) 
        {
            _ground.stepsSinceLastGrounded = 0;
 
            if (_jump.stepsSinceLastJump > 2)
                SetHasJump(false);

            if (_ground.groundContactCount > 1) {
				_ground.contactNormal.Normalize();
			}
		}
		else 
        {
			_ground.contactNormal = Vector3.up;
		}
	}

    bool SnapToGround () {
		if (_ground.stepsSinceLastGrounded > 1 || _jump.stepsSinceLastJump <= 2) {
			return false;
		}
		float speed = _move.velocity.magnitude;
		if (speed > _ground.maxSnapSpeed) {
			return false;
		}

        if (!Physics.Raycast(_rb.position, Vector3.down, out RaycastHit hit, _ground.probeDistance)) {
			return false;
		}

        if (hit.normal.y < _ground.minGroundDotProduct) {
			return false;
		}

		_ground.contactNormal = hit.normal;
		float dot = Vector3.Dot(_move.velocity, hit.normal);
		if (dot > 0f) {
			_move.velocity = (_move.velocity - hit.normal * dot).normalized * speed;
		}
		return true;
	}

    void OnCollisionStay (Collision collision) => EvaluateCollision(collision);

    void EvaluateCollision (Collision collision) 
    {
		for (int i = 0; i < collision.contactCount; i++) 
        {
			Vector3 normal = collision.GetContact(i).normal;

            if (normal.y >= _ground.minGroundDotProduct) 
            {
				_ground.groundContactCount += 1;
				_ground.contactNormal += normal;
			}
            else if (normal.y > -0.01f && normal.y < 0.05f) 
            {
				_ground.wallContactCount += 1;
				_ground.wallNormal += normal;
			}
		}
	}

    bool CheckSteepContacts () 
    {
		if (_ground.wallContactCount > 1) {
			_ground.wallNormal.Normalize();
			if (_ground.wallNormal.y >=_ground.minGroundDotProduct)
            {
				_ground.groundContactCount = 1;
				_ground.contactNormal = _ground.wallNormal;
				return true;
			}
		}
		return false;
	}
}
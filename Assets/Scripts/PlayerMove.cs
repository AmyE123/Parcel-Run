using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [System.Serializable]
    public class JumpInfo
    {
        [Range(0f, 10f)] public float height = 2f;
        [Range(0f, 10f)] public float wallJumpPower = 2f;
        [Range(0f, 100f)] public float extraJumpSpeed = 20f;
        [Range(0f, 100f)] public float extraJumpLift = 20f;
        [Range(0f, 5f)] public int maxAirJumps = 0;

        [HideInInspector] public bool hasExtraJump;
        [HideInInspector] public int stepsSinceLastJump;
        [HideInInspector] public bool isRequested;

        public float Speed => Mathf.Sqrt(-2f * Physics.gravity.y * height);
    }

    [System.Serializable]
    public class GroundInfo
    {
        [Range(0f, 90f)] public float maxSlopeAngle = 25f;
        [Range(0f, 100f)] public float maxSnapSpeed = 100f;
	    [Min(0f)] public float probeDistance = 1f;

        [HideInInspector] public int stepsSinceLastGrounded;
        [HideInInspector] public float minGroundDotProduct;

        [HideInInspector] public Vector3 contactNormal;
        [HideInInspector] public int groundContactCount;

        [HideInInspector] public Vector3 wallNormal;
        [HideInInspector] public int wallContactCount;

    }

    [System.Serializable]
    public class MoveInfo
    {
        [Range(0f, 100f)] public float maxAcceleration = 10f;
        [Range(0f, 100f)] public float maxAirAcceleration = 1f;
        [Range(0f, 100f)] public float maxSpeed = 10f;

        [HideInInspector] public Vector3 velocity;
        [HideInInspector] public Vector3 desiredVelocity;
    }


    [SerializeField] JumpInfo _jump;
    [SerializeField] GroundInfo _ground;
	[SerializeField] MoveInfo _move;
	[SerializeField] Collider _collider;
    [SerializeField] PlayerAnimations _anim;
    [SerializeField] Transform _cameraTransform;
    [SerializeField] bool isPlayer;
    
    Rigidbody _rb;

	public bool IsGrounded => _ground.groundContactCount > 0;

    bool OnWall => _ground.wallContactCount > 0;

    public void SetDesiredDirection(Vector3 velocity)
    {
        _move.desiredVelocity = velocity;
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

    Vector3 CameraForward
    {
        get
        {
            Vector3 fwd = _cameraTransform.forward;
            fwd.y = 0;
            return fwd.normalized;
        }
    }

    Vector3 CameraRight => _cameraTransform.right;

    // Update is called once per frame
    void Update()
    {
        if (isPlayer)
        {
            Vector2 playerInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            playerInput = Vector2.ClampMagnitude(playerInput, 1);

            Vector3 xComponent = playerInput.x * CameraRight;
            Vector3 yComponent = playerInput.y * CameraForward;

            _move.desiredVelocity = (xComponent + yComponent) * _move.maxSpeed;
            _jump.isRequested |= Input.GetButtonDown("Jump");

            HandleFacingDirection(xComponent + yComponent);
        }
        else 
        {
            
        }
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
		Vector3 xAxis = ProjectOnContactPlane(Vector3.right);
		Vector3 zAxis = ProjectOnContactPlane(Vector3.forward);

        float currentX = Vector3.Dot(_move.velocity, xAxis);
		float currentZ = Vector3.Dot(_move.velocity, zAxis);

		float acceleration = IsGrounded ? _move.maxAcceleration : _move.maxAirAcceleration;
		float maxSpeedChange = acceleration * Time.deltaTime;

		float newX = Mathf.MoveTowards(currentX, _move.desiredVelocity.x, maxSpeedChange);
		float newZ = Mathf.MoveTowards(currentZ, _move.desiredVelocity.z, maxSpeedChange);

        _move.velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
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
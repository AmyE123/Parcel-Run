using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementInfo;
    
public class MopedMove : MonoBehaviour 
{
    [SerializeField]
    Transform _frontWheel;

    [SerializeField]
    Transform _backWheel;

    [SerializeField] GroundInfo _ground;

    [Range(0f, 100f), SerializeField] private float maxAcceleration = 10f;
    [Range(0f, 100f), SerializeField] private float maxSpeed = 10f;
    [Range(0f, 100f), SerializeField] private float maxTurnSpeed = 10f;

    [SerializeField] Transform _frontWheelPosition;
    [SerializeField] Transform _backWheelPosition;
    [SerializeField] float _wheelRadius;
    
    private Vector3 velocity;
    private float angularSpeed;
    private float desiredSpeed;
    private float desiredTurn;

    private bool _isFrontGrounded;
    private bool _isBackGrounded;


    private CameraFollow _cameraFollow;
  
    Rigidbody _rb;

	public bool IsGrounded => _ground.groundContactCount > 0;

    public float DesiredVelocity => desiredSpeed;

    public Vector3 ActualVelocity => _rb.velocity;

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

    // Update is called once per frame
    void Update()
    {
        HandlePlayerInput();
    }

    void HandlePlayerInput()
    {
        if (_cameraFollow == null)
            _cameraFollow = FindObjectOfType<CameraFollow>();

        Vector2 playerInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        playerInput = Vector2.ClampMagnitude(playerInput, 1);

        Vector3 xComponent = playerInput.x * _cameraFollow.CameraRight;
        Vector3 yComponent = playerInput.y * _cameraFollow.CameraForward;

        desiredSpeed = playerInput.y * maxSpeed;
        desiredTurn = playerInput.x * maxTurnSpeed;
    }

    void FixedUpdate()
    {
        UpdateState();
        AdjustVelocity();

        _rb.velocity = velocity;
        ClearState();

        if (Physics.Raycast(_rb.position, Vector3.down, out RaycastHit hit, _ground.probeDistance)) 
        {
            Quaternion targetRot = Quaternion.LookRotation(transform.forward, hit.normal);
            transform.rotation = targetRot;
		}
    }

    private void CheckWheelsGrounded()
    {
        Transform fw = _frontWheelPosition;
        Transform bw = _backWheelPosition;
        _isFrontGrounded = _isBackGrounded = false;

        if (Physics.Raycast(fw.position, -fw.up, out RaycastHit hitFront, _wheelRadius + 0.05f))
        {
			_isFrontGrounded = true;
		}
        if (Physics.Raycast(bw.position, -bw.up, out RaycastHit hitBack, _wheelRadius + 0.05f))
        {
			_isBackGrounded = true;
		}
    }

    Vector3 ProjectOnContactPlane (Vector3 vector) 
    {
		return vector - _ground.contactNormal * Vector3.Dot(vector, _ground.contactNormal);
	}

    void AdjustVelocity () 
    {
		Vector3 xAxis = ProjectOnContactPlane(Vector3.right);
		Vector3 zAxis = ProjectOnContactPlane(Vector3.forward);

        float currentX = Vector3.Dot(velocity, xAxis);
		float currentZ = Vector3.Dot(velocity, zAxis);

		float acceleration = _isBackGrounded ? maxAcceleration : maxAcceleration;
		float maxSpeedChange = acceleration * Time.deltaTime;

        Vector2 currentVel = new Vector2(currentX, currentZ);
        Vector2 desiredVel = new Vector2(transform.forward.x, transform.forward.z)  * desiredSpeed;

        Vector2 newVel = Vector2.MoveTowards(currentVel, desiredVel, maxSpeedChange);

        velocity += xAxis * (newVel.x - currentX) + zAxis * (newVel.y - currentZ);

        Vector3 angVel = _rb.angularVelocity;
        angVel.y = desiredTurn;

        _rb.angularVelocity = angVel;
	}

    void UpdateState () 
    {
        _ground.stepsSinceLastGrounded += 1;
		velocity = _rb.velocity;

        CheckWheelsGrounded();

		if (IsGrounded || CheckSteepContacts()) 
        {
            _ground.stepsSinceLastGrounded = 0;
            _rb.useGravity = false;

            if (_ground.groundContactCount > 1) {
				_ground.contactNormal.Normalize();
			}
		}
		else 
        {
            _rb.useGravity = true;
			_ground.contactNormal = Vector3.up;
		}
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
    

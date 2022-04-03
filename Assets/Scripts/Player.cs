using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PersonMovement))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float _throwDistance;

    [SerializeField]
    private Transform _packageVisuals;

    [SerializeField]
    private GameObject _thrownPackagePrefab;

    [SerializeField]
    private GameObject _uiPrefab;

    PersonMovement _movement;
    CameraFollow _cameraFollow;

    private bool _hasPackage;
    private DeliveryHouse _currentDestination;
    private bool _canThrowItem;

    public bool CanThrowItem => _canThrowItem;

    public bool HasPackage => _hasPackage;

    public Vector3 CurrentDestination => _currentDestination == null ? Vector3.zero : _currentDestination.DoorPosition;

    public bool CanReceivePackage() => _hasPackage == false;

    public void TakePackage(DeliveryHouse destination)
    {
        _hasPackage = true;
        _currentDestination = destination;
        _packageVisuals.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        _movement = GetComponent<PersonMovement>();
        W2C.InstantiateAs<PlayerUI>(_uiPrefab).SetPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameCutscene.IsPlaying)
        {
            _movement.SetDesiredDirection(Vector3.zero);
            return;
        }

        HandlePlayerInput();
        DetermineDistanceFromHouse();
    }

    void HandlePlayerInput()
    {
        if (_cameraFollow == null)
            _cameraFollow = FindObjectOfType<CameraFollow>();

        Vector2 playerInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        playerInput = Vector2.ClampMagnitude(playerInput, 1);

        Vector3 xComponent = playerInput.x * _cameraFollow.CameraRight;
        Vector3 yComponent = playerInput.y * _cameraFollow.CameraForward;

        _movement.SetDesiredDirection(xComponent + yComponent);
        _movement.SetJumpRequested(Input.GetButtonDown("Jump"));

        if (Input.GetKeyDown(KeyCode.E))
            ThrowPackage();

    }

    void ThrowPackage()
    {
        if (_canThrowItem == false)
            return;

        _hasPackage = false;
        _canThrowItem = false;
        _packageVisuals.gameObject.SetActive(false);

        GameObject projectile = Instantiate(_thrownPackagePrefab);
        projectile.GetComponent<ThrownPackage>().Throw(transform.position, _currentDestination);

        _currentDestination = null;
    }

    void DetermineDistanceFromHouse()
    {
        _canThrowItem = false;

        if (_currentDestination != null)
        {
            Vector3 vecToTarget = _currentDestination.DoorPosition - transform.position;

            if (vecToTarget.magnitude <= _throwDistance)
            {
                if (Physics.Raycast(transform.position, vecToTarget.normalized, out RaycastHit hit, vecToTarget.magnitude))
                {
                    if (_currentDestination.IsThisYourDoor(hit.transform))
                    {
                        _canThrowItem = true;
                        Debug.DrawLine(transform.position, hit.point, Color.green);
                    }
                    else
                    {
                        Debug.DrawLine(transform.position, hit.point, Color.red);
                    }
                }
            }
        }
    }
}

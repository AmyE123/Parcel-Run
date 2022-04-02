using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PersonMovement))]
public class Player : MonoBehaviour
{
    PersonMovement _movement;
    CameraFollow _cameraFollow;

    // Start is called before the first frame update
    void Start()
    {
        _movement = GetComponent<PersonMovement>();
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

        _movement.SetDesiredDirection(xComponent + yComponent);
        _movement.SetJumpRequested(Input.GetButtonDown("Jump"));
    }
}

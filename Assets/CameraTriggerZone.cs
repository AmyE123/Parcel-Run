using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTriggerZone : MonoBehaviour
{
    [SerializeField]
    private TutorialCameraManager _man;

    [SerializeField]
    private CinemachineVirtualCamera _cam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _man.SetCamera(_cam);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TutorialCameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera[] _virtualCameras;

    public void SetCamera(CinemachineVirtualCamera cam)
    {
        foreach (var c in _virtualCameras)
        {
            c.gameObject.SetActive(cam == c);
        }
    }
}

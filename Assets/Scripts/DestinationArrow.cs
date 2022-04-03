using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationArrow : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    
    [SerializeField]
    private Transform _arrowObject;

    // Update is called once per frame
    void LateUpdate()
    {
        _arrowObject.gameObject.SetActive(_player.HasPackage);

        if (_player.HasPackage)
        {
            Vector3 dirToDest = _player.CurrentDestination - transform.position;
            dirToDest.y = 0;
            dirToDest.Normalize();

            transform.rotation = Quaternion.LookRotation(dirToDest, Vector3.up);
        }
    }
}

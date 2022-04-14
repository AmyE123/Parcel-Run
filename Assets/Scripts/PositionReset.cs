using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionReset : MonoBehaviour
{
    [SerializeField]
    private Vector3 _resetPosition;

    [SerializeField]
    private float _zKill;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < _zKill)
            transform.position = _resetPosition;        
    }
}

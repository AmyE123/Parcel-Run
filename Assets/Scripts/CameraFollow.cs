using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    float _lerpSpeed;

    [SerializeField]
    Transform _target;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, _lerpSpeed * Time.deltaTime);        
    }

    public Vector3 CameraForward
    {
        get
        {
            Vector3 fwd = transform.forward;
            fwd.y = 0;
            return fwd.normalized;
        }
    }

    public Vector3 CameraRight
    {
        get
        {
            return transform.right;
        }
    }
}

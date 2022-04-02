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
}

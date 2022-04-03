using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotate : MonoBehaviour
{
    [SerializeField]
    private Vector3 _rotateRate;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_rotateRate * Time.deltaTime);

    }
}

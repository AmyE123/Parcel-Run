using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenBob : MonoBehaviour
{
    [SerializeField]
    private float _bobAmount;

    [SerializeField]
    private float _bobSpeed;

    [SerializeField]
    private Vector3 _rotateAmount;

    [SerializeField]
    private Vector3 _rotateSpeed;

    private float _bobPhase;
    private Vector3 _rotatePhase;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _bobPhase += Time.deltaTime * _bobSpeed;
        _rotatePhase.x += Time.deltaTime * _rotateSpeed.x;
        _rotatePhase.y += Time.deltaTime * _rotateSpeed.y;
        _rotatePhase.z += Time.deltaTime * _rotateSpeed.z;

        transform.position = new Vector3(0, _bobAmount * Mathf.Sin(_bobPhase), 0);

        float x = Mathf.Sin(_rotatePhase.x) * _rotateAmount.x;
        float y = Mathf.Sin(_rotatePhase.y) * _rotateAmount.y;
        float z = Mathf.Sin(_rotatePhase.z) * _rotateAmount.z;

        transform.rotation = Quaternion.Euler(x, y, z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GunAimVisuals : MonoBehaviour
{
    [SerializeField] 
    Transform _meshTransform;

    [SerializeField]
    MeshRenderer _mesh;

    [SerializeField]
    Material _redMat;

    [SerializeField]
    Material _shootMat;

    [SerializeField]
    float _shootFlashTime;

    [SerializeField]
    float _shootBlinkTime;

    [SerializeField]
    float _shootSize;

    [SerializeField]
    float _aimSize;
    
    Vector3 _fromPos;
    Vector3 _toPos;

    bool _isActive;
    bool _isFlashActive;

    public void SetAiming(Vector3 from, Vector3 to)
    {
        _isActive = true;
        _fromPos = from;
        _toPos = to;

        _meshTransform.gameObject.SetActive(true);
        _meshTransform.localScale = new Vector3(_aimSize, 0.5f, _aimSize);

        if (_mesh.material != _redMat)
            _mesh.material = _redMat;
    }

    public void SetHolding(Vector3 from, Vector3 to)
    {
        _isActive = true;
        _fromPos = from;
        _toPos = to;

        _meshTransform.gameObject.SetActive(true);
        _meshTransform.localScale = new Vector3(_aimSize, 0.5f, _aimSize);

        if (_isFlashActive == false)
            StartCoroutine(FlashAim());
    
        if (_mesh.material != _redMat)
            _mesh.material = _redMat;
    }

    public void TakeShot(Vector3 from, Vector3 to)
    {
        _isActive = true;
        _fromPos = from;
        _toPos = to;

        _isFlashActive = false;

        _meshTransform.gameObject.SetActive(true);
        _meshTransform.localScale = new Vector3(_aimSize, 0.5f, _aimSize);

        if (_mesh.material != _shootMat)
            _mesh.material = _shootMat;

        _meshTransform.DOScale(new Vector3(_shootSize, 0.5f, _shootSize), _shootFlashTime * 0.3f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            _meshTransform.DOScale(new Vector3(0, 0.5f, 0), _shootFlashTime * 0.7f).SetEase(Ease.InSine).OnComplete(() => 
            {
                _isActive = false;
                _meshTransform.gameObject.SetActive(false);
            });
        });
    }

    IEnumerator FlashAim()
    {
        _isFlashActive = true;

        while (_isFlashActive)
        {
            _meshTransform.gameObject.SetActive(false);
            yield return new WaitForSeconds(_shootBlinkTime);
            
            _meshTransform.gameObject.SetActive(true);
            yield return new WaitForSeconds(_shootBlinkTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive == false)
            return;
        
        Vector3 vecToTarget = _toPos - _fromPos;
        transform.position = _fromPos;
        transform.localScale = new Vector3(1, 1, vecToTarget.magnitude);
        transform.LookAt(_toPos);
    }
}

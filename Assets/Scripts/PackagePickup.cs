using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PackagePickup : MonoBehaviour
{
    [SerializeField]
    private float _spinSpeed;

    [SerializeField]
    private float _bobSpeed;

    [SerializeField]
    private float _bobAmount;

    [SerializeField]
    private Transform _pickupBox;

    [SerializeField]
    private DeliveryHouse _targetHouse;

    private float _bobPhase;
    private Vector3 _startPos;
    private bool _isDestroying;

    void Start()
    {
        _pickupBox.localScale = Vector3.zero;
        _pickupBox.DOScale(1, 1).SetEase(Ease.OutExpo);
    }

    void Update()
    {
        _bobPhase += Time.deltaTime * _bobSpeed;
        _pickupBox.localPosition = new Vector3(0, _bobAmount * Mathf.Sin(_bobPhase), 0);

        transform.Rotate(0, _spinSpeed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (_isDestroying)
            return;

        if (other.tag != "Player")
            return;

        Player player = other.GetComponent<Player>();

        if (player.CanReceivePackage() == false)
            return;

        _isDestroying = true;
        _pickupBox.gameObject.SetActive(false);

        player.TakePackage(_targetHouse);
        transform.DOScale(new Vector3(0, 1, 0), 0.5f).SetEase(Ease.InSine).OnComplete(() => Destroy(gameObject));
    }
}

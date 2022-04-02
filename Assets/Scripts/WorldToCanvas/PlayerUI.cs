using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerUI : W2C
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private RectTransform _deliverRect;

    public void SetPlayer(Player player)
    {
        _player = player;
        SetPosition(_player.transform);
    }

    void Start()
    {
        _deliverRect.localScale = Vector3.zero;
        _deliverRect.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.CanThrowItem && _deliverRect.gameObject.activeSelf == false)
        {
            _deliverRect.gameObject.SetActive(true);
            _deliverRect.DOScale(1, 0.5f).SetEase(Ease.OutElastic);
        }

        if (_player.CanThrowItem == false && _deliverRect.gameObject.activeSelf == true)
        {
            DOTween.Kill(_deliverRect);
            _deliverRect.localScale = Vector3.zero;
            _deliverRect.gameObject.SetActive(false);
        }   
    }
}

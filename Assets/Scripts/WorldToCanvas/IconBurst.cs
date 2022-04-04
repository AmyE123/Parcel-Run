using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class IconBurst : W2C
{
    [SerializeField] float _appearTime;
    [SerializeField] float _lifetime;
    [SerializeField] float _vanishTime;

    [SerializeField] Ease _easeIn;
    [SerializeField] Image _image;

    public void Init(Vector3 position)
    {
        _trackRect.localScale = Vector3.zero;
        _trackRect.DOScale(1, _appearTime).SetEase(_easeIn);

        _image.DOFade(0, _vanishTime).SetDelay(_appearTime + _lifetime).OnComplete(() => Destroy(gameObject));
        SetPosition(position);
    }

    public void Init(Transform tran, Vector3 position)
    {
        _trackRect.localScale = Vector3.zero;
        _trackRect.DOScale(1, _appearTime).SetEase(_easeIn);

        _image.DOFade(0, _vanishTime).SetDelay(_appearTime + _lifetime).OnComplete(() => Destroy(gameObject));
        SetPosition(tran, position);
    }
}

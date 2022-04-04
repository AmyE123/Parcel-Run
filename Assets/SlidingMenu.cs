using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SlidingMenu : MonoBehaviour
{
    [SerializeField]
    private Vector2 _inactivePosition;

    [SerializeField]
    private float _transitionTime = 1f;

    [SerializeField]
    private Ease _appearEase = Ease.OutExpo;

    [SerializeField]
    private Ease _disappearEase = Ease.OutExpo;

    private RectTransform _rt;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
    }

    public void MakeAppear(float tTime=0, float delay=0)
    {
        gameObject.SetActive(true);

        DOTween.Kill(_rt);

        if (tTime == 0)
            tTime = _transitionTime;

        _rt.DOAnchorPos(Vector2.zero, _transitionTime)
            .SetEase(_appearEase).SetDelay(delay);
    }

    public void MakeDisappear(float tTime=0)
    {
        DOTween.Kill(_rt);

        if (tTime == 0)
            tTime = _transitionTime;

        _rt.DOAnchorPos(_inactivePosition, tTime)
            .SetEase(_disappearEase)
            .OnComplete(() => gameObject.SetActive(false));

    }
}

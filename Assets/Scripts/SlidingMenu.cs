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

    [SerializeField]
    private float _appearDelay;

    [SerializeField]
    private CanvasGroup _grp;

    private RectTransform _rt;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
    }

    public void MakeAppear()
    {
        gameObject.SetActive(true);

        DOTween.Kill(_rt);

        _rt.DOAnchorPos(Vector2.zero, _transitionTime).SetEase(_appearEase).SetDelay(_appearDelay);

        if (_grp != null)
            _grp.DOFade(1, _transitionTime).SetEase(_appearEase).SetDelay(_appearDelay);
    }

    public void MakeDisappear()
    {
        DOTween.Kill(_rt);

        _rt.DOAnchorPos(_inactivePosition, _transitionTime)
            .SetEase(_disappearEase)
            .OnComplete(() => gameObject.SetActive(false));

        if (_grp != null)
            _grp.DOFade(0, _transitionTime).SetEase(_disappearEase);

    }
}

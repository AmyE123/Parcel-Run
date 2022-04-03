using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CinematicBars : MonoBehaviour
{
    [SerializeField]
    private RectTransform[] _bars;

    [SerializeField]
    private float _transitionTime = 0.5f;
    
    public void Activate()
    {
        foreach (RectTransform rt in _bars)
        {
            DOTween.Kill(rt);
            rt.DOScaleY(1, _transitionTime).SetEase(Ease.OutExpo);
        }
    }

    public void Deactivate()
    {
        foreach (RectTransform rt in _bars)
        {
            DOTween.Kill(rt);
            rt.DOScaleY(0, _transitionTime).SetEase(Ease.OutExpo);
        }
    }
}

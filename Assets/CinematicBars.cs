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

    [SerializeField]
    private float _pauseTime = 0.2f;

    [SerializeField]
    private float _percentBlockScreen = 0.25f;
    
    public void TransitionIn(System.Action OnComplete)
    {
        StartCoroutine(TransitionInRoutine(OnComplete));
    }

    public void TransitionOut(System.Action OnComplete)
    {
        StartCoroutine(TransitionOutRoutine(OnComplete));
    }

    private IEnumerator TransitionInRoutine(System.Action OnComplete)
    {
        foreach (RectTransform rt in _bars)
        {
            rt.DOScaleY(1, _transitionTime).SetEase(Ease.Linear);
        }

        yield return new WaitForSeconds(_transitionTime + _pauseTime);
        OnComplete.Invoke();

        foreach (RectTransform rt in _bars)
        {
            rt.DOScaleY(_percentBlockScreen, _transitionTime).SetEase(Ease.Linear);
        }
    }

    private IEnumerator TransitionOutRoutine(System.Action OnComplete)
    {
        foreach (RectTransform rt in _bars)
        {
            rt.DOScaleY(1, _transitionTime).SetEase(Ease.Linear);
        }

        yield return new WaitForSeconds(_transitionTime + _pauseTime);
        OnComplete.Invoke();

        foreach (RectTransform rt in _bars)
        {
            rt.DOScaleY(0, _transitionTime).SetEase(Ease.Linear);
        }
    }
}

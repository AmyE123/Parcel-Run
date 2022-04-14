using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelSelectScreen : MonoBehaviour
{
    [SerializeField]
    private RectTransform[] _rects;

    [SerializeField]
    private CanvasGroup[] _grps;

    [SerializeField]
    float _moveAmount = 25f;

    [SerializeField]
    float _inTime = 1f;

    [SerializeField]
    float _outTime = 1f;

    [SerializeField]
    Ease _inEase;

    [SerializeField]
    Ease _outEase;

    [SerializeField]
    float _delayBetween = 0.2f;

    [SerializeField]
    float _delayOnStart = 0.2f;

    private bool _selectionMade;

    public void EnableScreen()
    {
        gameObject.SetActive(true);

        for (int i=0; i<3; i++)
        {
            DOTween.Kill(_rects[i]);
            DOTween.Kill(_grps[i]);
            float delay = (i * _delayBetween) + _delayOnStart;

            _rects[i].anchoredPosition = new Vector2(0, _moveAmount);
            _rects[i].DOAnchorPosY(0, _inTime).SetEase(_inEase).SetDelay(delay);
            _grps[i].alpha = 0;
            _grps[i].DOFade(1, _inTime).SetEase(_inEase).SetDelay(delay);
        }
    }

    public void DisableScreen()
    {
        for (int i=0; i<3; i++)
        {
            DOTween.Kill(_rects[i]);
            DOTween.Kill(_grps[i]);

            _rects[i].anchoredPosition = new Vector2(0, 0);
            _rects[i].DOAnchorPosY(-_moveAmount, _outTime).SetEase(_outEase).SetDelay(i * _delayBetween);

            bool disableObj = i == 2;
            _grps[i].DOFade(0, _outTime).SetEase(_outEase).SetDelay(i * _delayBetween).OnComplete(() =>
            {
                if (disableObj)
                {
                    gameObject.SetActive(false);
                }
            });
        }
    }

    public void ButtonPressed(string sceneToLoad)
    {
        if (_selectionMade)
            return;

        _selectionMade = true;
        FindObjectOfType<TransitionManager>().LoadScene(sceneToLoad);
    }
}

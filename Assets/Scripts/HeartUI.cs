using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeartUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform _outlineRect;

    [SerializeField]
    private Image _fillImage;

    [SerializeField]
    private Image _blackFillImage;

    private int _quartersRemaining = 4;

    private bool _isDestroyed;

    public void SetInitialFillAmount(int quarters)
    {
        _fillImage.fillAmount = quarters / 4f;
        _blackFillImage.fillAmount = quarters / 4f;
    }

    public void TakeDamage(int newAmount)
    {
        if (newAmount >= _quartersRemaining || _isDestroyed)
            return;

        DOTween.Kill(_outlineRect);

        _outlineRect.DOShakeAnchorPos(0.5f, 5, 20).OnComplete(() =>
        {
            _outlineRect.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutExpo);
        });

        _fillImage.fillAmount = newAmount / 4f;
    }

    public float GetReducePercent()
    {
        return _blackFillImage.fillAmount - _fillImage.fillAmount;
    }

    public void AnimateHeartAway(float time)
    {
        _blackFillImage.DOFillAmount(_fillImage.fillAmount, time).SetEase(Ease.Linear);
    }

    void Update()
    {
        if (_blackFillImage.fillAmount <= 0.01f && _isDestroyed == false)
            DestroyMe();

    }

    public void DestroyMe()
    {
        _isDestroyed = true;
        _outlineRect.DOScale(0, 0.5f).SetEase(Ease.InExpo).OnComplete(() => 
        {
            Destroy(gameObject);
            DOTween.Kill(_outlineRect);
        });
    }
}

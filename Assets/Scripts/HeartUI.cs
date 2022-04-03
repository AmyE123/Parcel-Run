using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform _outlineRect;

    [SerializeField]
    private Image _fillImage;

    [SerializeField]
    private Image _blackFillImage;

    private int _quartersRemaining = 4;

    public void SetInitialFillAmount(int quarters)
    {
        _fillImage.fillAmount = quarters / 4f;
        _blackFillImage.fillAmount = quarters / 4f;
    }

    public void TakeDamage(int newAmount)
    {
        if (newAmount >= _quartersRemaining)
            return;

        _fillImage.fillAmount = newAmount / 4f;
    }
}

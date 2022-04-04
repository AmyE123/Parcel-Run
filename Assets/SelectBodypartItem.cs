using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectBodypartItem : MonoBehaviour
{
    [SerializeField]
    private Image _icon;

    [SerializeField]
    private RectTransform _buttonRect;
    
    [SerializeField]
    private RectTransform _iconRect;

    [SerializeField]
    private float _appearTime = 0.5f;

    [SerializeField]
    private float _appearDelay = 0.15f;

    [SerializeField]
    private float _iconDelay = 0.05f;

    private CharacterCustomizor.CustomItem _item;

    public void DestroySelf()
    {
        DOTween.Kill(_buttonRect);
        DOTween.Kill(_iconRect);
        Destroy(gameObject);
    }

    public void ButtonPress()
    {
        FindObjectOfType<CharacterCustomizor>().SelectionMade(_item);
    }

    public void Init(CharacterCustomizor.CustomItem itm, int idx)
    {
        _icon.sprite = itm.icon;
        _item = itm;

        _buttonRect.localScale = Vector3.zero;
        _iconRect.localScale = Vector3.zero;

        _buttonRect.DOScale(1, _appearTime).SetEase(Ease.OutBack).SetDelay(idx * _iconDelay);
        _iconRect.DOScale(1, _appearTime).SetEase(Ease.OutBack).SetDelay((idx * _iconDelay) + _iconDelay);
    }
}

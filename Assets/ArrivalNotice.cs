using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class ArrivalNotice : MonoBehaviour
{
    Queue<Phase.EnemyDrop> _toDisplay = new Queue<Phase.EnemyDrop>();

    [SerializeField]
    TMP_Text[] _text;

    [SerializeField]
    Image _icon;

    [SerializeField]
    float _timeToDisplay;

    [SerializeField]
    float _transitionInTime;

    [SerializeField]
    Ease _transitionInEase;

    [SerializeField]
    float _transitionOutTime;

    [SerializeField]
    Ease _transitionOutEase;

    [SerializeField]
    Vector2 _inactivePos;

    [SerializeField]
    Vector2 _activePos;

    RectTransform _rt;

    public void AddToQueue(Phase.EnemyDrop drop)
    {
        _toDisplay.Enqueue(drop);
    }

    // Update is called once per frame
    void Start()
    {
        _rt = GetComponent<RectTransform>();
        StartCoroutine(ShowRoutine());
    }

    void SetItem(Phase.EnemyDrop drop)
    {
        _text[0].text = $"{drop.numberToDrop} more";

        if (drop.numberToDrop == 1)
            _text[1].text = $"has arrived in the";
        else
            _text[1].text = $"have arrived in the";

        _text[2].text = $" {drop.point}";

        _icon.sprite = drop.prefabToSpawn.GetComponent<Enemy>().face;
    }


    IEnumerator ShowRoutine()
    {
        while (true)
        {
            
            if (_toDisplay.Count > 0)
            {
                var nextItem = _toDisplay.Dequeue();
                SetItem(nextItem);

                _rt.DOAnchorPos(_activePos, _transitionInTime).SetEase(_transitionInEase);
                yield return new WaitForSeconds(_transitionInTime + _timeToDisplay);
                _rt.DOAnchorPos(_inactivePos, _transitionOutTime).SetEase(_transitionOutEase);
                yield return new WaitForSeconds(_transitionOutTime);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}

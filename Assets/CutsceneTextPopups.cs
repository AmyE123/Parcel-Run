using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CutsceneTextPopups : MonoBehaviour
{
    [SerializeField]
    private ScaleTransition[] _scales;

    void OnEnable()
    {
        StartAllTweens();
    }

    public void StartAllTweens()
    {
        List<RectTransform> _alreadyDone = new List<RectTransform>();

        foreach (ScaleTransition transit in _scales)
        {
            if (_alreadyDone.Contains(transit.target) == false)
            {
                DOTween.Kill(transit.target);
                transit.target.localScale = transit.startVal;
                _alreadyDone.Add(transit.target);
            }
            
            transit.target.DOScale(transit.endVal, transit.time).SetEase(transit.ease).SetDelay(transit.delay);
        }
    }

    [System.Serializable]
    public class ScaleTransition
    {
        public RectTransform target;
        public Vector3 startVal;
        public Vector3 endVal;
        public float time = 1;
        public float delay = 0;
        public Ease ease = Ease.OutExpo;
    }
}

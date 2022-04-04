using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LoseScreen : MonoBehaviour
{
    [SerializeField]
    private FadeTransition[] _fades;

    [SerializeField]
    private ScaleTransition[] _scales;

    [SerializeField]
    private TMP_Text _deliveryCount;

    [SerializeField]
    private TMP_Text _timeCount;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetValues(int totalDeliveries, int seconds)
    {
        _deliveryCount.text = totalDeliveries.ToString();
        _timeCount.text = seconds.ToString();
    }

    public void BtnPressAgain()
    {
        FindObjectOfType<TransitionManager>().ReloadCurrentScene();
    }

    public void BtnPressMenu()
    {
        FindObjectOfType<TransitionManager>().LoadScene("TitleScene");
    }

    void InitAllState()
    {
        foreach (FadeTransition transit in _fades)
        {
            DOTween.Kill(transit.target);
            transit.target.DOFade(transit.startVal, 0);
        }

        foreach (ScaleTransition transit in _scales)
        {
            DOTween.Kill(transit.target);
            transit.target.localScale = transit.startVal;
        }
    }

    public void StartAllTweens()
    {
        foreach (FadeTransition transit in _fades)
        {
            DOTween.Kill(transit.target);
            transit.target.DOFade(transit.startVal, 0);
            transit.target.DOFade(transit.endVal, transit.time).SetEase(transit.ease).SetDelay(transit.delay);
        }

        foreach (ScaleTransition transit in _scales)
        {
            DOTween.Kill(transit.target);
            transit.target.localScale = transit.startVal;
            transit.target.DOScale(transit.endVal, transit.time).SetEase(transit.ease).SetDelay(transit.delay);
        }
    }


    [System.Serializable]
    public class FadeTransition
    {
        public Graphic target;
        public float startVal;
        public float endVal;
        public float time = 1;
        public float delay = 0;
        public Ease ease = Ease.OutExpo;
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

    [System.Serializable]
    public class MoveTransition
    {
        public RectTransform target;
        public Vector2 startVal;
        public Vector2 endVal;
        public float time = 1;
        public float delay = 0;
        public Ease ease = Ease.OutExpo;
    }
}

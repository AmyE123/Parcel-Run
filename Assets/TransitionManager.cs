using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    [SerializeField]
    private Image[] _bars;

    [SerializeField]
    private float _transitionTime = 1f;

    [SerializeField]
    private float _pauseTime = 0.2f;

    private bool _alreadyExists;
    
    void Start()
    {
        if (_alreadyExists)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            _alreadyExists = true;
        }
    }

    public void LoadScene(string sceneToLoad)
    {
        StartCoroutine(TransitionRoutine(sceneToLoad));
    }

    private IEnumerator TransitionRoutine(string sceneToLoad)
    {
        foreach (Image img in _bars)
            img.DOFillAmount(1, _transitionTime / 2).SetEase(Ease.Linear);

        yield return new WaitForSeconds(_transitionTime + _pauseTime);
        SceneManager.LoadScene(sceneToLoad);

        foreach (Image img in _bars)
            img.DOFillAmount(0, _transitionTime).SetEase(Ease.Linear);
    }
}

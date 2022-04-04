using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseMenu : MonoBehaviour
{
    public bool IsPaused => _isActive;

    bool _isActive;

    [SerializeField]
    private SlidingMenu _slide;

    [SerializeField]
    private CanvasGroup _grp;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (_isActive)
                Deactivate();
            else
                Activate();
        }
    }

    void Start()
    {
        _grp.blocksRaycasts = _grp.interactable = false;
        _isActive = false;
        _grp.alpha = 0;

        _slide.MakeDisappear();
    }

    public void Activate()
    {
        if (_isActive)
            return;

        _grp.blocksRaycasts = _grp.interactable = true;
        _slide.MakeAppear();
        _isActive = true;
    }

    public void Deactivate()
    {
        if (_isActive == false)
            return;

        _grp.blocksRaycasts = _grp.interactable = false;
        _slide.MakeDisappear();
        _isActive = false;
    }

    public void BtnPressRestart()
    {
        FindObjectOfType<TransitionManager>().ReloadCurrentScene();
    }

    public void BtnPressQuit()
    {
        FindObjectOfType<TransitionManager>().LoadScene("TitleScene");
    }
}


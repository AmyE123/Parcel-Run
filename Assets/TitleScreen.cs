using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    private SlidingMenu _rootMenu;

    [SerializeField]
    private SlidingMenu _customTopBar;

    [SerializeField]
    private SlidingMenu _customBottomBar;

    [SerializeField]
    private Transform _rootCam;

    [SerializeField]
    private Transform _customCam;

    [SerializeField]
    private PlayerVisualApply _editorPlayer;

    [SerializeField]
    private SavedCharacter _saveData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToCustom()
    {
        _editorPlayer.SetVisuals(_saveData);
        _customCam.gameObject.SetActive(true);
        _rootCam.gameObject.SetActive(false);
        _rootMenu.MakeDisappear();
        _customTopBar.MakeAppear(2f, 1.2f);
    }

    public void GoToRoot()
    {
        _customCam.gameObject.SetActive(false);
        _rootCam.gameObject.SetActive(true);
        _rootMenu.MakeAppear(delay: 1.2f);
        _customTopBar.MakeDisappear();
        _customBottomBar.MakeDisappear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestarter : MonoBehaviour
{
    [SerializeField]
    KeyCode keyCode;

    [SerializeField]
    string sceneToLoad;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCode))
            FindObjectOfType<TransitionManager>().ReloadCurrentScene();
    }
}

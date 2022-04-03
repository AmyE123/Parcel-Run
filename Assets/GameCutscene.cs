using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCutscene : MonoBehaviour
{
    public static bool IsPlaying => _isPlaying;

    private static bool _isPlaying;

    [SerializeField]
    private float _destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        _isPlaying = true;
        Destroy(gameObject, _destroyTime);
        FindObjectOfType<CinematicBars>()?.Activate();
    }

    // Update is called once per frame
    void OnDestroy()
    {
        _isPlaying = false;
        FindObjectOfType<CinematicBars>()?.Deactivate();
    }
}

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
        StartCoroutine(DestroySelf());
    }

    // Update is called once per frame
    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(_destroyTime);

        FindObjectOfType<CinematicBars>()?.TransitionOut(() => 
        {
            _isPlaying = false;
            Destroy(gameObject);
        });
    }
}

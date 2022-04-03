using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameCutscene : MonoBehaviour
{
    public static bool IsPlaying => _isPlaying;

    private static bool _isPlaying;

    [SerializeField]
    private float _destroyTime;

    private Volume v;
    private DepthOfField dov;

    // Start is called before the first frame update
    void Start()
    {
        _isPlaying = true;
        StartCoroutine(DestroySelf());

        v = FindObjectOfType<Volume>();
        v.profile.TryGet(out dov);
        dov.active = false;
    }

    // Update is called once per frame
    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(_destroyTime);

        v = FindObjectOfType<Volume>();
        v.profile.TryGet(out dov);
        dov.active = true;

        FindObjectOfType<CinematicBars>()?.TransitionOut(() => 
        {
            _isPlaying = false;
            Destroy(gameObject);
        });
    }
}

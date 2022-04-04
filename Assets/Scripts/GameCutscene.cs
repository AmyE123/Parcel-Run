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

    // Start is called before the first frame update
    void Start()
    {
        _isPlaying = true;
        StartCoroutine(DestroySelf());

        v = FindObjectOfType<Volume>();

        if ( v != null)
        {
            v.profile.TryGet(out DepthOfField dov);
            dov.focusDistance.value = 12.2f;
            dov.focalLength.value = 197;
            dov.aperture.value = 7.3f;
        }

    }

    // Update is called once per frame
    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(_destroyTime);

        FindObjectOfType<CinematicBars>()?.TransitionOut(() => 
        {
            _isPlaying = false;

            Destroy(gameObject);

            v = FindObjectOfType<Volume>();
            if (v != null)
            {
                v.profile.TryGet(out DepthOfField dov);
                dov.focusDistance.value = 30;
                dov.focalLength.value = 260;
                dov.aperture.value = 3;
            }
        });
    }
}

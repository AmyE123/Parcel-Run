using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettingSetter : MonoBehaviour
{
    [SerializeField]
    SettingsData _settings;

    [SerializeField]
    bool isMusic;

    // Start is called before the first frame update
    void Start()
    {
        if (isMusic)
            GetComponent<AudioSource>().volume *= _settings.musicVolume;
        else
            GetComponent<AudioSource>().volume *= _settings.soundVolume;
    }
}

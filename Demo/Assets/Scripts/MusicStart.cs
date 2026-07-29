using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStart : MonoBehaviour
{
    public AudioSource _music;
    
    void Start()
    {
        if (_music != null)
        {
            _music.Stop();
        }
    }

    void Update()
    {
        if (!StartPause.IsGameStarted)
            return;

        if (_music != null && !_music.isPlaying)
        {
            _music.Play();
        }
    }
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance = null;
    public static MusicPlayer Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Play()
    {
        GetComponent<AudioSource>().Play();
    }

    public void Stop()
    {
        GetComponent<AudioSource>().Stop();
    }
}


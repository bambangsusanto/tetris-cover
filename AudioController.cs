using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    public AudioClip[] theme;
    public AudioClip[] sfx;

    AudioSource[] musicSources;
    
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            musicSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;
            }

            PlayTheme("Main");
        }
    }

    public void PlayTheme(string name)
    {
        for (int i = 0; i < theme.Length; i++)
        {
            if (theme[i].name == name)
            {
                musicSources[0].clip = theme[i];
                musicSources[0].loop = true;
                break;
            }
        }

        if (musicSources[0].clip != null)
            musicSources[0].Play();
    }

    public void PlaySFX(string name)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (sfx[i].name == name)
            {
                musicSources[1].clip = sfx[i];
                musicSources[1].loop = false;
            }
        }

        if (musicSources[1].clip != null)
            musicSources[1].Play();
    }

}

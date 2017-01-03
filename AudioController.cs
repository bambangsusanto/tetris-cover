using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    public AudioClip[] theme;
    public AudioClip[] sfx;

    string sceneName;

    AudioSource[] musicSources;

    // Difficulty variable is stored in this script because of static instance
    public int difficulty;

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

            sceneName = SceneManager.GetActiveScene().name;
            PlayTheme(sceneName);
        }
    }

    public void StopTheme()
    {
        FadeAway(2.0f);
    }

    public void PlayTheme(string name)
    {
        FadeIn(1.0f);

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

    IEnumerator Fade(float from, float to, float duration)
    {
        float percent = 0f;
        
        while (percent < 1)
        {
            percent += Time.deltaTime * (1 / duration);
            musicSources[0].volume = Mathf.Lerp(from ,to , percent);
            yield return null;
        }
    }

    public void FadeAway(float duration)
    {
        StartCoroutine(Fade(1f, 0f, duration));
    }

    public void FadeIn(float duration)
    {
        StartCoroutine(Fade(0f, 1f, duration));
    }

}

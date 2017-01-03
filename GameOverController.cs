using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour {

    public Image fadePlane;
    public Text scoreText;
    public Text gameOverShadow;
    public GameObject highscoreText;

    int highscore;
    int currentScore;

    public void Initiate()
    {
        gameObject.SetActive(true);
        StartCoroutine(AnimateFade(Color.clear, new Color(0, 0, 0, 0.45f), 0.2f));
        StartCoroutine(AnimateShadow(new Vector3(1f, 1f, 1f), new Vector3(1.2f, 1.2f, 1f), 0.5f));
    }

    IEnumerator AnimateFade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }

    IEnumerator AnimateShadow(Vector3 from, Vector3 to, float time)
    {
        float speed = 1 / time;
        float percent = 0f;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            gameOverShadow.transform.localScale = Vector3.Lerp(from, to, percent);
            yield return null;
        }
    }

    public void SetScoreText(int score)
    {
        scoreText.text = score.ToString("D6");
        currentScore = score;
    }

    public void CheckHighScore()
    {
        highscore = PlayerPrefs.GetInt("score");
        if (currentScore > highscore)
        {
            highscoreText.SetActive(true);
            PlayerPrefs.SetInt("score", currentScore);
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene("Game");        
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

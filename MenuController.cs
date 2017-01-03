using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Transform mainMenu;
    public Transform difficultyMenu;

    public Text highScoreText;
    
    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        AudioController.instance.PlayTheme(sceneName);

        int highscore = PlayerPrefs.GetInt("score");
        highScoreText.text = highscore.ToString("D6");
    }

    public void OpenDifficultyMenu()
    {
        mainMenu.gameObject.SetActive(false);
        difficultyMenu.gameObject.SetActive(true);
    }

    public void BackToMainMenu()
    {
        mainMenu.gameObject.SetActive(true);
        difficultyMenu.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame(int difficultyInButton)
    {
        AudioController.instance.difficulty = difficultyInButton;
        SceneManager.LoadScene("Game");
    }
}

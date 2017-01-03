using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Game State
    enum State { Playing, GameOver }
    State gameState;

    // Variable for shapes holder
    public GameObject[] shapes;
    GameObject shapeToBeSpawned;

    // Player feedback UI
    public Text scoreText;
    int score;
    int consecutiveCount;
    public GameObject gameOverUI;

    // Variable for mapping
    bool[,] map = new bool[10, 20];
    int blockCountInOneLine;

    // Variables for play area and game difficulty
    int playAreaWidth = 10;
    int playAreaHeight = 20;
    
    int difficulty;
    public Text hardModeText;
    public Text modeText;

    // Variables for next shape panel in the left
    public Transform nextShapeHolder;
    public GameObject[] nextShapes;
    GameObject nextShape;
    NextShape nextShapeToBeDestroyed;
    
    private void Start()
    {
        difficulty = AudioController.instance.difficulty;

        switch (difficulty)
        {
            case 0:
                modeText.text = "EASY";
                break;
            case 1:
                modeText.text = "MEDIUM";
                break;
            case 2:
                modeText.text = "HARD";
                break;
            default:
                break;
        }

        gameState = State.Playing;

        string sceneName = SceneManager.GetActiveScene().name;
        AudioController.instance.PlayTheme(sceneName);

        shapeToBeSpawned = shapes[Random.Range(0, shapes.Length)];
        SpawnNextBlock();
        blockCountInOneLine = 0;
        score = 0;
    }

    public void SpawnNextBlock()
    {
        if (gameState == State.Playing)
        {
            Instantiate(shapeToBeSpawned, shapeToBeSpawned.transform.position, Quaternion.identity);
            int randomizer = Random.Range(0, shapes.Length);
            shapeToBeSpawned = shapes[randomizer];

            if (difficulty < 2)
                InstantiateNextShape(randomizer);
            else
                hardModeText.gameObject.SetActive(true);
        }
        else
        {
            GameOver();
        }
    }

    public void MapBlock(Vector3 position)
    {
        if ((int)position.y + 10 >= 10)
            gameState = State.GameOver;
        else
            map[(int)position.x + 4, (int)position.y + playAreaHeight] = true;
        
    }

    public void CheckLines()
    {
        for (int i = 0; i < playAreaHeight; i++)
        {
            for (int j = 0; j < playAreaWidth; j++)
            {
                if (map[j, i])
                    blockCountInOneLine++;
            }

            if (blockCountInOneLine == playAreaWidth)
            {
                // Make mapping back to default.
                for (int j = i; j < playAreaHeight; j++)
                {
                    for (int k = 0; k < playAreaWidth; k++)
                    {
                        map[k, j] = false;
                    }
                }

                Block[] blocks = FindObjectsOfType<Block>();
                foreach (Block block in blocks)
                {
                    // Destroy one line of blocks
                    if ((int)(block.transform.localPosition.y) == (i - playAreaHeight))
                        Destroy(block.gameObject);

                    // Move downward remaining blocks above
                    if ((int)(block.transform.localPosition.y) > (i - playAreaHeight))
                    {
                        block.MoveBlock(Vector3.down);
                        MapBlock(block.transform.localPosition);
                    }
                }

                AddScore();
                consecutiveCount++;
                i--;
            }

            blockCountInOneLine = 0;
            
        }
        consecutiveCount = 0;
    }

    private void GameOver()
    {
        AudioController.instance.StopTheme();
        AudioController.instance.PlaySFX("Game Over");
        GameOverController gameOverController = gameOverUI.GetComponent<GameOverController>();
        gameOverController.Initiate();
        gameOverController.SetScoreText(score);
        gameOverController.CheckHighScore();
    }

    private void AddScore()
    {
        score += 100 * (int)Mathf.Pow(3, consecutiveCount);
        scoreText.text = score.ToString("D6");
    }

    private void InstantiateNextShape(int index)
    {
        nextShapeToBeDestroyed = FindObjectOfType<NextShape>();
        if (nextShapeToBeDestroyed != null)
            Destroy(nextShapeToBeDestroyed.gameObject);

        nextShape = nextShapes[index];
        Instantiate(nextShape, nextShapeHolder.transform.position, Quaternion.identity);

    }


}

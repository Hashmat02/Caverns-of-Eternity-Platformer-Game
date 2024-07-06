using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MazePlayer : MonoBehaviour
{
    float speed = 2.0f;
    public int gems = 0;
    public int lives = 3;

    public TextMeshProUGUI gemAmount;
    public TextMeshProUGUI youWin;
    public GameObject door;

    public GameObject[] lifeSprites; 
    public GameObject tryAgainButton; 
    public GameObject mainMenuButton; 
    public GameObject returnToPreviousLevelButton;
    public GameObject nextLevelButton;

    private bool levelCleared = false;
    private bool isGameOver = false;
    private Vector3 startPosition;
    private List<GameObject> collectedGems = new List<GameObject>();

    void Start()
    {
        youWin.text = "";  
        tryAgainButton.SetActive(false);
        mainMenuButton.SetActive(false);
        returnToPreviousLevelButton.SetActive(false);
        nextLevelButton.SetActive(false);
        UpdateLifeSprites();
        startPosition = transform.position;
    }

    void Update()
    {
        if (isGameOver || levelCleared) return;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
        }

        if (gems == 15)
        {
            Destroy(door);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Keys"))
        {
            gems++;
            gemAmount.text = "Gems: " + gems;
            collectedGems.Add(collision.gameObject);
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("FinalCrystal") && !levelCleared)
        {
            collision.gameObject.SetActive(false);
            LevelCleared();
        }

        if (collision.gameObject.CompareTag("Fire") || collision.gameObject.CompareTag("Walls"))
        {
            LoseLife();
        }
    }

    void LevelCleared()
    {
        levelCleared = true;
        youWin.text = "Level Cleared!";
        nextLevelButton.SetActive(true);
    }

    void LoseLife()
    {
        lives--;
        UpdateLifeSprites();

        if (lives > 0)
        {
            ShowRetryUI();
        }
        else
        {
            ShowGameOverUI();
        }
    }

    void UpdateLifeSprites()
    {
        for (int i = 0; i < lifeSprites.Length; i++)
        {
            lifeSprites[i].SetActive(i < lives);
        }
    }

    void ShowRetryUI()
    {
        isGameOver = true;
        tryAgainButton.SetActive(true);
        mainMenuButton.SetActive(true);
    }

    void ShowGameOverUI()
    {
        isGameOver = true;
        mainMenuButton.SetActive(true);
        returnToPreviousLevelButton.SetActive(true);
        tryAgainButton.SetActive(false);
    }

    public void TryAgain()
    {
        isGameOver = false;
        levelCleared = false;
        tryAgainButton.SetActive(false);
        mainMenuButton.SetActive(false);
        nextLevelButton.SetActive(false);
        youWin.text = "";
        ResetLevel();
    }

    void ResetLevel()
    {
        gems = 0;
        gemAmount.text = "Gems: " + gems;
        transform.position = startPosition;
        door.SetActive(true);

        foreach (GameObject gem in collectedGems)
        {
            if (gem != null)
            {
                gem.SetActive(true);
            }
        }
        collectedGems.Clear();

        GameObject finalCrystal = GameObject.FindGameObjectWithTag("FinalCrystal");
        if (finalCrystal != null)
        {
            finalCrystal.SetActive(true);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ReturnToPreviousLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex > 0)
        {
            SceneManager.LoadScene(currentSceneIndex - 1);
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("TestScene");
    }

}
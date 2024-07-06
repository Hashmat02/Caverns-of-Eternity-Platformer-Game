using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    //
    public static GameManager Instance;
    public int crystalsToNextLevel = 8;
    private int collectedCrystals = 0;
    public TextMeshProUGUI crystalCountText;
    public GameObject crystalPrefab;
    public float crystalSpawnInterval = 15f;
    private float crystalSpawnTimer;
    public string nextLevelSceneName = "Level2";
    public BoxCollider2D gridArea;
    private bool isCrystalActive = false;

    public int maxLives = 3;
    private int currentLives;
    public GameObject[] lifeObjects;
    public Snake snake;

    private bool isGameOver = false;

    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public string mainMenuSceneName = "MainMenu";

    private int highScore = 0;
    public TextMeshProUGUI highScoreText;

    public GameObject lifeLostPanel;
    public Button tryAgainButton;
    public Button mainMenuButton;

    public int score = 0;
    public TextMeshProUGUI scoreText;
    public int foodPoints = 1;
    public int crystalPoints = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentLives = maxLives;
        UpdateLifeDisplay();
        gameOverPanel.SetActive(false);

        UpdateCrystalDisplay();
        crystalSpawnTimer = crystalSpawnInterval;

        tryAgainButton.onClick.AddListener(TryAgain);
        mainMenuButton.onClick.AddListener(LoadMainMenu);

        UpdateScoreDisplay();
    }

    public void LoseLife()
    {
        if (isGameOver) return;

        currentLives--;
        UpdateLifeDisplay();

        // Reset crystal count
        ResetCrystalCount();

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            ShowLifeLostPanel();
        }
    }

    private void ShowLifeLostPanel()
    {
        lifeLostPanel.SetActive(true);
        snake.StopMovement();
    }


    private void ResetCrystalCount()
    {
        collectedCrystals = 0;
        UpdateCrystalDisplay();
    }

    private void UpdateLifeDisplay()
    {
        for (int i = 0; i < lifeObjects.Length; i++)
        {
            lifeObjects[i].SetActive(i < currentLives);
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over!");
        snake.StopMovement();
        gameOverPanel.SetActive(true);
        gameOverText.text = "Game Over!";

        if (SceneManager.GetActiveScene().name != "Level1")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            // If it's the first level, just show the main menu button
            mainMenuButton.gameObject.SetActive(true);
        }
        if (score > highScore)
        {
            highScore = score;
        }
        highScoreText.text = $"High Score: {highScore}";
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void Update()
    {
        if (!isGameOver && !isCrystalActive)
        {
            crystalSpawnTimer -= Time.deltaTime;
            if (crystalSpawnTimer <= 0)
            {
                SpawnCrystal();
                crystalSpawnTimer = crystalSpawnInterval;
            }
        }
    }

    public void CollectCrystal()
    {
        collectedCrystals++;
        AddScore(crystalPoints);
        UpdateCrystalDisplay();
        isCrystalActive = false;
        if (collectedCrystals >= crystalsToNextLevel)
        {
            LoadNextLevel();
        }
    }

    private void TryAgain()
    {
        lifeLostPanel.SetActive(false);
        ResetGame();
        snake.canMove = true;
    }

    private void UpdateCrystalDisplay()
    {
        crystalCountText.text = $"Crystals: {collectedCrystals} / {crystalsToNextLevel}";
    }

    private void SpawnCrystal()
    {
        Vector2 randomPosition = GetRandomPosition();
        GameObject crystal = Instantiate(crystalPrefab, randomPosition, Quaternion.identity);
        isCrystalActive = true;
        Debug.Log($"Crystal spawned at position: {randomPosition}");
    }

    private Vector2 GetRandomPosition()
    {
        Bounds bounds = this.gridArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(Mathf.Round(x), Mathf.Round(y));
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelSceneName);
    }

    public void ResetGame()
    {
        GameObject[] crystals = GameObject.FindGameObjectsWithTag("Crystal");
        foreach (GameObject crystal in crystals)
        {
            Destroy(crystal);
        }
        isCrystalActive = false;
        ResetCrystalCount();
        snake.ResetState();
        score = 0;
        UpdateScoreDisplay();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = $"Score: {score}";
    }

    public void CollectFood()
    {
        AddScore(foodPoints);
    }

}
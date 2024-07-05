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

    public int maxLives = 3;
    private int currentLives;
    public GameObject[] lifeObjects;
    public Snake snake;

    private bool isGameOver = false;

    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public string mainMenuSceneName = "MainMenu";


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
    }

    public void LoseLife()
    {
        if (isGameOver) return;

        currentLives--;
        UpdateLifeDisplay();

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            snake.ResetState();
        }
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
        if (!isGameOver)
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
        UpdateCrystalDisplay();

        if (collectedCrystals >= crystalsToNextLevel)
        {
            LoadNextLevel();
        }
    }

    private void UpdateCrystalDisplay()
    {
        crystalCountText.text = $"Crystals: {collectedCrystals} / {crystalsToNextLevel}";
    }

    private void SpawnCrystal()
    {
        Vector2 randomPosition = GetRandomPosition();
        GameObject crystal = Instantiate(crystalPrefab, randomPosition, Quaternion.identity);
    }

    private Vector2 GetRandomPosition()
    {
        // Implement this method to return a random position within your game boundaries
        // You can use similar logic to your food spawning
        Bounds bounds = this.gridArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
        return Vector2.zero; // Placeholder
    }
}
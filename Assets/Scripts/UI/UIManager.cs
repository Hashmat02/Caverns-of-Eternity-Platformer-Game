using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    public static UIManager instance;
    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;

    private bool isPaused = false; // Track if the game is currently paused

    void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;

        if (!gameOverScreen)
        {
            ErrorHandling.throwError("No GameOverPanel found.");
        }
        if (!pauseScreen)
        {
            ErrorHandling.throwError("No PausePanel found.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle pause state
            TogglePause();
        }
    }

    #region Game Over

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverScreen.SetActive(true);
        onGameOver?.Invoke();
    }

    #endregion

    #region Pause

    public void TogglePause()
    {
        isPaused = !isPaused; // Toggle pause state

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0; // Pause the game
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1; // Resume the game
    }

    #endregion
}

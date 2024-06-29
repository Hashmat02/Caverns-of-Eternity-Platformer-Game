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

	void Awake() {
		if (instance && instance != this) {
			Destroy(this);
			return;
		}
		instance = this;

		if (!gameOverScreen) {
			ErrorHandling.throwError("No GameOverPanel found.");
		}
		if (!pauseScreen) {
			ErrorHandling.throwError("No PausePanel found.");
		}
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If pause screen already active, unpause and vice versa
            PauseGame(!pauseScreen.activeInHierarchy);
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

    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);
        Time.timeScale = status ? 0 : 1; // Pause or resume the game
    }

    #endregion
}

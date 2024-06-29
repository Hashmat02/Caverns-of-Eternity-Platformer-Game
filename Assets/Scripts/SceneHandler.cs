using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {
    public static void GoToMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Constants.SCENE_MAIN_MENU);
    }

    public static void GoToGameplay() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Constants.SCENE_GAMEPLAY);
    }

	public static void ReloadScene() {
        Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

    public static void QuitGame() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.ExitPlaymode();
		return;
#endif
        Application.Quit();
    }
}

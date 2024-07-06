using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Constants.SCENE_MAIN_MENU);
    }

    public static void GoToGameplay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Constants.SCENE_GAMEPLAY);
    }

    public static void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void GoToMainMenuFromShop()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Constants.SCENE_MAIN_MENU);
    }

    public static void GoToShopFromMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Constants.SCENE_SHOP);
    }

    public static void GoToInventory()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Constants.SCENE_INVENTORY);
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
        return;
#endif
        Application.Quit();
    }
}

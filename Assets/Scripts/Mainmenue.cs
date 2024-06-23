using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenue : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GoToSettingsMenue()
    {
        SceneManager.LoadScene("SettingsMenue");
    }
    public void MainMenue()
    {
        SceneManager.LoadScene("Game Menue");
    }


}

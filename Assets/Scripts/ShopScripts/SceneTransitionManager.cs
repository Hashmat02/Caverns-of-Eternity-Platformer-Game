using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public void GoToMainMenu()
    {
        if (ShopManagerScript.Instance != null)
        {
            ShopManagerScript.Instance.SaveData(); // Save data before changing the scene
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToShop()
    {
        SceneManager.LoadScene("ShopScene");
    }
}

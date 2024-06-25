//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class KillerTrap : MonoBehaviour
//{
//    public int respawn;

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            SceneManager.LoadScene(respawn);
//        }
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerTrap : MonoBehaviour
{
    // Reference to the Game Over menu Canvas
    public GameObject gameOverMenu;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Show the Game Over menu
            gameOverMenu.SetActive(true);

            // Optionally, you can pause the game
            Time.timeScale = 0f;
        }
    }
}

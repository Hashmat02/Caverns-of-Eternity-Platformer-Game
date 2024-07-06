using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    private Laser_Tutorial _laserController;
    private bool doorOpen = false;

    void Start()
    {
        _laserController = FindObjectOfType<Laser_Tutorial>();
        if (!_laserController)
        {
            Debug.LogError("No Laser_Tutorial found in the scene.");
        }
    }

    void Update()
    {
        if (_laserController)
        {
            if (_laserController.m_lineRenderer.enabled)
            {
                if (!doorOpen)
                {
                    OpenDoor();
                }
            }
            else
            {
                if (doorOpen)
                {
                    CloseDoor();
                }
            }
        }
    }

    void OpenDoor()
    {
        doorOpen = true;
        Debug.Log("Door is open");
        // Implement door opening logic here, such as animation or changing sprite
    }

    void CloseDoor()
    {
        doorOpen = false;
        Debug.Log("Door is closed");
        // Implement door closing logic here, such as animation or changing sprite
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Trigger entered by: " + collider.tag);
        if (doorOpen && collider.CompareTag("Player"))
        {
            Debug.Log("Player reached the door, loading next level");
            SceneHandler.GoToNextLevel(); // Use SceneHandler to load the next level
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    private Laser_Tutorial _laserController;

    void Start()
    {
        _laserController = FindObjectOfType<Laser_Tutorial>();
        if (!_laserController)
        {
            Debug.LogError("No Laser_Tutorial found in the scene.");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") || collider.CompareTag("Box")) // Check for both Player and Box tags
        {
            _laserController.ActivateLaser(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") || collider.CompareTag("Box")) // Check for both Player and Box tags
        {
            _laserController.ActivateLaser(false);
        }
    }
}

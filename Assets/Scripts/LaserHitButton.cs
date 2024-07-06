using UnityEngine;

public class LaserHitButton : MonoBehaviour
{
    [SerializeField] private ReflectiveBox reflectiveBox; // Reference to the reflective box
    [SerializeField] private float moveDistance = 1.0f;   // Distance to move the reflective box

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            reflectiveBox.MoveLeft(moveDistance);
        }
    }
}

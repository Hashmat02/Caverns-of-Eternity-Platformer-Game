using UnityEngine;

public class ReflectiveBox : MonoBehaviour
{
    public Vector2 Reflect(Vector2 direction)
    {
        // Reflect 90 degrees
        if (direction == Vector2.right)
            return Vector2.up;
        if (direction == Vector2.up)
            return Vector2.left;
        if (direction == Vector2.left)
            return Vector2.down;
        if (direction == Vector2.down)
            return Vector2.right;

        return direction;
    }
}

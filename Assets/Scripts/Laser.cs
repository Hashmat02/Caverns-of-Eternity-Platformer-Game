using UnityEngine;
using System.Collections;

public class Laser_Tutorial : MonoBehaviour
{
    [SerializeField] private float defDistanceRay = 100;
    public Transform laserFirePoint;
    public LineRenderer m_lineRenderer;
    public float rotationSpeed = 30f; // Degrees per second

    private Transform m_transform;
    private Vector2 currentDirection = Vector2.right; // Default direction
    private Vector2 targetDirection = Vector2.right;
    private bool isRotating = false;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
    }

    private void Update()
    {
        HandleInput();
        ShootLaser();
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            targetDirection = Vector2.up;
            isRotating = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            targetDirection = Vector2.left;
            isRotating = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            targetDirection = Vector2.down;
            isRotating = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            targetDirection = Vector2.right;
            isRotating = true;
        }
        else
        {
            isRotating = false;
        }

        if (isRotating)
        {
            StopAllCoroutines();
            StartCoroutine(RotateToDirection(targetDirection));
        }
    }

    private IEnumerator RotateToDirection(Vector2 newDirection)
    {
        while (isRotating)
        {
            Vector2 currentDirection = m_transform.right;
            float angle = Vector2.SignedAngle(currentDirection, newDirection);
            float step = rotationSpeed * Time.deltaTime * Mathf.Sign(angle);

            if (Mathf.Abs(step) > Mathf.Abs(angle))
            {
                step = angle; // Snap to the final angle
            }

            m_transform.Rotate(Vector3.forward, step);

            yield return null;
        }
    }

    private void ShootLaser()
    {
        Vector2 laserDirection = m_transform.right;
        RaycastHit2D _hit = Physics2D.Raycast(laserFirePoint.position, laserDirection, defDistanceRay);
        if (_hit)
        {
            Draw2DRay(laserFirePoint.position, _hit.point);
        }
        else
        {
            Draw2DRay(laserFirePoint.position, (Vector2)laserFirePoint.position + laserDirection * defDistanceRay);
        }
    }

    private void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);
    }
}

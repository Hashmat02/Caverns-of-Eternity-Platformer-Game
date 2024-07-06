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
    private bool isActive = false; // Laser activation state

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
        if (m_lineRenderer == null)
        {
            m_lineRenderer = GetComponent<LineRenderer>();
            if (m_lineRenderer == null)
            {
                Debug.LogError("LineRenderer component is missing.");
                return;
            }
        }
        m_lineRenderer.enabled = false; // Ensure laser is off initially
    }

    private void Update()
    {
        if (!isActive) return; // Only handle input and shoot if laser is active

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
        Vector2 startPosition = laserFirePoint.position;
        DrawLaser(startPosition, laserDirection, defDistanceRay);
    }

    private void DrawLaser(Vector2 startPosition, Vector2 direction, float distance)
    {
        RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, distance);
        if (hit)
        {
            if (hit.collider.CompareTag("Reflective"))
            {
                ReflectiveBox reflectiveBox = hit.collider.GetComponent<ReflectiveBox>();
                if (reflectiveBox != null)
                {
                    Vector2 newDirection = reflectiveBox.Reflect(direction);
                    Vector2 newStartPosition = hit.point + newDirection * 0.1f; // Slight offset to avoid hitting the same point

                    RaycastHit2D secondHit = Physics2D.Raycast(newStartPosition, newDirection, distance);
                    if (secondHit)
                    {
                        Draw2DRay(newStartPosition, secondHit.point);
                    }
                    else
                    {
                        Draw2DRay(newStartPosition, newStartPosition + newDirection * distance);
                    }
                }
            }
            else
            {
                Draw2DRay(startPosition, hit.point);
            }

            // Tag the laser for detection
            GameObject hitObject = hit.collider.gameObject;
            if (!hitObject.CompareTag("Laser"))
            {
                hitObject.tag = "Laser";
            }
        }
        else
        {
            Draw2DRay(startPosition, startPosition + direction * distance);
        }
    }

    private void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        if (m_lineRenderer != null)
        {
            m_lineRenderer.positionCount = 2;
            m_lineRenderer.SetPosition(0, startPos);
            m_lineRenderer.SetPosition(1, endPos);
        }
    }

    public void ActivateLaser(bool activate)
    {
        isActive = activate;
        if (m_lineRenderer != null)
        {
            m_lineRenderer.enabled = activate;
            Debug.Log("Laser activated: " + activate);
        }
        else
        {
            Debug.LogError("LineRenderer is missing.");
        }
    }
}
            // Debug.LogError("LineRenderer is missing.");

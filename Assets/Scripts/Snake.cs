using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segments;
    public Transform segmentPrefab;
    public float initialSpeed = 5f;
    private float _currentSpeed;
    private float _speedIncreaseFactor = 1.01f; // 0.5% increase = 100% - 0.5% = 99.5% = 0.995
    private int _frame = 0;
    //
    private GameManager gameManager;
    private bool canMove = true;

    private void Start()
    {
        _segments = new List<Transform>();
        _segments.Add(this.transform);
        _currentSpeed = initialSpeed;
        //
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        //
        if (!canMove || gameManager.IsGameOver()) return;

        if (Input.GetKeyDown(KeyCode.UpArrow) && _direction != Vector2.down)
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && _direction != Vector2.up)
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && _direction != Vector2.right)
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && _direction != Vector2.left)
        {
            _direction = Vector2.right;
        }
    }

    private void FixedUpdate()
    {
        //
        if (!canMove || gameManager.IsGameOver()) return;

        _frame++;
        if (_frame >= Mathf.RoundToInt(initialSpeed / _currentSpeed * initialSpeed))
        {
            _frame = 0;
            Move();
        }
    }

    public void StopMovement()
    {
        canMove = false;
    }

    private void Move()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }
        this.transform.position = new Vector3
        (
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
        );
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position - (Vector3)_direction;
        _segments.Add(segment);
        IncreaseSpeed();
    }

    private void IncreaseSpeed()
    {
        _currentSpeed *= _speedIncreaseFactor;
        Debug.Log($"Speed increased! Current speed: {_currentSpeed}");
    }

    /*private void ResetState()
    {
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        _segments.Clear();
        _segments.Add(this.transform);
        this.transform.position = Vector3.zero;
        _direction = Vector2.right;
        _currentSpeed = initialSpeed;
        _frame = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Debug.Log("Collided with food");
            Grow();
        }
        else if (other.CompareTag("Obstacle"))
        {
            ResetState();
        }
    }*/

    public void ResetState()
    {
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        _segments.Clear();
        _segments.Add(this.transform);
        this.transform.position = Vector3.zero;
        _direction = Vector2.right;
        _currentSpeed = initialSpeed;
        _frame = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameManager.IsGameOver()) return;

        if (other.CompareTag("Food"))
        {
            Debug.Log("Collided with food");
            Grow();
        }
        else if (other.CompareTag("Obstacle"))
        {
            gameManager.LoseLife();
        }
    }
}


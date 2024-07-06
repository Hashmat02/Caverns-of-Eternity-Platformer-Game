using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSnake : MonoBehaviour
{
    public float lifetime = 1f;
    private float timer;
    private bool isCollected = false;

    private void OnEnable()
    {
        timer = lifetime;
        isCollected = false;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            GameManager.Instance.CollectCrystal();
            gameObject.SetActive(false);
        }
    }
}

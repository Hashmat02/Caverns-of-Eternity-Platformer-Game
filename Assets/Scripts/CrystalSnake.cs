using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSnake : MonoBehaviour
{
    public float lifetime = 10f; // How long the crystal stays on screen
    private float timer;

    private void OnEnable()
    {
        timer = lifetime;
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
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CollectCrystal();
            gameObject.SetActive(false);
        }
    }
}

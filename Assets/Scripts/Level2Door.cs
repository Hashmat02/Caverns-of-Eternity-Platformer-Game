using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Level2Door : MonoBehaviour
{
    private const int RequiredCrystals = 11;
    private int collectedCrystals = 0;
    private bool isTransitioning = false;

    [SerializeField]
    private string nextSceneName = "MazeGame";

    void Start()
    {
        Debug.Log("DoorController started. Required crystals: " + RequiredCrystals);
        UpdateCrystalCount();
    }

    void Update()
    {
        if (!isTransitioning)
        {
            UpdateCrystalCount();
        }
    }

    void UpdateCrystalCount()
    {
        GameObject[] crystals = GameObject.FindGameObjectsWithTag("CollectibleCrystal");
        collectedCrystals = RequiredCrystals - crystals.Length;
        Debug.Log("Current crystal count: " + collectedCrystals + "/" + RequiredCrystals);

        if (collectedCrystals >= RequiredCrystals && !isTransitioning)
        {
            StartCoroutine(TransitionToNextScene());
        }
    }

    IEnumerator TransitionToNextScene()
    {
        isTransitioning = true;
        Debug.Log("All crystals collected! Destroying door and preparing to load next scene.");

        Destroy(gameObject);

        Debug.Log("Waiting for 2 seconds...");
        yield return new WaitForSeconds(2f);

        Debug.Log("2 seconds have passed. Proceeding to load next scene.");
        LoadNextScene();
    }

    void LoadNextScene()
    {
        Debug.Log("Attempting to load scene: " + nextSceneName);

        if (Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            Debug.Log(nextSceneName + " scene found. Loading...");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Scene '" + nextSceneName + "' not found in build settings. Please check the scene name and build settings.");
        }
    }

    void OnDestroy()
    {
        if (isTransitioning)
        {
            Debug.Log("DoorController being destroyed. Ensuring scene transition...");
            LoadNextScene();
        }
    }
}
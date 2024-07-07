using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrystalsManager : MonoBehaviour {
    public static CrystalsManager instance;
    public static int crystalCount { get; private set; } = 0;
    public int totalCrystalsInLevel { get; private set; } = 0;
	private int _localCrystalCount;

    public delegate void OnCountChange();
    public static event OnCountChange onCountChange;

    void Awake() {
		if (instance && instance != this) {
		Destroy(this);
			return;
		}
		instance = this;
		loadCrystals();
    }

    void Start() 
	{
        CountTotalCrystalsInLevel();
		SceneManager.sceneLoaded += (scene, mode) => _localCrystalCount = 0;
    }

    public void add(int i) {
        crystalCount += i;
		_localCrystalCount += i;
        onCountChange?.Invoke();
        CheckForAchievements();
    }

	public void subtract(int i) {
		crystalCount -= i;
		_localCrystalCount -= i;
		onCountChange?.Invoke();
	}

	public void multiply(int i) {
		crystalCount *= i;
		_localCrystalCount *= i;
		onCountChange?.Invoke();
	}

	public void divide(int i) {
		crystalCount /= i;
		_localCrystalCount /= i;
		onCountChange?.Invoke();
	}

    public void loadCrystals() {
        crystalCount = DataPersistenceManager.loadInt(Constants.PREF_CRYSTALS);
        Debug.Log("Loaded Crystals: " + crystalCount);
    }

    public void saveCrystals() {
		if (Cheats.cheatsOn) {
			return;
		}
        DataPersistenceManager.save(Constants.PREF_CRYSTALS, crystalCount);
    }

    public void OnDisable() {
        saveCrystals();
    }

    void CountTotalCrystalsInLevel()
    {
        totalCrystalsInLevel = GameObject.FindGameObjectsWithTag(Constants.TAG_COLLECTIBLE_CRYSTAL).Length * Constants.CRYSTALS_VALUE_EACH;
        Debug.Log("Total Crystal Value in Level: " + totalCrystalsInLevel);
    }

    void CheckForAchievements()
    {
        if (crystalCount >= totalCrystalsInLevel)
        {
            AchievementManager.instance.SetAchievement("AllCrystalsCollected", true);
            Debug.Log("Achievement Unlocked: All Crystals Collected");
        }
    }
}


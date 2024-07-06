//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CrystalsManager : MonoBehaviour {
//    public static CrystalsManager instance;
//    public static int crystalCount { get; private set; } = 0;
//	public delegate void OnCountChange();
//	public static event OnCountChange onCountChange;

//    void Awake() {
//        if (instance && instance != this) {
//            Destroy(this);
//            return;
//        }
//        instance = this;
//		loadCrystals();
//    }

//	public void add(int i) {
//		crystalCount += i;
//		onCountChange?.Invoke();
//	}

//	public void subtract(int i) {
//		crystalCount -= i;
//		onCountChange?.Invoke();
//	}

//	public void multiply(int i) {
//		crystalCount *= i;
//		onCountChange?.Invoke();
//	}

//	public void divide(int i) {
//		crystalCount /= i;
//		onCountChange?.Invoke();
//	}

//	void loadCrystals() {
//		crystalCount = DataPersistenceManager.loadInt(Constants.PREF_CRYSTALS);
//	}

//    void saveCrystals() {
//        DataPersistenceManager.save(Constants.PREF_CRYSTALS, crystalCount);
//    }

//	void OnDisable() {
//		saveCrystals();
//	}
//}


using UnityEngine;

public class CrystalsManager : MonoBehaviour
{
    public static CrystalsManager instance;
    public static int crystalCount { get; private set; } = 0;
    public delegate void OnCountChange();
    public static event OnCountChange onCountChange;

    void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        loadCrystals();
    }

    public void add(int i)
    {
        crystalCount += i;
        onCountChange?.Invoke();
    }

    public void subtract(int i)
    {
        crystalCount -= i;
        onCountChange?.Invoke();
    }

    public void multiply(int i)
    {
        crystalCount *= i;
        onCountChange?.Invoke();
    }

    public void divide(int i)
    {
        crystalCount /= i;
        onCountChange?.Invoke();
    }

    public void loadCrystals()
    {
        crystalCount = DataPersistenceManager.loadInt(Constants.PREF_CRYSTALS);
        Debug.Log("Loaded Crystals: " + crystalCount); // Add debug log to ensure it's being loaded correctly
    }



    public void saveCrystals()
    {
        DataPersistenceManager.save(Constants.PREF_CRYSTALS, crystalCount);
    }

    public void OnDisable()
    {
        saveCrystals();
    }
}

using UnityEngine;

public class DataPersistenceManager {
	static public void save(string key, int data) {
		PlayerPrefs.SetInt(key, data);
	}

	static public void save(string key, float data) {
		PlayerPrefs.SetFloat(key, data);
	}

	static public void save(string key, string data) {
		PlayerPrefs.SetString(key, data);
	}

	static public int loadInt(string key) {
		if (!PlayerPrefs.HasKey(key)) {
			return -1;
		}
		return PlayerPrefs.GetInt(key);
	}

	static public float loadFloat(string key) {
		if (!PlayerPrefs.HasKey(key)) {
			return -1f;
		}
		return PlayerPrefs.GetFloat(key);
	}

	static public string loadString(string key) {
		if (!PlayerPrefs.HasKey(key)) {
			return "";
		}
		return PlayerPrefs.GetString(key);
	}

	static public void erase(string key) {
		if (!PlayerPrefs.HasKey(key)) {
			return;
		}
		PlayerPrefs.DeleteKey(key);
	}

	static public void eraseAll() {
		PlayerPrefs.DeleteAll();
	}
}

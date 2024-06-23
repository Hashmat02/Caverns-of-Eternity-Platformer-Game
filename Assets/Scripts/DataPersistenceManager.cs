using UnityEngine;

public class DataPersistenceManager {
	static public void save(string key, int data) {
		PlayerPrefs.SetInt(key, data);
	}

	static public void save(string key, string data) {
		PlayerPrefs.SetString(key, data);
	}

	static public int loadInt(string key) {
		return PlayerPrefs.GetInt(key);
	}

	static public string loadString(string key) {
		return PlayerPrefs.GetString(key);
	}

	static public void erase(string key) {
		PlayerPrefs.DeleteKey(key);
	}

	static public void eraseAll() {
		PlayerPrefs.DeleteAll();
	}
}
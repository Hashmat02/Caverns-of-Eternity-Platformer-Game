using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupsManager : MonoBehaviour {
	public enum Powerups {
		SPEED,
		SHIELD,
		REBIRTH,
		JUMP,
		RIDDLE
	}
	public static Dictionary<Powerups, int> powerupsCount;
	public static Dictionary<Powerups, KeyCode> keybinds;
	public static Dictionary<KeyCode, Powerups> keybindsInverse;

	void Awake() {
		keybinds = new Dictionary<Powerups, KeyCode>();

		keybinds[Powerups.SPEED] = KeyCode.Alpha1;
		keybinds[Powerups.SHIELD] = KeyCode.Alpha2;
		keybinds[Powerups.JUMP] = KeyCode.Alpha3;
		keybinds[Powerups.REBIRTH] = KeyCode.Alpha4;
		keybinds[Powerups.RIDDLE] = KeyCode.Alpha5;

		keybindsInverse = keybinds.ToDictionary(x => x.Value, x => x.Key);
	}

	void OnEnable() {
		powerupsCount = new Dictionary<Powerups, int>();

		powerupsCount[Powerups.SPEED] = DataPersistenceManager.loadInt(Constants.PREF_POWERUPS_SPEED);
		powerupsCount[Powerups.SHIELD] = DataPersistenceManager.loadInt(Constants.PREF_POWERUPS_SHIELD);
		powerupsCount[Powerups.JUMP] = DataPersistenceManager.loadInt(Constants.PREF_POWERUPS_JUMP);
		powerupsCount[Powerups.REBIRTH] = DataPersistenceManager.loadInt(Constants.PREF_POWERUPS_REBIRTH);
		powerupsCount[Powerups.RIDDLE] = DataPersistenceManager.loadInt(Constants.PREF_POWERUPS_RIDDLE);

		// if key not present in PlayerPrefs, default to 0
		powerupsCount[Powerups.SPEED] = powerupsCount[Powerups.SPEED] == -1 ? 0 : powerupsCount[Powerups.SPEED];
		powerupsCount[Powerups.SHIELD] = powerupsCount[Powerups.SHIELD] == -1 ? 0 : powerupsCount[Powerups.SHIELD];
		powerupsCount[Powerups.JUMP] = powerupsCount[Powerups.JUMP] == -1 ? 0 : powerupsCount[Powerups.JUMP];
		powerupsCount[Powerups.REBIRTH] = powerupsCount[Powerups.REBIRTH] == -1 ? 0 : powerupsCount[Powerups.REBIRTH];
		powerupsCount[Powerups.RIDDLE] = powerupsCount[Powerups.RIDDLE] == -1 ? 0 : powerupsCount[Powerups.RIDDLE];
	}

	void saveData() {
		if (Cheats.cheatsOn) {
			return;
		}
		DataPersistenceManager.save(Constants.PREF_POWERUPS_SPEED, powerupsCount[Powerups.SPEED]);
		DataPersistenceManager.save(Constants.PREF_POWERUPS_SHIELD, powerupsCount[Powerups.SHIELD]);
		DataPersistenceManager.save(Constants.PREF_POWERUPS_JUMP, powerupsCount[Powerups.JUMP]);
		DataPersistenceManager.save(Constants.PREF_POWERUPS_REBIRTH, powerupsCount[Powerups.REBIRTH]);
		DataPersistenceManager.save(Constants.PREF_POWERUPS_RIDDLE, powerupsCount[Powerups.RIDDLE]);
	}

	void OnDisable() {
		saveData();
	}
}

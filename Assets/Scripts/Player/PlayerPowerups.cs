using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerups : MonoBehaviour {
	private enum Powerups {
		SPEED,
		SHIELD,
		REBIRTH,
		JUMP,
		RIDDLE
	}
	private Dictionary<Powerups, int> _powerupsCount;
	private Dictionary<Powerups, KeyCode> _keybinds;
	private Dictionary<KeyCode, Powerups> _keybindsInverse;
	private Dictionary<KeyCode, Action> _powerupFunctionCalls;
	private PlayerController _controller;

	void Awake() {
		_keybinds = new Dictionary<Powerups, KeyCode>();
		_powerupFunctionCalls = new Dictionary<KeyCode, Action>();

		_keybinds[Powerups.SPEED] = KeyCode.Alpha1;
		_keybinds[Powerups.SHIELD] = KeyCode.Alpha2;
		_keybinds[Powerups.JUMP] = KeyCode.Alpha3;
		_keybinds[Powerups.REBIRTH] = KeyCode.Alpha4;
		_keybinds[Powerups.RIDDLE] = KeyCode.Alpha5;

		_keybindsInverse = _keybinds.ToDictionary(x => x.Value, x => x.Key);

		_powerupFunctionCalls[_keybinds[Powerups.SPEED]] = () => StartCoroutine(speedBoost());
		_powerupFunctionCalls[_keybinds[Powerups.SHIELD]] = () => shield();
		_powerupFunctionCalls[_keybinds[Powerups.JUMP]] = () => doubleJump();
		_powerupFunctionCalls[_keybinds[Powerups.REBIRTH]] = () => crystalRebirth();
	}

	void OnEnable() {
		_powerupsCount = new Dictionary<Powerups, int>();

		_powerupsCount[Powerups.SPEED] = DataPersistenceManager.loadInt(Constants.PREF_POWERUPS_SPEED);
		_powerupsCount[Powerups.SHIELD] = DataPersistenceManager.loadInt(Constants.PREF_POWERUPS_SHIELD);
		_powerupsCount[Powerups.REBIRTH] = DataPersistenceManager.loadInt(Constants.PREF_POWERUPS_REBIRTH);
		_powerupsCount[Powerups.JUMP] = DataPersistenceManager.loadInt(Constants.PREF_POWERUPS_JUMP);
		_powerupsCount[Powerups.RIDDLE] = DataPersistenceManager.loadInt(Constants.PREF_POWERUPS_RIDDLE);

		// if key not present in PlayerPrefs, default to 0
		_powerupsCount[Powerups.SPEED] = _powerupsCount[Powerups.SPEED] == -1 ? 0 : _powerupsCount[Powerups.SPEED];
		_powerupsCount[Powerups.SHIELD] = _powerupsCount[Powerups.SHIELD] == -1 ? 0 : _powerupsCount[Powerups.SHIELD];
		_powerupsCount[Powerups.REBIRTH] = _powerupsCount[Powerups.REBIRTH] == -1 ? 0 : _powerupsCount[Powerups.REBIRTH];
		_powerupsCount[Powerups.JUMP] = _powerupsCount[Powerups.JUMP] == -1 ? 0 : _powerupsCount[Powerups.JUMP];
		_powerupsCount[Powerups.RIDDLE] = _powerupsCount[Powerups.RIDDLE] == -1 ? 0 : _powerupsCount[Powerups.RIDDLE];
	}

	void Start() {
		_controller = GetComponent<PlayerController>();
	}
	
	void Update() {
		if (!Input.anyKeyDown) {
			return;
		}
		string inString = Input.inputString;
		foreach (char c in inString) {
			checkPowerup(c);
		}
	}

	void checkPowerup(char c) {
		if (!CharToKeyCode.dict.ContainsKey(c)) {
			return;
		}
		KeyCode code = CharToKeyCode.dict[c];
		if (!_powerupFunctionCalls.ContainsKey(code)) {
			return;
		}
		if (_powerupsCount[_keybindsInverse[code]] > 0) {
			_powerupFunctionCalls[code].Invoke();
		}
	}

	IEnumerator speedBoost() {
		_controller.multMaxVel(1.5f);
		_powerupsCount[Powerups.SPEED]--;
		float timer = 0f;
		while (timer < Constants.POWERUPS_SPEED_DURATION) {
			timer += Time.deltaTime;
			yield return null;
		}
		_controller.revertMaxVel();
	}

	void shield() {
		Player.instance.triggerShield();
		_powerupsCount[Powerups.SHIELD]--;
	}

	void doubleJump() {
		_controller.addMaxJumps(1);
		_powerupsCount[Powerups.JUMP]--;
	}

	void crystalRebirth() {
		Player.instance.addLives(1);
		_powerupsCount[Powerups.REBIRTH]--;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerups : MonoBehaviour {
	private PlayerController _controller;
	private Dictionary<KeyCode, Action> _powerupFunctionCalls;

	void Awake() {
		_powerupFunctionCalls = new Dictionary<KeyCode, Action>();

		_powerupFunctionCalls[
			PowerupsManager.keybinds[PowerupsManager.Powerups.SPEED]
		] = () => StartCoroutine(speedBoost());
		_powerupFunctionCalls[
			PowerupsManager.keybinds[PowerupsManager.Powerups.SHIELD]
		] = () => shield();
		_powerupFunctionCalls[
			PowerupsManager.keybinds[PowerupsManager.Powerups.JUMP]
		] = () => doubleJump();
		_powerupFunctionCalls[
			PowerupsManager.keybinds[PowerupsManager.Powerups.REBIRTH]
		]= () => crystalRebirth();
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

		if (
			PowerupsManager.powerupsCount[PowerupsManager.keybindsInverse[code]] > 0 || 
			Cheats.cheats[Cheats.CheatTypes.POWER]
		) {
			_powerupFunctionCalls[code].Invoke();
		}
	}

	void decrementPowerupCount(PowerupsManager.Powerups powerup) {
		if (Cheats.cheats[Cheats.CheatTypes.POWER]) {
			return;
		}
		PowerupsManager.powerupsCount[powerup]--;
	}

	IEnumerator speedBoost() {
		_controller.multMaxVel(1.5f);
		decrementPowerupCount(PowerupsManager.Powerups.SPEED);
		float timer = 0f;
		while (timer < Constants.POWERUPS_SPEED_DURATION) {
			timer += Time.deltaTime;
			yield return null;
		}
		_controller.revertMaxVel();
	}

	void shield() {
		Player.instance.triggerShield();
		decrementPowerupCount(PowerupsManager.Powerups.SHIELD);
	}

	void doubleJump() {
		_controller.addMaxJumps(1);
		decrementPowerupCount(PowerupsManager.Powerups.JUMP);
	}

	void crystalRebirth() {
		Player.instance.addLives(1);
		decrementPowerupCount(PowerupsManager.Powerups.REBIRTH);
	}
}

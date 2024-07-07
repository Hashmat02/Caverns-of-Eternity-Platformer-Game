using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour {
	public static Cheats instance;
	public static bool cheatsOn { get; private set; } = false;
	public enum CheatTypes {
		CHECKPOINT,
		LEVELNEXT,
		LEVELPREV,
		GODMODE,
		MONEY,
		DASH,
		POWER
	}
	public static Dictionary<CheatTypes, bool> cheats { get; private set; } = new Dictionary<CheatTypes, bool>();
	private Dictionary<CheatTypes, KeyCode> _keybinds = new Dictionary<CheatTypes, KeyCode>();
	private Dictionary<KeyCode, Action> _cheatFuncCalls = new Dictionary<KeyCode, Action>();
	
	void Awake() {
		if (instance && instance != this) {
			Destroy(this);
			return;
		}
		instance = this;
		
		cheatsOn = Helpers.GetArgs("-cheats") == "1";
#if UNITY_EDITOR
		cheatsOn = true;
#endif
		if (!cheatsOn) {
			Destroy(this);
			return;
		}

		cheats[CheatTypes.CHECKPOINT] = true;
		cheats[CheatTypes.GODMODE] = false;
		cheats[CheatTypes.DASH] = false;
		cheats[CheatTypes.POWER] = false;

		_keybinds[CheatTypes.CHECKPOINT] = KeyCode.Exclaim;
		_keybinds[CheatTypes.LEVELNEXT] = KeyCode.Plus;
		_keybinds[CheatTypes.LEVELPREV] = KeyCode.Minus;
		_keybinds[CheatTypes.GODMODE] = KeyCode.At;
		_keybinds[CheatTypes.MONEY] = KeyCode.Dollar;
		_keybinds[CheatTypes.POWER] = KeyCode.Hash;

		_cheatFuncCalls[_keybinds[CheatTypes.CHECKPOINT]] = checkpoint;
		_cheatFuncCalls[_keybinds[CheatTypes.LEVELNEXT]] = () => navigateLevels(true);
		_cheatFuncCalls[_keybinds[CheatTypes.LEVELPREV]] = () => navigateLevels(false);
		_cheatFuncCalls[_keybinds[CheatTypes.GODMODE]] = godMode;
		_cheatFuncCalls[_keybinds[CheatTypes.MONEY]] = unlimitedMoney;
		_cheatFuncCalls[_keybinds[CheatTypes.POWER]] = unlimitedPower;
	}

	void toggleCheatActive(CheatTypes type) {
		cheats[type] = !cheats[type];
	}

	void Update() {
		if (!Input.anyKeyDown) {
			return;
		}

		string inString = Input.inputString;
		foreach (char c in inString) {
			callCheat(c);
		}
	}

	void callCheat(char c) {
		if (!CharToKeyCode.dict.ContainsKey(c)) {
			return;
		}
		KeyCode code = CharToKeyCode.dict[c];
		if (!_cheatFuncCalls.ContainsKey(code)) {
			return;
		}
		_cheatFuncCalls[code].Invoke();
	}

	void checkpoint() {
		Debug.Log("Cheat used! Setting checkpoint.");
		Player.instance.setCheckpoint();
	}

	// if next is true, then move to next, if false, move to previous
	// hardcoded at the moment due to hardcoded integration in scene handler (idek)
	void navigateLevels(bool next) {
		if (next) {
			Debug.Log("Cheat used! Going to next level.");
			SceneHandler.GoToNextLevel();
			return;
		}
		Debug.Log("Cheat used! Going to previous level.");
		SceneHandler.GoToGameplay();
	}

	void godMode() {
		cheats[CheatTypes.GODMODE] = !cheats[CheatTypes.GODMODE];
		Debug.Log($"Cheat used! godMode is now {cheats[CheatTypes.GODMODE]}.");
		Player.instance.setInvincibility(cheats[CheatTypes.GODMODE]);
	}

	void unlimitedMoney() {
		Debug.Log("Cheat used! Adding a whole lotta money!");
		CrystalsManager.instance.add(9999);
	}

	void unlimitedPower() {
		Debug.Log("Cheat used! Use all the powers you want!");
		cheats[CheatTypes.POWER] = !cheats[CheatTypes.POWER];
	}
}

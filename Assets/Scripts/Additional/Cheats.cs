using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour {
	public static Cheats instance;
	public static bool cheatsOn { get; private set; } = false;
	public enum CheatTypes {
		CHECKPOINT,
		LEVELNAVIGATION,
		GODMODE,
		MONEY,
		DASH,
		POWER
	}
	public Dictionary<CheatTypes, bool> cheats { get; private set; } = new Dictionary<CheatTypes, bool>();
	
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
			return;
		}

		cheats[CheatTypes.CHECKPOINT] = false;
		cheats[CheatTypes.LEVELNAVIGATION] = false;
		cheats[CheatTypes.GODMODE] = false;
		cheats[CheatTypes.DASH] = false;
		cheats[CheatTypes.POWER] = false;
	}

	void Start() {
		if (!cheatsOn) {
			return;
		}
	}

	void checkpoint() {
		Player.instance.setCheckpoint();
	}

	// if next is true, then move to next, if false, move to previous
	// hardcoded at the moment due to hardcoded integration in scene handler (idek)
	void navigateLevels(bool next) {
		if (next) {
			SceneHandler.GoToNextLevel();
			return;
		}
		SceneHandler.GoToGameplay();
	}

	void godMode() {
		cheats[CheatTypes.GODMODE] = !cheats[CheatTypes.GODMODE];
		Player.instance.setInvincibility(cheats[CheatTypes.GODMODE]);
	}

	void unlimitedMoney() {
		CrystalsManager.instance.add(99999999);
	}

	void unlimitedPower() {
		cheats[CheatTypes.POWER] = !cheats[CheatTypes.POWER];
	}
}

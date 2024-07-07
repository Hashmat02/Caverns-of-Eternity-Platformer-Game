using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats {
	public static bool isCheats { get; private set; } = false;

	void Awake() {
		isCheats = Helpers.GetArg("-cheats") == "1";
#if UNITY_EDITOR
		isCheats = true;
#endif
	}
}

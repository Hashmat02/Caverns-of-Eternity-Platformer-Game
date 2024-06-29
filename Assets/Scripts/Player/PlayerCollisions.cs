using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag(Constants.TAG_TRAP_KILLER)) {
			UIManager.instance.GameOver();
		}
	}
}

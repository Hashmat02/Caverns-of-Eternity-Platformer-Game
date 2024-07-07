using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour {
	public delegate void OnCollision();
	public OnCollision onCollectible;
	void OnTriggerEnter2D(Collider2D collider) {
		switch (collider.tag) {
		case Constants.TAG_TRAP_KILLER:
			if (!Player.instance.isInvincible) {
				Player.instance.onKillHit?.Invoke();
			}
			break;
		case Constants.TAG_COLLECTIBLE_CRYSTAL:
			onCollectible?.Invoke();
			break;
		}
	}
}

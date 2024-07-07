using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // Player singleton
    public static Player instance;
	public int lives { get; private set; } = 1;
	public bool isInvincible { get; private set; } = false;
	public bool isShielded { get; private set; } = false;
	private Vector3 _savedPosition;
	private float _timer = 0f;
	private PlayerController _controller;

	public delegate void TriggerKillHit();
	public TriggerKillHit onKillHit;

    void Awake() {
        if (instance && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

	void Start() {
		_savedPosition = transform.position;
		_controller = GetComponent<PlayerController>();
		onKillHit = triggerDeath;
	}

	void Update() {
		onKillHit = isShielded ? shieldBreak :  triggerDeath;
	}

	void FixedUpdate() {
		_timer += Time.deltaTime;
		if (_timer < Constants.PLAYER_POSITION_SAVE_TIMER) {
			return;
		}
		_timer = 0f;
		_savedPosition = transform.position;
	}

	public void triggerDeath() {
		lives--;
		if (lives <= 0) {
			UIManager.instance.GameOver();
			return;
		}
		StartCoroutine(triggerInvincibility(Constants.PLAYER_INVINCIBILITY_TIME));
		revertToSavedPos();
	}

	private IEnumerator triggerInvincibility(float duration) {
		isInvincible = true;
		float timer = 0f;
		while (timer < duration) {
			timer += Time.deltaTime;
			yield return null;
		}
		isInvincible = false;
	}

	private void revertToSavedPos() {
		transform.position = _savedPosition;
	}

	public void addLives(int count) {
		lives += count;
	}

	public void triggerShield() {
		isShielded = true;
	}

	public void shieldBreak() {
		isShielded = false;
		_controller.forceJump = true;
	}
}

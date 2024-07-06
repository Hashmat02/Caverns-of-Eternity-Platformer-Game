using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {
	public static AudioManager instance;
	[NonSerialized] public bool muted = false;

	[SerializeField] private AudioMixer _mixer;

	[System.Serializable]
	public struct SoundSource {
		public AudioSource source;
		public AudioClip[] clips;
	}

	[Header("BG Audio")]
	[SerializeField] private SoundSource _bg;

	[Header("SFX")]
	[SerializeField] private SoundSource _playerSfx;
	[SerializeField] private SoundSource _uiSfx;
	[SerializeField] private SoundSource _collisionSfx;


	public enum Volumes {
		MASTER,
		MUSIC,
		SFX
	}

	public float[] volumes { get; private set; } = new float[Constants.MIXER_GROUP_COUNT];

	void Awake() {
		if (instance && instance != this) {
			Destroy(gameObject);
			return;
		}
		instance = this;
		checkForErrors();

    	volumes[(int)Volumes.MASTER] = DataPersistenceManager.loadFloat(Constants.MIXER_MASTER);
    	volumes[(int)Volumes.MUSIC] = DataPersistenceManager.loadFloat(Constants.MIXER_MUSIC);
    	volumes[(int)Volumes.SFX] = DataPersistenceManager.loadFloat(Constants.MIXER_SFX);

		for (int i = 0; i < volumes.Length; i++) {
			if (volumes[i] == -1f) {
				volumes[i] = 100.0f;
			}
		}
		muted = DataPersistenceManager.loadInt(Constants.MIXER_MUTED_ALL) != 0;
	}

	void checkForErrors() {
		if (!_mixer) {
			ErrorHandling.throwError("No Audio Mixer found.");
		}
		if (!_bg.source) {
			ErrorHandling.throwError("No Background Audio Source found.");
		}
		if (!_playerSfx.source) {
			ErrorHandling.throwError("No Player SFX Source found.");
		}
		if (!_collisionSfx.source) {
			ErrorHandling.throwError("No Collision SFX Source found.");
		}
		if (!_uiSfx.source) {
			ErrorHandling.throwError("No UI SFX Source found.");
		}
		if (_bg.clips.Length == 0) {
			ErrorHandling.throwError("No Background Audio Clips found.");
		}
	}
	
	void OnEnable() {
		SceneManager.sceneLoaded += updateBgAudio;
		SceneManager.sceneLoaded += addButtonSFX;
		SceneManager.sceneLoaded += addPlayerSFX;
		UIManager.onGameOver += playGameOverAudio;
	}

	void updateBgAudio(Scene scene, LoadSceneMode mode) {
		_bg.source?.Stop();
		switch (scene.name) {
		case Constants.SCENE_MAIN_MENU:
			_bg.source.clip = _bg.clips[0];
			break;
		case Constants.SCENE_GAMEPLAY:
			_bg.source.clip = _bg.clips[1];
			break;
		default:
			break;
		}
		_bg.source?.Play();
	}

	void oneShot(AudioSource source, AudioClip clip) {
		source?.PlayOneShot(clip);
	}

	void addPlayerSFX(Scene scene, LoadSceneMode mode) {
		if (!Player.instance) {
			return;
		}

		PlayerController controller = Player.instance.gameObject.GetComponent<PlayerController>();
		controller.playJump += () => oneShot(_playerSfx.source, _playerSfx.clips[0]);
		controller.landed += () => oneShot(_playerSfx.source, _playerSfx.clips[1]);

		PlayerCollisions collisions = Player.instance.gameObject.GetComponent<PlayerCollisions>();
		collisions.onCollectible += () => oneShot(_collisionSfx.source, _collisionSfx.clips[0]);
	}

	void addButtonSFX(Scene scene, LoadSceneMode mode) {
		Button[] buttons = FindObjectsOfType<Button>(true);
		foreach (var button in buttons) {
			button.onClick.AddListener(() => oneShot(_uiSfx.source, _uiSfx.clips[0]));
		}
	}

	void playGameOverAudio() {
		_bg.source?.Stop();
		_bg.source.clip = _bg.clips[2];
		_bg.source?.Play();
	}

    void Start() {
		setMixerVolume(Constants.MIXER_MUSIC, volumes[(int)Volumes.MUSIC]);
		setMixerVolume(Constants.MIXER_SFX, volumes[(int)Volumes.SFX]);
		if (muted) {
			setMixerVolume(Constants.MIXER_MASTER, 0.0f);
			return;
		}
		setMixerVolume(Constants.MIXER_MASTER, volumes[(int)Volumes.MASTER]);
    }

	void setMixerVolume(string group, float value) {
		_mixer.SetFloat(group, value != 0 ? Mathf.Log10(value / 100) * 20 : -80.0f);
	}

	public void muteToggleAll(bool mute) {
		muted = mute;
		if (muted) {
			setMixerVolume(Constants.MIXER_MASTER, 0.0f);
			return;
		}
		setMixerVolume(Constants.MIXER_MASTER, volumes[(int)Volumes.MASTER]);
		setMixerVolume(Constants.MIXER_MUSIC, volumes[(int)Volumes.MUSIC]);
		setMixerVolume(Constants.MIXER_SFX, volumes[(int)Volumes.SFX]);
	}

	public void updateVolume(Volumes group, float value) {
		volumes[(int)group] = value;
		if (muted) {
			return;
		}

		switch (group) {
		case Volumes.MASTER:
			setMixerVolume(Constants.MIXER_MASTER, volumes[(int)Volumes.MASTER]);
			break;
		case Volumes.MUSIC:
			setMixerVolume(Constants.MIXER_MUSIC, volumes[(int)Volumes.MUSIC]);
			break;
		case Volumes.SFX:
			setMixerVolume(Constants.MIXER_SFX, volumes[(int)Volumes.SFX]);
			break;
		default:
			break;
		}
	}

	void OnDisable() {
		DataPersistenceManager.save(Constants.MIXER_MASTER, volumes[(int)Volumes.MASTER]);
		DataPersistenceManager.save(Constants.MIXER_MUSIC, volumes[(int)Volumes.MUSIC]);
		DataPersistenceManager.save(Constants.MIXER_SFX, volumes[(int)Volumes.SFX]);
		DataPersistenceManager.save(Constants.MIXER_MUTED_ALL, muted ? 1 : 0);

		SceneManager.sceneLoaded -= updateBgAudio;
		SceneManager.sceneLoaded -= addButtonSFX;
		UIManager.onGameOver -= playGameOverAudio;
	}
}

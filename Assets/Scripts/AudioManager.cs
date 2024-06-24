using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {
	public static AudioManager instance;
	private bool _muted = false;

	[SerializeField] private AudioMixer _mixer;

	[Header("BG Audio")]
	[SerializeField] private AudioSource _bgSource;
	[SerializeField] private AudioClip[] _bgAudioClips;

	public enum Volumes {
		MASTER,
		MUSIC,
		SFX
	}

	public float[] volumes { get; private set; } = new float[Constants.MIXER_GROUP_COUNT];

	void Awake() {
		if (instance && instance != this) {
			Destroy(this);
			return;
		}
		instance = this;
		volumes = Helpers.populateArray(volumes, 100.0f);
	}
	
	void OnEnable() {
		SceneManager.sceneLoaded += updateBgAudio;
	}

	void updateBgAudio(Scene scene, LoadSceneMode mode) {
		_bgSource.Stop();
		switch (scene.name) {
		case Constants.SCENE_MAIN_MENU:
			_bgSource.clip = _bgAudioClips[0];
			break;
		case Constants.SCENE_GAMEPLAY:
			_bgSource.clip = _bgAudioClips[1];
			break;
		default:
			break;
		}
		_bgSource.Play();
	}

    void Start() {
    	volumes[(int)Volumes.MASTER] = DataPersistenceManager.loadFloat(Constants.MIXER_MASTER);
    	volumes[(int)Volumes.MUSIC] = DataPersistenceManager.loadFloat(Constants.MIXER_MUSIC);
    	volumes[(int)Volumes.SFX] = DataPersistenceManager.loadFloat(Constants.MIXER_SFX);
		_muted = DataPersistenceManager.loadInt(Constants.MIXER_MUTED_ALL) != 0;

		setMixerVolume(Constants.MIXER_MASTER, volumes[(int)Volumes.MASTER]);
		setMixerVolume(Constants.MIXER_MUSIC, volumes[(int)Volumes.MUSIC]);
		setMixerVolume(Constants.MIXER_SFX, volumes[(int)Volumes.SFX]);
    }

	void setMixerVolume(string group, float value) {
		_mixer.SetFloat(group, value != 0 ? Mathf.Log10(value / 100) * 20 : -80.0f);
	}

	public void muteToggleAll() {
		if (!_muted) {
			setMixerVolume(Constants.MIXER_MASTER, 0.0f);
			setMixerVolume(Constants.MIXER_MUSIC, 0.0f);
			setMixerVolume(Constants.MIXER_SFX, 0.0f);
			_muted = true;
			return;
		}
		setMixerVolume(Constants.MIXER_MASTER, volumes[(int)Volumes.MASTER]);
		setMixerVolume(Constants.MIXER_MUSIC, volumes[(int)Volumes.MUSIC]);
		setMixerVolume(Constants.MIXER_SFX, volumes[(int)Volumes.SFX]);
		_muted = false;
	}

	public void updateVolume(Volumes group, float value) {
		switch (group) {
		case Volumes.MASTER:
			volumes[(int)Volumes.MASTER] = value;
			setMixerVolume(Constants.MIXER_MASTER, volumes[(int)Volumes.MASTER]);
			break;
		case Volumes.MUSIC:
			volumes[(int)Volumes.MUSIC] = value;
			setMixerVolume(Constants.MIXER_MUSIC, volumes[(int)Volumes.MUSIC]);
			break;
		case Volumes.SFX:
			volumes[(int)Volumes.SFX] = value;
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
		DataPersistenceManager.save(Constants.MIXER_MUTED_ALL, _muted ? 1 : 0);
		SceneManager.sceneLoaded -= updateBgAudio;
	}
}

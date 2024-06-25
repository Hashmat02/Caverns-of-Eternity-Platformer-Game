using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour {
	[System.Serializable]
	struct VolumeConfig {
		public Slider slider;
		public AudioManager.Volumes mixerGroup;
		public Text text;
		public string label;
	}
	[SerializeField] private Toggle _mute;
	[SerializeField] private VolumeConfig[] _volumes;

	void Awake() {
		foreach (var vol in _volumes) {
			vol.slider.onValueChanged.AddListener((value) => {
				vol.text.text = vol.label + value;
				AudioManager.instance.updateVolume(vol.mixerGroup, value);
			});
		}
	}

	void OnEnable() {
		foreach (var vol in _volumes) {
			vol.slider.value = AudioManager.instance.volumes[(int)vol.mixerGroup];
			updateVolume(vol, vol.slider.value);
		}
		_mute.isOn = AudioManager.instance.muted;
	}
	
	void updateVolume(VolumeConfig vol, float value) {
		vol.text.text = vol.label + value;
		AudioManager.instance.updateVolume(vol.mixerGroup, value);
	}

	void OnDisable() {
		foreach (var vol in _volumes) {
			vol.slider.onValueChanged.RemoveAllListeners();
		}
	}
}

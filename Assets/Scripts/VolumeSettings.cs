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
				updateVolume(vol, value);
			});
		}
	}

	void OnEnable() {
		_mute.SetIsOnWithoutNotify(AudioManager.instance.muted);
		foreach (var vol in _volumes) {
			vol.text.text = vol.label + vol.slider.value;
			vol.slider.SetValueWithoutNotify(AudioManager.instance.volumes[(int)vol.mixerGroup]);
		}
	}
	
	void updateVolume(VolumeConfig vol, float value) {
		vol.text.text = vol.label + value;
		AudioManager.instance.updateVolume(vol.mixerGroup, value);
	}

	void OnDestroy() {
		foreach (var vol in _volumes) {
			vol.slider.onValueChanged.RemoveAllListeners();
		}
	}
}

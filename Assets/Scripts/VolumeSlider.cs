using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour {
	private Slider _slider;
	[SerializeField] Text _text;
	[SerializeField] private string _label;
	[SerializeField] private AudioManager.Volumes _mixerGroup;

	void Awake() {
		_slider = GetComponent<Slider>();
		_slider.onValueChanged.AddListener(updateVolume);
	}

	void OnEnable() {
		_slider.value = AudioManager.instance.volumes[(int)_mixerGroup];
		updateVolume(_slider.value);
	}
	
	void updateVolume(float value) {
		_text.text = _label + value;
		AudioManager.instance.updateVolume(_mixerGroup, value);
	}

	void OnDisable() {
		_slider.onValueChanged.RemoveListener(updateVolume);
	}
}

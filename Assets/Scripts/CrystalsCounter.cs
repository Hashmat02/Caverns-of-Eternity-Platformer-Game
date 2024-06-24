using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalsCounter : MonoBehaviour {
    private Text _text;
    [SerializeField] private string _label = "Crystals: ";

	void OnEnable() {
		CrystalsManager.onCountChange += updateText;
	}

	void Start() {
		_text = GetComponent<Text>();
		updateText();
	}
        
    public void updateText() {
		_text.text = _label + CrystalsManager.crystalCount;
	}

	void OnDisable() {
		CrystalsManager.onCountChange -= updateText;
	}
}

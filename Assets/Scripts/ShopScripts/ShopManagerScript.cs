using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ShopManagerScript : MonoBehaviour {
    public static ShopManagerScript instance;
    public TextMeshProUGUI crystalsText;

    void Awake() {
		if (instance && instance != this) {
			Destroy(this);
			return;
		}
		instance = this;
    }

    void Start() {
        UpdateCrystalsText(); // Update crystals display
    }

    public void Buy(PowerupsManager.Powerups powerup, int cost) {
		if (cost > CrystalsManager.crystalCount) {
			Debug.Log("Can't purchase. Not enough crystals.");
			return;
		}
		CrystalsManager.instance.subtract(cost);
		PowerupsManager.powerupsCount[powerup]++;
		UpdateCrystalsText();
		Debug.Log("Bought!");
		string logString = "Current Powerups Count:";
		logString += $"\nSpeed Boost: {PowerupsManager.powerupsCount[PowerupsManager.Powerups.SPEED]}";
		logString += $"\nShield: {PowerupsManager.powerupsCount[PowerupsManager.Powerups.SHIELD]}";
		logString += $"\nCrystal Rebirth: {PowerupsManager.powerupsCount[PowerupsManager.Powerups.REBIRTH]}";
		logString += $"\nDouble Jump: {PowerupsManager.powerupsCount[PowerupsManager.Powerups.JUMP]}";
		logString += $"\nRiddle Guide: {PowerupsManager.powerupsCount[PowerupsManager.Powerups.RIDDLE]}";
		Debug.Log(logString);
    }

    void UpdateCrystalsText() {
        crystalsText.text = $"Crystals: {CrystalsManager.crystalCount}"; // Update crystals display
    }
}

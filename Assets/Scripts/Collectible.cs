using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Player")) {
            CrystalsManager.instance.add(Constants.CRYSTALS_VALUE_EACH);
            gameObject.SetActive(false);
        }
    }
}

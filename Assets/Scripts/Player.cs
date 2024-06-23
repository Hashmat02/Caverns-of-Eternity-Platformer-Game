using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // Player singleton
    public static Player instance;

    void Awake() {
        if (instance && instance != this) {
            Destroy(this);
            return;
        }
        instance = this;
    }
}

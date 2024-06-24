using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {
    void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(gameObject.tag);
        if (objs.Length > 1) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}

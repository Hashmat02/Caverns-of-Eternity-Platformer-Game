using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour {
    [SerializeField] private PowerupsManager.Powerups _itemID;
	[SerializeField] private int _cost;

	void Awake() {
		GetComponent<Button>().onClick.AddListener(() =>ShopManagerScript.instance.Buy(_itemID, _cost));
	}
}

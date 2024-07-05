using UnityEngine;
using TMPro;

public class InventoryItemButton : MonoBehaviour
{
    public int ItemID;
    public TextMeshProUGUI QuantityTxt;

    public void ConsumeItem()
    {
        if (InventoryManagerScript.Instance != null)
        {
            InventoryManagerScript.Instance.ConsumeItem(ItemID);
            // Update UI or perform other actions as needed
            Debug.Log("Consuming item with ID: " + ItemID);
        }
        else
        {
            Debug.LogError("InventoryManagerScript instance not found!");
        }
    }
}

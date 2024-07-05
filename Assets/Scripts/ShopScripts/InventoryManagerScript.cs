using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InventoryManagerScript : MonoBehaviour
{
    public static InventoryManagerScript Instance { get; private set; }

    private Dictionary<int, int> inventory = new Dictionary<int, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optionally, if you want the instance to persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadInventoryFromPlayerPrefs();

        // Example: Update UI or activate items based on loaded inventory
        Debug.Log("Inventory loaded:");
        foreach (var item in inventory)
        {
            Debug.Log("Item " + item.Key + ": " + item.Value);
        }
    }

    void LoadInventoryFromPlayerPrefs()
    {
        string inventoryJson = PlayerPrefs.GetString("Inventory", "{}");
        inventory = JsonUtility.FromJson<Dictionary<int, int>>(inventoryJson);
    }

    public void ConsumeItem(int itemID)
    {
        if (inventory.ContainsKey(itemID) && inventory[itemID] > 0)
        {
            inventory[itemID]--;
            SaveInventoryToPlayerPrefs(); // Save updated inventory to PlayerPrefs
            Debug.Log("Item " + itemID + " consumed. New quantity: " + inventory[itemID]);

            // Example: Perform item activation or other logic here
            Debug.Log("Item activated or used.");

            // Call any additional logic here if needed, e.g., activate item effects
        }
        else
        {
            Debug.Log("Item " + itemID + " is not available.");
        }
    }

    void SaveInventoryToPlayerPrefs()
    {
        string inventoryJson = JsonUtility.ToJson(inventory);
        PlayerPrefs.SetString("Inventory", inventoryJson);
        PlayerPrefs.Save();
        Debug.Log("Inventory saved to PlayerPrefs.");
    }
}

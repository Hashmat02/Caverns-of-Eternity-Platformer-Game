using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ShopManagerScript : MonoBehaviour
{
    public static ShopManagerScript Instance { get; private set; }

    public int[,] shopItems = new int[4, 6]; // [property, itemID]
    public float coins;
    public TextMeshProUGUI CoinsTXT;

    private Dictionary<int, int> inventory = new Dictionary<int, int>(); // Dictionary to store inventory

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CoinsTXT.text = "Coins: " + coins.ToString();

        // Initialize shop items
        InitializeShopItems();

        // Initialize inventory from Player Prefs
        LoadInventoryFromPlayerPrefs();
    }

    void InitializeShopItems()
    {
        // Item IDs
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;
        shopItems[1, 5] = 5;

        // Prices
        shopItems[2, 1] = 10;
        shopItems[2, 2] = 20;
        shopItems[2, 3] = 30;
        shopItems[2, 4] = 40;
        shopItems[2, 5] = 50;

        // Quantities (initialize to 0)
        for (int i = 1; i <= 5; i++)
        {
            shopItems[3, i] = 0;
        }
    }

    public void Buy()
    {
        GameObject ButtonRef = EventSystem.current.currentSelectedGameObject;

        int itemID = ButtonRef.GetComponent<ButtonInfo>().ItemID;
        shopItems[3, itemID]++;
        ButtonRef.GetComponent<ButtonInfo>().QuantityTxt.text = "Quantity: " + shopItems[3, itemID].ToString();
    }

    public void Checkout()
    {
        float totalCost = 0;
        for (int i = 1; i <= 5; i++)
        {
            totalCost += shopItems[2, i] * shopItems[3, i];
        }

        if (coins >= totalCost)
        {
            coins -= totalCost;
            CoinsTXT.text = "Coins: " + coins.ToString();

            // Update inventory
            for (int i = 1; i <= 5; i++)
            {
                if (shopItems[3, i] > 0)
                {
                    if (inventory.ContainsKey(i))
                    {
                        inventory[i] += shopItems[3, i]; // Update inventory
                    }
                    else
                    {
                        inventory[i] = shopItems[3, i]; // Initialize if not already in inventory
                    }

                    Debug.Log("Item " + i + " activated."); // Log activation (replace with actual activation logic)
                    shopItems[3, i] = 0; // Reset selected quantity to 0
                }
            }

            // Save inventory to Player Prefs
            SaveInventoryToPlayerPrefs();

            // Update UI
            foreach (ButtonInfo buttonInfo in FindObjectsOfType<ButtonInfo>())
            {
                buttonInfo.QuantityTxt.text = "Quantity: " + shopItems[3, buttonInfo.ItemID].ToString();
            }
        }
        else
        {
            Debug.Log("Not enough coins for checkout");
        }
    }

    void SaveInventoryToPlayerPrefs()
    {
        string inventoryJson = JsonUtility.ToJson(inventory);
        PlayerPrefs.SetString("Inventory", inventoryJson);
        PlayerPrefs.Save();
    }

    void LoadInventoryFromPlayerPrefs()
    {
        string inventoryJson = PlayerPrefs.GetString("Inventory", "{}");
        inventory = JsonUtility.FromJson<Dictionary<int, int>>(inventoryJson);
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ShopManagerScript : MonoBehaviour
{
    public static ShopManagerScript Instance { get; private set; }

    public int[,] shopItems = new int[4, 6]; // [property, itemID]
    public int coins; // Use int for coins to avoid float precision issues
    public TextMeshProUGUI CoinsTXT;

    private Dictionary<int, int> inventory = new Dictionary<int, int>(); // Dictionary to store inventory

    void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure this object persists across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        LoadData();
        CoinsTXT.text = "Coins: " + coins.ToString();

        // Initialize shop items
        InitializeShopItems();
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

        // Log the current quantities in the shop
        Debug.Log("After buying, current quantities in the shop:");
        for (int i = 1; i <= 5; i++)
        {
            Debug.Log("Item " + i + ": " + shopItems[3, i]);
        }
    }

    public void Checkout()
    {
        int totalCost = 0; // Use int for total cost
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

            // Save data to PlayerPrefs
            SaveData();

            // Update UI
            foreach (ButtonInfo buttonInfo in FindObjectsOfType<ButtonInfo>())
            {
                buttonInfo.QuantityTxt.text = "Quantity: " + shopItems[3, buttonInfo.ItemID].ToString();
            }

            // Log the current inventory
            Debug.Log("After checkout, current inventory:");
            for (int i = 1; i <= 5; i++)
            {
                if (inventory.ContainsKey(i))
                {
                    Debug.Log("Item " + i + ": " + inventory[i]);
                }
                else
                {
                    Debug.Log("Item " + i + ": 0");
                }
            }
        }
        else
        {
            Debug.Log("Not enough coins for checkout");
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Coins", coins);

        string inventoryJson = JsonConvert.SerializeObject(inventory);
        PlayerPrefs.SetString("Inventory", inventoryJson);

        PlayerPrefs.Save();
    }

    void LoadData()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        string inventoryJson = PlayerPrefs.GetString("Inventory", "{}");
        inventory = JsonConvert.DeserializeObject<Dictionary<int, int>>(inventoryJson);

        CoinsTXT.text = "Coins: " + coins.ToString();
    }

    void OnApplicationQuit()
    {
        SaveData();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveData();
        }
    }
}

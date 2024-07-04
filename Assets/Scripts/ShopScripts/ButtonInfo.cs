using UnityEngine;
using TMPro;

public class ButtonInfo : MonoBehaviour
{
    public int ItemID;
    public TextMeshProUGUI PriceTxt;
    public TextMeshProUGUI QuantityTxt;
    public ShopManagerScript ShopManager; // Change this line

    void Update()
    {
        if (ShopManager != null)
        {
            PriceTxt.text = "Price: " + ShopManager.shopItems[2, ItemID].ToString();
            QuantityTxt.text = "Quantity: " + ShopManager.shopItems[3, ItemID].ToString();
        }
    }
}
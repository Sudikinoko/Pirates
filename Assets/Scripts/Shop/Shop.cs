using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    [Header("Items Sold")]
    [SerializeField] private List<EquipmentData> shopItems;

    public GameObject shopItemPrefab;

    private EquipmentData selectedEquipmentData;

    public void PopulateShopUI()
    {
        ShopUI.instance.PopulateShopUI(shopItems, shopItemPrefab);
    }

    public void SelectItemToBuy(EquipmentData equipmentData)
    {
        Debug.Log("Item Set: " + equipmentData.name);
        this.selectedEquipmentData = equipmentData;
    }

    public bool BuySelectedItem()
    {
        if (selectedEquipmentData != null && PlayerStats.instance.GetCurrentMoneyAmount() >= selectedEquipmentData.cost)
        {
            if (PlayerStats.instance.RemoveMoney(selectedEquipmentData.cost))
            {
                InventoryManager.instance.AddEquipment(selectedEquipmentData);
                return true;
            }
        }
        return false;
    }


}

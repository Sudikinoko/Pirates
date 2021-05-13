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
                if (selectedEquipmentData.type == EquipmentType.Ship)
                {
                    BuyShip();
                    return true;
                }

                InventoryManager.instance.AddEquipment(selectedEquipmentData);
                return true;
            }
        }
        return false;
    }

    public void BuyShip()
    {
        Transform playerTransform = FindPlayerController();

        Node[] nodes = playerTransform.GetComponentsInChildren<Node>();

        foreach (Node node in nodes)
        {
            node.RemoveEquipment();
        }

        GameObject newShip = Instantiate(selectedEquipmentData.prefab, playerTransform.position, playerTransform.rotation);
        if (newShip.GetComponent<Ship>() != null)
        {
            newShip.GetComponent<Ship>().shipData = (ShipData)selectedEquipmentData;
        }

        DestroyImmediate(playerTransform.gameObject);

        CameraController.instance.InitiateShipParameter();
    }

    Transform FindPlayerController()
    {
        return GameObject.FindGameObjectWithTag("Player").transform;
    }

}

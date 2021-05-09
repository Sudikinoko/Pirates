using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    [Header("Items Sold")]
    [SerializeField] private List<EquipmentData> shopItems;

    public GameObject shopItemPrefab;


    void PopulateShopUI()
    {
        ShopUI.instance.PopulateShopUI(shopItems, shopItemPrefab);
    }



}

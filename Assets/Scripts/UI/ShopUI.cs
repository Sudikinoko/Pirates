using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;

    public Transform shopContainer;

    GameObject equipmentBlueprint;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void PopulateShopUI(List<EquipmentData> shopItems, GameObject shopItemprefab)
    {
        ClearShop();

        foreach (EquipmentData equipment in shopItems)
        {
            GameObject shopItem = Instantiate(shopItemprefab, shopContainer);

            shopItem.GetComponent<Image>().color = equipment.backgroundColor;
            shopContainer.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(equipment));

            shopItem.transform.GetChild(0).GetComponent<Image>().sprite = equipment.inventoryImage;
            shopItem.transform.GetChild(1).GetComponent<Image>().sprite = equipment.Border;
            shopItem.transform.GetChild(2).GetComponent<Text>().text = equipment.name;
            shopItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = equipment.name;

        }
    }

    private void OnButtonClick(EquipmentData equipment)
    {
        Debug.Log(equipment.name);

        //TODO Set Item as Selected
    }

    public void ClearShop()
    {
        foreach (Transform child in shopContainer)
        {
            Destroy(child);
        }
    }



}

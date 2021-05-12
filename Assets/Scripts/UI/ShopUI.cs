using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;

    public GameObject ui;

    public Transform shopContainerWeapons;
    public Transform shopContainerUtility;
    public Transform shopContainerShips;

    public Shop target;

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

    private void Update()
    {
        RenderUI();
    }

    public void PopulateShopUI(List<EquipmentData> shopItems, GameObject shopItemprefab)
    {
        ClearShop();

        foreach (EquipmentData equipment in shopItems)
        {
            switch (equipment.type)
            {
                case EquipmentType.Weapon:
                    GameObject weaponItem = Instantiate(shopItemprefab, shopContainerWeapons);
                    weaponItem.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(equipment));


                    //shopItem.GetComponent<Image>().color = equipment.backgroundColor;

                    weaponItem.transform.GetChild(0).GetComponent<Image>().sprite = equipment.inventoryImage;
                    //shopItem.transform.GetChild(1).GetComponent<Image>().sprite = equipment.Border;
                    //shopItem.transform.GetChild(2).GetComponent<Text>().text = equipment.name;
                    //shopItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = equipment.name;
                    break;
                case EquipmentType.Utility:
                    GameObject utilityItem = Instantiate(shopItemprefab, shopContainerUtility);
                    utilityItem.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(equipment));


                    //shopItem.GetComponent<Image>().color = equipment.backgroundColor;

                    utilityItem.transform.GetChild(0).GetComponent<Image>().sprite = equipment.inventoryImage;
                    //shopItem.transform.GetChild(1).GetComponent<Image>().sprite = equipment.Border;
                    //shopItem.transform.GetChild(2).GetComponent<Text>().text = equipment.name;
                    //shopItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = equipment.name;
                    break;
                case EquipmentType.Ship:
                    GameObject shipItem = Instantiate(shopItemprefab, shopContainerShips);
                    shipItem.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(equipment));


                    //shopItem.GetComponent<Image>().color = equipment.backgroundColor;

                    shipItem.transform.GetChild(0).GetComponent<Image>().sprite = equipment.inventoryImage;
                    //shopItem.transform.GetChild(1).GetComponent<Image>().sprite = equipment.Border;
                    //shopItem.transform.GetChild(2).GetComponent<Text>().text = equipment.name;
                    //shopItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = equipment.name;
                    break;
            }




        }
    }

    public void OnButtonClick(EquipmentData equipment)
    {
        Debug.Log("Equipment selected: " + equipment.name);

        target.SelectItemToBuy(equipment);
    }

    public void Buy()
    {
        target.BuySelectedItem();
    }

    public void ClearShop()
    {
        foreach (Transform child in shopContainerWeapons)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in shopContainerUtility)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in shopContainerWeapons)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetTarget(Shop target)
    {
        this.target = target;
    }

    void RenderUI()
    {
        if (target != null)
        {
            transform.position = target.transform.position;
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }

}

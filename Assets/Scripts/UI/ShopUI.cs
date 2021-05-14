using System.Collections.Generic;
using TMPro;
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

    public Button buyButton;

    Image lastSelectedImage;

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
                    weaponItem.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(equipment, weaponItem));


                    //shopItem.GetComponent<Image>().color = equipment.backgroundColor;

                    weaponItem.transform.GetChild(0).GetComponent<Image>().sprite = equipment.inventoryImage;
                    weaponItem.transform.GetComponentInChildren<TextMeshProUGUI>().text = WeaponInfo((WeaponData)equipment);
                    //shopItem.transform.GetChild(1).GetComponent<Image>().sprite = equipment.Border;
                    //shopItem.transform.GetChild(2).GetComponent<Text>().text = equipment.name;
                    //shopItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = equipment.name;
                    break;
                case EquipmentType.Utility:
                    GameObject utilityItem = Instantiate(shopItemprefab, shopContainerUtility);
                    utilityItem.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(equipment, utilityItem));


                    //shopItem.GetComponent<Image>().color = equipment.backgroundColor;

                    utilityItem.transform.GetChild(0).GetComponent<Image>().sprite = equipment.inventoryImage;
                    utilityItem.transform.GetComponentInChildren<TextMeshProUGUI>().text = UtilityInfo((UtilityData)equipment);
                    //shopItem.transform.GetChild(1).GetComponent<Image>().sprite = equipment.Border;
                    //shopItem.transform.GetChild(2).GetComponent<Text>().text = equipment.name;
                    //shopItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = equipment.name;
                    break;
                case EquipmentType.Ship:
                    GameObject shipItem = Instantiate(shopItemprefab, shopContainerShips);
                    shipItem.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(equipment, shipItem));


                    //shopItem.GetComponent<Image>().color = equipment.backgroundColor;

                    shipItem.transform.GetChild(0).GetComponent<Image>().sprite = equipment.inventoryImage;
                    shipItem.transform.GetComponentInChildren<TextMeshProUGUI>().text = ShipInfo((ShipData)equipment);
                    //shopItem.transform.GetChild(1).GetComponent<Image>().sprite = equipment.Border;
                    //shopItem.transform.GetChild(2).GetComponent<Text>().text = equipment.name;
                    //shopItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = equipment.name;
                    break;
            }




        }
    }

    public void OnButtonClick(EquipmentData equipment, GameObject inventoryItem)
    {
        Debug.Log("Shop Equipment selected: " + equipment.name);

        if (lastSelectedImage != null)
        {
            lastSelectedImage.color = Color.white;
        }
        inventoryItem.transform.GetChild(0).GetComponent<Image>().color = Color.green;
        lastSelectedImage = inventoryItem.transform.GetChild(0).GetComponent<Image>();

        if (PlayerStats.instance.GetCurrentMoneyAmount() >= equipment.cost)
        {
            buyButton.interactable = true;
        }
        else
        {
            buyButton.interactable = false;
        }

        target.SelectItemToBuy(equipment);
    }

    private string WeaponInfo(WeaponData data)
    {
        string infoText = data.name
            + "\nRange: " + data.range
            + "\nDamage: " + data.dmg
            + "\nFire Rate: " + data.fireRate
            + "\nTurn Radius: " + data.turnAngle
            + "\nTurning Speed: " + data.turnRate
            + "\nCOST:" + data.cost;
        return infoText;
    }

    private string UtilityInfo(UtilityData data)
    {
        string infoText = data.name
            + "\nCOST:" + data.cost;
        return infoText;
    }

    private string ShipInfo(ShipData data)
    {
        string infoText = data.name
            + "\nHealth: " + data.health
            + "\nWeapons Slots: " + data.weaponAmount
            + "\nUtility Slots: " + data.utilityAmount
            + "\nArmor: " + data.armor
            + "\nRegeneration: " + data.regeneration
            + "\nAcceleration: " + data.acceleration
            + "\nCOST:" + data.cost;
        return infoText;
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

        foreach (Transform child in shopContainerShips)
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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;

    public Transform shopContainerWeapons;
    public Transform shopContainerUtility;
    public Transform shopContainerShips;

    public Transform target;

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
                    shopContainerWeapons.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(equipment));


                    //shopItem.GetComponent<Image>().color = equipment.backgroundColor;

                    //shopItem.transform.GetChild(0).GetComponent<Image>().sprite = equipment.inventoryImage;
                    //shopItem.transform.GetChild(1).GetComponent<Image>().sprite = equipment.Border;
                    //shopItem.transform.GetChild(2).GetComponent<Text>().text = equipment.name;
                    //shopItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = equipment.name;
                    break;
                case EquipmentType.Utility:
                    GameObject sutilityItem = Instantiate(shopItemprefab, shopContainerUtility);
                    shopContainerUtility.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(equipment));


                    //shopItem.GetComponent<Image>().color = equipment.backgroundColor;

                    //shopItem.transform.GetChild(0).GetComponent<Image>().sprite = equipment.inventoryImage;
                    //shopItem.transform.GetChild(1).GetComponent<Image>().sprite = equipment.Border;
                    //shopItem.transform.GetChild(2).GetComponent<Text>().text = equipment.name;
                    //shopItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = equipment.name;
                    break;
                case EquipmentType.Ship:
                    GameObject shipItem = Instantiate(shopItemprefab, shopContainerShips);
                    shopContainerShips.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(equipment));


                    //shopItem.GetComponent<Image>().color = equipment.backgroundColor;

                    //shopItem.transform.GetChild(0).GetComponent<Image>().sprite = equipment.inventoryImage;
                    //shopItem.transform.GetChild(1).GetComponent<Image>().sprite = equipment.Border;
                    //shopItem.transform.GetChild(2).GetComponent<Text>().text = equipment.name;
                    //shopItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = equipment.name;
                    break;
            }




        }
    }

    private void OnButtonClick(EquipmentData equipment)
    {
        Debug.Log(equipment.name);

        //TODO Set Item as Selected
    }

    public void ClearShop()
    {
        foreach (Transform child in shopContainerWeapons)
        {
            Destroy(child);
        }

        foreach (Transform child in shopContainerUtility)
        {
            Destroy(child);
        }

        foreach (Transform child in shopContainerWeapons)
        {
            Destroy(child);
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    void RenderUI()
    {
        if (target != null)
        {
            transform.position = target.position;
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }

}

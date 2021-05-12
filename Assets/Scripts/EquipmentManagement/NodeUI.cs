using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{

    public GameObject ui;

    public Button equipButton;
    public Button removeButton;

    private Node target;

    private int size;

    EquipmentManager equipmentManager;
    InventoryManager inventoryManager;

    public Transform itemContainer;
    public GameObject nodeUIItemPrefab;

    Image lastSelectedImage;

    private void Start()
    {
        inventoryManager = InventoryManager.instance;
        equipmentManager = EquipmentManager.instance;
        equipButton.interactable = false;
    }

    private void Update()
    {
        RenderUI();
    }

    public void SetTarget(Node target)
    {
        this.target = target;
        size = target.size;
        transform.position = target.GetBuildPosition();

        ui.SetActive(true);

        if (target.equipment == null)
        {
            removeButton.interactable = false;
        }
        else
        {
            removeButton.interactable = true;
        }

        if (equipmentManager.GetBlueprintToBuild() == null)
        {
            equipButton.interactable = false;
        }

        PopulateNodeUI();

    }

    public void PopulateNodeUI()
    {
        ClearShop();

        List<EquipmentData> equipmentBlueprints = inventoryManager.GetEquipmentList();

        foreach (EquipmentData equipmentData in equipmentBlueprints)
        {
            if (equipmentData.minNodeSize > size)
            {
                continue;
            }

            GameObject inventoryItem = Instantiate(nodeUIItemPrefab, itemContainer);

            inventoryItem.name = equipmentData.name;

            inventoryItem.GetComponentInChildren<Image>().sprite = equipmentData.inventoryImage;
            inventoryItem.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(equipmentData, inventoryItem));

            inventoryItem.transform.GetChild(1).gameObject.GetComponentInChildren<TextMeshProUGUI>().text = WeaponInfo((WeaponData)equipmentData);

        }
    }

    private string WeaponInfo(WeaponData data)
    {
        string infoText = data.name
            + "\nRange: " + data.range
            + "\nDamage: " + data.dmg
            + "\nFireRate: " + data.fireRate
            + "\nTurnRadius: " + data.turnAngle
            + "\nTurningSpeed: " + data.turnRate;
        return infoText;
    }


    private void OnButtonClick(EquipmentData equipment, GameObject inventoryItem)
    {
        Debug.Log(equipment.name);

        if (lastSelectedImage != null)
        {
            lastSelectedImage.color = Color.white;
        }
        inventoryItem.GetComponentInChildren<Image>().color = Color.green;
        lastSelectedImage = inventoryItem.GetComponentInChildren<Image>();
        SelectEquipmentToBuild(equipment);
    }

    public void ClearShop()
    {
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }
    }

    void RenderUI()
    {
        if (target != null)
        {
            transform.position = target.GetBuildPosition();
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }

    public void SelectEquipmentToBuild(EquipmentData equipmentBlueprint)
    {
        equipmentManager.SetBlueprintToBuild(equipmentBlueprint);
        equipButton.interactable = true;
    }

    public void FilterBySize()
    {
        //TODO Show only fitting Equiment
    }

    public void Hide()
    {
        ui.SetActive(false);
    }

    public void Equip()
    {
        target.Equip();
        equipmentManager.DeselectNode();
    }

    public void Remove()
    {
        target.RemoveEquipment();
        removeButton.interactable = false;
    }

}

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
            inventoryItem.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(equipmentData));

            inventoryItem.transform.GetChild(1).gameObject.GetComponentInChildren<TextMeshProUGUI>().text = equipmentData.name;

            //inventoryItem.transform.GetChild(0).GetComponentInChildren<TextMeshPro>().text = equipmentData.name;
            //inventoryItem.transform.GetChild(1).GetComponentInChildren<TextMeshPro>().text = equipmentData.name;
            //inventoryItem.transform.GetChild(2).GetComponentInChildren<TextMeshPro>().text = equipmentData.name;

        }
    }

    private void OnButtonClick(EquipmentData equipment)
    {
        Debug.Log(equipment.name);

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

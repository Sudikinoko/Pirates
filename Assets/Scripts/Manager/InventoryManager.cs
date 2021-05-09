using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public List<EquipmentData> equipmentBlueprints = new List<EquipmentData>();

    EquipmentManager equipmentManager;

    public static InventoryManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        equipmentManager = EquipmentManager.instance;
    }

    public void SelectEquipment()
    {
        equipmentManager.SetBlueprintToBuild(equipmentManager.standardBlueprintToBuild);
    }

    public void AddEquipment(EquipmentData equipmentBlueprint)
    {
        equipmentBlueprints.Add(equipmentBlueprint);
    }

    public void RemoveEquipment(EquipmentData equipmentBlueprint)
    {
        equipmentBlueprints.Remove(equipmentBlueprint);
    }

    public List<EquipmentData> GetEquipmentList()
    {
        return equipmentBlueprints;
    }

}

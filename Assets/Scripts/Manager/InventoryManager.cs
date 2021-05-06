using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    List<EquipmentBlueprint> equipmentBlueprints;

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

    public void AddEquipment(EquipmentBlueprint equipmentBlueprint)
    {
        equipmentBlueprints.Add(equipmentBlueprint);
    }

    public void RemoveEquipment(EquipmentBlueprint equipmentBlueprint)
    {
        equipmentBlueprints.Remove(equipmentBlueprint);
    }

    public List<EquipmentBlueprint> GetEquipmentList()
    {
        return equipmentBlueprints;
    }

}

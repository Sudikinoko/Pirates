using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshCollider))]
public class Node : MonoBehaviour
{

    public Color hoverColor;

    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 direction;
    public Vector3 positionOffset;

    public Transform buildPoint;

    [Header("Node Size for Weapon or Utility")]
    public int size;

    [HideInInspector]
    public GameObject equipment;
    [HideInInspector]
    public EquipmentData equipmentData;
    public EquipmentData oldEquipmentData;

    private Renderer rend;
    private Color startColor;

    EquipmentManager equipmentManager;
    InventoryManager inventoryManager;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        inventoryManager = InventoryManager.instance;
        equipmentManager = EquipmentManager.instance;
    }

    public Vector3 GetBuildPosition()
    {
        if (buildPoint != null)
        {
            return buildPoint.position;
        }

        return transform.position + positionOffset;
    }


    public void OnMouseEnter()
    {
        rend.material.color = hoverColor;

    }

    public void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        equipmentManager.SelectNode(this);

    }

    public void SetBlueprint(EquipmentData blueprint)
    {
        this.equipmentData = blueprint;
    }

    public void Equip()
    {
        if (equipmentData != null)
        {
            if (equipment != null)
            {
                RemoveEquipment();
            }

            oldEquipmentData = equipmentData;
            inventoryManager.RemoveEquipment(equipmentData);

            equipment = Instantiate(equipmentData.prefab, GetBuildPosition(), transform.rotation);
            equipment.transform.SetParent(this.transform);
            if (equipment.GetComponent<Weapon>() != null)
            {
                equipment.GetComponent<Weapon>().weaponData = (WeaponData)equipmentData;
            }
        }
    }

    public void Equip(EquipmentData blueprint)
    {
        this.equipmentData = blueprint;
        Equip();
    }

    public void RemoveEquipment()
    {
        inventoryManager.AddEquipment(oldEquipmentData);
        oldEquipmentData = null;
        Destroy(equipment);
    }

}

using UnityEngine;
using UnityEngine.EventSystems;

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
    public EquipmentData blueprint;

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
        this.blueprint = blueprint;
    }

    public void Equip()
    {
        if (blueprint != null)
        {
            if (equipment != null)
            {
                RemoveEquipment();
            }

            inventoryManager.RemoveEquipment(blueprint);

            equipment = Instantiate(blueprint.prefab, GetBuildPosition(), transform.rotation);
            equipment.transform.SetParent(this.transform);
        }
    }

    public void Equip(EquipmentData blueprint)
    {
        this.blueprint = blueprint;
        Equip();
    }

    public void RemoveEquipment()
    {
        //TODO Add to Inventory
        inventoryManager.AddEquipment(blueprint);
        blueprint = null;
        Destroy(equipment);
    }

}

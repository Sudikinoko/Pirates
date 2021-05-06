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
    public EquipmentBlueprint blueprint;

    private Renderer rend;
    private Color startColor;

    EquipmentManager equipmentManager;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

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

    public void SetBlueprint(EquipmentBlueprint blueprint)
    {
        this.blueprint = blueprint;
    }

    public void Equip()
    {
        if (blueprint != null)
        {
            equipment = Instantiate(blueprint.prefab, GetBuildPosition(), transform.rotation);
            equipment.transform.SetParent(this.transform);
        }
    }

    public void Equip(EquipmentBlueprint blueprint)
    {
        if (equipment != null)
        {
            RemoveEquipment();
        }

        this.blueprint = blueprint;
        Equip();
    }

    public void RemoveEquipment()
    {
        //TODO Add to Inventory

        blueprint = null;
        Destroy(equipment);
    }

}

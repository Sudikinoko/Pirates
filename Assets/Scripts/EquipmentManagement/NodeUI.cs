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

    private void Start()
    {
        equipmentManager = EquipmentManager.instance;
    }

    private void Update()
    {
        RenderHealthBar();
    }

    public void SetTarget(Node target)
    {
        this.target = target;

        transform.position = target.GetBuildPosition();

        ui.SetActive(true);

        if (target.equipment == null)
        {
            equipButton.interactable = true;
            removeButton.interactable = false;
        }
        else
        {
            removeButton.interactable = true;
        }

    }

    void RenderHealthBar()
    {
        if (target != null)
        {
            transform.position = target.GetBuildPosition();
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }

    public void SelectEquipmentToBuild(EquipmentBlueprint equipmentBlueprint)
    {
        equipmentManager.SetBlueprintToBuild(equipmentBlueprint);
    }

    public void SetNodeSize(int size)
    {
        this.size = size;
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

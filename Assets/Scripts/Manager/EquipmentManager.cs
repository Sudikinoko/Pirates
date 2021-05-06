using UnityEngine;

public class EquipmentManager : MonoBehaviour
{

    public static EquipmentManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public GameObject equipEffect;
    public GameObject removeEffect;

    public EquipmentBlueprint standardBlueprintToBuild;
    EquipmentBlueprint blueprintToBuild;
    private Node selectedNode;

    public NodeUI nodeUI;

    public void SelectNode(Node node)
    {
        blueprintToBuild = null;

        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }
        selectedNode = node;

        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        blueprintToBuild = null;
        selectedNode = null;
        nodeUI.Hide();
    }

    public void SetBlueprintToBuild(EquipmentBlueprint blueprint)
    {
        blueprintToBuild = blueprint;
        selectedNode.SetBlueprint(blueprintToBuild);
    }

    public EquipmentBlueprint GetBlueprintToBuild()
    {
        return blueprintToBuild;
    }
}

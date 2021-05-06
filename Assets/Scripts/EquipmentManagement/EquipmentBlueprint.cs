using UnityEngine;

[System.Serializable]
public class EquipmentBlueprint : MonoBehaviour
{
    public int amountInInventory;

    public string description;
    public Sprite inventoryImage;

    public GameObject prefab;
    public float cost;
    public bool unlocked;

    public GameObject buildEffect;

    public float GetSellAmount()
    {
        return cost / 2;
    }

}

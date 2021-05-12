using UnityEngine;

public enum Rarity
{
    Poor,
    Common,
    Rare,
    Mystical,
    Celestial
}



public enum EquipmentType
{
    Weapon,
    Utility,
    Ship
}

[CreateAssetMenu(menuName = "Equipment/New Equipment")]
public class EquipmentData : ScriptableObject
{
    public new string name;

    public int amount;

    [Header("Shop/Inventory Description")]
    public string description;
    public Sprite inventoryImage;
    public Sprite Border;
    public Color backgroundColor;

    public Rarity rarity;
    [HideInInspector]
    public EquipmentType type;

    [Header("Ingame")]
    public float cost;
    public bool unlocked;
    [Range(1, 5)]
    public int minNodeSize;

    public GameObject prefab;


    public GameObject buildEffect;
}


using UnityEngine;

[CreateAssetMenu(menuName = "Equipment/New Ship")]
public class ShipData : EquipmentData
{

    public float cameraConstructionSize = 10f;
    public float cameraZoomedOutSize = 100f;
    [Header("Shop Info")]
    public float weaponAmount;
    public float utilityAmount;

    [Header("Ingame Stats")]
    public float health;
    public float armor;
    public float regeneration;
    public float speed;
    public float acceleration;
    public float turningRate;
    public float mass;
    public float drag;
    public float angularDrag;
    public AnimationCurve turnHabit;

    [Header("Bounty")]
    public float minValue; //Price | LootAmount,...
    public float maxValue; //Price | LootAmount,...

}

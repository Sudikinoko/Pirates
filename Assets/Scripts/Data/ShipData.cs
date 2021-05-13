
using UnityEngine;

[CreateAssetMenu(menuName = "Equipment/New Ship")]
public class ShipData : EquipmentData
{
    [HideInInspector]
    public new EquipmentType type = EquipmentType.Ship;

    public float cameraConstructionSize = 10f;

    public float zoomedOutSize = 100f;

    public float health;
    public float armor;
    public float regeneration;
    public float speed;
    public float acceleration;
    public float turningRate;
    public float mass;
    public float drag;
    public float angularDrag;

}

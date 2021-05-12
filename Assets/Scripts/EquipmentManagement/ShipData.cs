
using UnityEngine;

[CreateAssetMenu(menuName = "Equipment/New Ship")]
public class ShipData : EquipmentData
{
    [HideInInspector]
    public new EquipmentType type = EquipmentType.Ship;

    public float cameraConstructionSize = 10f;

    public float zoomedOutSize = 100f;

}

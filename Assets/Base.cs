using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Base : MonoBehaviour
{

    GameUI gameUI;

    public bool isShop = true;
    public bool isShipyard = true;

    public List<EquipmentBlueprint> blueprintsToSell;

    // Start is called before the first frame update
    void Start()
    {
        gameUI = GameUI.instance;
    }

    void OnTriggerEnter(Collider col)
    {
        PlayerController playerController = col.transform.root.GetComponent<PlayerController>();
        if (playerController != null)
        {
            if (isShop)
            {
                gameUI.EnableShop();
            }
            if (isShipyard)
            {
                gameUI.EnableConstruction();
            }
        }

    }

    void OnTriggerExit(Collider col)
    {
        PlayerController playerController = col.transform.root.GetComponent<PlayerController>();
        if (playerController != null)
        {
            gameUI.DisableShop();
            gameUI.DisableConstruction();
        }

    }
}

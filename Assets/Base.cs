using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Base : MonoBehaviour
{

    public Transform respawn;

    GameUI gameUI;
    ShopUI shopUI;

    public bool isShop = true;
    public bool isShipyard = true;

    public Shop shop;

    // Start is called before the first frame update
    void Start()
    {
        gameUI = GameUI.instance;
        shopUI = ShopUI.instance;
    }

    void OnTriggerEnter(Collider col)
    {
        PlayerController playerController = col.transform.root.GetComponent<PlayerController>();
        if (playerController != null)
        {
            if (isShop)
            {
                gameUI.EnableShop();
                Camera.main.GetComponent<CameraController>().shopTransform = shop.transform;
                shopUI.SetTarget(shop);
                shopUI.ui.SetActive(true);
                shop.PopulateShopUI();
            }
            if (isShipyard)
            {
                gameUI.EnableConstruction();
            }
            playerController.SetHomeBase(this);
            playerController.GetComponent<Ship>().Heal(float.PositiveInfinity);
        }

    }

    void OnTriggerExit(Collider col)
    {
        PlayerController playerController = col.transform.root.GetComponent<PlayerController>();
        if (playerController != null)
        {
            shopUI.ui.SetActive(false);
            gameUI.DisableShop();
            gameUI.DisableConstruction();
        }

    }
}

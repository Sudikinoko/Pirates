using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Base : MonoBehaviour
{

    public Transform respawn;

    GameUI gameUI;
    ShopUI shopUI;

    public bool isShop = true;
    public bool isShipyard = true;

    public float healPerSecond = 10f;
    private bool isHealing = false;

    public Shop shop;

    private Ship playerShip;

    // Start is called before the first frame update
    void Start()
    {
        gameUI = GameUI.instance;
        shopUI = ShopUI.instance;
    }

    void Update()
    {
        if (isHealing)
        {
            playerShip.Heal(healPerSecond * Time.deltaTime);
        }
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
            playerShip = playerController.GetComponent<Ship>();
            playerController.SetHomeBase(this);
            isHealing = true;
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
            isHealing = false;
        }

    }
}

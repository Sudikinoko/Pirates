using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager shopManager;

    private void Awake()
    {
        if (shopManager == null)
        {
            shopManager = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

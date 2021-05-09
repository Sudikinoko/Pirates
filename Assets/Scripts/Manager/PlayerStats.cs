using UnityEngine;

public class PlayerStats : MonoBehaviour, ICollector
{
    GameUI gameUI;

    public static PlayerStats instance;

    public float money;

    public int level;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(instance);
        }
    }

    public void AddMoney(float amount)
    {
        money += amount;
        gameUI.UpdateCoins();
    }

    public bool RemoveMoney(float amount)
    {
        if (money > amount)
        {
            money -= amount;
            gameUI.UpdateCoins();
            return true;
        }
        return false;
    }

    public float GetCurrentMoneyAmount()
    {
        return money;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameUI = GameUI.instance;
        money = 0f;
        gameUI.UpdateCoins();
    }

}

using UnityEngine;

public class Stats : MonoBehaviour, ICollector
{

    float money;

    public int level;

    public void AddMoney(float amount)
    {
        if (gameObject.tag == "Player")
        {
            PlayerStats.instance.AddMoney(amount);
        }
        else
        {
            Debug.Log("No Money!");
            money += amount;
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

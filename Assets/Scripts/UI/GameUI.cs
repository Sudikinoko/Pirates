using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    public static GameUI instance;

    PlayerStats playerStats;
    PlayerController playerController;

    public Text playerGold;
    public Button constructionButton;
    public Button shopButton;

    private CameraState state;
    private CameraState stateBefore;

    public Image healthBar;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerSpeedText;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        playerStats = PlayerStats.instance;
        playerController = FindPlayerController();
        UpdateCoins();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController == null)
        {
            playerController = FindPlayerController();
        }
    }

    PlayerController FindPlayerController()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void UpdadeHealthBar(float health, float maxHealth)
    {
        healthBar.fillAmount = health / maxHealth;
        playerHealthText.text = health.ToString("F0") + "/" + maxHealth.ToString("F0");
    }

    public void UpdateSpeed(float speed)
    {
        playerSpeedText.text = "Speed: " + speed.ToString("F2");
    }

    public void UpdateCoins()
    {
        if (playerStats != null)
        {
            playerGold.text = Mathf.Floor(playerStats.GetCurrentMoneyAmount()) + " Gold";
        }
    }

    public void SetCameraState(int cameraState)
    {
        CameraState tempState = (CameraState)cameraState;

        Camera main = Camera.main;
        CameraController cameraController = main.GetComponent<CameraController>();


        if (cameraController != null)
        {
            bool toogleConstruction = tempState == CameraState.ConstructMode && stateBefore == CameraState.ConstructMode;

            tempState = toogleConstruction ? CameraState.ZoomedOut : tempState;
            stateBefore = tempState;

            cameraController.SetCameraState(tempState);

            if (tempState == CameraState.ConstructMode)
            {
                Debug.Log("Enable Construction Mode");
                playerController.EnableConstructionMode();
            }
            else
            {
                Debug.Log("Disable Construction Mode");
                playerController.DisableConstructionMode();
            }
        }
    }

    public void EnableShop()
    {
        shopButton.interactable = true;
        shopButton.GetComponentInChildren<Text>().enabled = true;
    }

    public void DisableShop()
    {
        shopButton.interactable = false;
        shopButton.GetComponentInChildren<Text>().enabled = false;
    }

    public void EnableConstruction()
    {
        constructionButton.interactable = true;
        constructionButton.GetComponentInChildren<Text>().enabled = true;
    }

    public void DisableConstruction()
    {
        constructionButton.interactable = false;
        constructionButton.GetComponentInChildren<Text>().enabled = false;
    }
}

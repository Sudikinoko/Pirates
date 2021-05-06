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
        playerController = PlayerController.instance;
        UpdateCoins();
    }

    // Update is called once per frame
    void Update()
    {
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

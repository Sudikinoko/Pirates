using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    TutorialManager instance;


    public GameObject[] popups;

    int activePopupIndex = 0;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdatePopups();
    }


    public void UpdatePopups()
    {
        for (int i = 0; i < popups.Length; i++)
        {
            popups[i].GetComponent<Canvas>().enabled = i == activePopupIndex;
        }
        if (activePopupIndex > popups.Length - 1)
        {
            Debug.Log("Destroy Tutorial");
            Destroy(this.gameObject);
        }
    }


    public void NextPopup()
    {
        activePopupIndex++;
        UpdatePopups();
    }
}

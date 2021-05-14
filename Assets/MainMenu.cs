using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad = "Game";

    public void NewGame()
    {
        SceneManager.LoadSceneAsync(levelToLoad);
    }

    public void Continue()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }

}

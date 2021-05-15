using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad = "Game";

    public TextMeshProUGUI text;

    public void NewGame()
    {
        StartCoroutine(LoadScene());
    }

    public void Continue()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!asyncOperation.isDone)
        {
            //Output the current progress
            text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";
            yield return null;
        }
    }

}

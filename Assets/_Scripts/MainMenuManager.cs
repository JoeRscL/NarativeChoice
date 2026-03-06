using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string gameplaySceneName = "GameplayScene";

    public void NewGame()
    {
        PlayerPrefs.DeleteKey("Save_Day");
        PlayerPrefs.Save();
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
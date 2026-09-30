using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level01");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level02");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
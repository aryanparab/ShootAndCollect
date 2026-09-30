using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public GameObject endPanel;
    public TMP_Text endText;

    [Header("Level")]
    public string nextSceneName;

    private bool gameEnded = false;

    void Awake()
    {
        Instance = this;

        Time.timeScale = 1f;

        if (endPanel != null)
        {
            endPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (!gameEnded || Keyboard.current == null)
            return;

        // R = restart current level
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            RestartLevel();
        }

        // M = return to main menu
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            LoadMenu();
        }

        // Enter = next level after winning
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            LoadNextLevel();
        }
    }

    public void GameOver()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        if (endPanel != null)
        {
            endPanel.SetActive(true);
        }

        if (endText != null)
        {
            endText.text =
                "GAME OVER\n\n" +
                "R - Restart\n" +
                "M - Main Menu";
        }

        Time.timeScale = 0f;
    }

    public void PlayerWins()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        if (endPanel != null)
        {
            endPanel.SetActive(true);
        }

        if (endText != null)
        {
            string message =
                "YOU WIN!\n\n";

            if (!string.IsNullOrEmpty(nextSceneName))
            {
                message +=
                    "ENTER - Next Level\n";
            }

            message +=
                "R - Restart\n" +
                "M - Main Menu";

            endText.text = message;
        }

        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }

    public void LoadNextLevel()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            LoadMenu();
            return;
        }

        Time.timeScale = 1f;

        SceneManager.LoadScene(nextSceneName);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuController : MonoBehaviour
{
    public GameObject gameOverPanel;

    public static bool IsGameOver { get; private set; } = false;

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        IsGameOver = false;
        Time.timeScale = 1f; 
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
        IsGameOver = true;

        Debug.Log("ShowGameOver called");

        if (MusicManager.Instance != null)
        {
            Debug.Log("Switching to death music");
            MusicManager.Instance.PlayDeathMusic();
        }
        else
        {
            Debug.LogWarning("MusicManager.Instance is null");
        }
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;
        IsGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        IsGameOver = false;
        SceneManager.LoadScene("MainMenu");
    }
}

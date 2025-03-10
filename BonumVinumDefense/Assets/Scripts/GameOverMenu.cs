using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameOverMenu : MonoBehaviour
{
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject); // Supprime le GameManager s'il persiste
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

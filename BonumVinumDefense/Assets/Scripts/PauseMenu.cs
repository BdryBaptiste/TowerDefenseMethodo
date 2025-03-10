using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);   
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        
    }

    public void PauseGame()
    {
        if (isPaused) return;

        Debug.Log("PauseGame() appelé"); 
        pauseMenu.SetActive(true);
        Debug.Log("pauseMenu activé : " + pauseMenu.activeSelf);
        Time.timeScale = 0f;
        isPaused = true;
    }


    public void ResumeGame()
    {
        if (!isPaused) return;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        Time.timeScale = 1f;
        isPaused = false;

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

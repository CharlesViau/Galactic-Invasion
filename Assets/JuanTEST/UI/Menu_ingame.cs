using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_ingame : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject menuPanel;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Time.timeScale = 0f; // Pause the game
                menuPanel.SetActive(true);
                isPaused = true;
            }
            else
            {
                Time.timeScale = 1f; // Resume the game
                menuPanel.SetActive(false);
                isPaused = false;
            }
        }
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
        isPaused = false;
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex); // Reload the current scene
    }
    
    public void QuitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    
    public void QuitToDesktop()
    {
        Application.Quit(); // Quit the application
    }


}

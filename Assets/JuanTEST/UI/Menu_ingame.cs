using System.Collections;
using System.Collections.Generic;
using Enemies;
using Projectiles;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_ingame : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject menuPanel;
    
    private void Start()
    {
        ConfineCursor();
    }
    
    void Update()
    {
        Debug.Log(Cursor.lockState);
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Time.timeScale = 0f; // Pause the game
                menuPanel.SetActive(true);
                isPaused = true;
                ToggleCursorLock();
            }
            else
            {
                Time.timeScale = 1f; // Resume the game
                menuPanel.SetActive(false);
                isPaused = false;
                ToggleCursorLock();
            }
        }
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
        isPaused = false;
        ToggleCursorLock();
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        EnemyManager.Instance.Clear();
        ProjectileManager.Instance.Clear();
        Controller.Instance.Reset();
        SceneManager.LoadScene(currentSceneIndex); // Reload the current scene
        Cursor.visible = false;
    }
    
    public void QuitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    
    public void QuitToDesktop()
    {
        Application.Quit(); // Quit the application
    }

    private static void ConfineCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private static void ToggleCursorLock()
    {
        if (Cursor.lockState == CursorLockMode.Confined)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            ConfineCursor();
        }
    }
}

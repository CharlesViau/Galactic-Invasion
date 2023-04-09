using Enemies;
using Projectiles;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_ingame : MonoBehaviour
{
    public GameObject menuPanel;
    public bool IsPaused { get; private set; }
    public static Menu_ingame Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ConfineCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsPaused)
            {
                Time.timeScale = 0f; // Pause the game
                menuPanel.SetActive(true);
                IsPaused = true;
                ToggleCursorLock();
            }
            else
            {
                Time.timeScale = 1f; // Resume the game
                menuPanel.SetActive(false);
                IsPaused = false;
                ToggleCursorLock();
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
        IsPaused = false;
        ToggleCursorLock();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
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
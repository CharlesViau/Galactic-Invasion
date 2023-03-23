using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverMenu : MonoBehaviour
{
    public void ReplayGame()
    {
        // Replace "MainScene" with the name of your main game scene
        SceneManager.LoadScene("UI-gameTEST");
    }

    public void ReturnToMainMenu()
    {
        // Replace "MainMenu" with the name of your main menu scene
        SceneManager.LoadScene("Main Menu");
    }

}

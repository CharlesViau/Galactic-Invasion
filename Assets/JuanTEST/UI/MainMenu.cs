using System.Collections;
using System.Collections.Generic;
using Enemies;
using Projectiles;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 0.5f;
    
    [Header("Fullscreen Setting")]
    [SerializeField] private Toggle fullscreenToggle = null;
    [SerializeField] private bool defaultFullscreen = true;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;
    
    [Header("Panels")]
    [SerializeField] private GameObject warningPanel = null;
    [SerializeField] private GameObject mainMenuPanel = null;

    public void PlayGame()
    {
        EnemyManager.Instance.Clear();
        ProjectileManager.Instance.Clear();
        if (Controller.Instance != null) Controller.Instance.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
        volumeSlider.SetValueWithoutNotify(volume);
    }

    public void ApplySettings()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        PlayerPrefs.SetInt("isFullscreen", Screen.fullScreen ? 1 : 0);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetSettings()
    {
        SetVolume(defaultVolume);

        if (volumeSlider != null) {
            volumeSlider.value = defaultVolume;
        }
        
        fullscreenToggle.isOn = defaultFullscreen;
        OnFullscreenToggleValueChanged(defaultFullscreen);

        PlayerPrefs.SetFloat("masterVolume", defaultVolume);
        PlayerPrefs.SetInt("isFullscreen", defaultFullscreen ? 1 : 0);
    }

    public void LoadSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat("masterVolume", defaultVolume);
        SetVolume(savedVolume);
        bool savedFullscreen = PlayerPrefs.GetInt("isFullscreen", 1) == 1;
        fullscreenToggle.isOn = savedFullscreen;
        OnFullscreenToggleValueChanged(savedFullscreen);
    }

    public void BackButton()
    {
        float savedVolume = PlayerPrefs.GetFloat("masterVolume", defaultVolume);
        if (Mathf.Abs(AudioListener.volume - savedVolume) > 0.01f)
        {
            ShowWarningPanel(true);
        }
        else
        {
            mainMenuPanel.SetActive(true);
        }
        
    }

    public void SetFullscreenMode()
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
    }

    public void SetWindowedMode()
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
    }

    public void OnFullscreenToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            SetFullscreenMode();
        }
        else
        {
            SetWindowedMode();
        }
    }
    
    public void ConfirmBack()
    {
        ShowWarningPanel(false);
        ApplySettings();
        mainMenuPanel.SetActive(true);
    }

    public void CancelBack()
    {
        ShowWarningPanel(false);
        LoadSettings();
        
    }
    
    private void ShowWarningPanel(bool show)
    {
        warningPanel.SetActive(show);
        mainMenuPanel.SetActive(!show);
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(1);
        confirmationPrompt.SetActive(false);
    }
    
}


using System.Collections;
using System.Collections.Generic;
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

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
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
        StartCoroutine(ConfirmationBox());
    }

    public void ResetSettings()
    {
        SetVolume(defaultVolume);

        if (volumeSlider != null) {
            volumeSlider.value = defaultVolume;
        }

        PlayerPrefs.SetFloat("masterVolume", defaultVolume);
    }

    public void LoadSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat("masterVolume", defaultVolume);
        SetVolume(savedVolume);
    }

    public void BackButton()
    {
        LoadSettings();
        // Add any other actions you want to perform when the back button is clicked
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

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(1);
        confirmationPrompt.SetActive(false);
    }
}


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

    [Header("Brightness Setting")]
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private float defaultBrightness = 0.5f;

    [Header("Sensitivity Setting")]
    [SerializeField] private TMP_Text sensitivityTextValue = null;
    [SerializeField] private Slider sensitivitySlider = null;
    [SerializeField] private int defaultSensitivity = 4;

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
    }

    public void SetBrightness(float brightness)
    {
        if (brightnessTextValue != null && brightnessSlider != null) {
            RenderSettings.ambientIntensity = brightness;
            brightnessTextValue.text = brightness.ToString("0.0");
        }
    }

    public void SetSensitivity(float sensitivity)
    {
        if (sensitivityTextValue != null) {
            int roundedSensitivity = Mathf.RoundToInt(sensitivity);
            sensitivityTextValue.text = roundedSensitivity.ToString();
        }
    }


    public void ApplySettings()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        PlayerPrefs.SetFloat("brightness", RenderSettings.ambientIntensity);
        PlayerPrefs.SetFloat("sensitivity", sensitivitySlider.value);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetSettings()
    {
        SetVolume(defaultVolume);
        SetBrightness(defaultBrightness);
        SetSensitivity(defaultSensitivity);

        if (volumeSlider != null) {
            volumeSlider.value = defaultVolume;
        }
        if (brightnessSlider != null) {
            brightnessSlider.value = defaultBrightness;
        }
        if (sensitivitySlider != null) {
            sensitivitySlider.value = defaultSensitivity;
        }

        PlayerPrefs.SetFloat("masterVolume", defaultVolume);
        PlayerPrefs.SetFloat("brightness", defaultBrightness);
        PlayerPrefs.SetInt("sensitivity", defaultSensitivity);
    }

    public void LoadSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat("masterVolume", defaultVolume);
        SetVolume(savedVolume);

        float savedBrightness = PlayerPrefs.GetFloat("brightness", defaultBrightness);
        SetBrightness(savedBrightness);

        int savedSensitivity = PlayerPrefs.GetInt("sensitivity", defaultSensitivity);
        SetSensitivity(savedSensitivity);

        if (volumeSlider != null) {
            volumeSlider.value = savedVolume;
        }
        if (brightnessSlider != null) {
            brightnessSlider.value = savedBrightness;
        }
        if (sensitivitySlider != null) {
            sensitivitySlider.value = savedSensitivity;
        }
    }
    
    public void BackButton()
    {
        LoadSettings();
        // Add any other actions you want to perform when the back button is clicked
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}


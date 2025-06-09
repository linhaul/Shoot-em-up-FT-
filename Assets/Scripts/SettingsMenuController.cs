using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SettingsMenuController : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;
    public GameObject keybindsPanel;

    public TMP_Dropdown resolutionDropdown;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private Resolution[] resolutions;

    void Start()
    {
        LoadResolutions();
    }

    void LoadResolutions()
    {
        resolutions = Screen.resolutions
            .Select(res => new Resolution { width = res.width, height = res.height })
            .Distinct()
            .ToArray();

        resolutionDropdown.ClearOptions();

        var options = resolutions.Select(res => $"{res.width} x {res.height}").ToList();
        resolutionDropdown.AddOptions(options);

        int currentIndex = System.Array.FindIndex(resolutions,
            r => r.width == Screen.currentResolution.width && r.height == Screen.currentResolution.height);

        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, FullScreenMode.Windowed);
        Debug.Log($"Установлено разрешение: {res.width}x{res.height}");
    }

    public void SetMusicVolume(float volume)
    {
        Debug.Log("Музыка громкость: " + volume);
    }

    public void SetSFXVolume(float volume)
    {
        Debug.Log("SFX громкость: " + volume);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OpenKeybindsPanel()
    {
        settingsPanel.SetActive(false);
        keybindsPanel.SetActive(true);
    }

    public void BackToSettingsPanel()
    {
        keybindsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
}

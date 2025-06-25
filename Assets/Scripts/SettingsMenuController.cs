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
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;

    private const string RESOLUTION_PREF_KEY = "ResolutionIndex";
    private const string FULLSCREEN_PREF_KEY = "Fullscreen";

    void Start()
    {
        LoadResolutions();
        ApplySavedSettings();

        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicVolumeSlider.value = savedMusicVolume;

        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);

        float savedSfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        sfxVolumeSlider.value = savedSfxVolume;
        sfxVolumeSlider.onValueChanged.RemoveAllListeners();
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void LoadResolutions()
    {
        resolutions = Screen.resolutions
            .Select(res => new Resolution { width = res.width, height = res.height })
            .Distinct()
            .Where(res => res.width >= 1280 && res.width <= 1920 && res.height >= 720 && res.height <= 1080)
            .OrderByDescending(r => r.width * r.height)
            .ToArray();

        resolutionDropdown.ClearOptions();
        var options = resolutions.Select(res => $"{res.width} x {res.height}").ToList();
        resolutionDropdown.AddOptions(options);

        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    void ApplySavedSettings()
    {
        int savedIndex = PlayerPrefs.GetInt(RESOLUTION_PREF_KEY, resolutions.Length - 1);
        bool isFullscreen = PlayerPrefs.GetInt(FULLSCREEN_PREF_KEY, 1) == 1;

        if (savedIndex < 0 || savedIndex >= resolutions.Length)
            savedIndex = resolutions.Length - 1;

        Resolution res = resolutions[savedIndex];

        Screen.SetResolution(res.width, res.height, isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);

        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = isFullscreen;
    }

    public void SetResolution(int index)
    {
        if (resolutions == null || resolutions.Length == 0)
            return;

        Resolution res = resolutions[index];
        bool isFullscreen = fullscreenToggle.isOn;

        Screen.SetResolution(res.width, res.height, isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);

        PlayerPrefs.SetInt(RESOLUTION_PREF_KEY, index);
        PlayerPrefs.Save();

        Debug.Log($"Установлено разрешение: {res.width}x{res.height}, Fullscreen: {isFullscreen}");
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Resolution current = resolutions[resolutionDropdown.value];
        FullScreenMode mode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;

        Screen.SetResolution(current.width, current.height, mode);

        PlayerPrefs.SetInt(FULLSCREEN_PREF_KEY, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("Fullscreen переключен: " + isFullscreen);
    }

    public void SetMusicVolume(float volume)
    {
        Debug.Log("Музыка громкость: " + volume);
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetMusicVolume(volume);
        }
    }


    public void SetSFXVolume(float volume)
    {
        Debug.Log("SFX громкость: " + volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
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

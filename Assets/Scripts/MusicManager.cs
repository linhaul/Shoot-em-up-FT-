using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip gameMusic;
    public AudioClip deathMusic;
    public AudioClip bossMusic;

    [Header("Settings")]
    [Range(0f, 1f)] public float defaultVolume = 1f;
    public float fadeSpeed = 1f;

    private AudioSource[] sources = new AudioSource[2];
    private int activeSourceIndex = 0;
    private Coroutine transitionCoroutine;
    private float targetVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            sources[0] = gameObject.AddComponent<AudioSource>();
            sources[1] = gameObject.AddComponent<AudioSource>();

            foreach (var src in sources)
            {
                src.loop = true;
                src.playOnAwake = false;
            }

            targetVolume = PlayerPrefs.GetFloat("MusicVolume", defaultVolume);
            foreach (var src in sources)
                src.volume = 0f;

            SceneManager.sceneLoaded += OnSceneLoaded;

            if (mainMenuMusic) mainMenuMusic.LoadAudioData();
            if (gameMusic) gameMusic.LoadAudioData();
            if (deathMusic) deathMusic.LoadAudioData();
            if (bossMusic) bossMusic.LoadAudioData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
            transitionCoroutine = null;
        }

        if (scene.name == "MainMenu")
            PlayMusic(mainMenuMusic);
        else if (scene.name == "SampleScene")
            PlayMusic(gameMusic);
        else
            StopMusic();
    }

    public void PlayMusic(AudioClip newClip)
    {
        if (newClip == null)
        {
            Debug.LogWarning("MusicManager: null clip");
            return;
        }

        AudioSource current = sources[activeSourceIndex];
        if (current.clip == newClip)
        {
            Debug.Log("MusicManager: музыка уже играет");
            return;
        }

        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        int nextIndex = 1 - activeSourceIndex;
        AudioSource next = sources[nextIndex];
        next.clip = newClip;
        next.volume = 0f;
        next.Play();

        transitionCoroutine = StartCoroutine(Crossfade(activeSourceIndex, nextIndex));
        activeSourceIndex = nextIndex;
    }

    public void PlayDeathMusic() => PlayMusic(deathMusic);
    public void PlayBossMusic() => PlayMusic(bossMusic);

    public void StopMusic()
    {
        foreach (var src in sources)
        {
            src.Stop();
            src.clip = null;
        }
    }

    private IEnumerator Crossfade(int fromIndex, int toIndex)
    {
        AudioSource from = sources[fromIndex];
        AudioSource to = sources[toIndex];

        while (to.volume < targetVolume || from.volume > 0f)
        {
            to.volume = Mathf.Min(to.volume + fadeSpeed * Time.unscaledDeltaTime, targetVolume);
            from.volume = Mathf.Max(from.volume - fadeSpeed * Time.unscaledDeltaTime, 0f);
            yield return null;
        }

        from.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        targetVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("MusicVolume", targetVolume);
        PlayerPrefs.Save();

        foreach (var src in sources)
        {
            if (src.isPlaying)
                src.volume = targetVolume;
        }
    }
}

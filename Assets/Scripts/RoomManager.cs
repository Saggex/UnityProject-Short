using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

/// <summary>
/// Handles room transitions and atmosphere settings per room.
/// </summary>
public class RoomManager : PersistentSingleton<RoomManager>
{
    public string CurrentRoom { get; private set; }
    private string pendingSpawnId;

    [SerializeField] private SoundManager soundManager;
    [Header("Fade Settings")]
    [SerializeField] private SpriteRenderer fadeOverlay;   // full screen black UI Image
    [SerializeField] public float fadeDuration = 1f;
    private float ogFadeDuration;
    public UnityEvent onFadedOut = new UnityEvent();
    protected override void Awake()
    {
        ogFadeDuration = fadeDuration;
        base.Awake();
        if (Instance != this) return;

        CurrentRoom = SceneManager.GetActiveScene().name;

        // Ensure overlay starts invisible
        if (fadeOverlay != null)
        {
            fadeOverlay.gameObject.SetActive(true);
            var c = fadeOverlay.color;
            c.a = 0f;
            fadeOverlay.color = c;
        }
    }


    /// <summary>
    /// Loads a room scene with fade out and in.
    /// </summary>
    public void LoadRoom(string sceneName, string spawnPointId = null, bool saveGame = true)
    {
        if (fadeOverlay == null)
        {
            Debug.LogWarning("No fade overlay assigned on RoomManager!");
            SceneManager.LoadScene(sceneName);
            return;
        }

        CurrentRoom = sceneName;
        pendingSpawnId = spawnPointId;
        StartCoroutine(FadeTransition(sceneName,false, saveGame));
    }


    /// <summary>
    /// Loads a room scene with fade out and in.
    /// </summary>
    public void LoadRoomInstant(string sceneName, string spawnPointId = null, bool saveGame = true)
    {
        if (fadeOverlay == null)
        {
            Debug.LogWarning("No fade overlay assigned on RoomManager!");
            SceneManager.LoadScene(sceneName);
            return;
        }


        CurrentRoom = sceneName;
        pendingSpawnId = spawnPointId;
        StartCoroutine(UIFadeTransition(sceneName, false, saveGame));
    }

    /// <summary>
    /// Loads a room scene with fade out and in.
    /// </summary>
    public void LoadRoom(string sceneName, float customFadeTime, bool lateToTimerTeset = false, string spawnPointId = null, bool SaveAfterLoading = true)
    {
        fadeDuration = customFadeTime;
        if (fadeOverlay == null)
        {
            Debug.LogWarning("No fade overlay assigned on RoomManager!");
            SceneManager.LoadScene(sceneName);
            return;
        }

        CurrentRoom = sceneName;
        pendingSpawnId = spawnPointId;
        StartCoroutine(FadeTransition(sceneName, true, SaveAfterLoading));

    }

    private IEnumerator UIFadeTransition(string sceneName, bool lateTimeReset = false, bool SaveAfterLoading = true)
    {
        // Fade out
        yield return StartCoroutine(UIFade(0f, 1f));
        onFadedOut?.Invoke();
        // Load scene
        if (SaveAfterLoading)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {

            SceneManager.sceneLoaded += OnSceneLoadedNoSave;
        }
        SceneManager.LoadScene(sceneName);

        // Wait one frame so scene loads before fading in
        yield return null;
        if (!lateTimeReset)
            fadeDuration = ogFadeDuration;
        // Fade in
        yield return StartCoroutine(UIFade(1f, 0f));
        if (lateTimeReset)
            fadeDuration = ogFadeDuration;

    }

    private IEnumerator FadeTransition(string sceneName, bool lateTimeReset = false, bool SaveAfterLoading = true)
    {
        // Fade out
        yield return StartCoroutine(Fade(0f, 1f));

        onFadedOut?.Invoke();
        // Load scene
        if (SaveAfterLoading)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {

            SceneManager.sceneLoaded += OnSceneLoadedNoSave;
        }
        SceneManager.LoadScene(sceneName);

        // Wait one frame so scene loads before fading in
        yield return null;
        if (!lateTimeReset)
            fadeDuration = ogFadeDuration;
        // Fade in
        yield return StartCoroutine(Fade(1f, 0f));
        if (lateTimeReset)
            fadeDuration = ogFadeDuration;

    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        Color c = fadeOverlay.color;
        CanvasGroup UICanvasGroup = FindObjectOfType<CanvasGroup>();


        while (t < fadeDuration)
        {

            t += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, t / fadeDuration);
            if (UICanvasGroup)
            {
                //UICanvasGroup.interactable = false;
                UICanvasGroup.alpha = 1 - alpha;
            }
            fadeOverlay.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        fadeOverlay.color = new Color(c.r, c.g, c.b, to);
        if (UICanvasGroup)
        {
            UICanvasGroup.alpha = 1 - to;
            //UICanvasGroup.interactable = to<from;
        }
    }

    private IEnumerator UIFade(float from, float to)
    {
        float t = 0f;
        CanvasGroup UICanvasGroup = FindObjectOfType<CanvasGroup>();


        while (t < fadeDuration)
        {

            t += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, t / fadeDuration);
            if (UICanvasGroup)
            {
                //UICanvasGroup.interactable = false;
                UICanvasGroup.alpha = 1 - alpha;
            }
            yield return null;
        }


        if (UICanvasGroup)
        {
            UICanvasGroup.alpha = 1 - to;
            //UICanvasGroup.interactable = to<from;
        }
    }

    private void OnSceneLoadedNoSave(Scene scene, LoadSceneMode mode)
    {

        if (!string.IsNullOrEmpty(pendingSpawnId))
        {
            var spawn = GameObject.Find(pendingSpawnId);
            if (spawn != null)
            {
                var player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    player.transform.position = spawn.transform.position;
                }
            }
            pendingSpawnId = null;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (!string.IsNullOrEmpty(pendingSpawnId))
        {
            var spawn = GameObject.Find(pendingSpawnId);
            if (spawn != null)
            {
                var player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    player.transform.position = spawn.transform.position;
                }
            }
            pendingSpawnId = null;
        }

        //SaveLoadManager.Instance.Save();

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

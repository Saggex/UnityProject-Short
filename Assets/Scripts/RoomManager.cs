using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

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
    [SerializeField] private float fadeDuration = 1f;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return;

        CurrentRoom = SceneManager.GetActiveScene().name;

        // Ensure overlay starts invisible
        if (fadeOverlay != null)
        {
            var c = fadeOverlay.color;
            c.a = 0f;
            fadeOverlay.color = c;
        }
    }

    /// <summary>
    /// Loads a room scene with fade out and in.
    /// </summary>
    public void LoadRoom(string sceneName, string spawnPointId = null)
    {
        if (fadeOverlay == null)
        {
            Debug.LogWarning("No fade overlay assigned on RoomManager!");
            SceneManager.LoadScene(sceneName);
            return;
        }

        CurrentRoom = sceneName;
        pendingSpawnId = spawnPointId;
        StartCoroutine(FadeTransition(sceneName));
    }

    private IEnumerator FadeTransition(string sceneName)
    {
        // Fade out
        yield return StartCoroutine(Fade(0f, 1f));

        // Load scene
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);

        // Wait one frame so scene loads before fading in
        yield return null;

        // Fade in
        yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        Color c = fadeOverlay.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, t / fadeDuration);
            fadeOverlay.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        fadeOverlay.color = new Color(c.r, c.g, c.b, to);
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

        SaveLoadManager.Instance.Save();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles room transitions and atmosphere settings per room.
/// </summary>
public class RoomManager : PersistentSingleton<RoomManager>
{
    public string CurrentRoom { get; private set; }

    private string pendingSpawnId;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
        {
            return;
        }
        CurrentRoom = SceneManager.GetActiveScene().name;
    }


    [SerializeField] private SoundManager soundManager;
    /// <summary>
    /// Loads a room scene by name and records it as the current room.
    /// Optionally moves the player to a spawn point in the new scene.
    /// </summary>
    public void LoadRoom(string sceneName, string spawnPointId = null)
    {
        CurrentRoom = sceneName;
        pendingSpawnId = spawnPointId;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
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
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

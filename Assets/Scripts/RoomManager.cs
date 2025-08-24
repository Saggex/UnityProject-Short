using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles room transitions and atmosphere settings per room.
/// </summary>
public class RoomManager : PersistentSingleton<RoomManager>
{
    public string CurrentRoom { get; private set; }

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
    /// </summary>
    public void LoadRoom(string sceneName)
    {
        CurrentRoom = sceneName;
        SceneManager.LoadScene(sceneName);
    }


}

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles room transitions and atmosphere settings per room.
/// </summary>
public class RoomManager : PersistentSingleton<RoomManager>
{
    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
        {
            return;
        }
    }


    [SerializeField] private SoundManager soundManager;
    /// <summary>
    /// Loads a room scene by name.
    /// </summary>
    public void LoadRoom(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    
}

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles room transitions and atmosphere settings per room.
/// </summary>
public class RoomManager : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    /// <summary>
    /// Loads a room scene by name.
    /// </summary>
    public void LoadRoom(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    
}

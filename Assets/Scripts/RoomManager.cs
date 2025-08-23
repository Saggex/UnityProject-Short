using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles room transitions and atmosphere settings per room.
/// </summary>
public class RoomManager : MonoBehaviour
{
    /// <summary>
    /// Loads a room scene by name.
    /// </summary>
    public void LoadRoom(string sceneName)
    {
        // Placeholder for transition effects.
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Applies unique sound and lighting settings for a room.
    /// </summary>
    public void ApplyAtmosphere(string roomId)
    {
        // Implementation specific to project setup.
    }
}

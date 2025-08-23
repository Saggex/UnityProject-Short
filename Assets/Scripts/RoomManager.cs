using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles room transitions and atmosphere settings per room.
/// </summary>
public class RoomManager : MonoBehaviour
{
    [System.Serializable]
    public class RoomSettings
    {
        public string roomId;
        public AudioClip ambientClip;
        public Color ambientLight = Color.black;
    }

    [SerializeField] private SoundManager soundManager;
    [SerializeField] private RoomSettings[] rooms;

    /// <summary>
    /// Loads a room scene by name.
    /// </summary>
    public void LoadRoom(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Applies unique sound and lighting settings for a room.
    /// </summary>
    public void ApplyAtmosphere(string roomId)
    {
        foreach (var room in rooms)
        {
            if (room.roomId != roomId) continue;
            if (soundManager != null && room.ambientClip != null)
            {
                soundManager.PlayMusic(room.ambientClip);
            }

            RenderSettings.ambientLight = room.ambientLight;
            break;
        }
    }
}

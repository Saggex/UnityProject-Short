using UnityEngine;

/// <summary>
/// Represents a doorway that transitions the player to another room.
/// </summary>
public class Door : MonoBehaviour
{
    [SerializeField] private string targetScene;

    private RoomManager roomManager;

    private void Awake()
    {
        roomManager = FindObjectOfType<RoomManager>();
    }

    /// <summary>
    /// Loads the configured scene and applies atmosphere settings.
    /// </summary>
    public void Enter()
    {
        if (roomManager == null) return;
        if (!string.IsNullOrEmpty(targetScene))
        {
            roomManager.LoadRoom(targetScene);
        }
    }
}

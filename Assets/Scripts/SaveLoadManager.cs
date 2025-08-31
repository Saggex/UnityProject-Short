using System.Collections;
using UnityEngine;

/// <summary>
/// Handles serialising and deserialising persistent game data.
/// Uses Unity's PlayerPrefs system for storage.
/// </summary>
public class SaveLoadManager : PersistentSingleton<SaveLoadManager>
{
    private const string RoomKey = "Save_Room";
    private const string PlayerXKey = "Save_Player_X";
    private const string PlayerYKey = "Save_Player_Y";
    private const string PlayerZKey = "Save_Player_Z";

    /// <summary>
    /// Saves the current game state to PlayerPrefs.
    /// </summary>
    public void Save()
    {
        PlayerPrefs.SetString(RoomKey, RoomManager.Instance.CurrentRoom);

        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 pos = player.transform.position;
            PlayerPrefs.SetFloat(PlayerXKey, pos.x);
            PlayerPrefs.SetFloat(PlayerYKey, pos.y);
            PlayerPrefs.SetFloat(PlayerZKey, pos.z);
        }

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads the game state from PlayerPrefs.
    /// </summary>
    public void Load()
    {
        if (!SaveExists()) return;
        StartCoroutine(LoadRoutine());
    }

    private IEnumerator LoadRoutine()
    {
        string roomName = PlayerPrefs.GetString(RoomKey, string.Empty);
        if (!string.IsNullOrEmpty(roomName))
        {
            RoomManager.Instance.LoadRoom(roomName);
            yield return null;
        }

        var player = GameObject.FindWithTag("Player");
        if (player != null && PlayerPrefs.HasKey(PlayerXKey) && PlayerPrefs.HasKey(PlayerYKey) && PlayerPrefs.HasKey(PlayerZKey))
        {
            player.transform.position = new Vector3(
                PlayerPrefs.GetFloat(PlayerXKey),
                PlayerPrefs.GetFloat(PlayerYKey),
                PlayerPrefs.GetFloat(PlayerZKey));
        }
    }

    /// <summary>
    /// Deletes the save data.
    /// </summary>
    public void Delete()
    {
        DestroyState.ResetAll();
        InventorySystem.Instance?.ClearInventory();
        PlayerPrefs.DeleteKey(RoomKey);
        PlayerPrefs.DeleteKey(PlayerXKey);
        PlayerPrefs.DeleteKey(PlayerYKey);
        PlayerPrefs.DeleteKey(PlayerZKey);
        PlayerPrefs.DeleteKey(InventorySystem.InventoryKey);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Returns whether save data exists.
    /// </summary>
    public bool SaveExists()
    {
        return PlayerPrefs.HasKey(RoomKey);
    }
}

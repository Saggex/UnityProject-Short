using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Handles serialising and deserialising persistent game data.
/// Uses a single JSON save file under
/// <see cref="Application.persistentDataPath"/>.
/// </summary>
public class SaveLoadManager : PersistentSingleton<SaveLoadManager>
{
    private const string FileName = "save.json";

    /// <summary>
    /// Data representation of the player's progress.
    /// </summary>
    [System.Serializable]
    public class SaveData
    {
        public string roomName;
        public float[] playerPosition;
        public List<string> inventoryIds;
    }

    /// <summary>
    /// Saves the current game state to disk.
    /// </summary>
    public void Save()
    {
        var data = new SaveData();
        data.roomName = RoomManager.Instance.CurrentRoom;

        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 pos = player.transform.position;
            data.playerPosition = new float[3] { pos.x, pos.y, pos.z };
        }
        data.inventoryIds = InventorySystem.Instance.GetItemIds();

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(), json);
    }

    /// <summary>
    /// Loads the game state from disk.
    /// </summary>
    public void Load()
    {
        string path = GetPath();
        if (!File.Exists(path)) return;
        string json = File.ReadAllText(path);
        var data = JsonUtility.FromJson<SaveData>(json);
        StartCoroutine(LoadRoutine(data));
    }

    private IEnumerator LoadRoutine(SaveData data)
    {
        RoomManager.Instance.LoadRoom(data.roomName);
        yield return null;

        var player = GameObject.FindWithTag("Player");
        if (player != null && data.playerPosition != null && data.playerPosition.Length == 3)
        {
            player.transform.position = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
        }

        InventorySystem.Instance.SetItemsByIds(data.inventoryIds);
    }

    /// <summary>
    /// Deletes the save file.
    /// </summary>
    public void Delete()
    {
        string path = GetPath();
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    /// <summary>
    /// Returns whether a save file exists.
    /// </summary>
    public bool SaveExists()
    {
        return File.Exists(GetPath());
    }

    private string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, FileName);
    }
}

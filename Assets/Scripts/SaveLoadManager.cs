using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles serialising and deserialising persistent game data.
/// Supports multiple save slots stored as JSON files under
/// <see cref="Application.persistentDataPath"/>.
/// </summary>
public class SaveLoadManager : PersistentSingleton<SaveLoadManager>
{
    private const string FilePattern = "save_{0}.json";

    /// <summary>
    /// Data representation of the player's progress.
    /// </summary>
    [System.Serializable]
    public class SaveData
    {
        public string sceneName;
        public float[] playerPosition;
        public List<string> inventoryIds;
    }

    /// <summary>
    /// Saves the current game state to the specified slot.
    /// </summary>
    public void Save(int slot)
    {
        var data = new SaveData();
        data.sceneName = SceneManager.GetActiveScene().name;

        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 pos = player.transform.position;
            data.playerPosition = new float[3] { pos.x, pos.y, pos.z };
        }
        data.inventoryIds = InventorySystem.Instance.GetItemIds();

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(slot), json);
    }

    /// <summary>
    /// Loads the game state from the specified slot.
    /// </summary>
    public void Load(int slot)
    {
        string path = GetPath(slot);
        if (!File.Exists(path)) return;
        string json = File.ReadAllText(path);
        var data = JsonUtility.FromJson<SaveData>(json);
        StartCoroutine(LoadRoutine(data));
    }

    private IEnumerator LoadRoutine(SaveData data)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(data.sceneName);
        while (!op.isDone)
        {
            yield return null;
        }

        var player = GameObject.FindWithTag("Player");
        if (player != null && data.playerPosition != null && data.playerPosition.Length == 3)
        {
            player.transform.position = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
        }

        InventorySystem.Instance.SetItemsByIds(data.inventoryIds);
    }

    /// <summary>
    /// Deletes the save file at the given slot.
    /// </summary>
    public void Delete(int slot)
    {
        string path = GetPath(slot);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    /// <summary>
    /// Returns whether a save file exists in the slot.
    /// </summary>
    public bool SaveExists(int slot)
    {
        return File.Exists(GetPath(slot));
    }

    private string GetPath(int slot)
    {
        return Path.Combine(Application.persistentDataPath, string.Format(FilePattern, slot));
    }
}

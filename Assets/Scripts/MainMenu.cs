using UnityEngine;

/// <summary>
/// Handles top level menu actions such as starting a new game,
/// loading a save and quitting the application.
/// Hook these methods up to UI buttons.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] private string firstSceneName = "Bedroom";

    /// <summary>
    /// Starts a fresh game by clearing the save file
    /// and loading the first gameplay scene.
    /// </summary>
    public void StartNewGame()
    {
        SaveLoadManager.Instance.Delete();
        InventorySystem.Instance.SetItemsByIds(null);
        RoomManager.Instance.LoadRoom(firstSceneName);
    }

    /// <summary>
    /// Continues from the existing save if present.
    /// </summary>
    public void ContinueGame()
    {
        if (SaveLoadManager.Instance.SaveExists())
        {
            SaveLoadManager.Instance.Load();
        }
    }

    /// <summary>
    /// Exits the application.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

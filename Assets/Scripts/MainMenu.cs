using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles top level menu actions such as starting a new game,
/// loading a save and quitting the application.
/// Hook these methods up to UI buttons.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] private string firstSceneName = "Bedroom";

    /// <summary>
    /// Starts a fresh game by clearing the specified save slot
    /// and loading the first gameplay scene.
    /// </summary>
    public void StartNewGame(int slot)
    {
        SaveLoadManager.Instance.Delete(slot);
        InventorySystem.Instance.SetItemsByIds(null);
        SceneManager.LoadScene(firstSceneName);
    }

    /// <summary>
    /// Loads a saved game from the provided slot if one exists.
    /// </summary>
    public void LoadGame(int slot)
    {
        if (SaveLoadManager.Instance.SaveExists(slot))
        {
            SaveLoadManager.Instance.Load(slot);
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

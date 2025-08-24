using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Toggles game pausing and exposes UI events for saving,
/// loading or returning to the main menu.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuRoot;
    [SerializeField] private string mainMenuScene = "MainMenu";
    private bool isPaused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume(); else Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        if (menuRoot != null) menuRoot.SetActive(true);
        isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        if (menuRoot != null) menuRoot.SetActive(false);
        isPaused = false;
    }

    public void SaveGame()
    {
        SaveLoadManager.Instance.Save();
    }

    public void LoadGame()
    {
        SaveLoadManager.Instance.Load();
        Resume();
    }

    public void QuitToMenu()
    {
        GameObject.Destroy(UIManager.Instance.gameObject);
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneName; // The name of the scene to load

    private void Start()
    {
        // If this script is on a Button, hook up the click event automatically
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => LoadScene(sceneName));
        }
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}

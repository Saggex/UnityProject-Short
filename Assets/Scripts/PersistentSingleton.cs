using UnityEngine;

/// <summary>
/// Generic singleton base that persists across scene loads.
/// </summary>
public abstract class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// The single instance of this component.
    /// </summary>
    public static T Instance { get; private set; }

    /// <summary>
    /// Ensures only one instance exists and persists across scenes.
    /// </summary>
    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this as T;
        DontDestroyOnLoad(gameObject);
    }
}

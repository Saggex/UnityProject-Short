#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// Utility for generating placeholder prefabs for core systems.
/// </summary>
public static class PrefabBuilder
{
    private const string PrefabFolder = "Assets/Prefabs";

    [MenuItem("Tools/Build Prefabs")]
    public static void BuildPrefabs()
    {
        EnsureFolder();
        CreatePrefab<PlayerController>("Player");
        CreatePrefab<InventorySystem>("InventorySystem");
        CreatePrefab<PuzzleManager>("PuzzleManager");
        CreatePrefab<GhostAI>("Ghost");
        CreatePrefab<RoomManager>("RoomManager");
        CreatePrefab<SoundManager>("SoundManager");
        CreatePrefab<UIManager>("UIManager");
        CreatePrefab<QuestManager>("QuestManager");
    }

    private static void EnsureFolder()
    {
        if (!AssetDatabase.IsValidFolder(PrefabFolder))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
    }

    private static void CreatePrefab<T>(string name) where T : Component
    {
        var go = new GameObject(name);
        go.AddComponent<T>();
        var path = $"{PrefabFolder}/{name}.prefab";
        PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
    }
}
#endif

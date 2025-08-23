#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Automates initial game setup by creating required prefabs and
/// populating all scenes with core objects.
/// </summary>
public static class GameSetupEditor
{
    private const string CanvasPrefabPath = "Assets/Prefabs/UICanvas.prefab";
    private const string InventoryButtonPrefabPath = "Assets/Prefabs/InventoryButton.prefab";

    [MenuItem("Tools/Setup Game")]
    public static void SetupGame()
    {
        EnsureSortingLayers();
        CreateCanvasPrefab();

        var sceneGuids = AssetDatabase.FindAssets("t:scene", new[] { "Assets/Scenes" });
        foreach (var guid in sceneGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var scene = EditorSceneManager.OpenScene(path);
            EnsureSpriteLayers();
            EnsureGameplayObjects();
            EditorSceneManager.SaveScene(scene);
        }

        Debug.Log("Game setup complete.");
    }

    private static void EnsureSpriteLayers()
    {
        EnsureRootObject("Background");
        EnsureRootObject("Middleground");
        EnsureRootObject("Foreground");
    }

    private static void EnsureRootObject(string name)
    {
        if (GameObject.Find(name) == null)
        {
            new GameObject(name);
        }
    }

    private static void EnsureGameplayObjects()
    {
        var inventory = Object.FindObjectOfType<InventorySystem>();
        if (inventory == null)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/InventorySystem.prefab");
            var inst = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            inventory = inst.GetComponent<InventorySystem>();
        }

        var ui = Object.FindObjectOfType<UIManager>();
        if (ui == null)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(CanvasPrefabPath);
            var inst = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            ui = inst.GetComponent<UIManager>();
        }

        var player = Object.FindObjectOfType<PlayerController>();
        if (player == null)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
            var inst = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            player = inst.GetComponent<PlayerController>();
        }

        var playerSO = new SerializedObject(player);
        playerSO.FindProperty("inventory").objectReferenceValue = inventory;
        playerSO.FindProperty("ui").objectReferenceValue = ui;
        playerSO.ApplyModifiedProperties();
    }

    private static void CreateCanvasPrefab()
    {
        if (AssetDatabase.LoadAssetAtPath<GameObject>(CanvasPrefabPath) != null)
            return;

        var canvasGO = new GameObject("Canvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();
        var uiManager = canvasGO.AddComponent<UIManager>();
        var inventoryUI = canvasGO.AddComponent<InventoryUI>();

        var inventoryPanel = new GameObject("InventoryPanel");
        inventoryPanel.transform.SetParent(canvasGO.transform, false);
        var panelImage = inventoryPanel.AddComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.5f);
        inventoryPanel.SetActive(false);

        var gridGO = new GameObject("InventoryGrid");
        gridGO.transform.SetParent(inventoryPanel.transform, false);

        var flavourTextGO = new GameObject("FlavourText");
        flavourTextGO.transform.SetParent(canvasGO.transform, false);
        var flavourText = flavourTextGO.AddComponent<Text>();
        flavourText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        var promptGO = new GameObject("Prompt");
        promptGO.transform.SetParent(canvasGO.transform, false);
        var promptText = promptGO.AddComponent<Text>();
        promptText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        promptGO.SetActive(false);

        var uiSO = new SerializedObject(uiManager);
        uiSO.FindProperty("inventoryContainer").objectReferenceValue = gridGO.transform;
        uiSO.FindProperty("inventoryButtonPrefab").objectReferenceValue =
            AssetDatabase.LoadAssetAtPath<InventoryButton>(InventoryButtonPrefabPath);
        uiSO.FindProperty("flavourText").objectReferenceValue = flavourText;
        uiSO.FindProperty("prompt").objectReferenceValue = promptGO;
        uiSO.ApplyModifiedProperties();

        var invSO = new SerializedObject(inventoryUI);
        invSO.FindProperty("inventoryPanel").objectReferenceValue = inventoryPanel;
        invSO.ApplyModifiedProperties();

        var eventSystemGO = new GameObject("EventSystem");
        eventSystemGO.AddComponent<EventSystem>();
        eventSystemGO.AddComponent<StandaloneInputModule>();

        PrefabUtility.SaveAsPrefabAsset(canvasGO, CanvasPrefabPath);
        Object.DestroyImmediate(canvasGO);
        Object.DestroyImmediate(eventSystemGO);
    }

    private static void EnsureSortingLayers()
    {
        var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        var layersProp = tagManager.FindProperty("m_SortingLayers");
        AddSortingLayer(layersProp, "Background");
        AddSortingLayer(layersProp, "Middleground");
        AddSortingLayer(layersProp, "Foreground");
        tagManager.ApplyModifiedProperties();
    }

    private static void AddSortingLayer(SerializedProperty layersProp, string name)
    {
        for (int i = 0; i < layersProp.arraySize; i++)
        {
            var entry = layersProp.GetArrayElementAtIndex(i);
            if (entry.FindPropertyRelative("name").stringValue == name)
                return;
        }
        layersProp.InsertArrayElementAtIndex(layersProp.arraySize);
        var newEntry = layersProp.GetArrayElementAtIndex(layersProp.arraySize - 1);
        newEntry.FindPropertyRelative("name").stringValue = name;
    }
}
#endif

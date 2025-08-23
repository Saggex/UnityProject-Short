using UnityEngine;

/// <summary>
/// Toggles the inventory panel when the player presses a key.
/// </summary>
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private KeyCode toggleKey = KeyCode.I;
    [SerializeField] private UIManager ui;

    private void Awake()
    {
        Debug.Log("[InventoryUI] Initialized");
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey) && inventoryPanel != null)
        {
            bool newActive = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(newActive);
            Debug.Log($"[InventoryUI] Inventory panel active = {inventoryPanel.activeSelf}");
            if (newActive)
            {
                ui?.RefreshInventory();
            }
        }
    }
}

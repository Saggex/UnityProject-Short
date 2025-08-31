using UnityEngine;

/// <summary>
/// Toggles the inventory panel when the player presses a key.
/// </summary>
    public class InventoryUI : PersistentSingleton<InventoryUI>
    {
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private KeyCode toggleKey = KeyCode.I;

        public bool IsInventoryOpen => inventoryPanel != null && inventoryPanel.activeSelf;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
        {
            return;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey) && inventoryPanel != null)
        {
            showInventory();
        }
    }

    public void showInventory()
    {
        bool newActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(newActive);
        if (newActive)
        {
            UIManager.Instance?.RefreshInventory();
        }
    }
}

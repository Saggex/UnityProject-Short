using UnityEngine;

/// <summary>
/// Toggles the inventory panel when the player presses a key.
/// </summary>
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private KeyCode toggleKey = KeyCode.I;

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey) && inventoryPanel != null)
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
}

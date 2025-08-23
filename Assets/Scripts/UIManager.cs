using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays inventory, flavour text, and interaction prompts.
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform inventoryContainer;
    [SerializeField] private InventoryButton inventoryButtonPrefab;
    [SerializeField] private Text flavourText;
    [SerializeField] private GameObject prompt;
    [SerializeField] private InventorySystem inventory;

    private void Awake()
    {
        if (inventory == null)
        {
            inventory = FindObjectOfType<InventorySystem>();
        }
        if (inventory != null)
        {
            inventory.ItemAdded += OnInventoryChanged;
            inventory.ItemRemoved += OnInventoryChanged;
            Debug.Log("[UIManager] Bound to InventorySystem and subscribed to events");
            RefreshInventory(inventory);
        }
        else
        {
            Debug.LogWarning("[UIManager] No InventorySystem found");
        }
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.ItemAdded -= OnInventoryChanged;
            inventory.ItemRemoved -= OnInventoryChanged;
            Debug.Log("[UIManager] Unsubscribed from InventorySystem events");
        }
    }

    private void OnInventoryChanged(Item item)
    {
        Debug.Log($"[UIManager] Inventory changed due to {item.DisplayName}");
        RefreshInventory();
    }

    /// <summary>
    /// Updates the inventory UI with current items.
    /// </summary>
    public void RefreshInventory(InventorySystem inv = null)
    {
        if (inv != null)
        {
            inventory = inv;
        }

        if (inventoryContainer == null || inventoryButtonPrefab == null || inventory == null)
        {
            Debug.Log("[UIManager] Missing references for inventory refresh");
            return;
        }

        foreach (Transform child in inventoryContainer)
        {
            Destroy(child.gameObject);
        }

        int index = 0;
        foreach (var item in inventory.GetAllItems())
        {
            var button = Instantiate(inventoryButtonPrefab, inventoryContainer);
            var rt = button.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2((index % 4) * 70, -(index / 4) * 70);
            button.Initialize(item, this);
            Debug.Log($"[UIManager] Created button for {item.DisplayName} at index {index}");
            index++;
        }
    }

    /// <summary>
    /// Shows flavour text in the UI.
    /// </summary>
    public void ShowFlavourText(string text)
    {
        if (flavourText == null) return;
        Debug.Log($"[UIManager] Flavour text: {text}");
        flavourText.text = text;
    }

    /// <summary>
    /// Shows or hides an interaction prompt.
    /// </summary>
    public void TogglePrompt(bool isVisible)
    {
        if (prompt != null)
        {
            Debug.Log($"[UIManager] Prompt visibility set to {isVisible}");
            prompt.SetActive(isVisible);
        }
    }
}

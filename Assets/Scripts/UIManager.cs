using UnityEngine;
/// <summary>
/// Displays inventory, flavour text, and interaction prompts.
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform inventoryContainer;
    [SerializeField] private InventoryButton inventoryButtonPrefab;
    [SerializeField] private TypewriterText flavourText;
    [SerializeField] private TypewriterText prompt;
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
            RefreshInventory(inventory);
        }
        else
        {
            Debug.LogWarning("No InventorySystem found");
        }
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.ItemAdded -= OnInventoryChanged;
            inventory.ItemRemoved -= OnInventoryChanged;
        }
    }

    private void OnInventoryChanged(Item item)
    {
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
            index++;
        }
    }

    /// <summary>
    /// Shows flavour text in the UI.
    /// </summary>
    public void ShowFlavourText(string text)
    {
        flavourText?.Show(text);
    }

    /// <summary>
    /// Shows an interaction prompt.
    /// </summary>
    public void ShowPrompt(string text)
    {
        prompt?.Show(text);
    }

    /// <summary>
    /// Hides the interaction prompt immediately.
    /// </summary>
    public void HidePrompt()
    {
        prompt?.Hide();
    }
}

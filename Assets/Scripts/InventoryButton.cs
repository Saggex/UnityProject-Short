using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI button representing an inventory item. Displays the item's sprite and
/// forwards click events to the UIManager for flavour text display.
/// </summary>
[RequireComponent(typeof(Button), typeof(Image))]
public class InventoryButton : MonoBehaviour
{
    public Item item;
    public UIManager ui;
    public Image icon;
    public Button button;

    private void Awake()
    {
        icon = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    /// <summary>
    /// Initializes the button with an item and UI manager reference.
    /// </summary>
    public void Initialize(Item item, UIManager ui)
    {
        this.item = item;
        this.ui = ui;
        if (icon != null)
        {
            icon.sprite = item.Sprite;
        }
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }
        Debug.Log($"[InventoryButton] Initialized for {item.DisplayName}");
    }
    
    public void OnClick()
    {
        if (ui != null && item != null)
        {
            Debug.Log($"[InventoryButton] {item.DisplayName} clicked");
            ui.ShowFlavourText(item.Description);
        }
    }
}

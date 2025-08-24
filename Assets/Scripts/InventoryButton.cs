using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI button representing an inventory item. Displays the item's sprite and
/// forwards click events to the UIManager for flavour text display.
/// </summary>
[RequireComponent(typeof(Button))]
public class InventoryButton : MonoBehaviour
{
    public Item item;
    public Image icon;
    public Button button;

    private void Awake()
    {
        if(!icon)
        icon = GetComponentInChildren<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    /// <summary>
    /// Initializes the button with an item.
    /// </summary>
    public void Initialize(Item item)
    {
        this.item = item;
        if (icon != null)
        {
            icon.sprite = item.Sprite;
        }
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }
    }

    private void OnClick()
    {
        if (item != null)
        {
            UIManager.Instance?.ShowFlavourText(item.Description);
        }
    }
}

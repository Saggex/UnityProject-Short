using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays inventory, flavour text, and interaction prompts.
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private Text inventoryText;
    [SerializeField] private Text flavourText;
    [SerializeField] private GameObject prompt;

    /// <summary>
    /// Updates the inventory UI with current items.
    /// </summary>
    public void RefreshInventory(InventorySystem inventory)
    {
        if (inventoryText == null || inventory == null) return;
        inventoryText.text = string.Join(", ", inventory.GetAllItems().Select(i => i.DisplayName));
    }

    /// <summary>
    /// Shows flavour text in the UI.
    /// </summary>
    public void ShowFlavourText(string text)
    {
        if (flavourText == null) return;
        flavourText.text = text;
    }

    /// <summary>
    /// Shows or hides an interaction prompt.
    /// </summary>
    public void TogglePrompt(bool isVisible)
    {
        if (prompt != null)
        {
            prompt.SetActive(isVisible);
        }
    }
}

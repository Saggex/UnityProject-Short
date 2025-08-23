<<<<<<< HEAD
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
=======
using UnityEngine;
>>>>>>> main

/// <summary>
/// Displays inventory, flavour text, and interaction prompts.
/// </summary>
public class UIManager : MonoBehaviour
{
<<<<<<< HEAD
    [SerializeField] private Text inventoryText;
    [SerializeField] private Text flavourText;
    [SerializeField] private GameObject prompt;

=======
>>>>>>> main
    /// <summary>
    /// Updates the inventory UI with current items.
    /// </summary>
    public void RefreshInventory(InventorySystem inventory)
    {
<<<<<<< HEAD
        if (inventoryText == null || inventory == null) return;
        inventoryText.text = string.Join(", ", inventory.GetAllItems().Select(i => i.DisplayName));
=======
        // Placeholder for UI refresh logic.
>>>>>>> main
    }

    /// <summary>
    /// Shows flavour text in the UI.
    /// </summary>
    public void ShowFlavourText(string text)
    {
<<<<<<< HEAD
        if (flavourText == null) return;
        flavourText.text = text;
=======
        // Placeholder for displaying text to the player.
>>>>>>> main
    }

    /// <summary>
    /// Shows or hides an interaction prompt.
    /// </summary>
    public void TogglePrompt(bool isVisible)
    {
<<<<<<< HEAD
        if (prompt != null)
        {
            prompt.SetActive(isVisible);
        }
=======
        // Placeholder for prompt visibility.
>>>>>>> main
    }
}

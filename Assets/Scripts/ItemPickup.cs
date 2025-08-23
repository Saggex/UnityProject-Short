using UnityEngine;

/// <summary>
/// Component that allows the player to pick up a referenced Item via interaction.
/// </summary>
public class ItemPickup : MonoBehaviour
{
    [SerializeField] private Item item;

    /// <summary>
    /// The item granted when picked up.
    /// </summary>
    public Item Item => item;
}

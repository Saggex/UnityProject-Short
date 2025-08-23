using UnityEngine;

/// <summary>
/// Base class for game items that can be picked up and used.
/// </summary>
[CreateAssetMenu(menuName = "Game/Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private string displayName;
    [TextArea] [SerializeField] private string description;
    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;
    public string Id => id;
    public string DisplayName => displayName;
    public string Description => description;
}

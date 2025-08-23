using UnityEngine;

/// <summary>
/// Changes the sprite color when the player is close enough to interact.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ProximityHighlight : MonoBehaviour, IHighlightable
{
    [SerializeField] private Color highlightColor = Color.yellow;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    /// <inheritdoc />
    public void SetHighlighted(bool highlighted)
    {
        if (spriteRenderer == null) return;
        spriteRenderer.color = highlighted ? highlightColor : originalColor;
    }
}

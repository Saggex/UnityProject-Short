using UnityEngine;

/// <summary>
/// Swaps the sprite's material to an outline version when highlighted.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Highlightable : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    private Material originalMaterial;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalMaterial = sr.material;
    }

    /// <summary>
    /// Enable or disable outline highlight.
    /// </summary>
    public void SetHighlighted(bool highlighted)
    {
        if (sr == null) return;
        sr.material = highlighted ? outlineMaterial : originalMaterial;
    }
}

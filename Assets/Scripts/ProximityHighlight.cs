using UnityEngine;

/// <summary>
/// Changes the sprite color when the player is close enough to interact.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ProximityHighlight : MonoBehaviour, IHighlightable
{
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private float outlineSize = 1f;
    [SerializeField] private Shader outlineShader;

    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Material outlineMaterial;

    private void Awake()
    {
        Debug.Log($"[ProximityHighlight] Awake on {name}");
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalMaterial = spriteRenderer.material;
        }
        else
        {
            Debug.LogWarning($"[ProximityHighlight] No SpriteRenderer found on {name}");
        }

        if (outlineShader == null)
        {
            outlineShader = Shader.Find("Custom/Outline");
            Debug.Log($"[ProximityHighlight] Outline shader {(outlineShader != null ? "found" : "not found")} via Shader.Find on {name}");
        }

        if (outlineShader != null)
        {
            outlineMaterial = new Material(outlineShader);
            Debug.Log($"[ProximityHighlight] Outline material created for {name}");
        }
        else
        {
            Debug.LogWarning($"[ProximityHighlight] Outline shader missing for {name}, highlighting will fallback to original material");
        }
    }

    /// <inheritdoc />
    public void SetHighlighted(bool highlighted)
    {
        if (spriteRenderer == null)
        {
            Debug.LogWarning($"[ProximityHighlight] SpriteRenderer missing on {name} when trying to set highlight");
            return;
        }

        Debug.Log($"[ProximityHighlight] SetHighlighted({highlighted}) on {name}");

        if (highlighted && outlineMaterial != null)
        {
            outlineMaterial.SetColor("_OutlineColor", highlightColor);
            outlineMaterial.SetFloat("_OutlineSize", outlineSize);
            spriteRenderer.material = outlineMaterial;
            Debug.Log($"[ProximityHighlight] Applied outline material to {name}");
        }
        else
        {
            spriteRenderer.material = originalMaterial;
            Debug.Log($"[ProximityHighlight] Restored original material on {name}");
        }
    }
}

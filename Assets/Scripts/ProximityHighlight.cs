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
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalMaterial = spriteRenderer.material;
        }
        else
        {
            Debug.LogWarning($"No SpriteRenderer found on {name}");
        }

        if (outlineShader == null)
        {
            outlineShader = Shader.Find("Custom/Outline");
        }

        if (outlineShader != null)
        {
            outlineMaterial = new Material(outlineShader);
            outlineMaterial.CopyPropertiesFromMaterial(originalMaterial);
        }
        else
        {
            Debug.LogWarning($"Outline shader missing for {name}, highlighting will fallback to original material");
        }
    }

    /// <inheritdoc />
    public void SetHighlighted(bool highlighted)
    {
        if (spriteRenderer == null)
        {
            Debug.LogWarning($"SpriteRenderer missing on {name} when trying to set highlight");
            return;
        }

        if (highlighted && outlineMaterial != null)
        {
            //outlineMaterial.CopyPropertiesFromMaterial(originalMaterial);
            outlineMaterial.SetColor("_OutlineColor", highlightColor);
            outlineMaterial.SetFloat("_OutlineSize", outlineSize);
            
            spriteRenderer.material = outlineMaterial;
           
        }
        else
        {
            spriteRenderer.material = originalMaterial;
        }
    }
}

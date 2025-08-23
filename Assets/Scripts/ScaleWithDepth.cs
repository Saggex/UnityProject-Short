using UnityEngine;

// Attach to the character
public class ScaleWithDepth : MonoBehaviour
{
    public float topY = 5f;     // far point in background
    public float bottomY = -5f; // near point in background
    public float minScale = 0.5f;
    public float maxScale = 1.5f;

    void Update()
    {
        float t = Mathf.InverseLerp(topY, bottomY, transform.position.y);
        float scale = Mathf.Lerp(minScale, maxScale, t);
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}

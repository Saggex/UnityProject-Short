using UnityEngine;
using System.Collections.Generic;

// Attach to the character
public class ScaleWithDepth : MonoBehaviour
{
    public bool copyFromOther = false;
    public float topY = 5f;     // far point in background
    public float bottomY = -5f; // near point in background
    public float minScale = 0.5f;
    public float maxScale = 1.5f;

    private void Start()
    {
        if (copyFromOther)
        {
            List<ScaleWithDepth> others = new List<ScaleWithDepth>();
                others.AddRange(Object.FindObjectsByType<ScaleWithDepth>(FindObjectsSortMode.InstanceID));
            foreach(ScaleWithDepth other in others)
            {
                topY = other.topY;
                bottomY = other.bottomY;
                minScale = other.minScale;
                maxScale = other.maxScale;
            }
        }
    }

    void Update()
    {
        float t = Mathf.InverseLerp(topY, bottomY, transform.position.y);
        float scale = Mathf.Lerp(minScale, maxScale, t);
        transform.localScale = new Vector3(scale, scale, 1f);
        
    }
}

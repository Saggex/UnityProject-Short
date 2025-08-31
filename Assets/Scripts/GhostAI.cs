using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Simple ghost behavior that blocks progress until a required item is used.
/// </summary>
public class GhostAI : MonoBehaviour
{
    [SerializeField] private string id;
    [SerializeField] private string[] requiredItemIds;
    [SerializeField] private bool consumeItem = true;
    [SerializeField] private bool isDefeated;
    [SerializeField] private UnityEvent onDefeated;
    [SerializeField] private UnityEvent onFailed;
    [SerializeField] [TextArea] private string[] successResponses;
    [SerializeField] [TextArea] private string[] failedResponses;
    public Material dissolveMaterial;
    public List<SpriteRenderer> renderers;
    public float dissolveDuration = 1;
    public Vector2 dissolveRange = new Vector2(-0.5f, 1);

    /// <summary>
    /// Item ids required to clear the ghost.
    /// </summary>
    public string[] RequiredItemIds => requiredItemIds;

    private void Awake()
    {
        if (DestroyState.IsDestroyed(GetId()))
        {
            isDefeated = true;
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Attempts to interact with the ghost using the player's inventory.
    /// </summary>
    public bool Interact()
    {
        if (isDefeated) return false;
        var inventory = InventorySystem.Instance;
        var ui = UIManager.Instance;
        if (requiredItemIds != null && requiredItemIds.Length > 0)
        {
            foreach (var id in requiredItemIds)
            {
                if (inventory == null || !inventory.HasItem(id))
                {
                    onFailed?.Invoke();
                    ui?.ShowFlavourText(GetRandomResponse(failedResponses) ?? $"You need {string.Join(", ", requiredItemIds)}");
                    return false;
                }
            }
            if (consumeItem)
            {
                foreach (var id in requiredItemIds)
                {
                    inventory?.UseItem(id);
                }
                ui?.RefreshInventory(inventory);
            }
        }
        isDefeated = true;
        DestroyState.MarkDestroyed(GetId());

        StartCoroutine(Dissolve());
        return true;
    }

    private IEnumerator Dissolve()
    {

        var ui = UIManager.Instance;
        float t = 0f;
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        


        while (t < dissolveDuration && dissolveMaterial)
        {
            
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(dissolveRange.x, dissolveRange.y, t / dissolveDuration);
            dissolveMaterial.SetFloat("_DissolveProgress", alpha);
            foreach(SpriteRenderer renderer in renderers)
            {
                renderer.material = dissolveMaterial;
            }
            yield return null;
        }
        onDefeated?.Invoke();
        var success = GetRandomResponse(successResponses);
        if (!string.IsNullOrEmpty(success))
        {
            ui?.ShowFlavourText(success);
        }
        gameObject.SetActive(false);
    }

    private string GetId()
    {
        return string.IsNullOrEmpty(id) ? gameObject.name : id;
    }

    private string GetRandomResponse(string[] responses)
    {
        if (responses == null || responses.Length == 0) return null;
        return responses[Random.Range(0, responses.Length)];
    }
}

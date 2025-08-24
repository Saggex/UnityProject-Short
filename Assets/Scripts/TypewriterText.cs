using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Displays text with a typewriter effect and optional vertex animations defined by inline tags.
/// Supports <c><jitter></c> and <c><float></c> tags and fades out after a delay.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class TypewriterText : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private float charactersPerSecond = 40f;
    [SerializeField] private float fadeDelay = 2f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float jitterAmount = 0.5f;
    [SerializeField] private float floatAmplitude = 2f;
    [SerializeField] private float floatFrequency = 2f;

    private CanvasGroup canvasGroup;
    private readonly List<EffectRange> effects = new List<EffectRange>();
    private TMP_MeshInfo[] originalMeshInfo;
    private bool isAnimating;

    private enum EffectType { Jitter, Float }

    private struct EffectRange
    {
        public int start;
        public int end;
        public EffectType type;
    }

    private void Awake()
    {
        if (textComponent == null)
        {
            textComponent = GetComponentInChildren<TMP_Text>();
        }
        canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows the provided text with effects, restarting any existing animation.
    /// </summary>
    public void Show(string source)
    {
        StopAllCoroutines();
        ParseEffects(source, out var plainText);
        textComponent.text = plainText;
        textComponent.maxVisibleCharacters = 0;
        textComponent.ForceMeshUpdate();
        originalMeshInfo = textComponent.textInfo.CopyMeshInfoVertexData();
        canvasGroup.alpha = 1f;
        gameObject.SetActive(true);
        isAnimating = true;
        StartCoroutine(TypeRoutine());
    }

    /// <summary>
    /// Hides the text immediately.
    /// </summary>
    public void Hide()
    {
        StopAllCoroutines();
        isAnimating = false;
        gameObject.SetActive(false);
    }

    private IEnumerator TypeRoutine()
    {
        int total = textComponent.textInfo.characterCount;
        for (int i = 0; i <= total; i++)
        {
            textComponent.maxVisibleCharacters = i;
            yield return new WaitForSeconds(1f / charactersPerSecond);
        }
        yield return new WaitForSeconds(fadeDelay);
        yield return FadeOut();
        isAnimating = false;
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    private void ParseEffects(string input, out string plain)
    {
        effects.Clear();
        plain = string.Empty;
        int plainIndex = 0;
        var stack = new Stack<(EffectType type, int start)>();
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '<')
            {
                int end = input.IndexOf('>', i);
                if (end != -1)
                {
                    string tag = input.Substring(i + 1, end - i - 1).ToLower();
                    if (tag == "jitter" || tag == "float")
                    {
                        stack.Push((tag == "jitter" ? EffectType.Jitter : EffectType.Float, plainIndex));
                        i = end;
                        continue;
                    }
                    else if (tag == "/jitter" || tag == "/float")
                    {
                        if (stack.Count > 0)
                        {
                            var (type, start) = stack.Pop();
                            effects.Add(new EffectRange { start = start, end = plainIndex, type = type });
                        }
                        i = end;
                        continue;
                    }
                }
            }
            plain += input[i];
            plainIndex++;
        }
        while (stack.Count > 0)
        {
            var (type, start) = stack.Pop();
            effects.Add(new EffectRange { start = start, end = plainIndex, type = type });
        }
    }

    private void LateUpdate()
    {
        if (!isAnimating || effects.Count == 0)
            return;

        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var dst = textInfo.meshInfo[i].vertices;
            var src = originalMeshInfo[i].vertices;
            System.Array.Copy(src, dst, src.Length);
        }

        float time = Time.time;

        foreach (var effect in effects)
        {
            for (int i = effect.start; i < effect.end && i < textComponent.maxVisibleCharacters; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int materialIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;

                Vector3 offset = Vector3.zero;
                switch (effect.type)
                {
                    case EffectType.Jitter:
                        offset = (Vector3)Random.insideUnitCircle * jitterAmount;
                        break;
                    case EffectType.Float:
                        offset = new Vector3(0, Mathf.Sin(time * floatFrequency + i) * floatAmplitude, 0);
                        break;
                }

                var vertices = textInfo.meshInfo[materialIndex].vertices;
                vertices[vertexIndex + 0] += offset;
                vertices[vertexIndex + 1] += offset;
                vertices[vertexIndex + 2] += offset;
                vertices[vertexIndex + 3] += offset;
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}

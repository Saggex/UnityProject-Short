using System.Collections;
using UnityEngine;

/// <summary>
/// Triggers random coughing fits that temporarily disable player movement.
/// </summary>
[RequireComponent(typeof(AudioSource), typeof(PlayerController))]
public class PlayerCough : MonoBehaviour
{
    [Tooltip("Sound that plays when the player coughs.")]
    [SerializeField] private AudioClip coughClip;

    [Tooltip("Random delay range in seconds between coughs.")]
    [SerializeField] private Vector2 coughInterval = new Vector2(10f, 30f);

    private PlayerController controller;
    private AudioSource audioSource;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(CoughLoop());
    }

    private IEnumerator CoughLoop()
    {
        while (true)
        {
            float wait = Random.Range(coughInterval.x, coughInterval.y);
            yield return new WaitForSeconds(wait);
            yield return CoughRoutine();
        }
    }

    private IEnumerator CoughRoutine()
    {
        if (coughClip == null)
        {
            yield break;
        }

        controller.enabled = false;
        Rigidbody2D rb = controller.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        audioSource.PlayOneShot(coughClip);

        yield return new WaitForSeconds(coughClip.length);

        controller.enabled = true;
    }
}

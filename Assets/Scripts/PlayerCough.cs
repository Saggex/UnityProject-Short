using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Triggers random coughing fits that temporarily disable player movement.
/// </summary>
[RequireComponent(typeof(AudioSource), typeof(PlayerController))]
public class PlayerCough : MonoBehaviour
{
    [Tooltip("Sound that plays when the player coughs.")]
    [SerializeField] private List<AudioClip> coughClips;
    [SerializeField] private string SceneToLoadOnDeath = "StarterScene";


    [Tooltip("Random delay range in seconds between coughs.")]
    [SerializeField] private Vector2 coughInterval = new Vector2(10f, 30f);
    [SerializeField] private float deathDistance = 0.1f;

    private PlayerController controller;
    private AudioSource audioSource;
    private GhostChaser chaser;
    public SpriteRenderer deathOverlay;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {

        chaser = FindObjectOfType<GhostChaser>();
        StartCoroutine(CoughLoop());

    }
    private void Update()
    {
        if (deathOverlay && chaser)
        {
            Color tempColor = deathOverlay.color;
            tempColor.a = Mathf.Min(Mathf.Max(1 + deathDistance - Vector3.Distance(chaser.transform.position, controller.transform.position) / 15, 0), 1);
            deathOverlay.color = tempColor;

        }
    }

    private IEnumerator CoughLoop()
    {
        while (true)
        {
            float wait = Random.Range(coughInterval.x, coughInterval.y);
            if (chaser)
            {
                float distance = Vector3.Distance(chaser.transform.position, controller.transform.position) / 15;
                wait *= distance;

                if (distance < deathDistance)
                {

                    SaveLoadManager.Instance.Delete();
                    RoomManager.Instance.LoadRoom(SceneToLoadOnDeath);

                }
            }
            Debug.Log("Coughed. Will wait for " + wait + " Seconds;");
            yield return new WaitForSeconds(wait);
            yield return CoughRoutine();
        }
    }

    private IEnumerator CoughRoutine()
    {
        AudioClip coughClip = coughClips[Random.Range(0, coughClips.Count)];
        if (coughClip == null)
        {
            yield break;
        }

        controller.isCoughing = true;
        Rigidbody2D rb = controller.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        audioSource.PlayOneShot(coughClip);

        yield return new WaitForSeconds(coughClip.length);

        controller.isCoughing = false;
    }
}

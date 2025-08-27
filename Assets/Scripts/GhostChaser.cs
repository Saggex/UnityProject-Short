using UnityEngine;

/// <summary>
/// Simple behaviour that moves a ghost towards the player with a bit of randomness.
/// </summary>
public class GhostChaser : MonoBehaviour
{
    [Tooltip("Movement speed towards the player in units per second.")]
    public float speed = 1f;

    [Tooltip("Amount of random drift applied to the ghost's movement each frame.")]
    public float wanderStrength = 0.5f;

    private Transform player;

    private void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        Vector2 toPlayer = (Vector2)(player.position - transform.position);
        Vector2 randomOffset = Random.insideUnitCircle * wanderStrength;
        Vector2 direction = (toPlayer + randomOffset).normalized;

        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
}


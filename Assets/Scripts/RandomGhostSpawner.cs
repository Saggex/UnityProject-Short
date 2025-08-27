using UnityEngine;

/// <summary>
/// Spawns a roaming ghost outside the camera view with a random chance.
/// </summary>
public class RandomGhostSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField, Range(0f,1f)] private float spawnChance = 0.3f;
    [SerializeField] private float minExtraDistance = 2f;
    [SerializeField] private float maxExtraDistance = 5f;

    [Header("Ghost Behaviour")]
    [SerializeField] private float ghostSpeed = 1f;
    [SerializeField] private float ghostWanderStrength = 0.5f;

    private void Start()
    {
        if (Random.value <= spawnChance)
        {
            SpawnGhost();
        }
    }

    private void SpawnGhost()
    {
        if (ghostPrefab == null) return;

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        var cam = Camera.main;
        if (playerObj == null || cam == null) return;

        Vector2 playerPos = playerObj.transform.position;
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        float screenRadius = Mathf.Max(camWidth, camHeight);

        float distance = screenRadius + Random.Range(minExtraDistance, maxExtraDistance);
        Vector2 direction = Random.insideUnitCircle.normalized;
        Vector2 spawnPos = playerPos + direction * distance;
        
        spawnPos = new Vector2(spawnPos.x, -Mathf.Abs(spawnPos.y));

        var ghost = Instantiate(ghostPrefab, spawnPos, Quaternion.identity);
        
        var chaser = ghost.AddComponent<GhostChaser>();
        chaser.speed = ghostSpeed;
        chaser.wanderStrength = ghostWanderStrength;
        
    }
}


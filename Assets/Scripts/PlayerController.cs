using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles player movement, crouch/tiptoe, and basic interaction input.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(ScaleWithDepth))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float tiptoeSpeed = 1.5f;

    [Header("References")]
    [SerializeField] private InventorySystem inventory;
    [SerializeField] private UIManager ui;

    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 facing = Vector2.down;
    private bool isTiptoeing;

    public List<ItemPickup> nearbyPickups = new();
    public readonly List<GhostAI> nearbyGhosts = new();
    public readonly List<Door> nearbyDoors = new();

    private void Awake()
    {
        Debug.Log($"[PlayerController] Awake on {name}");
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Debug.Log("[PlayerController] Update");
        HandleInput();
    }

    private void FixedUpdate()
    {
        Debug.Log("[PlayerController] FixedUpdate");
        Move();
    }

    private void HandleInput()
    {
        Debug.Log("[PlayerController] HandleInput start");
        input = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) input.y += 1f;
        if (Input.GetKey(KeyCode.S)) input.y -= 1f;
        if (Input.GetKey(KeyCode.A)) input.x -= 1f;
        if (Input.GetKey(KeyCode.D)) input.x += 1f;

        if (input.sqrMagnitude > 0.01f)
        {
            facing = input;
        }

        isTiptoeing = Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Fire2");

        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire1"))
        {
            Debug.Log("[PlayerController] Interaction input detected");
            Interact();
        }

        Debug.Log($"[PlayerController] HandleInput result - input: {input}, facing: {facing}, isTiptoeing: {isTiptoeing}");
    }

    private void Move()
    {
        float speed = isTiptoeing ? tiptoeSpeed : walkSpeed;
        rb.velocity = input.normalized * speed;
        Debug.Log($"[PlayerController] Move - speed: {speed}, velocity: {rb.velocity}");
    }

    private void Interact()
    {
        Debug.Log("[PlayerController] Interact called");
        if (nearbyPickups.Count > 0)
        {
            var pickup = nearbyPickups[0];
            Debug.Log($"[PlayerController] Interacting with pickup {pickup.name}");
            pickup.Interact(inventory, ui);
            return;
        }

        if (nearbyDoors.Count > 0)
        {
            var door = nearbyDoors[0];
            Debug.Log($"[PlayerController] Interacting with door {door.name}");
            door.Interact(inventory, ui);
            return;
        }

        if (nearbyGhosts.Count > 0)
        {
            var ghost = nearbyGhosts[0];
            Debug.Log($"[PlayerController] Interacting with ghost {ghost.name}");
            ghost.Interact(inventory, ui);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"[PlayerController] OnTriggerEnter2D with {collision.name}");
        var pickup = collision.GetComponent<ItemPickup>();
        if (pickup != null && !nearbyPickups.Contains(pickup))
        {
            nearbyPickups.Add(pickup);
            Debug.Log($"[PlayerController] Pickup {pickup.name} added to nearby list");
        }

        var door = collision.GetComponent<Door>();
        if (door != null && !nearbyDoors.Contains(door))
        {
            nearbyDoors.Add(door);
            Debug.Log($"[PlayerController] Door {door.name} added to nearby list");
        }

        var ghost = collision.GetComponent<GhostAI>();
        if (ghost != null && !nearbyGhosts.Contains(ghost))
        {
            nearbyGhosts.Add(ghost);
            Debug.Log($"[PlayerController] Ghost {ghost.name} added to nearby list");
        }

        var highlight = collision.GetComponent<IHighlightable>();
        if (highlight != null)
        {
            Debug.Log($"[PlayerController] Highlighting {collision.name}");
            highlight.SetHighlighted(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log($"[PlayerController] OnTriggerExit2D with {collision.name}");
        var pickup = collision.GetComponent<ItemPickup>();
        if (pickup != null)
        {
            nearbyPickups.Remove(pickup);
            Debug.Log($"[PlayerController] Pickup {pickup.name} removed from nearby list");
        }

        var door = collision.GetComponent<Door>();
        if (door != null)
        {
            nearbyDoors.Remove(door);
            Debug.Log($"[PlayerController] Door {door.name} removed from nearby list");
        }

        var ghost = collision.GetComponent<GhostAI>();
        if (ghost != null)
        {
            nearbyGhosts.Remove(ghost);
            Debug.Log($"[PlayerController] Ghost {ghost.name} removed from nearby list");
        }

        var highlight = collision.GetComponent<IHighlightable>();
        if (highlight != null)
        {
            Debug.Log($"[PlayerController] Removing highlight from {collision.name}");
            highlight.SetHighlighted(false);
        }
    }
}

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

    private readonly List<ItemPickup> nearbyPickups = new();
    private readonly List<GhostAI> nearbyGhosts = new();
    private readonly List<Door> nearbyDoors = new();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void HandleInput()
    {
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
            Interact();
        }
    }

    private void Move()
    {
        float speed = isTiptoeing ? tiptoeSpeed : walkSpeed;
        rb.velocity = input.normalized * speed;
    }

    private void Interact()
    {
        if (nearbyPickups.Count > 0)
        {
            var pickup = nearbyPickups[0];
            inventory.AddItem(pickup.Item);
            ui?.RefreshInventory(inventory);
            ui?.ShowFlavourText($"Picked up {pickup.Item.DisplayName}");
            Destroy(pickup.gameObject);
            return;
        }

        if (nearbyDoors.Count > 0)
        {
            var door = nearbyDoors[0];
            door.Enter();
            return;
        }

        if (nearbyGhosts.Count > 0)
        {
            var ghost = nearbyGhosts[0];
            if (inventory.HasItem(ghost.RequiredItemId))
            {
                var item = inventory.UseItem(ghost.RequiredItemId);
                ghost.TrySatisfy(item);
                ui?.RefreshInventory(inventory);
            }
            else
            {
                ui?.ShowFlavourText($"You need {ghost.RequiredItemId}");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pickup = collision.GetComponent<ItemPickup>();
        if (pickup != null && !nearbyPickups.Contains(pickup))
        {
            nearbyPickups.Add(pickup);
        }

        var door = collision.GetComponent<Door>();
        if (door != null && !nearbyDoors.Contains(door))
        {
            nearbyDoors.Add(door);
        }

        var ghost = collision.GetComponent<GhostAI>();
        if (ghost != null && !nearbyGhosts.Contains(ghost))
        {
            nearbyGhosts.Add(ghost);
        }

        var highlight = collision.GetComponent<IHighlightable>();
        highlight?.SetHighlighted(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var pickup = collision.GetComponent<ItemPickup>();
        if (pickup != null)
        {
            nearbyPickups.Remove(pickup);
        }

        var door = collision.GetComponent<Door>();
        if (door != null)
        {
            nearbyDoors.Remove(door);
        }

        var ghost = collision.GetComponent<GhostAI>();
        if (ghost != null)
        {
            nearbyGhosts.Remove(ghost);
        }

        var highlight = collision.GetComponent<IHighlightable>();
        highlight?.SetHighlighted(false);
    }
}

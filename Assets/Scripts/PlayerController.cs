using UnityEngine;

/// <summary>
/// Handles player movement, crouch/tiptoe, and basic interaction input.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float tiptoeSpeed = 1.5f;

    [Header("References")]
    [SerializeField] private InventorySystem inventory;
<<<<<<< HEAD
    [SerializeField] private UIManager ui;
    [SerializeField] private float interactRange = 1f;

    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 facing = Vector2.down;
=======

    private Rigidbody2D rb;
    private Vector2 input;
>>>>>>> main
    private bool isTiptoeing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
<<<<<<< HEAD
=======
        // TODO: Gather input for movement and interactions.
>>>>>>> main
        HandleInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void HandleInput()
    {
<<<<<<< HEAD
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input.sqrMagnitude > 0.01f)
        {
            facing = input;
        }

        isTiptoeing = Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Fire2");

        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire1"))
=======
        // Placeholder for keyboard/controller input.
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isTiptoeing = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.E))
>>>>>>> main
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
<<<<<<< HEAD
        var hit = Physics2D.Raycast(rb.position, facing, interactRange);
        if (!hit)
            return;

        var pickup = hit.collider.GetComponent<ItemPickup>();
        if (pickup != null)
        {
            inventory.AddItem(pickup.Item);
            ui?.RefreshInventory(inventory);
            ui?.ShowFlavourText($"Picked up {pickup.Item.DisplayName}");
            Destroy(pickup.gameObject);
            return;
        }

        var ghost = hit.collider.GetComponent<GhostAI>();
        if (ghost != null)
        {
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
=======
        // Placeholder for interaction logic.
>>>>>>> main
    }
}

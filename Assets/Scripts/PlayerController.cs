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

    private Rigidbody2D rb;
    private Vector2 input;
    private bool isTiptoeing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // TODO: Gather input for movement and interactions.
        HandleInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void HandleInput()
    {
        // Placeholder for keyboard/controller input.
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isTiptoeing = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.E))
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
        // Placeholder for interaction logic.
    }
}

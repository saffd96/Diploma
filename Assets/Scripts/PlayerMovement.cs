using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float speed;
    [SerializeField] private bool isRunActive;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private float groundDetectRadius;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isMultipleJumpsActive;
    [SerializeField] private int extraJumps;

    private int jumps;
    private bool isGrounded;
    private bool isFacingRight;
    private Rigidbody2D rb;
    private float moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = true;
        isGrounded = false;
        jumps = extraJumps;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        Jump();
        Run();
    }

    private void Run()
    {
        if (isRunActive && Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed *= speedMultiplier;
        }
        else if (isRunActive && Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed /= speedMultiplier;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            jumps = extraJumps;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0 && isMultipleJumpsActive)
        {
            rb.velocity = Vector2.up * jumpForce;
            jumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isMultipleJumpsActive)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    private void Move()
    {
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, groundDetectRadius, whatIsGround);
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        var scaleTransform = transform;
        var newScale = scaleTransform.localScale;

        isFacingRight = !isFacingRight;
        newScale.x *= -1;
        scaleTransform.localScale = newScale;
    }
}

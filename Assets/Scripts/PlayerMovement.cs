using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private bool isRunActive;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private float groundDetectRadius;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isMultipleJumpsActive;
    [SerializeField] private int extraJumps;

    [SerializeField] private float speed;
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
        speed = maxSpeed;
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
        switch (isRunActive)
        {
            case true when Input.GetKeyDown(KeyCode.LeftShift):
                speed *= speedMultiplier;

                break;
            case true when Input.GetKeyUp(KeyCode.LeftShift):
                speed /= speedMultiplier;

                break;
        }
    }

    private void Jump()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        if (isGrounded && jumps != extraJumps)
        {
            jumps = extraJumps;
        }

        if (jumps > 0 && isMultipleJumpsActive)
        {
            CalculateJumpVelocity();
            jumps--;
        }
        else if (isGrounded && !isMultipleJumpsActive)
        {
            CalculateJumpVelocity();
        }
    }

    private void CalculateJumpVelocity()
    {
        rb.velocity = Vector2.up * jumpForce;
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

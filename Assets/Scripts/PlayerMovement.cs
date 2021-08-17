using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private bool isRunActive;
    [SerializeField] private float runningSpeedMultiplier;
    [SerializeField] private GameObject runVfx;
    [SerializeField] private Transform colliderDetector;
    [SerializeField] private float colliderDetectRadius;
    [Space]
    [SerializeField] private Transform legsPosition;
    [SerializeField] private float groundDetectRadius;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isMultipleJumpsActive;
    [SerializeField] private int extraJumps;
    [SerializeField] private GameObject extraJumpVfx;

    [Header("Shadow Settings")]
    [SerializeField] private Transform shadowTransform;
    [SerializeField] private float shadowShowRange = 3f;

    // private StateMachine stateMachine;

    private RaycastHit2D hit;
    private GameObject dustFromRun;
    private PlayerAnimationController playerAnimationController;

    private Rigidbody2D rb;

    private float moveHorizontalInput;
    private float moveVerticalInput;

    private float speed;

    private int jumps;

    private bool isGrounded;
    private bool isFacingRight;

    private bool isShiftPressed;
    private bool isShadowEnabled;
    private bool isClimbing;
    private bool isPushing;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(legsPosition.position, groundDetectRadius);
        Gizmos.DrawWireSphere(colliderDetector.position, colliderDetectRadius);
    }
    //
    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.collider.CompareTag(Tags.PushingObject))
    //     {
    //         CheckPushCondition();
    //     }
    // }
    //
    // private void OnCollisionExit2D(Collision2D other)
    // {
    //     if (other.gameObject.CompareTag(Tags.PushingObject))
    //     {
    //         CheckPushCondition();
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(Tags.ClimbObject)) return;

        isClimbing = true;
        CheckClimbCondition();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(Tags.ClimbObject)) return;

        isClimbing = false;
        CheckClimbCondition();
    }

    private void Awake()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = true;
        isGrounded = false;
        jumps = extraJumps;
        speed = maxSpeed;
    }

    private void FixedUpdate()
    {
        Move();
        Climb();
    }

    private void Update()
    {
        CheckJumpCondition();
        CheckRunCondition();
        CheckPushCondition();
        MoveShadow();
    }

    private void Move()
    {
        isGrounded = Physics2D.OverlapCircle(legsPosition.position, groundDetectRadius, whatIsGround);
        playerAnimationController.SetIsGrounded(isGrounded);

        moveHorizontalInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(moveHorizontalInput * speed, rb.velocity.y);

        if (!isGrounded&&!isClimbing)
        {
            playerAnimationController.SetVelocity(rb.velocity.y);
        }

        if (moveHorizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveHorizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Climb()
    {
        moveVerticalInput = Input.GetAxis("Vertical");

        playerAnimationController.SetSpeed(Mathf.Abs(moveHorizontalInput));

        if (isClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, moveVerticalInput * speed);
            playerAnimationController.SetClimbingSpeed(Mathf.Abs(moveVerticalInput));
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

    private void Jump()
    {
        playerAnimationController.Jump();
        rb.velocity = Vector2.up * jumpForce;
    }

    private void CheckRunCondition()
    {
        SetActiveDustFromRun();

        switch (isRunActive)
        {
            case true when Input.GetKey(KeyCode.LeftShift) && isGrounded && !isShiftPressed:
                isShiftPressed = true;
                dustFromRun = Instantiate(runVfx, transform);
                dustFromRun.SetActive(false);
                playerAnimationController.SetIsRunning(true);
                speed *= runningSpeedMultiplier;

                break;
            case true when Input.GetKeyUp(KeyCode.LeftShift):
                Destroy(dustFromRun);
                playerAnimationController.SetIsRunning(false);
                speed /= runningSpeedMultiplier;
                isShiftPressed = false;

                break;
        }
    }

    private void CheckJumpCondition()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        if (isGrounded)
        {
            jumps = extraJumps;
        }

        switch (isMultipleJumpsActive)
        {
            case false:
                Jump();

                break;
            case true when jumps >= 0:
                Jump();
                jumps--;

                if (!isGrounded)
                {
                    Instantiate(extraJumpVfx, transform.position, quaternion.identity);
                }

                break;
        }
    }

    private void CheckClimbCondition()
    {
        if (isClimbing)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        else
        {
            playerAnimationController.SetClimbingSpeed(0);
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        playerAnimationController.SetIsClimbing(isClimbing);
    }

    private void CheckPushCondition()
    {
        isPushing = Physics2D.OverlapCircle(colliderDetector.position, colliderDetectRadius, LayerMask.GetMask(Layers.InteractObjects));
        playerAnimationController.SetIsPushing(isPushing);
    }

    private void SetActiveDustFromRun()
    {
        if (dustFromRun != null)
        {
            dustFromRun.SetActive(
                playerAnimationController.GetSpeed() > 0.5f && playerAnimationController.GetGrounded());
        }
    }

    private void MoveShadow()
    {
        var position = transform.position;

        hit = Physics2D.Raycast(position, Vector2.down, shadowShowRange, LayerMask.GetMask(Layers.Ground));

        isShadowEnabled = hit.collider != null;

        if (isShadowEnabled)
        {
            shadowTransform.position = hit.point;
        }

        shadowTransform.gameObject.SetActive(isShadowEnabled);
    }
}

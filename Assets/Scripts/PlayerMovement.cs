using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(AnimationController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private bool isRunActive;
    [SerializeField] private float runningSpeedMultiplier;
    [SerializeField] private GameObject runVfx;
    [Space]
    [SerializeField] private Transform feetsPostion;
    [SerializeField] private float groundDetectRadius;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isMultipleJumpsActive;
    [SerializeField] private int extraJumps;
    [SerializeField] private GameObject extraJumpVfx;
    [SerializeField] private float extraJumpVfxLifeTime;

    private GameObject dustFromRun;
    private GameObject jumpVfx;
    private AnimationController animationController;
    private float speed;
    private int jumps;
    private bool isGrounded;
    private bool isFacingRight;
    private Rigidbody2D rb;
    private float moveInput;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(feetsPostion.position, groundDetectRadius);
    }

    private void Awake()
    {
        animationController = GetComponent<AnimationController>();
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
        CheckJumpCondition();
        CheckRunCondition();
    }

    private void Move()
    {
        isGrounded = Physics2D.OverlapCircle(feetsPostion.position, groundDetectRadius, whatIsGround);
        animationController.SetBool(AnimationBoolNames.IsGrounded, isGrounded);

        moveInput = Input.GetAxis("Horizontal");

        animationController.SetFloat(AnimationFloatNames.Speed, Mathf.Abs(moveInput));

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

    private void Jump()
    {
        animationController.SetTrigger(AnimationTriggerNames.Jump);
        rb.velocity = Vector2.up * jumpForce;
    }

    private void CheckRunCondition()
    {
        SetActiveDustFromRun();

        switch (isRunActive)
        {
            case true when Input.GetKeyDown(KeyCode.LeftShift):
                dustFromRun = Instantiate(runVfx, transform);
                dustFromRun.SetActive(false);
                animationController.SetBool(AnimationBoolNames.IsRunning, true);
                speed *= runningSpeedMultiplier;

                break;
            case true when Input.GetKeyUp(KeyCode.LeftShift):
                Destroy(dustFromRun);
                animationController.SetBool(AnimationBoolNames.IsRunning, false);
                speed /= runningSpeedMultiplier;

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
        else
        {
            jumpVfx = Instantiate(extraJumpVfx, transform.position, quaternion.identity);
            Destroy(jumpVfx, extraJumpVfxLifeTime);
        }

        switch (isMultipleJumpsActive)
        {
            case false:
                Jump();

                break;
            case true when jumps >= 0:
                Jump();
                jumps--;

                break;
        }
    }

    private void SetActiveDustFromRun()
    {
        if (dustFromRun != null)
        {
            dustFromRun.SetActive(animationController.GetFloat(AnimationFloatNames.Speed) > 0.5f
                && animationController.GetBool(AnimationBoolNames.IsGrounded));
        }
    }
}

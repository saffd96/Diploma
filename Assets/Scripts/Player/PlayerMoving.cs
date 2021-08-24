using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private Transform legsPosition;
    [SerializeField] private float groundDetectRadius;
    [SerializeField] private LayerMask whatIsGround;
    
    [Header("VFX Settings")]
    [SerializeField] private GameObject runVfx;
    [SerializeField] private GameObject extraJumpVfx;
    
    
    [Header("Push Settings")]
    [SerializeField] private float pushColliderDetectRadius;
    [SerializeField] private Transform colliderDetector;

    private bool isShiftPressed = false;
    
    private int jumps;

    private float speed;
    
    private GameObject dustFromRun;

    private Player player;
    private bool isFacingRight;
    private bool isGrounded;
    private bool isClimbing;
    private bool isPushing;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(legsPosition.position, groundDetectRadius);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(colliderDetector.position, pushColliderDetectRadius);
    }

    
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
        player = GetComponent<Player>();
        isFacingRight = true;
        isGrounded = false;
        jumps = player.ExtraJumps;
        speed = player.MaxSpeed;
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
    }

    private void Move()
    {
        isGrounded = Physics2D.OverlapCircle(legsPosition.position, groundDetectRadius, whatIsGround);
        player.PlayerAnimationController.SetIsGrounded(isGrounded);

        player.Rb.velocity = new Vector2(player.MoveHorizontalInput * speed, player.Rb.velocity.y);

        if (!isGrounded && !isClimbing)
        {
            player.PlayerAnimationController.SetVelocity(player.Rb.velocity.y);
        }

        if (player.MoveHorizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (player.MoveHorizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Climb()
    {
        player.PlayerAnimationController.SetSpeed(Mathf.Abs(player.MoveHorizontalInput));

        if (isClimbing)
        {
            player.Rb.velocity = new Vector2(player.Rb.velocity.x, player.MoveVerticalInput * speed);
            player.PlayerAnimationController.SetClimbingSpeed(Mathf.Abs(player.MoveVerticalInput));
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
        player.PlayerAnimationController.Jump();
        player.Rb.velocity = Vector2.up * player.JumpForce;
    }
    
    private void CheckRunCondition()
    {
        SetActiveDustFromRun();

        switch (player.IsRunActive)
        {
            case true when Input.GetKey(KeyCode.LeftShift) && isGrounded && !isShiftPressed:
                isShiftPressed = true;
                dustFromRun = Instantiate(runVfx, transform);
                dustFromRun.SetActive(false);
                player.PlayerAnimationController.SetIsRunning(true);
                speed *= player.RunningSpeedMultiplier;

                break;
            case true when Input.GetKeyUp(KeyCode.LeftShift):
                Destroy(dustFromRun);
                player.PlayerAnimationController.SetIsRunning(false);
                speed /= player.RunningSpeedMultiplier;
                isShiftPressed = false;

                break;
        }
    }

    private void CheckJumpCondition()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        if (isGrounded)
        {
            jumps = player.ExtraJumps;
        }

        switch (player.IsMultipleJumpsActive)
        {
            case false:
                Jump();

                break;
            case true when jumps >= 0:
                Jump();
                jumps--;

                if (!isGrounded)
                {
                    Instantiate(extraJumpVfx, transform.position, Quaternion.identity);
                }

                break;
        }
    }

    private void CheckClimbCondition()
    {
        if (isClimbing)
        {
            player.Rb.bodyType = RigidbodyType2D.Kinematic;
        }
        else
        {
            player.PlayerAnimationController.SetClimbingSpeed(0);
            player.Rb.bodyType = RigidbodyType2D.Dynamic;
        }

        player.PlayerAnimationController.SetIsClimbing(isClimbing);
    }

    private void CheckPushCondition()
    {
        isPushing = Physics2D.OverlapCircle(colliderDetector.position, pushColliderDetectRadius,
            LayerMask.GetMask(Layers.InteractObjects));

        player.PlayerAnimationController.SetIsPushing(isPushing);
    }

    private void SetActiveDustFromRun()
    {
        if (dustFromRun != null)
        {
            dustFromRun.SetActive(
                player.PlayerAnimationController.GetSpeed() > 0.5f && player.PlayerAnimationController.GetGrounded());
        }
    }
}

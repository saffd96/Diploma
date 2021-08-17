using Unity.Mathematics;
using UnityEngine;

public class PlayerSM : MonoBehaviour
{
    private enum State
    {
        Idle,
        Walk,
        Climb,
        Jump,
        Push,
        Run,
        Throw,
        Attack,
        WalkAndAttack,
        Dead
    }
    
    [Header("Move Settings")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private bool isRunActive;
    [SerializeField] private float runningSpeedMultiplier;
    [SerializeField] private GameObject runVfx;
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
    
    private RaycastHit2D hit;
    private GameObject dustFromRun;
    private PlayerAnimationController playerAnimationController;
    private float speed;
    private int jumps;
    private bool isGrounded;
    private bool isFacingRight;
    private Rigidbody2D rb;
    private float moveInput;
    private bool isShiftPressed;
    private bool isShadowEnabled;
    
    private State currentState;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(legsPosition.position, groundDetectRadius);
    }
    
    protected void Awake()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = true;
        isGrounded = false;
        jumps = extraJumps;
        speed = maxSpeed;
        
        SetState(State.Idle);
    }

    private void Update()
    {
        if (currentState == State.Dead) return;
        Debug.Log(currentState);
        
        MoveShadow();
        CheckNewState();
        UpdateCurrentState();
    }
    
    private void SetState(State newState)
    {
        if (currentState == newState)
        {
            return;
        }

        switch (newState)
        {
            case State.Idle:
                moveInput = 0;
                
                break;
            
            case State.Walk:
                moveInput = 0f;
                
                break;
            
            case State.Run:
                moveInput = 0f;

                break;
            
            case State.Jump:
                moveInput = 0f;

                break;
        }

        currentState = newState;
    }

    private void CheckNewState()
    {
        isGrounded = Physics2D.OverlapCircle(legsPosition.position, groundDetectRadius, whatIsGround);
        moveInput = Input.GetAxis("Horizontal");

        if (isGrounded)
        {
            SetState(Mathf.Abs(Input.GetAxis("Horizontal")) > 0f ? State.Walk : State.Idle);
        }

        else if (currentState == State.Walk && isRunActive && isShiftPressed)
        {
            SetState(State.Run);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetState(State.Jump);
        }
    }

    private void UpdateCurrentState()
    {
        switch (currentState)
        {
            case State.Idle:

                break;
            case State.Walk:
                Walk();

                break;
            case State.Run:
                Run();

                break;
            case State.Jump:
                JumpState();

                break;
        }
    }

    private void Walk()
    {
        playerAnimationController.SetIsGrounded(isGrounded);
        moveInput = Input.GetAxis("Horizontal");
        playerAnimationController.SetSpeed(Mathf.Abs(moveInput));
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        
        if (!isGrounded)
        {
            playerAnimationController.SetVelocity(rb.velocity.y);
        }

        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Run()
    {
        SetActiveDustFromRun();

        switch (isRunActive)
        {
            case true when Input.GetKey(KeyCode.LeftShift):
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

    private void JumpState()
    {

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

    private void Flip()
    {
        var scaleTransform = transform;
        var newScale = scaleTransform.localScale;

        isFacingRight = !isFacingRight;
        newScale.x *= -1;
        scaleTransform.localScale = newScale;
    }
    
    private void SetActiveDustFromRun()
    {
        if (dustFromRun != null)
        {
            dustFromRun.SetActive(
                playerAnimationController.GetSpeed() > 0.5f && playerAnimationController.GetGrounded());
        }
    }
    
    private void Jump()
    {
        playerAnimationController.Jump();
        rb.velocity = Vector2.up * jumpForce;
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
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerMovement : DamageableObject
{
    [Header("Player Settings")]
    [SerializeField] private int attackValue;
    [SerializeField] private float attackRate;
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private Transform stoneSpawner;
    [SerializeField] private bool isRangeAttackEnabled;
    
    
    [Header("Move Settings")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private bool isRunActive;
    [SerializeField] private float runningSpeedMultiplier;
    [SerializeField] private GameObject runVfx;
    [Space]
    [SerializeField] private Transform legsPosition;
    [SerializeField] private float groundDetectRadius;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Collider Settings")]
    [SerializeField] private float colliderDetectRadius;
    [SerializeField] private float attackRadius;
    [SerializeField] private Transform colliderDetector;

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

    private Rigidbody2D rb;
    
    private GameObject stone;

    private float moveHorizontalInput;
    private float moveVerticalInput;

    private float speed;
    private float attackTimer;
    private float rangeAttackTimer;

    private int jumps;

    private bool isGrounded;
    private bool isFacingRight;

    private bool isMeleeAttack;

    private bool isShiftPressed;
    private bool isShadowEnabled;
    private bool isClimbing;
    private bool isPushing;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(legsPosition.position, groundDetectRadius);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(colliderDetector.position, colliderDetectRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(colliderDetector.position, attackRadius);
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
        Attack();
    }

    private void Move()
    {
        isGrounded = Physics2D.OverlapCircle(legsPosition.position, groundDetectRadius, whatIsGround);
        playerAnimationController.SetIsGrounded(isGrounded);

        moveHorizontalInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(moveHorizontalInput * speed, rb.velocity.y);

        if (!isGrounded && !isClimbing)
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

    private void Attack()
    {
        attackTimer += Time.deltaTime;
        rangeAttackTimer += Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            isMeleeAttack = true;
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            isMeleeAttack = false;
        }
        else
        {
            return;
        }
        
        if (!isGrounded || isShiftPressed || isPushing || (isClimbing && moveVerticalInput > 0)) return;
        
        switch (isMeleeAttack)
        {
            case true when attackTimer > attackRate:
                MeleeAttack();
                attackTimer = 0;

                break;
            case false when rangeAttackTimer > attackRate && moveHorizontalInput == 0 && isRangeAttackEnabled:
                RangeAttack();
                rangeAttackTimer = 0;

                break;
        }

    }

    private void MeleeAttack()
    {
        playerAnimationController.Attack();

        if (moveHorizontalInput < 0.1)
        {
            playerAnimationController.SetAttackType(Random.Range(1, 3));
        }

        var damageableObjects =
                Physics2D.OverlapCircleAll(colliderDetector.position, attackRadius, 
                    LayerMask.GetMask(Layers.Enemy));

        foreach (var enemy in damageableObjects)
        {
            enemy.GetComponent<DamageableObject>().ApplyDamage(attackValue);
        }
    }

    private void RangeAttack()
    {
        playerAnimationController.Throw();

        StartCoroutine(Throw());
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
        isPushing = Physics2D.OverlapCircle(colliderDetector.position, colliderDetectRadius,
            LayerMask.GetMask(Layers.InteractObjects));

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

    public override void ApplyDamage(int amount)
    {
        playerAnimationController.GetDamage();
        health -= amount;
    }

    private IEnumerator Throw()
    {
        yield return new WaitForSeconds(0.2f);

        stone = Instantiate(stonePrefab, stoneSpawner.position, Quaternion.identity);
        stone.GetComponent<Rigidbody2D>().AddForce(Vector2.right*transform.localScale * 1.5f, ForceMode2D.Impulse);
        Destroy(stone, 3);
    }
}

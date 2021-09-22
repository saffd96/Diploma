using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerAnimationController))]
public class SuperPlayer : DamageableObject
{
    [Header("Player Settings")]
    [SerializeField] private int attackValue;
    [SerializeField] private float attackRate;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private Transform stoneSpawner;
    [SerializeField] private bool isRangeAttackEnabled;
    [SerializeField] private GameObject shield;
    [SerializeField] private float invulnerableTime = 1f;
    [SerializeField] private GameObject invulnerableItemShield;
    [SerializeField] private bool isInvulnerableItem;
    [SerializeField] private float invulnerableItemTime = 10f;

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
    
    [Header("Other Settings")]
    [SerializeField] private GameObject trajectoryPointPrefab;
    [SerializeField] private GameObject[] trajectoryPoints;
    [SerializeField] private int numberOfPoints = 20;
    

    private PlayerAnimationController playerAnimationController;

    private RaycastHit2D hit;

    private Rigidbody2D rb;

    private GameObject stone;
    private GameObject dustFromRun;

    private float moveHorizontalInput;
    private float moveVerticalInput;
    private float attackTimer;
    private float rangeAttackTimer;
    private float speed;
    private float climbSpeed;

    private int jumps;
    private int stonesMax;

    private bool isGrounded;
    private bool isFacingRight;
    private bool isMeleeAttack;
    private bool isShiftPressed;
    private bool isCtrlPressed;
    private bool isShadowEnabled;
    private bool isClimbing;
    private bool isPushing;
    private bool isShieldEnabled;

    private Vector2 directionVector;
    
    public int CurrentStones { get; private set; }

    public static event Action OnSuperPlayerHpChanged;
    public static event Action OnSuperPlayerStonesChanged;
    public static event Action OnSuperPlayerDeath;

    private void OnEnable()
    {
        ExitLvl.OnExitLvlCollision += SaveStats;
    }

    private void OnDisable()
    {
        ExitLvl.OnExitLvlCollision -= SaveStats;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(legsPosition.position, groundDetectRadius);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(colliderDetector.position, colliderDetectRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(colliderDetector.position, attackRadius);
    }

    protected override void Awake()
    {
        base.Awake();
        playerAnimationController = GetComponent<PlayerAnimationController>();
        rb = GetComponent<Rigidbody2D>();

        climbSpeed = maxSpeed;

        if (GameHandler.LevelsCompleted > 0)
        {
            LoadStats();
        }

        isFacingRight = true;
        IsDead = false;
        isGrounded = false;
        jumps = extraJumps;
        speed = maxSpeed;
    }

    private void Start()
    {
        CreateTrajectoryPoints();
    }
    
    private void FixedUpdate()
    {
        if (IsDead) return;

        Move();
        Climb();
    }

    private void Update()
    {
        UpDateTrajectoryPoints();
        GetMousePosition();

        if (IsDead) return;

        //TODO не зыбать удалить
        if (Input.GetKeyDown(KeyCode.E))
        {
            RangeAttackPowerUp();
        }

        CheckShield();

        CheckJumpCondition();
        CheckRunCondition();
        CheckPushCondition();
        MoveShadow();
        Attack();

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

    private void SaveStats()
    {
        PlayerPrefs.SetInt(SaveLoadConstants.AttackValuePrefsKey, attackValue);
        PlayerPrefs.SetInt(SaveLoadConstants.IsRangeAttackEnabledPrefsKey, isRangeAttackEnabled ? 1 : 0);
        PlayerPrefs.SetFloat(SaveLoadConstants.MaxSpeedPrefsKey, maxSpeed);
        PlayerPrefs.SetInt(SaveLoadConstants.IsRunActivePrefsKey, isRunActive ? 1 : 0);
        PlayerPrefs.SetFloat(SaveLoadConstants.RunningSpeedMultiplierPrefsKey, runningSpeedMultiplier);
        PlayerPrefs.SetFloat(SaveLoadConstants.JumpForcePrefsKey, jumpForce);
        PlayerPrefs.SetInt(SaveLoadConstants.IsMultipleJumpsActivePrefsKey, isMultipleJumpsActive ? 1 : 0);
        PlayerPrefs.SetInt(SaveLoadConstants.IsShieldActivePrefsKey, isShieldEnabled ? 1 : 0);
        PlayerPrefs.SetInt(SaveLoadConstants.ExtraJumpsPrefsKey, extraJumps);
        PlayerPrefs.SetInt(SaveLoadConstants.StonesMaxPrefsKey, stonesMax);
        PlayerPrefs.SetInt(SaveLoadConstants.CurrentStonesPrefsKey, CurrentStones);
        PlayerPrefs.SetInt(SaveLoadConstants.MaxHealthPrefsKey, maxHealth);
        PlayerPrefs.SetInt(SaveLoadConstants.CurrentHealthPrefsKey, CurrentHealth);
    }

    private void LoadStats()
    {
        attackValue = PlayerPrefs.GetInt(SaveLoadConstants.AttackValuePrefsKey);
        isRangeAttackEnabled = PlayerPrefs.GetInt(SaveLoadConstants.IsRangeAttackEnabledPrefsKey) == 1 ? true : false;
        maxSpeed = PlayerPrefs.GetFloat(SaveLoadConstants.MaxSpeedPrefsKey);
        isRunActive = PlayerPrefs.GetInt(SaveLoadConstants.IsRunActivePrefsKey) == 1 ? true : false;
        runningSpeedMultiplier = PlayerPrefs.GetFloat(SaveLoadConstants.RunningSpeedMultiplierPrefsKey);
        jumpForce = PlayerPrefs.GetFloat(SaveLoadConstants.JumpForcePrefsKey);
        isMultipleJumpsActive = PlayerPrefs.GetInt(SaveLoadConstants.IsMultipleJumpsActivePrefsKey) == 1 ? true : false;
        isShieldEnabled = PlayerPrefs.GetInt(SaveLoadConstants.IsShieldActivePrefsKey) == 1 ? true : false;
        extraJumps = PlayerPrefs.GetInt(SaveLoadConstants.ExtraJumpsPrefsKey);
        stonesMax = PlayerPrefs.GetInt(SaveLoadConstants.StonesMaxPrefsKey);
        CurrentStones = PlayerPrefs.GetInt(SaveLoadConstants.CurrentStonesPrefsKey);
        maxHealth = PlayerPrefs.GetInt(SaveLoadConstants.MaxHealthPrefsKey);
        CurrentHealth = PlayerPrefs.GetInt(SaveLoadConstants.CurrentHealthPrefsKey);
    }

    private void CheckShield()
    {
        shield.SetActive(isShieldEnabled);
        invulnerableItemShield.SetActive(isInvulnerableItem);
    }

    public override void ApplyDamage(int amount)
    {
        if (isShieldEnabled || isInvulnerableItem)
        {
            if (isShieldEnabled)
            {
                AudioManager.Instance.PLaySfx(SfxType.ShieldUnActive);
                isShieldEnabled = false;
            }

            return;
        }

        base.ApplyDamage(amount);
        AudioManager.Instance.PLaySfx(SfxType.PlayerHit);
        OnSuperPlayerHpChanged?.Invoke();
        CameraShake.Instance.ShakeCamera(7, 0.1f);
        playerAnimationController.GetDamage();

        IsInvulnerable = true;
        StartCoroutine(InvulnerablePlayer(invulnerableTime));
    }

    public void AddSpeed()
    {
        maxSpeed++;
        speed = maxSpeed;
    }

    public void EnableRun()
    {
        RunningSpeedPowerUp();
    }

    public void AddRunningSpeedMultiplier()
    {
        RunningSpeedPowerUp();
    }

    private void RunningSpeedPowerUp()
    {
        if (isRunActive)
        {
            runningSpeedMultiplier += 0.25f;
        }
        else
        {
            isRunActive = true;
        }
    }

    public void AddJumpForce()
    {
        jumpForce++;
    }

    public void AddAdditionalJumps()
    {
        ExtraJumpsPowerUp();
    }

    public void EnableExtraJumps()
    {
        ExtraJumpsPowerUp();
    }

    private void ExtraJumpsPowerUp()
    {
        if (isMultipleJumpsActive)
        {
            extraJumps++;
        }
        else
        {
            isMultipleJumpsActive = true;
        }
    }

    public void AddAdditionalStones()
    {
        RangeAttackPowerUp();
    }

    public void EnableRangeAttack()
    {
        RangeAttackPowerUp();
    }

    private void RangeAttackPowerUp()
    {
        if (isRangeAttackEnabled)
        {
            stonesMax += 5;
            CurrentStones += 5;
            OnSuperPlayerStonesChanged?.Invoke();
        }
        else
        {
            isRangeAttackEnabled = true;
            stonesMax += 3;
            CurrentStones = stonesMax;
            OnSuperPlayerStonesChanged?.Invoke();
        }
    }

    public void AddAttackValue()
    {
        attackValue++;
    }

    public void AddMaxLives()
    {
        HpChangePowerUp();
    }

    public void FillHp()
    {
        HpChangePowerUp();
    }

    private void HpChangePowerUp()
    {
        if (CurrentHealth == maxHealth)
        {
            maxHealth++;
            CurrentHealth++;
            OnSuperPlayerHpChanged?.Invoke();
        }
        else
        {
            CurrentHealth = maxHealth;
            OnSuperPlayerHpChanged?.Invoke();
        }
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

        if ((moveHorizontalInput > 0 && !isFacingRight) || (GetMousePosition().x > transform.position.x &&
            !isFacingRight && moveHorizontalInput == 0))
        {
            Flip();
        }
        else if ((moveHorizontalInput < 0 && isFacingRight) || (GetMousePosition().x < transform.position.x &&
            isFacingRight && moveHorizontalInput == 0))
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
            rb.velocity = new Vector2(rb.velocity.x, moveVerticalInput * climbSpeed);
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
        AudioManager.Instance.PLaySfx(SfxType.Jump);
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
        if (CurrentStones > 0)
        {
            StartCoroutine(Throw());
            CurrentStones--;
            playerAnimationController.Throw();
            OnSuperPlayerStonesChanged?.Invoke();
        }
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
            case false when isGrounded:
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

    private Vector3 GetMousePosition()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        directionVector = (mousePosition - stoneSpawner.position).normalized;
        
        return mousePosition;
    }

    private IEnumerator Throw()
    {
        yield return new WaitForSeconds(0.2f);

        AudioManager.Instance.PLaySfx(SfxType.Throw);
        
        stone = Instantiate(stonePrefab, stoneSpawner.position, Quaternion.identity);

        stone.GetComponent<Rigidbody2D>().velocity = directionVector * throwForce;
        Destroy(stone, 3);
    }

    private Vector2 GetTrajectoryPointPosition(float t)
    {
        return (Vector2)stoneSpawner.position + (directionVector * throwForce * t) + 0.5f * Physics2D.gravity * (t * t);
    }
    
    private void CreateTrajectoryPoints()
    {
        transform.position = GameHandler.StartPosition;
        
        trajectoryPoints = new GameObject[numberOfPoints];

        for (int i = 0; i < numberOfPoints; i++)
        {
            trajectoryPoints[i] = Instantiate(trajectoryPointPrefab, stoneSpawner.position, Quaternion.identity);
        }
    }
    

    private void UpDateTrajectoryPoints()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCtrlPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCtrlPressed = false;
        }
        
        for (var i = 0; i < trajectoryPoints.Length; i++)
        {
            trajectoryPoints[i].transform.position = GetTrajectoryPointPosition(i * 0.025f);
            trajectoryPoints[i].SetActive(isCtrlPressed);
        }
    }


    protected override void Die()
    {
        base.Die();
        IsDead = true;
        playerAnimationController.SetIsDead(IsDead);
        rb.velocity = Vector2.zero;

        AudioManager.Instance.PLaySfx(SfxType.PlayerDeath);

        OnSuperPlayerDeath?.Invoke();
        Destroy(gameObject);
    }

    public void AddShieldItem()
    {
        isShieldEnabled = true;
        AudioManager.Instance.PLaySfx(SfxType.ShieldActive);
    }

    public void AddStonesItem()
    {
        CurrentStones += Random.Range(1, 3);
        OnSuperPlayerStonesChanged?.Invoke();
    }

    public void PotionItem()
    {
        CurrentHealth++;

        if (CurrentHealth > MAXHealth)
        {
            CurrentHealth = MAXHealth;
        }

        OnSuperPlayerHpChanged?.Invoke();
    }

    public void InvulnerableItem()
    {
        StartCoroutine(InvulnerablePlayer(invulnerableItemTime));
        isInvulnerableItem = true;
    }

    private IEnumerator InvulnerablePlayer(float time)
    {
        yield return new WaitForSeconds(time);

        IsInvulnerable = false;
        isInvulnerableItem = false;
    }
}

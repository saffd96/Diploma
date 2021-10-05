using System;
using System.Collections;
using UnityEngine;
using PlayerComponents;

[RequireComponent(typeof(PlayerAttack), typeof(PlayerMoving), typeof(PlayerPowerUp))]
public class Player : DamageableObject
{
    [Header("Components")]
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerMoving playerMoving;
    [SerializeField] private PlayerVfx playerVfx;

    [Header("ShadowSettings")]
    [SerializeField] private Transform shadowTransform;
    [SerializeField] private float shadowShowRange = 3f;

    [Space]
    [SerializeField] private float invulnerableTime = 1f;

    [Header("TrajectorySettings")]
    [SerializeField] private GameObject trajectoryPointPrefab;
    [SerializeField] private GameObject[] trajectoryPoints;
    [SerializeField] private int numberOfPoints = 20;

    private PlayerAnimationController playerAnimationController;

    private RaycastHit2D hit;

    private PlayerItemEffects playerItemEffects;

    private SpriteRenderer sr;

    private bool isGrounded;
    private bool isClimbing;
    private bool isFacingRight;
    private bool isCtrlPressed;
    private bool isShadowEnabled;

    private bool invulnerableAnimationSwitch;

    public static event Action OnPlayerHpChanged;
    public static event Action OnPlayerDeath;

    private void OnEnable()
    {
        ExitLvl.OnExitLvlCollision += SaveStats;
        BossExitLvl.OnBossExitDoorCollision += ResetStats;
        PlayerItemEffects.OnPotionPickup += AddHp;
        PlayerItemEffects.OnInvulnerablePickup += Invulnerable;
        PlayerPowerUp.OnPotionPowerUp += HpChange;
    }

    private void OnDisable()
    {
        ExitLvl.OnExitLvlCollision -= SaveStats;
        BossExitLvl.OnBossExitDoorCollision -= ResetStats;
        PlayerItemEffects.OnPotionPickup -= AddHp;
        PlayerItemEffects.OnInvulnerablePickup -= Invulnerable;
        PlayerPowerUp.OnPotionPowerUp -= HpChange;
    }

    protected override void Awake()
    {
        base.Awake();
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerItemEffects = GetComponent<PlayerItemEffects>();

        sr = GetComponent<SpriteRenderer>();
        
        playerAttack.AttackTimer = playerAttack.AttackRate;
        playerAttack.RangeAttackTimer = playerAttack.AttackRate;

        if (GameHandler.LevelsCompleted > 0)
        {
            LoadStats();
        }

        isFacingRight = true;
        IsDead = false;
        isGrounded = false;
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
            playerAttack.RangeAttackPowerUp();
        }

        playerItemEffects.CheckShield();
        playerMoving.CheckJumpCondition();
        playerMoving.CheckRunCondition();
        playerMoving.CheckPushCondition();
        Attack();
        MoveShadow();
    }

    private void AnimationInvulnerable()
    {
        
        if (IsInvulnerable)
        {
            sr.color = invulnerableAnimationSwitch ? Color.clear : Color.white;
            invulnerableAnimationSwitch = !invulnerableAnimationSwitch;
        }
    }

    public override void ApplyDamage(int amount)
    {
        if (playerItemEffects.IsShieldEnabled || playerItemEffects.IsInvulnerableItem)
        {
            if (playerItemEffects.IsShieldEnabled)
            {
                if (playerVfx.UnableShieldVfx != null)
                {
                    Instantiate(playerVfx.UnableShieldVfx, transform.position, Quaternion.identity, transform);
                }

                AudioManager.Instance.PLaySfx(SfxType.ShieldUnActive);
                playerItemEffects.IsShieldEnabled = false;
            }

            return;
        }

        base.ApplyDamage(amount);
        CameraShake.Instance.ShakeCamera(7, 0.1f);
        AudioManager.Instance.PLaySfx(SfxType.PlayerHit);
        OnPlayerHpChanged?.Invoke();
        playerAnimationController.GetDamage();
        IsInvulnerable = true;

        InvokeRepeating(nameof(AnimationInvulnerable), 0, 0.1f);
        StartCoroutine(InvulnerablePlayer(invulnerableTime));
    }

    protected override void Die()
    {
        base.Die();
        IsDead = true;
        playerAnimationController.SetIsDead(IsDead);
        playerMoving.Rb.velocity = Vector2.zero;

        AudioManager.Instance.PLaySfx(SfxType.PlayerDeath);

        OnPlayerDeath?.Invoke();
        Destroy(gameObject);
    }

    private void Attack()
    {
        playerAttack.AttackTimer += Time.deltaTime;
        playerAttack.RangeAttackTimer += Time.deltaTime;

        if (!isGrounded || playerMoving.IsShiftPressed || playerMoving.IsPushing) return;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            playerAttack.IsMeleeAttack = true;
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            playerAttack.IsMeleeAttack = false;
        }
        else
        {
            return;
        }

        playerAttack.Attack();
    }

    private void Move()
    {
        playerAnimationController.SetIsGrounded(isGrounded);
        isGrounded = Physics2D.OverlapCircle(playerMoving.LegsPosition.position, playerMoving.GroundDetectRadius,
            playerMoving.WhatIsGround);

        playerMoving.IsGrounded = isGrounded;

        playerMoving.Move();

        if (!isGrounded && !isClimbing)
        {
            playerAnimationController.SetVelocity(playerMoving.Rb.velocity.y);
        }

        if ((playerMoving.MoveHorizontalInput > 0 && !isFacingRight) || (GetMousePosition().x > transform.position.x &&
            !isFacingRight && playerMoving.MoveHorizontalInput == 0))
        {
            Flip();
        }
        else if ((playerMoving.MoveHorizontalInput < 0 && isFacingRight) ||
            (GetMousePosition().x < transform.position.x &&
                isFacingRight && playerMoving.MoveHorizontalInput == 0))
        {
            Flip();
        }
    }

    private void Climb()
    {
        isClimbing = playerMoving.IsClimbing;

        playerMoving.Climb();
    }

    private void SaveStats()
    {
        PlayerPrefs.SetInt(SaveLoadConstants.AttackValuePrefsKey, playerAttack.AttackValue);
        PlayerPrefs.SetInt(SaveLoadConstants.IsRangeAttackEnabledPrefsKey, playerAttack.IsRangeAttackEnabled ? 1 : 0);
        PlayerPrefs.SetFloat(SaveLoadConstants.MaxSpeedPrefsKey, playerMoving.MAXSpeed);
        PlayerPrefs.SetInt(SaveLoadConstants.IsRunActivePrefsKey, playerMoving.IsRunActive ? 1 : 0);
        PlayerPrefs.SetFloat(SaveLoadConstants.RunningSpeedMultiplierPrefsKey, playerMoving.RunningSpeedMultiplier);
        PlayerPrefs.SetFloat(SaveLoadConstants.JumpForcePrefsKey, playerMoving.JumpForce);
        PlayerPrefs.SetInt(SaveLoadConstants.IsMultipleJumpsActivePrefsKey, playerMoving.IsMultipleJumpsActive ? 1 : 0);
        PlayerPrefs.SetInt(SaveLoadConstants.IsShieldActivePrefsKey, playerItemEffects.IsShieldEnabled ? 1 : 0);
        PlayerPrefs.SetInt(SaveLoadConstants.ExtraJumpsPrefsKey, playerMoving.ExtraJumps);
        PlayerPrefs.SetInt(SaveLoadConstants.StonesMaxPrefsKey, playerAttack.StonesMax);
        PlayerPrefs.SetInt(SaveLoadConstants.CurrentStonesPrefsKey, playerAttack.CurrentStones);
        PlayerPrefs.SetInt(SaveLoadConstants.MaxHealthPrefsKey, maxHealth);
        PlayerPrefs.SetInt(SaveLoadConstants.CurrentHealthPrefsKey, CurrentHealth);
    }

    private void ResetStats()
    {
        PlayerPrefs.DeleteKey(SaveLoadConstants.AttackValuePrefsKey);
        PlayerPrefs.DeleteKey(SaveLoadConstants.IsRangeAttackEnabledPrefsKey);
        PlayerPrefs.DeleteKey(SaveLoadConstants.MaxSpeedPrefsKey);
        PlayerPrefs.DeleteKey(SaveLoadConstants.IsRunActivePrefsKey);
        PlayerPrefs.DeleteKey(SaveLoadConstants.RunningSpeedMultiplierPrefsKey);
        PlayerPrefs.DeleteKey(SaveLoadConstants.JumpForcePrefsKey);
        PlayerPrefs.DeleteKey(SaveLoadConstants.IsMultipleJumpsActivePrefsKey);
        PlayerPrefs.DeleteKey(SaveLoadConstants.IsShieldActivePrefsKey);
        PlayerPrefs.DeleteKey(SaveLoadConstants.ExtraJumpsPrefsKey);
        PlayerPrefs.DeleteKey(SaveLoadConstants.StonesMaxPrefsKey);
        PlayerPrefs.DeleteKey(SaveLoadConstants.CurrentStonesPrefsKey);
        PlayerPrefs.DeleteKey(SaveLoadConstants.MaxHealthPrefsKey);
        PlayerPrefs.DeleteKey(SaveLoadConstants.CurrentHealthPrefsKey);
    }

    private void LoadStats()
    {
        playerAttack.AttackValue = PlayerPrefs.GetInt(SaveLoadConstants.AttackValuePrefsKey);
        playerAttack.IsRangeAttackEnabled =
                PlayerPrefs.GetInt(SaveLoadConstants.IsRangeAttackEnabledPrefsKey) == 1 ? true : false;

        playerMoving.MAXSpeed = PlayerPrefs.GetFloat(SaveLoadConstants.MaxSpeedPrefsKey);
        playerMoving.IsRunActive = PlayerPrefs.GetInt(SaveLoadConstants.IsRunActivePrefsKey) == 1 ? true : false;
        playerMoving.RunningSpeedMultiplier = PlayerPrefs.GetFloat(SaveLoadConstants.RunningSpeedMultiplierPrefsKey);
        playerMoving.JumpForce = PlayerPrefs.GetFloat(SaveLoadConstants.JumpForcePrefsKey);
        playerMoving.IsMultipleJumpsActive =
                PlayerPrefs.GetInt(SaveLoadConstants.IsMultipleJumpsActivePrefsKey) == 1 ? true : false;

        playerItemEffects.IsShieldEnabled =
                PlayerPrefs.GetInt(SaveLoadConstants.IsShieldActivePrefsKey) == 1 ? true : false;

        playerMoving.ExtraJumps = PlayerPrefs.GetInt(SaveLoadConstants.ExtraJumpsPrefsKey);
        playerAttack.StonesMax = PlayerPrefs.GetInt(SaveLoadConstants.StonesMaxPrefsKey);
        playerAttack.CurrentStones = PlayerPrefs.GetInt(SaveLoadConstants.CurrentStonesPrefsKey);
        maxHealth = PlayerPrefs.GetInt(SaveLoadConstants.MaxHealthPrefsKey);
        CurrentHealth = PlayerPrefs.GetInt(SaveLoadConstants.CurrentHealthPrefsKey);
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
        playerAttack.DirectionVector = (mousePosition - playerAttack.StoneSpawner.position).normalized;

        return mousePosition;
    }

    private Vector2 GetTrajectoryPointPosition(float t)
    {
        return (Vector2)playerAttack.StoneSpawner.position +
                playerAttack.DirectionVector * playerAttack.ThrowForce * t +
                0.5f * Physics2D.gravity * (t * t);
    }

    private void CreateTrajectoryPoints()
    {
        trajectoryPoints = new GameObject[numberOfPoints];

        for (var i = 0; i < numberOfPoints; i++)
        {
            trajectoryPoints[i] =
                    Instantiate(trajectoryPointPrefab, playerAttack.StoneSpawner.position, Quaternion.identity,
                        transform);
        }
    }

    private void UpDateTrajectoryPoints()
    {
        if (!playerAttack.IsRangeAttackEnabled)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCtrlPressed = true;
            AudioManager.Instance.PlayButtonOnClickSfx();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCtrlPressed = false;
            AudioManager.Instance.PlayButtonOnClickSfx();
        }

        for (var i = 0; i < trajectoryPoints.Length; i++)
        {
            trajectoryPoints[i].transform.position = GetTrajectoryPointPosition(i * 0.025f);
            trajectoryPoints[i].SetActive(isCtrlPressed);
        }
    }

    private IEnumerator InvulnerablePlayer(float time)
    {
        yield return new WaitForSeconds(time);

        IsInvulnerable = false;

        playerItemEffects.IsInvulnerableItem = false;
        
        CancelInvoke(nameof(AnimationInvulnerable));
        sr.color = Color.white;

    }

    private void Flip()
    {
        var scaleTransform = transform;
        var newScale = scaleTransform.localScale;

        isFacingRight = !isFacingRight;
        newScale.x *= -1;
        scaleTransform.localScale = newScale;
    }

    private void AddHp(int amount)
    {
        CurrentHealth += amount;

        if (CurrentHealth > MAXHealth)
        {
            CurrentHealth = MAXHealth;
        }

        OnPlayerHpChanged?.Invoke();
    }

    private void Invulnerable(float time)
    {
        StartCoroutine(InvulnerablePlayer(time));
    }

    private void HpChange()
    {
        if (CurrentHealth == maxHealth)
        {
            maxHealth++;
            CurrentHealth = maxHealth;
        }
        else
        {
            CurrentHealth = maxHealth;
        }

        OnPlayerHpChanged?.Invoke();
    }
}

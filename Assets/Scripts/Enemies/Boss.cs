using System;
using Pathfinding;
using UnityEngine;

public class Boss : BaseEnemy

{
    [Header("Attack")]
    [SerializeField] private int attackValue;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackRadius;
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackTime;
    [SerializeField] private Transform attackPoint;

    [Header("Speed")]
    [SerializeField] private float speed;

    [Header("Vfx")]
    [SerializeField] private GameObject bossAttackVfx;

    private AIPath aiPath;

    private AIDestinationSetter aiDestinationSetter;

    private Transform player;

    private bool playerInAttackRange;
    private bool playerInChaseRange;

    private bool isRageActive;

    private float attackTimer;
    private float velocity;
    private float distance;

    public static event Action OnBossDeath;
    public static event Action OnBossHpChanged;
    public static event Action OnEnterChaseZone;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    protected override void Awake()
    {
        base.Awake();

        aiPath = GetComponent<AIPath>();
        aiPath.maxSpeed = speed;
        aiDestinationSetter = GetComponentInChildren<AIDestinationSetter>();
    }

    protected void Start()
    {
        player = FindObjectOfType<SuperPlayer>().transform;
    }

    private void Update()
    {
        if (IsDead || player == null) return;

        CalculateVariables();

        Flip();

        CheckState();
    }

    public void DealDamage()
    {
        AudioManager.Instance.PLaySfx(SfxType.BossAttack);

        if (bossAttackVfx != null)
        {
            Instantiate(bossAttackVfx, attackPoint.position, Quaternion.identity, transform);
        }

        var damageableObjects =
                Physics2D.OverlapCircleAll(attackPoint.position, attackRadius,
                    LayerMask.GetMask(Layers.Player));

        foreach (var enemy in damageableObjects)
        {
            enemy.GetComponent<DamageableObject>().ApplyDamage(attackValue);
        }
    }

    private void CalculateVariables()
    {
        velocity = aiPath.desiredVelocity.x;
        distance = transform.position.x - player.transform.position.x;
        playerInAttackRange = attackRange > transform.position.x - player.position.x;
        playerInChaseRange = chaseRange > transform.position.x - player.position.x;
        Animator.SetFloat(AnimationFloatNames.Speed, Mathf.Abs(velocity));
    }

    private void CheckState()
    {
        if (playerInChaseRange && aiDestinationSetter.target == null ||
            playerInChaseRange && distance > attackRange)
        {
            Chase();
        }
        else if (playerInChaseRange && playerInAttackRange)
        {
            Attack();
        }

        if (CurrentHealth == MAXHealth / 2 && !isRageActive)
        {
            ActivateRage();
        }
    }

    private void Flip()
    {
        if (transform.position.x < player.transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (transform.position.x > player.transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void Chase()
    {
        OnEnterChaseZone?.Invoke();
        aiDestinationSetter.target = player;
        Animator.ResetTrigger(AnimationTriggerNames.Attack);
    }

    private void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer < attackTime) return;

        Animator.SetTrigger(AnimationTriggerNames.Attack);

        attackTimer = 0;
    }

    protected override void Die()
    {
        base.Die();
        Animator.SetBool(AnimationBoolNames.IsDead, IsDead);
        aiDestinationSetter.target = null;
        AudioManager.Instance.PLaySfx(SfxType.BossDeath);

        Coll2D.enabled = false;
        Rb2D.Sleep();
        aiPath.enabled = false;
        OnBossDeath?.Invoke();
    }

    private void ActivateRage()
    {
        Animator.SetBool(AnimationBoolNames.IsRageActive, true);
        isRageActive = true;
        AudioManager.Instance.PLaySfx(SfxType.BossRage);
        speed = 8;
        aiPath.maxSpeed = speed;
        attackTime /= 2;
    }

    public override void ApplyDamage(int amount)
    {
        base.ApplyDamage(amount);
        AudioManager.Instance.PLaySfx(SfxType.BossHit);
        OnBossHpChanged?.Invoke();
    }
}

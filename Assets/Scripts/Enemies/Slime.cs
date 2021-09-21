using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class Slime : BaseEnemy
{
    [SerializeField] protected float TargetDetectionValue = 0.1f;
    [SerializeField] protected Transform Bottom;
    [SerializeField] private float idleTime;
    [SerializeField] private float speed;
    [SerializeField] protected GameObject TargetPrefab;
    
    protected Transform target1;
    protected Transform target2;
    
    protected Transform CurrentTargetPosition;
    protected AIDestinationSetter AIDestinationSetter;
    protected float Distance;

    private AIPath aiPath;
    private float idleTimer;

    private bool isGrounded;

    protected override void Awake()
    {
        base.Awake();
        aiPath = GetComponent<AIPath>();
        aiPath.maxSpeed = speed;
        AIDestinationSetter = GetComponent<AIDestinationSetter>();
        CreateTargets();

        var targetIndex = Random.Range(1, 3);

        AIDestinationSetter.target = CurrentTargetPosition = targetIndex == 1 ? target1 : target2;
    }
    
    private void Update()
    {
        if (IsDead) return;

        isGrounded = Physics2D.OverlapCircle(Bottom.position, 0.2f, LayerMask.GetMask(Layers.Ground));

        Animator.SetBool(AnimationBoolNames.IsGrounded, isGrounded);
        Animator.SetFloat(AnimationFloatNames.Velocity, aiPath.desiredVelocity.magnitude);

        CheckDistance();
        CheckFlip();
    }

    private void CreateTargets()
    {
        target1 = Instantiate(TargetPrefab, transform.position + Vector3.left, Quaternion.identity).transform;
        target2 = Instantiate(TargetPrefab, transform.position + Vector3.right, Quaternion.identity).transform;
    }
    
    private void CheckFlip()
    {
        if (transform.position.x < CurrentTargetPosition.transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (transform.position.x > CurrentTargetPosition.transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    protected virtual void CheckDistance()
    {
        Distance = Vector3.Distance(transform.position, CurrentTargetPosition.position);

        if (Distance <= TargetDetectionValue)
        {
            ChangeTarget();
        }
    }

    public override void ApplyDamage(int amount)
    {
        base.ApplyDamage(amount);

        if (CurrentHealth >= 1)
        {
            AudioManager.Instance.PLaySfx(SfxType.SlimeHurt);
        }
    }

    protected override void Die()
    {
        base.Die();
        AudioManager.Instance.PLaySfx(SfxType.SlimeDeath);
        Coll2D.enabled = false;
        Rb2D.Sleep();
        IsDead = true;
        AIDestinationSetter.target = null;
        Animator.SetBool(AnimationBoolNames.IsDead, IsDead);
        aiPath.enabled = false;
    }

    protected virtual void ChangeTarget()
    {
        idleTimer -= Time.deltaTime;

        if (idleTimer <= 0)
        {
            AIDestinationSetter.target = CurrentTargetPosition = CurrentTargetPosition == target1 ? target2 : target1;
            idleTimer = idleTime;
        }
    }
}

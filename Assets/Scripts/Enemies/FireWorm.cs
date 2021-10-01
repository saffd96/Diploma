using System.Collections;
using UnityEngine;

public class FireWorm : BaseEnemy
{
    [Header("Attack")]
    [SerializeField] private float throwForce = 1f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackTime = 1.5f;
    [SerializeField] private Transform fireballSpawner;
    [SerializeField] private GameObject fireball;

    private GameObject projectile;
    private float attackTimer;
    private float distance;
    private Transform player;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(fireballSpawner.position, attackRange);
    }

    private void Start()
    {
        attackTimer = attackTime;
        player = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        if (IsDead || player == null) return;

        CalculateVariables();

        CheckState();
    }

    private void CalculateVariables()
    {
        distance = Mathf.Sqrt(
            (player.position.x - fireballSpawner.position.x) * (player.position.x - fireballSpawner.position.x) +
            ((player.position.y - fireballSpawner.position.y) * (player.position.y - fireballSpawner.position.y)));
    }

    private void CheckState()
    {
        if (attackRange > distance)
        {
            Attack();
        }
    }

    private void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer < attackTime) return;

        Animator.SetTrigger(AnimationTriggerNames.Attack);

        StartCoroutine(DelaySpawn());

        attackTimer = 0;
    }

    private void CreateFireBall()
    {
        var a = Mathf.Sqrt(player.position.x * player.position.x + player.position.y * player.position.y);
        var b = Mathf.Sqrt(fireballSpawner.position.x * fireballSpawner.position.x + fireballSpawner.position.y * fireballSpawner.position.y);
        var fireballAngle = 180 * Mathf.Cos((a * b) / (Mathf.Abs(a) * Mathf.Abs(b))) / Mathf.PI;

        if (player.position.x> transform.position.x)
        {
            fireballAngle += transform.eulerAngles.z;
        }
        else
        {
            fireballAngle -= transform.eulerAngles.z;
        }
        
        projectile = Instantiate(fireball, fireballSpawner.position, Quaternion.Euler(0, 0, fireballAngle), transform);
        projectile.transform.localScale = projectile.transform.parent.localScale;
        projectile.GetComponent<Rigidbody2D>()
               .AddForce((player.position - transform.position).normalized * throwForce, ForceMode2D.Impulse);
    }

    protected override void Die()
    {
        base.Die();
        Animator.SetBool(AnimationBoolNames.IsDead, IsDead);

        AudioManager.Instance.PLaySfx(SfxType.FireWormDie);

        Coll2D.enabled = false;
    }

    IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(0.13f);

        CreateFireBall();
    }
}

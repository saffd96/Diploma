using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack settings")]
    [SerializeField] private int attackValue;
    [SerializeField] private float attackRate;
    [SerializeField] private float attackRadius;
    [SerializeField] private bool isRangeAttackEnabled;
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private Transform stoneSpawner;
    [SerializeField] private Transform attackCollider;

    private Player player;
    private GameObject stone;

    private float attackTimer;
    private float rangeAttackTimer;
    private bool isMeleeAttack;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCollider.position, attackRadius);
    }

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        Attack();
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
        
        // if (!player.IsGrounded /*|| player.IsShiftPressed*/ || player.IsPushing || (player.IsClimbing && player.MoveVerticalInput > 0)) return;
        // помоги зарефакторить)))))
        
        switch (isMeleeAttack)
        {
            case true when attackTimer > attackRate:
                MeleeAttack();
                attackTimer = 0;

                break;
            case false when rangeAttackTimer > attackRate && player.MoveHorizontalInput == 0 && isRangeAttackEnabled:
                RangeAttack();
                rangeAttackTimer = 0;

                break;
        }

    }

    private void MeleeAttack()
    {
        player.PlayerAnimationController.Attack();

        if (player.MoveHorizontalInput < 0.1)
        {
            player.PlayerAnimationController.SetAttackType(Random.Range(1, 3));
        }

        var damageableObjects =
                Physics2D.OverlapCircleAll(attackCollider.position, attackRadius, 
                    LayerMask.GetMask(Layers.Enemy));

        foreach (var enemy in damageableObjects)
        {
            enemy.GetComponent<DamageableObject>().ApplyDamage(attackValue);
        }
    }

    private void RangeAttack()
    {
        player.PlayerAnimationController.Throw();

        StartCoroutine(Throw());
    }
    private IEnumerator Throw()
    {
        yield return new WaitForSeconds(0.2f);

        stone = Instantiate(stonePrefab, stoneSpawner.position, Quaternion.identity);
        stone.GetComponent<Rigidbody2D>().AddForce(Vector2.right*transform.localScale * 1.5f, ForceMode2D.Impulse);
        Destroy(stone, 3);
    }
}

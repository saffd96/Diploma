using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayerComponents
{
    [RequireComponent(typeof(PlayerAnimationController), typeof(PlayerVfx))]
    public class PlayerAttack : MonoBehaviour
    {
        public bool IsRangeAttackEnabled;

        public int StonesMax;
        public int AttackValue;

        public float AttackRate;
        public float ThrowForce = 10f;
        
        public Transform StoneSpawner;

        [HideInInspector]
        public float AttackTimer;
        [HideInInspector]
        public float RangeAttackTimer;
        [HideInInspector]
        public bool IsMeleeAttack;
        [HideInInspector]
        public int CurrentStones;
        [HideInInspector]
        public Vector2 DirectionVector;

        [SerializeField] private GameObject stonePrefab;
        [SerializeField] private float attackRadius;
        [SerializeField] private Transform colliderDetector;

        private PlayerAnimationController playerAnimationController;
        private PlayerVfx playerVfx;
        private GameObject stone;

        public static event Action OnStonesChanged;

        private void OnEnable()
        {
            PlayerItemEffects.OnStonesPickup += AddStones;
            PlayerPowerUp.OnRangePowerUp += RangeAttackPowerUp;
            PlayerPowerUp.OnMeleePowerUp += AddDamage;
        }

        private void OnDisable()
        {
            PlayerItemEffects.OnStonesPickup -= AddStones;
            PlayerPowerUp.OnRangePowerUp -= RangeAttackPowerUp;
            PlayerPowerUp.OnMeleePowerUp -= AddDamage;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(colliderDetector.position, attackRadius);
        }

        private void Start()
        {
            playerAnimationController = GetComponent<PlayerAnimationController>();
            playerVfx = GetComponent<PlayerVfx>();
        }

        public void Attack()
        {
            switch (IsMeleeAttack)
            {
                case true when AttackTimer > AttackRate:
                    MeleeAttack();
                    AttackTimer = 0;

                    break;
                case false when RangeAttackTimer > AttackRate && IsRangeAttackEnabled:
                    RangeAttack();
                    RangeAttackTimer = 0;

                    break;
            }
        }

        private void MeleeAttack()
        {
            playerAnimationController.Attack();

            if (playerAnimationController.GetSpeed() < 0.1)
            {
                playerAnimationController.SetAttackType(Random.Range(1, 3));
            }

            AudioManager.Instance.PLaySfx(SfxType.PlayerAttack);

            if (playerVfx.AttackVfx != null)
            {
                Instantiate(playerVfx.AttackVfx, colliderDetector.position, Quaternion.identity);
            }

            var damageableObjects =
                    Physics2D.OverlapCircleAll(colliderDetector.position, attackRadius,
                        LayerMask.GetMask(Layers.Enemy));

            foreach (var enemy in damageableObjects)
            {
                enemy.GetComponent<DamageableObject>().ApplyDamage(AttackValue);
            }
        }

        private void RangeAttack()
        {
            if (CurrentStones > 0)
            {
                StartCoroutine(Throw());
                CurrentStones--;
                playerAnimationController.Throw();
                OnStonesChanged?.Invoke();
            }
        }

        private IEnumerator Throw()
        {
            yield return new WaitForSeconds(0.2f);

            AudioManager.Instance.PLaySfx(SfxType.Throw);

            stone = Instantiate(stonePrefab, StoneSpawner.position, Quaternion.identity);

            stone.GetComponent<Rigidbody2D>().velocity = DirectionVector * ThrowForce;
            Destroy(stone, 3);
        }

        private void AddStones(int amount)
        {
            CurrentStones += amount;
            OnStonesChanged?.Invoke();
        }

        public void RangeAttackPowerUp() // make private
        {
            if (IsRangeAttackEnabled)
            {
                StonesMax += 5;
                AddStones(5);
            }
            else
            {
                IsRangeAttackEnabled = true;
                StonesMax += 1;
                AddStones(1);
            }
        }

        private void AddDamage()
        {
            AttackValue++;
        }
    }
}

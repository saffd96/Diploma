using System.Collections;
using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(PlayerVfx), typeof(PlayerAnimationController))]
    public class PlayerMoving : MonoBehaviour
    {
        public bool IsRunActive;
        public bool IsMultipleJumpsActive;

        public float MAXSpeed;
        public float RunningSpeedMultiplier = 1.5f;
        public float JumpForce;
        public float GroundDetectRadius;

        public Transform LegsPosition;

        public LayerMask WhatIsGround;

        [HideInInspector]
        public int ExtraJumps = 1;
        [HideInInspector]
        public Rigidbody2D Rb;
        [HideInInspector]
        public bool IsClimbing;
        [HideInInspector]
        public bool IsGrounded;

        [SerializeField] private float colliderDetectRadius;
        [SerializeField] private Transform colliderDetector;

        [SerializeField] private int maxStamina = 100;
        [SerializeField] private int runUsedStaminaAmount = 2;
        [SerializeField] private int jumpUsedStamina = 10;

        private int currentStamina;

        private StaminaBarView staminaBarView;
        private PlayerVfx playerVfx;

        private PlayerAnimationController playerAnimationController;

        private float moveVerticalInput;
        private float speed;
        private float climbSpeed;

        private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
        private Coroutine regen;

        private int jumps;

        public float MoveHorizontalInput { get; private set; }
        public bool IsShiftPressed { get; private set; }
        public bool IsPushing { get; private set; }

        public PlayerMoving(LayerMask whatIsGround, float groundDetectRadius, Transform legsPosition)
        {
            WhatIsGround = whatIsGround;
            GroundDetectRadius = groundDetectRadius;
            LegsPosition = legsPosition;
        }

        private void OnEnable()
        {
            PlayerPowerUp.OnAddSpeedPowerUp += AddSpeed;
            PlayerPowerUp.OnRunPowerUp += RunningSpeedPowerUp;
            PlayerPowerUp.OnJumpPowerUp += AddJumpForce;
            PlayerPowerUp.OnExtraJumpPowerUp += AddExtraJumps;
        }

        private void OnDisable()
        {
            PlayerPowerUp.OnAddSpeedPowerUp -= AddSpeed;
            PlayerPowerUp.OnRunPowerUp -= RunningSpeedPowerUp;
            PlayerPowerUp.OnJumpPowerUp -= AddJumpForce;
            PlayerPowerUp.OnExtraJumpPowerUp -= AddExtraJumps;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(LegsPosition.position, GroundDetectRadius);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(colliderDetector.position, colliderDetectRadius);
        }

        protected void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
            staminaBarView = FindObjectOfType<StaminaBarView>();
            climbSpeed = MAXSpeed;
            jumps = ExtraJumps;
            speed = MAXSpeed;
            currentStamina = maxStamina;
            staminaBarView.SetMaxValue(maxStamina);
            staminaBarView.SetValue(currentStamina);
        }

        private void Start()
        {
            playerVfx = GetComponent<PlayerVfx>();
            playerAnimationController = GetComponent<PlayerAnimationController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(Tags.ClimbObject)) return;

            IsClimbing = true;
            CheckClimbCondition();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag(Tags.ClimbObject)) return;

            IsClimbing = false;
            CheckClimbCondition();
        }

        public void Move()
        {
            MoveHorizontalInput = Input.GetAxis("Horizontal");

            Rb.velocity = new Vector2(MoveHorizontalInput * speed, Rb.velocity.y);
        }

        public void Climb()
        {
            moveVerticalInput = Input.GetAxis("Vertical");

            playerAnimationController.SetSpeed(Mathf.Abs(MoveHorizontalInput));

            if (IsClimbing)
            {
                Rb.velocity = new Vector2(Rb.velocity.x, moveVerticalInput * climbSpeed);
                playerAnimationController.SetClimbingSpeed(Mathf.Abs(moveVerticalInput));
            }
        }

        public void CheckRunCondition()
        {
            playerVfx.RunVfx.SetActive(IsShiftPressed && IsGrounded && Mathf.Abs(MoveHorizontalInput) > 0.25f);

            playerVfx.SetActiveDustFromRun();

            if (IsShiftPressed && IsGrounded && Mathf.Abs(MoveHorizontalInput) > 0.25f)
            {
                UseStamina(runUsedStaminaAmount);
            }

            if (currentStamina <= runUsedStaminaAmount)
            {
                StopRun();

                return;
            }

            switch (IsRunActive)
            {
                case true when Input.GetKey(KeyCode.LeftShift) && IsGrounded && !IsShiftPressed && currentStamina >= runUsedStaminaAmount:
                    StartRun();

                    break;
                case true when Input.GetKeyUp(KeyCode.LeftShift):
                    StopRun();

                    break;
            }
        }

        public void CheckJumpCondition()
        {
            if (!Input.GetKeyDown(KeyCode.Space)) return;

            if (IsGrounded)
            {
                jumps = ExtraJumps;
            }

            if (currentStamina <= jumpUsedStamina)
            {
                return;
            }

            switch (IsMultipleJumpsActive)
            {
                case false when IsGrounded:
                    Jump();

                    break;
                case true when jumps >= 0:
                    Jump();
                    jumps--;

                    if (!IsGrounded && playerVfx.ExtraJumpVfx != null)
                    {
                        playerVfx.CreateJumpVfx();
                    }

                    break;
            }
        }

        private void CheckClimbCondition()
        {
            if (IsClimbing)
            {
                Rb.bodyType = RigidbodyType2D.Kinematic;
            }
            else
            {
                playerAnimationController.SetClimbingSpeed(0);
                Rb.bodyType = RigidbodyType2D.Dynamic;
            }

            playerAnimationController.SetIsClimbing(IsClimbing);
        }

        public void CheckPushCondition()
        {
            IsPushing = Physics2D.OverlapCircle(colliderDetector.position, colliderDetectRadius,
                LayerMask.GetMask(Layers.InteractObjects));

            playerAnimationController.SetIsPushing(IsPushing);
        }

        private void StartRun()
        {
            IsShiftPressed = true;

            playerAnimationController.SetIsRunning(true);
            speed *= RunningSpeedMultiplier;
        }

        private void StopRun()
        {
            playerAnimationController.SetIsRunning(false);
            speed = MAXSpeed;
            IsShiftPressed = false;
        }

        private void Jump()
        {
            UseStamina(jumpUsedStamina);

            playerAnimationController.Jump();

            Rb.velocity = Vector2.up * JumpForce;
            AudioManager.Instance.PLaySfx(SfxType.Jump);
        }

        private void AddSpeed()
        {
            MAXSpeed++;
            speed = MAXSpeed;
        }

        private void RunningSpeedPowerUp()
        {
            if (IsRunActive)
            {
                RunningSpeedMultiplier += 0.25f;
            }
            else
            {
                IsRunActive = true;
            }
        }

        private void AddJumpForce()
        {
            JumpForce++;
        }

        private void AddExtraJumps()
        {
            if (IsMultipleJumpsActive)
            {
                ExtraJumps++;
            }
            else
            {
                IsMultipleJumpsActive = true;
            }
        }

        private void UseStamina(int amount)
        {
            if (currentStamina - amount >= 0)
            {
                currentStamina -= amount;

                staminaBarView.SetValue(currentStamina);

                if (regen != null)
                {
                    StopCoroutine(regen);
                }

                regen = StartCoroutine(RegenStamina());
            }
        }

        private IEnumerator RegenStamina()
        {
            yield return new WaitForSeconds(2);

            while (currentStamina < maxStamina)
            {
                currentStamina += maxStamina / 100;
                staminaBarView.SetValue(currentStamina);

                yield return regenTick;
            }

            regen = null;
        }
    }
}

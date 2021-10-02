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

        private PlayerVfx playerVfx;
        
        private PlayerAnimationController playerAnimationController;

        private float moveVerticalInput;
        private float speed;
        private float climbSpeed;

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
            climbSpeed = MAXSpeed;
            jumps = ExtraJumps;
            speed = MAXSpeed;
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
            playerVfx.RunVfx.SetActive(IsShiftPressed && IsGrounded && Mathf.Abs(MoveHorizontalInput)>0.25f );

            playerVfx.SetActiveDustFromRun();

            switch (IsRunActive)
            {
                case true when Input.GetKey(KeyCode.LeftShift) && IsGrounded && !IsShiftPressed:
                    IsShiftPressed = true;

                    playerAnimationController.SetIsRunning(true);
                    speed *= RunningSpeedMultiplier;

                    break;
                case true when Input.GetKeyUp(KeyCode.LeftShift):

                    playerAnimationController.SetIsRunning(false);
                    speed /= RunningSpeedMultiplier;
                    IsShiftPressed = false;

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

        private void Jump()
        {
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
    }
}

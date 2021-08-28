using UnityEngine;

[RequireComponent(typeof(PlayerAnimationController))]
public class Player : DamageableObject
{
    [SerializeField] private float runningSpeedMultiplier;
    [SerializeField] private bool isRunActive;
    [SerializeField] private float maxSpeed;
    [Space]
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isMultipleJumpsActive;
    [SerializeField] private int extraJumps;

    public float MaxSpeed => maxSpeed;
    public float JumpForce => jumpForce;
    public bool IsRunActive => isRunActive;
    public int ExtraJumps => extraJumps;
    public bool IsMultipleJumpsActive => isMultipleJumpsActive;
    public float RunningSpeedMultiplier => runningSpeedMultiplier;

    public Rigidbody2D Rb { get; private set; }
    public PlayerAnimationController PlayerAnimationController { get; private set; }
    public float MoveHorizontalInput { get; private set; }
    public float MoveVerticalInput { get; private set; }

    private new void Awake()
    {
        PlayerAnimationController = GetComponent<PlayerAnimationController>();
        Rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveHorizontalInput = Input.GetAxis("Horizontal");
        MoveVerticalInput = Input.GetAxis("Vertical");
    }

    public override void ApplyDamage(int amount)
    {
        base.ApplyDamage(amount);
        PlayerAnimationController.GetDamage();
    }
}

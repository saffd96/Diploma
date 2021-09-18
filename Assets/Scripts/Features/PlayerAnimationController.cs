using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour

{
    private Animator anim;

    private int hurtId;
    private int attackId;
    private int throwId;
    private int jumpId;
    private int attackTypeId;
    private int speedId;
    private int velocityId;
    private int isRunningId;
    private int isGroundedId;
    private int isDeadId;
    private int isClimbingId;
    private int isClimbingSpeedId;
    private int isPushingId;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        jumpId = Animator.StringToHash(AnimationTriggerNames.Jump);
        hurtId = Animator.StringToHash(AnimationTriggerNames.Hurt);
        attackId = Animator.StringToHash(AnimationTriggerNames.Attack);
        throwId = Animator.StringToHash(AnimationTriggerNames.Throw);

        speedId = Animator.StringToHash(AnimationFloatNames.Speed);
        velocityId = Animator.StringToHash(AnimationFloatNames.Velocity);
        isClimbingSpeedId = Animator.StringToHash(AnimationFloatNames.ClimbingSpeed);

        attackTypeId = Animator.StringToHash(AnimationIntNames.AttackType);

        isRunningId = Animator.StringToHash(AnimationBoolNames.IsRunning);
        isGroundedId = Animator.StringToHash(AnimationBoolNames.IsGrounded);
        isDeadId = Animator.StringToHash(AnimationBoolNames.IsDead);
        isClimbingId = Animator.StringToHash(AnimationBoolNames.IsClimbing);
        isPushingId = Animator.StringToHash(AnimationBoolNames.IsPushing);
    }

    public void Jump()
    {
        anim.SetTrigger(jumpId);
    }

    public void Attack()
    {
        anim.SetTrigger(attackId);
    }

    public void Throw()
    {
        anim.SetTrigger(throwId);
    }

    public void GetDamage()
    {
        anim.SetTrigger(hurtId);
    }

    public void SetSpeed(float value)
    {
        anim.SetFloat(speedId, value);
    }

    public void SetClimbingSpeed(float value)
    {
        anim.SetFloat(isClimbingSpeedId, value);
    }

    public void SetVelocity(float value)
    {
        anim.SetFloat(velocityId, value);
    }

    public void SetAttackType(int value)
    {
        anim.SetInteger(attackTypeId, value);
    }

    public void SetIsRunning(bool value)
    {
        anim.SetBool(isRunningId, value);
    }

    public void SetIsGrounded(bool value)
    {
        anim.SetBool(isGroundedId, value);
    }

    public void SetIsClimbing(bool value)
    {
        anim.SetBool(isClimbingId, value);
    }

    public void SetIsPushing(bool value)
    {
        anim.SetBool(isPushingId, value);
    }

    public void SetIsDead(bool value)
    {
        anim.SetBool(isDeadId, value);
    }

    public float GetSpeed()
    {
        return Mathf.Abs(anim.GetFloat(speedId));
    }

    public bool GetGrounded()
    {
        return anim.GetBool(isGroundedId);
    }
}
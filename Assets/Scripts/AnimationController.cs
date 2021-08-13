using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour

{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }

    public void SetFloat(string floatName, float value)
    {
        anim.SetFloat(floatName, value);
    }

    public void SetBool(string boolName, bool value)
    {
        anim.SetBool(boolName, value);
    }

    public float GetFloat(string floatName)
    {
        return anim.GetFloat(floatName);
    }

    public bool GetBool(string boolName)
    {
        return anim.GetBool(boolName);
    }
}

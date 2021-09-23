using UnityEngine;

public class VfxAnimationController : MonoBehaviour
{
    [SerializeField] private AnimationClip anim;
    private float clipLifeTime;

    private void Awake()
    {
        clipLifeTime = anim.length;

        if (!anim.isLooping)
        {
            Destroy(gameObject, clipLifeTime);
        }
    }
}

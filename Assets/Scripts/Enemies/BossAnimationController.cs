using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    [SerializeField] private Boss boss;

    public void DealDamage()
    {
        boss.DealDamage();
    }
}

using UnityEngine;

public class SlimeControllerHelper : MonoBehaviour
{
    private BigSlime bigSlime;

    private void Awake()
    {
        bigSlime = GetComponentInParent<BigSlime>();
    }

    private void EndSpin()
    {
       // bigSlime.IsSpinEnded = true;
    }

}

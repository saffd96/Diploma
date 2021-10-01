using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag(Tags.Player))
        {
            other.gameObject.GetComponent<Player>().ApplyDamage(damageAmount);
        }
    }
}

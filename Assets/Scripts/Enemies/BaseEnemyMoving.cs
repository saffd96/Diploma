using UnityEngine;

public class BaseEnemyMoving : MonoBehaviour
{
    [SerializeField] private float speed;

    private bool targetSwitch;
    private bool isTargetSet;
    private bool isFacingRight = true;

    private Rigidbody2D rb;

    public bool IsTargetSet
    {
        get => isTargetSet;
        set => isTargetSet = value;
    }
    public Vector2 Target { get; private set; }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(Target, 0.2f);
    }
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
    }

    private void Update()
    {
        Move();
    }

    public void GetTarget()
    {
        if (isTargetSet) return;

        if (targetSwitch)
        {
            Target = new Vector2(transform.position.x + 3, transform.position.y);
        }
        else
        {
            Target = new Vector2(transform.position.x - 3, transform.position.y);
        }
        Flip();
        isTargetSet = true;
    }

    private void Move()
    {
        if (isFacingRight)
        {
            rb.velocity = (Vector2.right * speed * Time.deltaTime).normalized;
        }
        else
        {
            rb.velocity = (Vector2.left * speed * Time.deltaTime).normalized;
        }
    }

    private void Flip()
    {
        var scaleTransform = transform;
        var newScale = scaleTransform.localScale;

        isFacingRight = !isFacingRight;
        newScale.x *= -1;
        scaleTransform.localScale = newScale;
        targetSwitch = !targetSwitch;
    }
}

using UnityEngine;

public class BaseEnemyMoving : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private float leftOffset = 3;
    [SerializeField] private float rightOffset = 3;

    [SerializeField] private bool isFacingRight = true;

    private bool targetSwitch;

    private bool isFirstTime = true;

    private Rigidbody2D rb;

    public bool IsTargetSet { get; set; }
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

    private void FixedUpdate()
    {
        Move();
    }

    public void GetTarget()
    {
        if (IsTargetSet) return;

        if (targetSwitch)
        {
            Target = isFirstTime
                    ? new Vector2(transform.position.x + rightOffset / 2, transform.position.y)
                    : new Vector2(transform.position.x + rightOffset, transform.position.y);
        }
        else
        {
            Target = isFirstTime
                    ? new Vector2(transform.position.x - leftOffset / 2, transform.position.y)
                    : new Vector2(transform.position.x - leftOffset, transform.position.y);
        }

        isFirstTime = false;
        IsTargetSet = true;
        Flip();
    }

    private void Move()
    {
        if (isFacingRight)
        {
            rb.velocity = (Vector2.right * Time.deltaTime).normalized * speed;
        }
        else
        {
            rb.velocity = (Vector2.left * Time.deltaTime).normalized * speed;
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

using UnityEngine;

public class ArenaProjectile : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 8f;

    [Header("Damage")]
    public int damage = 1;

    private Rigidbody rb;
    private Vector3 moveDirection;

    private float lastBounceTime = -1f;
    private const float bounceCooldown = 0.05f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;

        rb.collisionDetectionMode =
            CollisionDetectionMode.ContinuousDynamic;

        rb.interpolation =
            RigidbodyInterpolation.Interpolate;

        // We want a completely flat projectile
        rb.constraints =
            RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;
    }

    void Start()
    {
        moveDirection = transform.forward;

        moveDirection.y = 0f;
        moveDirection.Normalize();

        rb.linearVelocity =
            moveDirection * speed;
    }

    void FixedUpdate()
    {
        // NEVER allow physics to slow the projectile down.
        rb.linearVelocity =
            moveDirection * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        // -------------------------
        // MONSTER
        // -------------------------

        MonsterChase monster =
            collision.gameObject.GetComponentInParent<MonsterChase>();

        MonsterHealth health =
            collision.gameObject.GetComponentInParent<MonsterHealth>();

        if (monster != null)
        {
            if (monster.IsStunned() && health != null)
            {
                health.TakeDamage(damage);

                Debug.Log(
                    "Arena projectile hit stunned monster!"
                );
            }

            // IMPORTANT:
            // Monster does NOT change projectile trajectory.
            return;
        }

        // Prevent rapid double-bounces at corners.
        if (Time.time - lastBounceTime < bounceCooldown)
            return;

        lastBounceTime = Time.time;

        if (collision.contactCount == 0)
            return;

        Vector3 normal =
            collision.contacts[0].normal;

        normal.y = 0f;

        if (normal.sqrMagnitude < 0.01f)
            return;

        normal.Normalize();

        Vector3 reflected =
            Vector3.Reflect(
                moveDirection,
                normal
            );

        reflected.y = 0f;

        if (reflected.sqrMagnitude < 0.01f)
        {
            reflected = -moveDirection;
        }

        moveDirection =
            reflected.normalized;

        // Push slightly away from wall so it doesn't
        // immediately collide with the same wall again.
        transform.position +=
            moveDirection * 0.08f;

        rb.linearVelocity =
            moveDirection * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        // Only use this for the new inner walls
        if (!other.CompareTag("BounceWall"))
            return;

        // Prevent rapid repeated bouncing
        if (Time.time - lastBounceTime < bounceCooldown)
            return;

        lastBounceTime = Time.time;

        // Find the closest point on the wall
        Vector3 closestPoint =
            other.ClosestPoint(transform.position);

        // Calculate direction away from the wall
        Vector3 normal =
            transform.position - closestPoint;

        normal.y = 0f;

        // If Unity cannot determine a good normal,
        // simply reverse the projectile
        if (normal.sqrMagnitude < 0.01f)
        {
            moveDirection = -moveDirection;
        }
        else
        {
            normal.Normalize();

            Vector3 reflected =
                Vector3.Reflect(
                    moveDirection,
                    normal
                );

            reflected.y = 0f;

            moveDirection =
                reflected.normalized;
        }

        // Push projectile slightly away from wall
        transform.position +=
            moveDirection * 0.15f;

        rb.linearVelocity =
            moveDirection * speed;

        Debug.Log("BOUNCED OFF INNER WALL");
    }

    public void BounceFromWall(Transform wall)
    {
        Vector3 directionToProjectile =
            transform.position - wall.position;

        directionToProjectile.y = 0f;

        // Horizontal-looking wall
        if (Mathf.Abs(wall.localScale.x) >
            Mathf.Abs(wall.localScale.z))
        {
            moveDirection.z *= -1f;
        }
        // Vertical-looking wall
        else
        {
            moveDirection.x *= -1f;
        }

        moveDirection.Normalize();

        transform.position +=
            moveDirection * 0.2f;

        rb.linearVelocity =
            moveDirection * speed;

        Debug.Log("BOUNCE!");
    }
}
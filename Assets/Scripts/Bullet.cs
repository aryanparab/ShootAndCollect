using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public void Fire(Vector3 direction)
    {
        rb.linearVelocity = direction.normalized * speed;
    }

    void FixedUpdate()
    {
        // Keep projectile moving forever at constant speed
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            rb.linearVelocity =
                rb.linearVelocity.normalized * speed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Hit monster
        if (collision.gameObject.CompareTag("Monster"))
        {
            MonsterHealth health =
                collision.gameObject.GetComponent<MonsterHealth>();

            MonsterChase chase =
                collision.gameObject.GetComponent<MonsterChase>();

            // Monster only takes damage while stunned
            if (health != null &&
                chase != null &&
                chase.IsStunned())
            {
                health.TakeDamage(damage);

                Debug.Log("STUNNED MONSTER HIT!");
            }

            // Projectile still ricochets afterward
        }

        // Bounce off whatever was hit
        if (collision.contactCount > 0)
        {
            Vector3 incoming =
                rb.linearVelocity.normalized;

            Vector3 normal =
                collision.contacts[0].normal;

            Vector3 reflected =
                Vector3.Reflect(
                    incoming,
                    normal
                );

            rb.linearVelocity =
                reflected.normalized * speed;
        }
    }
}
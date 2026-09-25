using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 14f;
    public int damage = 1;
    public float stunDuration = 0.4f;

    private Rigidbody rb;
    private bool hasLanded = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 direction)
    {
        hasLanded = false;

        rb.useGravity = false;
        rb.linearVelocity = direction.normalized * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasLanded)
            return;

        // Ignore the player that fired the bullet
        if (collision.gameObject.CompareTag("Player"))
            return;

        // Hit monster
        if (collision.gameObject.CompareTag("Monster"))
        {
            MonsterHealth health =
                collision.gameObject.GetComponent<MonsterHealth>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }

            MonsterChase chase =
                collision.gameObject.GetComponent<MonsterChase>();

            if (chase != null)
            {
                chase.Stun(stunDuration);
            }
        }

        Land();
    }

    void Land()
    {
        hasLanded = true;

        // Stop the fired movement
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Allow bullet to fall onto the ground
        rb.useGravity = true;
    }

    public bool CanBePickedUp()
    {
        return hasLanded;
    }
}
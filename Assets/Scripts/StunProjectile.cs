using UnityEngine;

public class StunProjectile : MonoBehaviour
{
    public float speed = 18f;
    public float stunDuration = 2f;
    public float lifetime = 3f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("StunProjectile needs Rigidbody!");
            return;
        }

        rb.useGravity = false;

        rb.collisionDetectionMode =
            CollisionDetectionMode.ContinuousDynamic;
    }

    public void Fire(Vector3 direction)
    {
        rb.linearVelocity =
            direction.normalized * speed;

        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(
            "STUN PROJECTILE HIT: " +
            collision.gameObject.name
        );

        // Ignore player
        PlayerStealth player =
            collision.gameObject.GetComponentInParent<PlayerStealth>();

        if (player != null)
        {
            return;
        }

        MonsterChase monster =
            collision.gameObject.GetComponentInParent<MonsterChase>();

        if (monster != null)
        {
            Debug.Log("FOUND MONSTER - STUNNING");

            monster.Stun(stunDuration);

            Destroy(gameObject);
            return;
        }

        // Destroy if it hits anything else
        Destroy(gameObject);
    }
}
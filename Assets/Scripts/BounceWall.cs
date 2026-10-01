using UnityEngine;

public class BounceWall : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        ArenaProjectile projectile =
            collision.gameObject.GetComponentInParent<ArenaProjectile>();

        if (projectile != null)
        {
            projectile.BounceFromWall(transform);
        }
    }
}
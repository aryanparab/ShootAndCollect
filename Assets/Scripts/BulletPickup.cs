using UnityEngine;

public class BulletPickup : MonoBehaviour
{
    private Bullet bullet;

    void Awake()
    {
        bullet = GetComponentInParent<Bullet>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (bullet == null)
            return;

        if (!bullet.CanBePickedUp())
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerShoot playerShoot =
            other.GetComponent<PlayerShoot>();

        if (playerShoot != null)
        {
            playerShoot.RecoverBullet();
            Destroy(bullet.gameObject);
        }
    }
}
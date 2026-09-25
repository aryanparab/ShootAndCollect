using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform gunTip;

    public int maxAmmo = 6;
    public int currentAmmo = 6;

    public float fireCooldown = 0.4f;

    private float nextFireTime = 0f;

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (Time.time < nextFireTime)
            return;

        if (currentAmmo <= 0)
        {
            Debug.Log("NO BULLETS");
            return;
        }

        GameObject newBullet =
            Instantiate(
                bulletPrefab,
                gunTip.position,
                gunTip.rotation
            );

        Bullet bullet =
            newBullet.GetComponent<Bullet>();

        bullet.Fire(gunTip.forward);

        currentAmmo--;

        nextFireTime =
            Time.time + fireCooldown;

        Debug.Log("Ammo: " + currentAmmo);
    }

    public void RecoverBullet()
    {
        if (currentAmmo < maxAmmo)
        {
            currentAmmo++;
            Debug.Log("Recovered bullet. Ammo: " + currentAmmo);
        }
    }
}
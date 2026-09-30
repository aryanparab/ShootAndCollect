using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStunGun : MonoBehaviour
{
    public GameObject stunProjectilePrefab;
    public Transform gunTip;
    private PlayerStealth playerStealth;

    public float cooldown = 1f;

    private float nextFireTime = 0f;

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Fire();
        }
    }
    void Awake()
    {
        playerStealth =
            GetComponent<PlayerStealth>();
    }
    void Fire()
    {
        if (playerStealth != null &&
            playerStealth.IsHidden)
        {
            Debug.Log("Cannot shoot while hiding!");
            return;
        }
        if (Time.time < nextFireTime)
            return;

        if (stunProjectilePrefab == null || gunTip == null)
        {
            Debug.LogError("GunTip or StunProjectile prefab is not assigned.");
            return;
        }

        // Spawn slightly beyond the end of the gun
        Vector3 spawnPosition =
            gunTip.position + gunTip.forward * 0.4f;

        GameObject projectileObject =
            Instantiate(
                stunProjectilePrefab,
                spawnPosition,
                gunTip.rotation
            );

        Collider projectileCollider =
            projectileObject.GetComponent<Collider>();

        // Ignore all Player / Gun colliders
        if (projectileCollider != null)
        {
            Collider[] playerColliders =
                GetComponentsInChildren<Collider>();

            foreach (Collider playerCollider in playerColliders)
            {
                Physics.IgnoreCollision(
                    projectileCollider,
                    playerCollider
                );
            }
        }

        StunProjectile projectile =
            projectileObject.GetComponent<StunProjectile>();

        if (projectile != null)
        {
            projectile.Fire(gunTip.forward);
        }

        nextFireTime = Time.time + cooldown;
    }
}
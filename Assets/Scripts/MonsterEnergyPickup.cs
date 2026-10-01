using UnityEngine;
using System.Collections;

public class MonsterEnergyPickup : MonoBehaviour
{
    public float duration = 5f;
    public float speedMultiplier = 2f;

    private bool used = false;

    void OnTriggerEnter(Collider other)
    {
        if (used)
            return;

        // Only activate when the PLAYER touches it
        PlayerMovement player =
            other.GetComponentInParent<PlayerMovement>();

        if (player == null)
            return;

        MonsterChase monster =
            FindFirstObjectByType<MonsterChase>();

        if (monster == null)
            return;

        used = true;

        StartCoroutine(
            ActivateMonsterEnergy(monster)
        );
    }

    IEnumerator ActivateMonsterEnergy(MonsterChase monster)
    {
        float originalPatrolSpeed =
            monster.patrolSpeed;

        float originalChaseSpeed =
            monster.chaseSpeed;

        // Double monster speed
        monster.patrolSpeed *= speedMultiplier;
        monster.chaseSpeed *= speedMultiplier;

        Debug.Log("MONSTER ENERGY ACTIVATED!");

        // Make the pickup disappear
        Renderer pickupRenderer =
            GetComponent<Renderer>();

        if (pickupRenderer != null)
            pickupRenderer.enabled = false;

        Collider pickupCollider =
            GetComponent<Collider>();

        if (pickupCollider != null)
            pickupCollider.enabled = false;

        // Monster stays fast for 5 seconds
        yield return new WaitForSeconds(duration);

        // Return monster to normal speed
        if (monster != null)
        {
            monster.patrolSpeed =
                originalPatrolSpeed;

            monster.chaseSpeed =
                originalChaseSpeed;
        }

        Debug.Log("Monster Energy ended.");

        Destroy(gameObject);
    }
}
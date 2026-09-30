using UnityEngine;

public class ArenaProjectileDamage : MonoBehaviour
{
    public int damage = 1;

    private MonsterHealth currentMonster;
    private int monsterColliderCount = 0;
    private bool damagedThisPass = false;

    void OnTriggerEnter(Collider other)
    {
        MonsterChase monster =
            other.GetComponentInParent<MonsterChase>();

        MonsterHealth health =
            other.GetComponentInParent<MonsterHealth>();

        if (monster == null || health == null)
            return;

        // We entered another collider belonging to the same monster
        if (currentMonster == health)
        {
            monsterColliderCount++;
            return;
        }

        // New monster encounter
        currentMonster = health;
        monsterColliderCount = 1;
        damagedThisPass = false;

        if (!monster.IsStunned())
        {
            Debug.Log(
                "Projectile crossed monster, but monster was not stunned."
            );

            return;
        }

        ApplyDamage(health);
    }

    void OnTriggerStay(Collider other)
    {
        MonsterChase monster =
            other.GetComponentInParent<MonsterChase>();

        MonsterHealth health =
            other.GetComponentInParent<MonsterHealth>();

        if (monster == null || health == null)
            return;

        if (currentMonster != health)
            return;

        // Allows player to stun AFTER projectile has already
        // started overlapping the monster.
        if (!damagedThisPass && monster.IsStunned())
        {
            ApplyDamage(health);
        }
    }

    void OnTriggerExit(Collider other)
    {
        MonsterHealth health =
            other.GetComponentInParent<MonsterHealth>();

        if (health == null)
            return;

        if (health != currentMonster)
            return;

        monsterColliderCount--;

        // Projectile has completely left every collider
        // belonging to this monster.
        if (monsterColliderCount <= 0)
        {
            currentMonster = null;
            monsterColliderCount = 0;
            damagedThisPass = false;
        }
    }

    void ApplyDamage(MonsterHealth health)
    {
        if (damagedThisPass)
            return;

        damagedThisPass = true;

        health.TakeDamage(damage);

        Debug.Log("PROJECTILE PASS: EXACTLY 1 DAMAGE");
    }
}
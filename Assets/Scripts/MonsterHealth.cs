using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    private Renderer monsterRenderer;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;

        monsterRenderer = GetComponent<Renderer>();

        if (monsterRenderer != null)
        {
            originalColor = monsterRenderer.material.color;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("Monster HP: " + currentHealth);

        if (monsterRenderer != null)
        {
            StartCoroutine(HitFlash());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator HitFlash()
    {
        monsterRenderer.material.color = Color.white;

        yield return new WaitForSeconds(0.1f);

        monsterRenderer.material.color = originalColor;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
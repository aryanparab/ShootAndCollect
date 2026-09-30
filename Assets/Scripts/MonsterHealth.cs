using UnityEngine;
using TMPro;

public class MonsterHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;

    private int currentHealth;

    [Header("UI")]
    public TMP_Text healthText;

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

        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log(
            "Monster HP: " +
            currentHealth +
            "/" +
            maxHealth
        );

        UpdateHealthUI();

        if (monsterRenderer != null)
        {
            StartCoroutine(HitFlash());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text =
                "MONSTER HP: " +
                currentHealth +
                " / " +
                maxHealth;
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
        Debug.Log("MONSTER DEFEATED!");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerWins();
        }
    }
}
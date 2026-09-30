using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        PlayerStealth stealth =
            collision.gameObject.GetComponent<PlayerStealth>();

        if (stealth == null)
            return;

        // Hidden players are safe from monster contact
        if (stealth.IsHidden)
        {
            Debug.Log("Monster touched hidden player - no death");
            return;
        }

        GameManager.Instance.GameOver();
    }
}
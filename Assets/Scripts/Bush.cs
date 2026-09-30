using UnityEngine;

public class Bush : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerStealth stealth =
            other.GetComponentInParent<PlayerStealth>();

        if (stealth != null)
        {
            stealth.EnterBush();
        }
    }

    void OnTriggerExit(Collider other)
    {
        PlayerStealth stealth =
            other.GetComponentInParent<PlayerStealth>();

        if (stealth != null)
        {
            stealth.ExitBush();
        }
    }
}
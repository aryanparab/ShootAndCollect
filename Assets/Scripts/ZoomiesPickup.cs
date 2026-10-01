using UnityEngine;
using System.Collections;

public class ZoomiesPickup : MonoBehaviour
{
    public float multiplier = 2f;
    public float duration = 5f;

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ZOOMIES TRIGGER TOUCHED BY: " + other.name);

        PlayerMovement player =
            other.GetComponentInParent<PlayerMovement>();

        if (player == null || collected)
            return;

        collected = true;

        Debug.Log("ZOOMIES COLLECTED!");

        StartCoroutine(ActivateZoomies(player));
    }

    private IEnumerator ActivateZoomies(PlayerMovement player)
    {
        float originalMoveSpeed = player.moveSpeed;
        float originalBackwardSpeed = player.backwardSpeed;

        player.moveSpeed *= multiplier;
        player.backwardSpeed *= multiplier;

        Debug.Log("ZOOMIES ACTIVE! Speed = " + player.moveSpeed);

        // Hide the pickup
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
            rend.enabled = false;

        // Stop detecting it again
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        // Stay fast for 5 seconds
        yield return new WaitForSeconds(duration);

        // Return to normal
        player.moveSpeed = originalMoveSpeed;
        player.backwardSpeed = originalBackwardSpeed;

        Debug.Log("ZOOMIES ENDED! Speed = " + player.moveSpeed);

        Destroy(gameObject);
    }
}
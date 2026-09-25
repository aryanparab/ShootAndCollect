using UnityEngine;

public class MonsterChase : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float turnSpeed = 5f;
    private bool stunned = false;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Stun(float duration)
        {
            StartCoroutine(StunRoutine(duration));
        }

        System.Collections.IEnumerator StunRoutine(float duration)
        {
            stunned = true;

            yield return new WaitForSeconds(duration);

            stunned = false;
        }
    void FixedUpdate()
    {
        if (stunned)
        return;
        if (player == null)
            return;

        Vector3 direction =
            player.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        direction.Normalize();

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        Quaternion newRotation =
            Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                turnSpeed * Time.fixedDeltaTime
            );

        rb.MoveRotation(newRotation);

        Vector3 movement =
            transform.forward *
            moveSpeed *
            Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);
    }
}
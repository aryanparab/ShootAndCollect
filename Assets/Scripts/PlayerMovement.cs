using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float backwardSpeed = 4f;
    public float turnSpeed = 140f;

    private Rigidbody rb;

    private float moveInput;
    private float turnInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        moveInput = 0f;
        turnInput = 0f;

        if (Keyboard.current.wKey.isPressed)
            moveInput = 1f;

        if (Keyboard.current.sKey.isPressed)
            moveInput = -1f;

        if (Keyboard.current.aKey.isPressed)
            turnInput = -1f;

        if (Keyboard.current.dKey.isPressed)
            turnInput = 1f;
    }

    void FixedUpdate()
    {
        // Prevent collision physics from spinning the player
        rb.angularVelocity = Vector3.zero;

        float currentSpeed =
            moveInput >= 0f
            ? moveSpeed
            : backwardSpeed;

        Vector3 movement =
            transform.forward *
            moveInput *
            currentSpeed *
            Time.fixedDeltaTime;

        rb.MovePosition(
            rb.position + movement
        );

        float rotationAmount =
            turnInput *
            turnSpeed *
            Time.fixedDeltaTime;

        Quaternion rotation =
            Quaternion.Euler(
                0f,
                rotationAmount,
                0f
            );

        rb.MoveRotation(
            rb.rotation * rotation
        );
    }
}
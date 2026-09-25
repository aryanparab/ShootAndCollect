using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float backwardSpeed = 4f;
    public float turnSpeed = 140f;

    void Update()
    {
        if (Keyboard.current == null)
            return;

        float move = 0f;
        float turn = 0f;

        if (Keyboard.current.wKey.isPressed)
            move = 1f;

        if (Keyboard.current.sKey.isPressed)
            move = -1f;

        if (Keyboard.current.aKey.isPressed)
            turn = -1f;

        if (Keyboard.current.dKey.isPressed)
            turn = 1f;

        float currentSpeed =
            move >= 0 ? moveSpeed : backwardSpeed;

        transform.Translate(
            Vector3.forward *
            move *
            currentSpeed *
            Time.deltaTime
        );

        transform.Rotate(
            Vector3.up,
            turn *
            turnSpeed *
            Time.deltaTime
        );
    }
}
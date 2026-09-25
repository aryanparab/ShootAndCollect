using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float backwardSpeed = 4f;
    public float turnSpeed = 140f;

    void Update()
    {
        float move = 0f;
        float turn = 0f;

        if (Input.GetKey(KeyCode.W))
            move = 1f;

        if (Input.GetKey(KeyCode.S))
            move = -1f;

        if (Input.GetKey(KeyCode.A))
            turn = -1f;

        if (Input.GetKey(KeyCode.D))
            turn = 1f;

        float speed = move >= 0 ? moveSpeed : backwardSpeed;

        transform.Translate(
            Vector3.forward * move * speed * Time.deltaTime
        );

        transform.Rotate(
            Vector3.up,
            turn * turnSpeed * Time.deltaTime
        );
    }
}
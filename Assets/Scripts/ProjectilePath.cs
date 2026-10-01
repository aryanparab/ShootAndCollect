using UnityEngine;

public class ProjectilePingPong : MonoBehaviour
{
    public enum MoveDirection
    {
        Right,
        Left,
        Up,
        Down,
        Custom
    }

    [Header("Movement")]
    public MoveDirection direction = MoveDirection.Right;
    public float distance = 10f;
    public float speed = 8f;

    [Header("Custom Direction")]
    public Vector3 customDirection = Vector3.right;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private bool movingToEnd = true;

    private float lastDistance;
    private MoveDirection lastDirection;
    private Vector3 lastCustomDirection;

    void Start()
    {
        startPosition = transform.position;

        RecalculateEndPosition();

        lastDistance = distance;
        lastDirection = direction;
        lastCustomDirection = customDirection;
    }

    void Update()
    {
        if (distance != lastDistance ||
            direction != lastDirection ||
            customDirection != lastCustomDirection)
        {
            RecalculateEndPosition();

            lastDistance = distance;
            lastDirection = direction;
            lastCustomDirection = customDirection;
        }

        Vector3 target =
            movingToEnd
            ? endPosition
            : startPosition;

        transform.position =
            Vector3.MoveTowards(
                transform.position,
                target,
                speed * Time.deltaTime
            );

        if (Vector3.Distance(
            transform.position,
            target
        ) <= 0.05f)
        {
            movingToEnd = !movingToEnd;
        }
    }

    void RecalculateEndPosition()
    {
        Vector3 moveDirection =
            GetDirection().normalized;

        endPosition =
            startPosition +
            moveDirection * distance;
    }

    Vector3 GetDirection()
    {
        switch (direction)
        {
            case MoveDirection.Right:
                return Vector3.right;

            case MoveDirection.Left:
                return Vector3.left;

            case MoveDirection.Up:
                return Vector3.forward;

            case MoveDirection.Down:
                return Vector3.back;

            case MoveDirection.Custom:
                return customDirection;

            default:
                return Vector3.right;
        }
    }

    // NEW: reverse when projectile touches an inner wall
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BounceWall"))
        {
            movingToEnd = !movingToEnd;

            Debug.Log("PROJECTILE HIT INNER WALL - REVERSING");
        }
    }
}
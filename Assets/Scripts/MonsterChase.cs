using UnityEngine;
using UnityEngine.AI;

public class MonsterChase : MonoBehaviour
{
    public Transform player;

    private PlayerStealth playerStealth;

    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    [Header("Vision")]
    public float detectionDistance = 18f;
    public float loseDistance = 28f;

    [Header("Patrol")]
    public float patrolRadius = 8f;
    public float patrolWaitTime = 1f;

    private NavMeshAgent agent;

    private bool canSeePlayer = false;
    private bool stunned = false;

    private Vector3 lastKnownPosition;
    private bool hasLastKnownPosition = false;

    private float patrolWaitTimer = 0f;

    private Coroutine stunCoroutine;

    private Renderer monsterRenderer;
    private Color originalColor;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        monsterRenderer = GetComponent<Renderer>();

        if (monsterRenderer != null)
        {
            originalColor = monsterRenderer.material.color;
        }
    }

    void Start()
    {
        if (player != null)
        {
            playerStealth =
                player.GetComponent<PlayerStealth>();
        }

        ChooseNewPatrolPoint();
    }

    void Update()
    {
        if (player == null ||
            agent == null ||
            !agent.isOnNavMesh)
        {
            return;
        }

        // -------------------------
        // STUNNED
        // -------------------------

        if (stunned)
        {
            agent.isStopped = true;
            return;
        }

        agent.isStopped = false;

        // -------------------------
        // PLAYER HIDING
        // -------------------------

        if (playerStealth != null &&
            playerStealth.IsHidden)
        {
            canSeePlayer = false;

            // Forget active chase
            hasLastKnownPosition = false;

            Patrol();

            return;
        }

        // -------------------------
        // NORMAL AI
        // -------------------------

        CheckLineOfSight();

        if (canSeePlayer)
        {
            ChasePlayer();
        }
        else if (hasLastKnownPosition)
        {
            InvestigateLastKnownPosition();
        }
        else
        {
            Patrol();
        }
    }

    // =========================
    // CHASING
    // =========================

    void ChasePlayer()
    {
        agent.speed = chaseSpeed;

        lastKnownPosition =
            player.position;

        hasLastKnownPosition = true;

        agent.SetDestination(
            player.position
        );
    }

    // =========================
    // LOST PLAYER
    // =========================

    void InvestigateLastKnownPosition()
    {
        agent.speed = patrolSpeed;

        agent.SetDestination(
            lastKnownPosition
        );

        if (!agent.pathPending &&
            agent.remainingDistance <=
            agent.stoppingDistance + 0.2f)
        {
            hasLastKnownPosition = false;

            ChooseNewPatrolPoint();
        }
    }

    // =========================
    // PATROL
    // =========================

    void Patrol()
    {
        agent.speed = patrolSpeed;

        if (agent.pathPending)
            return;

        if (agent.remainingDistance <=
            agent.stoppingDistance + 0.2f)
        {
            patrolWaitTimer +=
                Time.deltaTime;

            if (patrolWaitTimer >=
                patrolWaitTime)
            {
                ChooseNewPatrolPoint();

                patrolWaitTimer = 0f;
            }
        }
    }

    void ChooseNewPatrolPoint()
    {
        Vector3 randomDirection =
            Random.insideUnitSphere *
            patrolRadius;

        randomDirection +=
            transform.position;

        randomDirection.y =
            transform.position.y;

        if (NavMesh.SamplePosition(
            randomDirection,
            out NavMeshHit hit,
            patrolRadius,
            NavMesh.AllAreas))
        {
            agent.SetDestination(
                hit.position
            );
        }
    }

    // =========================
    // LINE OF SIGHT
    // =========================

    void CheckLineOfSight()
    {
        if (playerStealth != null &&
            playerStealth.IsHidden)
        {
            canSeePlayer = false;
            return;
        }

        Vector3 origin =
            transform.position +
            Vector3.up * 0.75f;

        Vector3 target =
            player.position +
            Vector3.up * 0.75f;

        Vector3 direction =
            target - origin;

        float distance =
            direction.magnitude;

        // If monster already sees player,
        // allow a larger range before losing them
        float currentSightDistance =
            canSeePlayer
            ? loseDistance
            : detectionDistance;

        if (distance > currentSightDistance)
        {
            canSeePlayer = false;
            return;
        }

        direction.Normalize();

        if (Physics.Raycast(
            origin,
            direction,
            out RaycastHit hit,
            currentSightDistance))
        {
            canSeePlayer =
                hit.collider.CompareTag("Player");
        }
        else
        {
            canSeePlayer = false;
        }
    }

    // =========================
    // STUN
    // =========================

    public void Stun(float duration)
    {
        if (stunCoroutine != null)
        {
            StopCoroutine(
                stunCoroutine
            );
        }

        stunCoroutine =
            StartCoroutine(
                StunRoutine(duration)
            );
    }

    System.Collections.IEnumerator
        StunRoutine(float duration)
    {
        stunned = true;

        Debug.Log("MONSTER IS STUNNED");

        if (monsterRenderer != null)
        {
            monsterRenderer.material.color =
                Color.cyan;
        }

        if (agent != null &&
            agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        yield return new WaitForSeconds(
            duration
        );

        stunned = false;

        if (monsterRenderer != null)
        {
            monsterRenderer.material.color =
                originalColor;
        }

        if (agent != null &&
            agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }

        Debug.Log("MONSTER STUN ENDED");

        stunCoroutine = null;
    }

    public bool IsStunned()
    {
        return stunned;
    }
}
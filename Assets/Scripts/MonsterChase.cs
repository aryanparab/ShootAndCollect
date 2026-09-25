using UnityEngine;
using UnityEngine.AI;

public class MonsterChase : MonoBehaviour
{
    public Transform player;

    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    [Header("Vision")]
    public float sightDistance = 20f;

    [Header("Patrol")]
    public float patrolRadius = 8f;
    public float patrolWaitTime = 1f;

    private NavMeshAgent agent;

    private bool canSeePlayer = false;
    private bool stunned = false;

    private Vector3 lastKnownPosition;
    private bool hasLastKnownPosition = false;

    private float patrolWaitTimer = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        ChooseNewPatrolPoint();
    }

    void Update()
    {
        if (player == null || agent == null || !agent.isOnNavMesh)
            return;

        if (stunned)
        {
            agent.isStopped = true;
            return;
        }

        agent.isStopped = false;

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

    void ChasePlayer()
    {
        agent.speed = chaseSpeed;

        lastKnownPosition = player.position;
        hasLastKnownPosition = true;

        agent.SetDestination(player.position);
    }

    void InvestigateLastKnownPosition()
    {
        agent.speed = patrolSpeed;

        agent.SetDestination(lastKnownPosition);

        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance + 0.2f)
        {
            hasLastKnownPosition = false;

            ChooseNewPatrolPoint();
        }
    }

    void Patrol()
    {
        agent.speed = patrolSpeed;

        if (agent.pathPending)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance + 0.2f)
        {
            patrolWaitTimer += Time.deltaTime;

            if (patrolWaitTimer >= patrolWaitTime)
            {
                ChooseNewPatrolPoint();
                patrolWaitTimer = 0f;
            }
        }
    }

    void ChooseNewPatrolPoint()
    {
        Vector3 randomDirection =
            Random.insideUnitSphere * patrolRadius;

        randomDirection += transform.position;
        randomDirection.y = transform.position.y;

        if (NavMesh.SamplePosition(
            randomDirection,
            out NavMeshHit hit,
            patrolRadius,
            NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void CheckLineOfSight()
    {
        Vector3 origin =
            transform.position + Vector3.up * 0.75f;

        Vector3 target =
            player.position + Vector3.up * 0.75f;

        Vector3 direction =
            target - origin;

        float distance = direction.magnitude;

        if (distance > sightDistance)
        {
            canSeePlayer = false;
            return;
        }

        direction.Normalize();

        if (Physics.Raycast(
            origin,
            direction,
            out RaycastHit hit,
            sightDistance))
        {
            canSeePlayer =
                hit.collider.CompareTag("Player");
        }
        else
        {
            canSeePlayer = false;
        }
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunRoutine(duration));
    }

    System.Collections.IEnumerator StunRoutine(float duration)
    {
        stunned = true;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }

        yield return new WaitForSeconds(duration);

        stunned = false;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }
    }
}
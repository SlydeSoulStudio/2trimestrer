using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow2 : MonoBehaviour
{
    public Transform player;

    public float chaseRange = 5f;
    public float attackRange = 1.2f;

    NavMeshAgent agent;
    State currentState;

    public Transform[] patrolPoints;
    int currentPoint = 0;

    public float waitTime = 4f;
    float waitCounter;

    Animator animator;

    [Header("Speeds")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;

    [Header("Chase Memory")]
    public float loseSightTime = 5f;
    float loseSightCounter = 0f;

    [Header("Vision Settings")]
    public float viewRange = 10f;
    public float viewAngle = 90f;
    public LayerMask obstacleMask;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip walkClip;
    public AudioClip runClip;
    public AudioClip alertClip;
    public AudioClip attackClip;

    public float walkStepInterval = 0.6f;
    public float runStepInterval = 0.35f;

    float stepTimer = 0f;
    bool alertPlayed = false;
    bool attackSoundPlayed = false;

    enum State
    {
        Idle,
        Chase,
        Attack
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Idle;

        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPoint].position);
        }

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        bool canSee = CanSeePlayer() || currentState == State.Chase;

        if (canSee)
        {
            loseSightCounter = loseSightTime;

            if (distance <= attackRange)
                currentState = State.Attack;
            else if (distance <= chaseRange)
                currentState = State.Chase;
        }
        else
        {
            if (currentState == State.Chase || currentState == State.Attack)
            {
                loseSightCounter -= Time.deltaTime;

                if (loseSightCounter <= 0f || distance > chaseRange * 1.5f)
                {
                    currentState = State.Idle;
                }
                else
                {
                    currentState = State.Chase;
                }
            }
            else
            {
                currentState = State.Idle;
            }
        }

        HandleState();
    }

    void HandleState()
    {
        switch (currentState)
        {
            case State.Idle:
                agent.isStopped = false;
                agent.speed = patrolSpeed;
                Patrol();
                HandleFootsteps(walkStepInterval);
                alertPlayed = false;
                attackSoundPlayed = false;
                break;

            case State.Chase:
                agent.isStopped = false;
                agent.speed = chaseSpeed;
                agent.SetDestination(player.position);

                Vector3 chaseDir = player.position - transform.position;
                chaseDir.y = 0;
                if (chaseDir != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(chaseDir);

                PlayAlert();
                HandleFootsteps(runStepInterval);
                attackSoundPlayed = false;
                break;

            case State.Attack:
                agent.isStopped = true;
                agent.speed = 0f;

                Vector3 attackDir = player.position - transform.position;
                attackDir.y = 0;
                if (attackDir != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(attackDir);

                animator.SetTrigger("Attack");

                if (!attackSoundPlayed)
                {
                    audioSource.PlayOneShot(attackClip);
                    attackSoundPlayed = true;
                }

                GetComponent<EnemyCombat>().TryAttack();
                break;
        }

        animator.SetBool("patroling", currentState == State.Chase);
        animator.SetFloat("speed", agent.velocity.magnitude);

        if (agent.isOnOffMeshLink)
            animator.SetBool("jumping", true);
        else
            animator.SetBool("jumping", false);
    }

    void HandleFootsteps(float interval)
    {
        if (agent.velocity.magnitude > 0.1f && !agent.isStopped)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                audioSource.PlayOneShot(interval == walkStepInterval ? walkClip : runClip);
                stepTimer = interval;
            }
        }
    }

    void PlayAlert()
    {
        if (!alertPlayed)
        {
            audioSource.PlayOneShot(alertClip);
            alertPlayed = true;
        }
    }

    void Patrol()
    {
        if (agent.remainingDistance < 0.5f)
        {
            animator.SetBool("patroling", false);

            waitCounter += Time.deltaTime;
            if (waitCounter >= waitTime)
            {
                currentPoint = (currentPoint + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[currentPoint].position);
                waitCounter = 0f;
                animator.SetBool("patroling", true);
            }
        }
    }

    bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 direction = (player.position - origin).normalized;
        float distance = Vector3.Distance(origin, player.position);

        if (distance <= attackRange)
            return true;

        if (distance > viewRange)
            return false;

        float angle = Vector3.Angle(transform.forward, direction);
        if (angle > viewAngle / 2f)
            return false;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, viewRange))
        {
            if (hit.transform.CompareTag("Player"))
                return true;
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, viewRange);

        Gizmos.color = Color.blue;
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;

        Gizmos.DrawRay(origin, leftBoundary * viewRange);
        Gizmos.DrawRay(origin, rightBoundary * viewRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}



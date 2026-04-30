using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR.Haptics;

public class NewMonoBehaviourScript : MonoBehaviour
{

    public Transform player;

    public float chaseRange = 5f;
    public float attackRange = 2f;


    enum State
    {
        Idle,
        Chase,
        Attack
    }
    NavMeshAgent agent;
    public Transform[] patrolPoints;
    int currentPoint = 0;

    float waitTime = 2f;
    float waitCounter;

    Animator animator;

    State currentState;

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Idle;

        //start patrolling
        if (patrolPoints.Length > 0 ) //check the table if its not empty
        {
          agent.SetDestination(patrolPoints[currentPoint].position);
            animator.SetBool("patrolling", true);
        }
        

        animator.SetBool("patrolling", currentState == State.Chase || currentState==State.Idle);
        animator.SetBool("attacking", currentState == State.Attack);


        /*float verticalVelocity = agent.velocity.y;
        if ( verticalVelocity > 1f )
        {
            animator.SetBool("jumping", true);
        }
        else if ( verticalVelocity < -1f )
        {
            animator.SetBool("falling", currentState == State.Attack);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > chaseRange)
        {
            currentState = State.Idle;
        }
        else if (distance > attackRange)
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Attack;
        }

        HandleState();

    }
    void HandleState()
    {
        switch(currentState)
        {
            case State.Idle:
                //agent.SetDestination(transform.position);
                Patrol();
                break;

            case State.Chase:
                agent.SetDestination(player.position);
                break;

            case State.Attack:
                Debug.Log("Enemy is attacking");
                break;
        }



    }
    void Patrol()
    {
        if (agent.remainingDistance < 0.5f) //check if we reached the patrolling point
        {

            animator.SetBool("patrolling", false);
            waitCounter += Time.deltaTime;
            if(waitCounter>=waitTime)
            {
                currentPoint = (currentPoint + 1) % patrolPoints.Length;
                

                agent.SetDestination(patrolPoints[currentPoint].position);
                waitCounter = 0f;
                animator.SetBool("patrolling", true);
            }
        }
    }
}

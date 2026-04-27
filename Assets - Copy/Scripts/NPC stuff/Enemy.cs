using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private float chaseDistance = 15f;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float giveUpDistance = 20f;
    [SerializeField] private float chaseCheckAngle = 60f;
    [SerializeField] private float attackCooldown = 1.5f;

    private EnemyState _currentState;
    private bool _isWaiting = false;
    private bool _canAttack = true;

    private void Start()
    {
        _currentState = EnemyState.IDLE;
        agent.updateRotation = true;
    }

    private void FixedUpdate()
    {
        switch (_currentState)
        {
            case EnemyState.IDLE:
                HandleIdleState();
                break;

            case EnemyState.PATROL:
                HandlePatrolState();
                break;

            case EnemyState.CHASE:
                HandleChaseState();
                break;

            case EnemyState.ATTACK:
                HandleAttackState();
                break;
        }
    }

    private void HandleIdleState()
    {
        enemyAnim.SetBool("idle", true);
        agent.isStopped = true;

        if (!_isWaiting)
            StartCoroutine(WaitAndChooseARandomPointAndMove(5));

        CheckForPlayer();
    }

    private void HandlePatrolState()
    {
        enemyAnim.SetBool("walk", true);
        agent.isStopped = false;

        if (agent.remainingDistance <= 0.2f)
        {
            _currentState = EnemyState.IDLE;
            enemyAnim.SetBool("walk", false);
        }

        CheckForPlayer();
    }

    private void HandleChaseState()
    {
        enemyAnim.SetBool("chase", true);
        agent.isStopped = false;
        agent.SetDestination(playerTransform.position);

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= attackDistance)
        {
            _currentState = EnemyState.ATTACK;
            enemyAnim.SetBool("chase", false);
        }
        else if (distance >= giveUpDistance)
        {
            _currentState = EnemyState.IDLE;
            enemyAnim.SetBool("chase", false);
        }
    }

    private void HandleAttackState()
    {
        agent.isStopped = true;
        transform.LookAt(playerTransform);

        if (_canAttack)
        {
            StartCoroutine(AttackRoutine());
        }

        // If player runs away
        if (Vector3.Distance(transform.position, playerTransform.position) > attackDistance)
        {
            _currentState = EnemyState.CHASE;
        }
    }

    private IEnumerator AttackRoutine()
    {
        _canAttack = false;
    
        enemyAnim.SetTrigger("attack");
    
        if (playerTransform != null)
        {
            PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(25);
            }
        }
    
        yield return new WaitForSeconds(attackCooldown);
        _canAttack = true;
    }

    private void CheckForPlayer()
    {
        if (IsPlayerInRange() && IsInFOV())
        {
            _currentState = EnemyState.CHASE;
            enemyAnim.SetBool("idle", false);
            enemyAnim.SetBool("walk", false);
        }
    }

    // ───── Existing helper functions (Cleaned up) ─────
    private IEnumerator WaitAndChooseARandomPointAndMove(float timeToWait)
    {
        _isWaiting = true;
        yield return new WaitForSeconds(timeToWait);
        _currentState = EnemyState.PATROL;
        ChooseARandomPointAndMove();
        _isWaiting = false;
    }

    private void ChooseARandomPointAndMove()
    {
        if (patrolPoints.Length == 0) return;

        int index = Random.Range(0, patrolPoints.Length);
        agent.SetDestination(patrolPoints[index].position);
    }

    private bool IsPlayerInRange() => Vector3.Distance(transform.position, playerTransform.position) <= chaseDistance;

    private bool IsInFOV()
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        return Vector3.Angle(transform.forward, directionToPlayer) <= chaseCheckAngle;
    }
}
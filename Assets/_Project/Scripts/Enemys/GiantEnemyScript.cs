using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class GiantEnemyScript : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent navAgent;
    public Animator animator;
    public ParticleSystem hitEffect;
    public Transform player;

    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    [Header("Stats")]
    public float health = 100f;
    public int damage = 10;

    [Header("AI Settings")]
    public float walkPointRange = 10f;
    public float sightRange = 12f;
    public float attackRange = 2f;
    public float timeBetweenAttacks = 1.5f;

    private Vector3 walkPoint;
    private bool walkPointSet;
    private bool alreadyAttacked;
    private bool isDead;
    private bool takeDamage;

    [Header("Ses")]
    public AudioSource AlanGirmeSes;
    private bool isChasingSoundPlaying = false;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }
    private void PlayGunSound()
    {
        if (AlanGirmeSes != null && !AlanGirmeSes.isPlaying)
        {
            AlanGirmeSes.Play();
        }
    }
    private void Update()
    {
        if (isDead || player == null) return;

        bool playerInSightRange =
            Physics.CheckSphere(transform.position, sightRange, playerLayer);
        bool playerInAttackRange =
            Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            PlayGunSound();
            isChasingSoundPlaying = true;
        }
        else if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }
        else if (!playerInSightRange && takeDamage)
        {
            ChasePlayer();
        }

        animator.SetFloat("Velocity", navAgent.velocity.magnitude);
    }

    // -------------------- PATROL --------------------

    private void Patroling()
    {
        navAgent.isStopped = false;

        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
            navAgent.SetDestination(walkPoint);

        if (Vector3.Distance(transform.position, walkPoint) < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        Vector3 potentialPoint = new Vector3(
            transform.position.x + randomX,
            transform.position.y,
            transform.position.z + randomZ
        );

        if (Physics.Raycast(potentialPoint, Vector3.down, 2f, groundLayer))
        {
            walkPoint = potentialPoint;
            walkPointSet = true;
        }
    }

    // -------------------- CHASE --------------------

    private void ChasePlayer()
    {
        if (player == null) return;

        navAgent.isStopped = false;
        navAgent.SetDestination(player.position);
        transform.LookAt(new Vector3(
            player.position.x,
            transform.position.y,
            player.position.z));
    }

    // -------------------- ATTACK --------------------

    private void AttackPlayer()
    {
        navAgent.isStopped = true;
        transform.LookAt(new Vector3(
            player.position.x,
            transform.position.y,
            player.position.z));

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            animator.SetBool("Attack", true);

            // DAMAGE LOGIC (Raycast / Trigger eklenebilir)

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        if (isDead) return;

        alreadyAttacked = false;
        animator.SetBool("Attack", false);
    }

    // -------------------- DAMAGE --------------------

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        health -= damageAmount;

        if (hitEffect != null)
            hitEffect.Play();

        StartCoroutine(TakeDamageCoroutine());

        if (health <= 0f)
            StartCoroutine(Die());
    }

    private IEnumerator TakeDamageCoroutine()
    {
        takeDamage = true;
        yield return new WaitForSeconds(2f);
        takeDamage = false;
    }

    // -------------------- DEATH --------------------

    private IEnumerator Die()
    {
        isDead = true;
        navAgent.isStopped = true;
        animator.SetBool("Dead", true);

        yield return new WaitForSeconds(1.8f);
        Destroy(gameObject);
    }

    // -------------------- GIZMOS --------------------

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

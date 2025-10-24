//using Unity.VisualScripting;
//using UnityEngine;

using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public Transform[] patrolPoints; // Patrol noktalarý
    private int currentPoint = 0;

    [Header("Detection Settings")]
    public Transform player;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;

    private Animator animator;
    private Rigidbody rb;

    private enum EnemyState { Idle, Walk, Run, Attack, Hit, Cutscene }
    private EnemyState currentState = EnemyState.Idle;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        GoToIdle();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Idle:
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsRunning", false);

                if (distanceToPlayer <= attackRange)
                {
                    TriggerAttack();
                }
                else if (distanceToPlayer <= detectionRange)
                {
                    GoToRun();
                }
                else
                {
                    Patrol();
                }
                break;

            case EnemyState.Walk:
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsRunning", false);
                Patrol();
                break;

            case EnemyState.Run:
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsRunning", true);
                ChasePlayer();
                break;

            case EnemyState.Attack:
                // Animasyon üzerinden geri dönüþ Idle olacak
                break;

            case EnemyState.Hit:
                // Hit animasyonu oynar, otomatik Idle’a geçer
                break;

            case EnemyState.Cutscene:
                // Cutscene kontrolü
                break;
        }
    }

    #region State Methods

    void GoToIdle()
    {
        currentState = EnemyState.Idle;
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
    }

    void GoToWalk()
    {
        currentState = EnemyState.Walk;
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsRunning", false);
    }

    void GoToRun()
    {
        currentState = EnemyState.Run;
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", true);
    }

    void TriggerAttack()
    {
        currentState = EnemyState.Attack;
        animator.SetTrigger("AttackTrigger");
        StartCoroutine(WaitForAnimation(animator, "Attack", GoToIdle));
    }

    public void TriggerHit()
    {
        currentState = EnemyState.Hit;
        animator.SetTrigger("HitTrigger");
        StartCoroutine(WaitForAnimation(animator, "Hit1", GoToIdle)); // Hit1 yerine random yapabilirsin
    }

    public void TriggerCutscene()
    {
        currentState = EnemyState.Cutscene;
        animator.SetTrigger("CutsceneTrigger");
        // Cutscene bitince Idle’a dön
        StartCoroutine(WaitForAnimation(animator, "Cutscene", GoToIdle));
    }

    #endregion

    #region Movement Methods

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPoint];
        Vector3 direction = (targetPoint.position - transform.position).normalized;

        rb.MovePosition(transform.position + direction * walkSpeed * Time.deltaTime);
        transform.LookAt(new Vector3(targetPoint.position.x, transform.position.y, targetPoint.position.z));

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * runSpeed * Time.deltaTime);
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    #endregion

    #region Helper Coroutine

    IEnumerator WaitForAnimation(Animator anim, string animationName, System.Action callback)
    {
        // Animasyonun uzunluðunu al
        float clipLength = 0f;
        foreach (var clip in anim.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
            {
                clipLength = clip.length;
                break;
            }
        }

        yield return new WaitForSeconds(clipLength);
        callback?.Invoke();
    }

    #endregion
}

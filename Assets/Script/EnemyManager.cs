using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private float health = 100f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float runningDistance = 10f;
    private bool isDead = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Damage();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            agent.speed = 0;
            animator.SetBool("isAttacking", true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            agent.speed = 0.8f;
            animator.SetBool("isAttacking", false);
        }
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Damage()
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        animator.SetBool("isDead", true);
        agent.enabled = false; // Disable the NavMeshAgent to stop the enemy from moving.
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationDuration);
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (!isDead)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= runningDistance)
            {
                agent.speed = 5f;
                animator.SetBool("isRunning", true);
            }
            else
            {
                agent.speed = 0.8f;
                animator.SetBool("isRunning", false);
            }

            agent.destination = player.transform.position;

            if (agent.velocity.magnitude > 0 && !animator.GetBool("isRunning"))
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
    }
}

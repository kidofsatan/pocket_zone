using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject playerObject;
    private Transform playerTransform;
    [SerializeField] private float damage;
    public float detectionRange = 5f;
    public float moveSpeed = 2f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    private float lastAttackTime;
    private Rigidbody2D rb;
    [SerializeField]private Animator animator;
    public int attackSFXIndex;

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerObject.transform;
        rb = GetComponent<Rigidbody2D>();
        animator= GetComponent<Animator>();
    }

    void Update()
    {
        // Проверка расстояния между врагом и игроком
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Игрок находится в радиусе обнаружения
            Debug.Log("Игрок близко!");
            MoveTowardsPlayer(distanceToPlayer);
        }
    }

    private void MoveTowardsPlayer(float distanceToPlayer)
    {
        // движение к игроку
        if (distanceToPlayer > attackRange)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
            animator.SetBool("isMoving", true);
            FlipTowardsPlayer(direction);
        }
        // атака
        else
        {
            animator.SetBool("isMoving", false);
            rb.velocity = Vector2.zero;
            if (Time.time > lastAttackTime + attackCooldown)
            {
                // Атака игрока
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
    }

    // разворот к игроку
    private void FlipTowardsPlayer(Vector2 direction)
    {
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // право
        }
        else if (direction.x < 0)
        {
            // Смотрит влево
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // лево
        }
    }

    private void AttackPlayer()
    {
        animator.SetTrigger("attack");
        AudioManager.Instance.PlaySFX(attackSFXIndex);
        playerObject.GetComponent<PlayerStats>().TakeDamage(damage);
        Debug.Log("Атака игрока!");
        StartCoroutine(ReturnToIdle());
    }

    IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("idle");
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("Соприкосновение с игроком!");
    //    }
    //}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movmentSpeed = 5f;
    public Animator animator;
    [SerializeField] Joystick myJoystick;

    private Rigidbody2D rb;
    private Vector3 moveDir;
    private Vector3 lastMoveDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float moveX = 0f;
        float moveY = 0f;
        moveX = myJoystick.Horizontal;
        moveY = myJoystick.Vertical;

        if (moveX < 0f)
        {
            LookLeft();
        }
        if (moveX > 0f)
        {
            LookRight();
        }

        if (moveX != 0 || moveY != 0)
        {
            animator.SetBool("isMoving", true);
            AudioManager.Instance.PlaySteps();
            lastMoveDir = moveDir;
        }
        else
        {
            animator.SetBool("isMoving", false);
            AudioManager.Instance.StopSteps();
        }

        moveDir = new Vector3(moveX, moveY).normalized;
    }

    public void FaceEnemy(Vector3 enemyPosition)
    {
        Vector3 directionToEnemy = enemyPosition - transform.position;

        if (directionToEnemy.x > 0)
        {
            LookRight();
        }
        else
        {
            LookLeft();
        }
    }

    public void LookRight()
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    public void LookLeft()
    {
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDir * movmentSpeed;
    }
}
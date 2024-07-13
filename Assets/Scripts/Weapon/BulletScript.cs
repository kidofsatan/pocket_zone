using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public float damage = 10;
    private GameObject target;
    private Vector3 direction;

    void Start()
    {
        // ���������� ���� ����� lifeTime ������
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (target != null)
        {
            // ������� ���� � ����
            direction = (target.transform.position - transform.position).normalized;
        }
        // ������� ���� � �������� �����������
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("����� � ����������");
        if (other.gameObject == target)
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
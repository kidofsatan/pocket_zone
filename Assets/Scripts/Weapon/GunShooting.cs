using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunShooting : MonoBehaviour
{
    public Inventory inventory;
    public int ammoPerShot = 1;
    public GameObject bulletPrefab; // ������ ����
    public float bulletSpeed = 10f;
    public Button shotButton;
    private GameObject closestEnemy;
    private Animator animator;
    private PlayerController playerController;
    private PlayerEnemyFinder enemyFinder;

    private void Start()
    {
        animator = inventory.gameObject.GetComponent<Animator>();
        playerController = inventory.gameObject.GetComponent<PlayerController>();
        enemyFinder = GetComponentInParent<PlayerEnemyFinder>();
    }

    private void Update()
    {
        closestEnemy = enemyFinder.FindClosestEnemy(); // ��������� ���������� closestEnemy

        if (inventory.ammoCount == 0 || closestEnemy == null)
        {
            shotButton.interactable = false;
        }
        else if (inventory.ammoCount > 0 && closestEnemy != null)
        {
            shotButton.interactable = true;
        }
    }

    public void Shoot()
    {
        // ���������, ���� �� ������� � ���� ����������
        if (inventory.ammoCount >= ammoPerShot)
        {
            Debug.Log("��������� ���� -" + closestEnemy);

            if (closestEnemy != null)
            {
                playerController.FaceEnemy(closestEnemy.transform.position);
            }
            // ������� � ��������� ����
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            AudioManager.Instance.PlaySFX(0);
            animator.SetTrigger("shoot");
            BulletScript bulletScript = bullet.GetComponent<BulletScript>();
            bulletScript.speed = bulletSpeed;

            // ���� ���������� ����� � ������������� ��� ��� ���� ��� ����
            if (closestEnemy != null)
            {
                bulletScript.SetTarget(closestEnemy);
            }
            else
            {
                // ���� ��� �����, ���� ����� � �����������, ���� ������� ��������
                Vector3 direction = transform.right * (transform.localScale.x > 0 ? 1 : -1);
                bulletScript.SetDirection(direction);
            }
            // ��������� ���������� ��������
            inventory.UseAmmo(ammoPerShot);
            StartCoroutine(ReturnToIdle());
        }
        else
        {
            Debug.Log("��� ��������!");
        }
    }

    IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("idle");
    }
}




//    void Fire()
//    {

//        if (Time.time > nextFire && closestEnemyPostion != null && bulletNumber > 0)
//        {
//            for (int i = 0; i < bulletNumber; i++)
//            {
//                StartCoroutine(ShootBullet(i * .1f));
//            }

//            nextFire = Time.time + fireRate;

//        }

//    }





//    void FindClosestEnemy()
//    {
//        float distanceToClosestEnemy = Mathf.Infinity;
//        Enemy_Health closestEnemy = null;
//        Enemy_Health[] allEnemies = GameObject.FindObjectsOfType<Enemy_Health>();

//        foreach (Enemy_Health currentEnemy in allEnemies)
//        {
//            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
//            if (distanceToEnemy < distanceToClosestEnemy && distanceToEnemy < Distance * 10)
//            {
//                distanceToClosestEnemy = distanceToEnemy;
//                closestEnemy = currentEnemy;

//                closestEnemyPostion = closestEnemy.transform;

//            }
//        }

//        if (closestEnemy != null)
//        {
//            Debug.DrawLine(this.transform.position, closestEnemy.transform.position);


//        }
//    }


//    IEnumerator ShootBullet(float delay)
//    {
//        yield return new WaitForSeconds(delay);

//        //AudioManager.instance.PlaySound("Spell");
//        //  GameObject Bullett = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

//       // GameObject Bullett = ObjectPoolingManager.instance.spawnGameObject(bulletPrefab, transform.position, Quaternion.identity);
//        Bullett.GetComponent<TurretBullet>().EnemyPosition = closestEnemyPostion;
//    }
//}


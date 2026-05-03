using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public float speed = 8f;
    public float rotateSpeed = 200f;
    public float lifeTime = 5f;
    public int damage = 1;

    private Transform target;

    void Start()
    {
        FindTarget();
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        
        if (target == null)
        {
            FindTarget();

            
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            return;
        }

        Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        transform.Rotate(0, 0, -rotateAmount * rotateSpeed * Time.deltaTime);

        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float closestDist = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);

            if (dist < closestDist)
            {
                closestDist = dist;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy.transform;
        }
    }

    // ?? รองรับทั้ง Trigger และ Collision
    void OnTriggerEnter2D(Collider2D col)
    {
        HitEnemy(col.gameObject);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        HitEnemy(col.gameObject);
    }

    void HitEnemy(GameObject obj)
    {
        if (obj.CompareTag("Enemy"))
        {
            Enemy enemy = obj.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
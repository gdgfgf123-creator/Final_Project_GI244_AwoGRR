using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;

    public int maxHP = 3;
    public int hp;

    public int maxShield = 2;
    public int shield;

    public GameObject shieldVisual;

    public WaveSpawner spawner; // ?? เพิ่มตรงนี้

    private Transform player;

    void Start()
    {
        hp = maxHP;
        shield = maxShield;

        UpdateShieldVisual();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }

    public void TakeDamage(int dmg)
    {
        if (shield > 0)
        {
            shield -= dmg;

            if (shield < 0)
            {
                hp += shield;
                shield = 0;
            }
        }
        else
        {
            hp -= dmg;
        }

        UpdateShieldVisual();

        Debug.Log("Enemy HP: " + hp + " | Shield: " + shield);

        if (hp <= 0)
        {
            if (spawner != null)
            {
                spawner.EnemyDied(); // ?? สำคัญมาก
            }

            Destroy(gameObject);
        }
    }

    void UpdateShieldVisual()
    {
        if (shieldVisual != null)
        {
            shieldVisual.SetActive(shield > 0);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = col.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1, transform.position);
            }
        }
    }
}
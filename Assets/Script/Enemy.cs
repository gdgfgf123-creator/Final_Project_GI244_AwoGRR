using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;

    public int hp = 3;
    public int shield = 2;

    private Transform player;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if (p != null)
        {
            player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // ?? ไล่ Player
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    // ?? รับดาเมจจากกระสุน
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

        Debug.Log("Enemy HP: " + hp + " | Shield: " + shield);

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    // ?? ชน Player แล้วทำดาเมจ
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
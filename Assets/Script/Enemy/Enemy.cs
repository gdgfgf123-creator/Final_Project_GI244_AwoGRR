using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;

    public int maxHP = 3;
    public int hp;

    public int maxShield = 2;
    public int shield;
    public Slider hpSlider;
    public Slider shielSlider;
    public GameObject shieldVisual;

    public WaveSpawner spawner;

    private Transform player;
    private bool isDead = false; 

    void Start()
    {
        hp = maxHP;
        shield = maxShield;
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = hp;
        }

        if (shielSlider != null)
        {
            shielSlider.maxValue = maxShield;
            shielSlider.value = shield;
        }
        UpdateShieldVisual();
        UpdateHPUI();
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }
    }

    void Update()
    {
        if (player == null || isDead) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return; 

        if (shield > 0)
        {
            int damageToShield = Mathf.Min(shield, dmg);
            shield -= damageToShield;

            int leftoverDamage = dmg - damageToShield;
            hp -= leftoverDamage;
        }
        else
        {
            hp -= dmg;
        }
        UpdateHPUI();
        UpdateShieldVisual();

        Debug.Log("Enemy HP: " + hp + " | Shield: " + shield);

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        if (spawner != null)
        {
            spawner.EnemyDied();
        }

        Destroy(gameObject);
    }

    void UpdateShieldVisual()
    {
        if (shieldVisual != null)
        {
            shieldVisual.SetActive(shield > 0);
        }
    }
    void UpdateHPUI()
    {
        if (hpSlider != null)
            hpSlider.value = hp;

        if (shielSlider != null)
            shielSlider.value = shield;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (isDead) return;

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
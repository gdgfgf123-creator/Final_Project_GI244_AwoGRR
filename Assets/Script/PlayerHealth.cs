using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 10;
    public int hp;

    public int maxShield = 5;
    public int shield;
    public Slider hpSlider;

    public float knockbackForce = 5f;

    public GameObject shieldVisual; 

    private Rigidbody2D rb;
    private bool canTakeDamage = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        hp = maxHP;
        shield = maxShield;

        hpSlider.maxValue = maxHP;   
        hpSlider.value = hp;         

        UpdateShieldVisual();

        InvokeRepeating(nameof(AutoHeal), 5f, 5f);
    }

    void AutoHeal()
    {
        if (hp < maxHP)
        {
            hp += 1;
            UpdateHPUI();
            Debug.Log("Heal +1 | HP: " + hp);
        }
    }

    public void TakeDamage(int dmg, Vector2 enemyPos)
    {
        if (!canTakeDamage) return;

        canTakeDamage = false;

        // ??? Shield ¡èÍ¹
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

        // ?? Knockback
        Vector2 dir = (transform.position - (Vector3)enemyPos).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);

        UpdateShieldVisual(); // ?? ÍÑ»à´µâÅè
        UpdateHPUI();
        Debug.Log("HP: " + hp + " | Shield: " + shield);

        StartCoroutine(DamageCooldown());
        StartCoroutine(KnockbackCooldown());

        if (hp <= 0)
        {
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

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;
    }

    IEnumerator KnockbackCooldown()
    {
        PlayerController controller = GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.isKnockback = true;
        }

        yield return new WaitForSeconds(0.2f);

        if (controller != null)
        {
            controller.isKnockback = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1, col.transform.position);
        }
    }
    void UpdateHPUI()
    {
        if (hpSlider != null)
        {
            hpSlider.value = hp;
        }
    }
}
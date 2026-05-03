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
    public Slider shielSlider;

    public float knockbackForce = 5f;

    public GameObject shieldVisual;

    private Rigidbody2D rb;
    private bool canTakeDamage = true;
    private bool isDead = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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

        InvokeRepeating(nameof(AutoHeal), 5f, 5f);
    }

    void AutoHeal()
    {
        if (hp < maxHP && !isDead)
        {
            hp += 1;
            UpdateHPUI();
        }
    }

    public void AddShield(int amount)
    {
        shield += amount;

        if (shield > maxShield)
            shield = maxShield;

        UpdateShieldVisual();
        UpdateHPUI();
    }

    public void TakeDamage(int dmg, Vector2 enemyPos)
    {
        if (!canTakeDamage || isDead) return;

        canTakeDamage = false;
        if (shield > 0)
        {
            int damageToShield = Mathf.Min(shield, dmg);
            shield -= damageToShield;

            int leftover = dmg - damageToShield;
            hp -= leftover;
        }
        else
        {
            hp -= dmg;
        }

        // Knockback
        Vector2 dir = (transform.position - (Vector3)enemyPos).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);

        UpdateShieldVisual();
        UpdateHPUI();

        StartCoroutine(DamageCooldown());
        StartCoroutine(KnockbackCooldown());

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        GameManager.instance.GameOver();

        Destroy(gameObject);
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
            controller.isKnockback = true;

        yield return new WaitForSeconds(0.2f);

        if (controller != null)
            controller.isKnockback = false;
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
            hpSlider.value = hp;

        if (shielSlider != null)
            shielSlider.value = shield;
    }
}
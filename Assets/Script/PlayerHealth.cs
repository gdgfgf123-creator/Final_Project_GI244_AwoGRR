using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int hp = 10;
    public int shield = 5;

    public float knockbackForce = 5f;

    private Rigidbody2D rb;
    private bool canTakeDamage = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // ?? Auto Heal ทุก 5 วินาที
        InvokeRepeating(nameof(AutoHeal), 5f, 5f);
    }

    void AutoHeal()
    {
        hp += 1;
        Debug.Log("Heal +1 | HP: " + hp);
    }

    public void TakeDamage(int dmg, Vector2 enemyPos)
    {
        if (!canTakeDamage) return;
        StartCoroutine(KnockbackCooldown());
        // ??? Shield ก่อน
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
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);

        Debug.Log("HP: " + hp + " | Shield: " + shield);

        // ?? กันโดนรัว
        StartCoroutine(DamageCooldown());

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
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
}
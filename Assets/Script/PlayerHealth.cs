using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int hp = 10;
    public int shield = 5;

    void Start()
    {
        InvokeRepeating("AutoHeal", 5f, 5f);
    }

    void AutoHeal()
    {
        hp += 1;
    }

    public void TakeDamage(int dmg)
    {
        if (shield > 0)
        {
            shield -= dmg;
        }
        else
        {
            hp -= dmg;
        }

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }
}
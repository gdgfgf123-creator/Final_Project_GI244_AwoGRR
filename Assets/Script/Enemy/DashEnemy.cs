using UnityEngine;

public class DashEnemy : MonoBehaviour
{
    public float dashForce = 10f;
    public float cooldown = 2f;

    private Transform player;
    private Rigidbody2D rb;
    private float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            Dash();
            timer = 0;
        }
    }

    void Dash()
    {
        Vector2 dir = (player.position - transform.position).normalized;

        rb.AddForce(dir * dashForce, ForceMode2D.Impulse);
    }
}
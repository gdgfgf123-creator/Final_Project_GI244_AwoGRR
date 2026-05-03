using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public SpriteRenderer firePointSprite;
    public GameObject normalBullet;
    public GameObject homingBullet;

    private GameObject currentBullet;

    public Transform firePoint;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public bool isKnockback = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (normalBullet == null)
        {
            Debug.LogError("? normalBullet ยังไม่ได้ใส่!");
            return;
        }

        currentBullet = normalBullet;

        Debug.Log("Start Bullet: " + currentBullet.name);
    }

    void Update()
    {
        if (isKnockback) return;

        Move();
        Aim();
        Shoot();
    }

    void Move()
    {
        float x = 0;
        float y = 0;

        if (Keyboard.current.aKey.isPressed) x = -1;
        if (Keyboard.current.dKey.isPressed) x = 1;
        if (Keyboard.current.sKey.isPressed) y = -1;
        if (Keyboard.current.wKey.isPressed) y = 1;

        moveInput = new Vector2(x, y).normalized;
        rb.linearVelocity = moveInput * speed;
    }

    void Aim()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir = mousePos - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    void Shoot()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (currentBullet == null)
            {
                Debug.LogError("? currentBullet เป็น null!");
                return;
            }

            Debug.Log("ยิง: " + currentBullet.name);
            Instantiate(currentBullet, firePoint.position, firePoint.rotation);
        }
    }

    public void SetHomingBullet(float duration)
    {
        if (homingBullet == null)
        {
            Debug.LogError("? homingBullet ยังไม่ได้ใส่!");
            return;
        }

        StopAllCoroutines();

        currentBullet = homingBullet;
        if (firePointSprite != null)
            firePointSprite.color = Color.red;

        Debug.Log("?? เปลี่ยนเป็น Homing");

        StartCoroutine(HomingDuration(duration));
    }

    IEnumerator HomingDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        currentBullet = normalBullet;
        if (firePointSprite != null)
            firePointSprite.color = Color.gray;
        Debug.Log("? กลับเป็นปกติ");
    }
}
using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
    public int shieldAmount = 1; // เพิ่มโล่กี่หน่วย

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerHealth player = col.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.AddShield(shieldAmount);
            }

            Destroy(gameObject); // เก็บแล้วหาย
        }
    }
}
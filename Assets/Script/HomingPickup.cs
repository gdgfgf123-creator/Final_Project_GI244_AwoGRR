using UnityEngine;

public class HomingPickup : MonoBehaviour
{
    public float duration = 5f;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("?? เก็บ Homing");

            PlayerController player = col.GetComponent<PlayerController>();

            if (player != null)
            {
                player.SetHomingBullet(duration);
            }
            else
            {
                Debug.LogError("? หา PlayerController ไม่เจอ");
            }

            Destroy(gameObject);
        }
    }
}
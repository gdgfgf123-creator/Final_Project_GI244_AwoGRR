using UnityEngine;

public class FollowNoRotate : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        if (target == null) return;

        // ?? µ“¡·§Ë X,Y ·µË≈ÁÕ§ Z
        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            -10f   // ?? ≈ÁÕ§ Z
        );

        transform.rotation = Quaternion.identity;
    }
}
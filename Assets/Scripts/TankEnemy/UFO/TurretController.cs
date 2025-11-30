using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform target;          // Player
    public float rotateSpeed = 8f;

    void Update()
    {
        if (target == null) return;

        Vector3 dir = (target.position - transform.position);
        dir.y = 0; // ❗ Giữ turret xoay theo trục Y, không ngẩng lên xuống

        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }
    }
}

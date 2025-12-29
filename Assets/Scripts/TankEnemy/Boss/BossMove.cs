using UnityEngine;

public class BossMove : MonoBehaviour
{
    [Header("Cấu hình Thân xe (Chassis)")]
    public float bodyTurnSpeed = 5f;
    public float moveSpeed = 3f;

    [Header("Cấu hình Tháp pháo (Turret)")]
    public Transform turretTransform; // Kéo Object tháp pháo vào đây
    public float turretTurnSpeed = 10f;
    public Transform player;

    // 1. Xoay THÁP PHÁO về phía Player (Luôn nhắm bắn)
     void Update()
    {
        //TurretLookAt();
    }
    public void TurretLookAt()
    {
        if (player == null || turretTransform == null) return;

        Vector3 direction = player.position - turretTransform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        turretTransform.rotation = Quaternion.RotateTowards(turretTransform.rotation, targetRotation, turretTurnSpeed * Time.deltaTime);
    }


    // 3. Di chuyển tiến lên (Dựa theo hướng của THÂN XE)
    public void MoveForward()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
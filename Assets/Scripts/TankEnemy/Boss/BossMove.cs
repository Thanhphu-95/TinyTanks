using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMove : MonoBehaviour
{
    [Header("--- Di chuyển ---")]
    public List<Transform> movePoints = new List<Transform>();
    public float moveSpeed = 5f;
    public float rotateSpeed = 150f;
    public float angleThreshold = 5f;

    [Header("--- Tháp súng ---")]
    public Transform turretTransform; // Kéo tháp súng vào đây
    public float turretRotateSpeed = 5f; // Tốc độ xoay tháp súng
    public Transform playerTransform;  // Kéo Player vào đây (hoặc tìm bằng code)

    private NavMeshAgent agent;
    private int currentIndex = -1;

    void Start()
    {
        

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // Tự xoay thân xe
        agent.speed = 0;

        // Tự tìm Player nếu chưa kéo vào Inspector
        if (playerTransform == null)
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        StartCoroutine(DetachMovePointsAfterDelay(3f));
        // Kiểm tra NavMesh trước khi bắt đầu
        if (agent.isOnNavMesh)
            PickNextPoint();
        else
            Debug.LogError("Boss chưa nằm trên NavMesh! Hãy kéo nó sát mặt đất.");
    }

    void Update()
    {
        if (!agent.isOnNavMesh || !agent.isActiveAndEnabled) return;

       
        
    }

    // ==========================
    // 🚗 LOGIC DI CHUYỂN THÂN XE
    // ==========================
    public void HandleMovement()
    {
        Vector3 targetDir = agent.steeringTarget - transform.position;
        targetDir.y = 0;

        if (targetDir.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            float angle = Quaternion.Angle(transform.rotation, targetRot);

            // Thân xe xoay hướng về điểm di chuyển tiếp theo
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);

            // Chỉ chạy khi thân xe đã nhìn gần đúng hướng
            agent.speed = (angle < angleThreshold) ? moveSpeed : 0;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            PickNextPoint();
        }
    }

    // ==========================
    // 🔫 LOGIC XOAY THÁP SÚNG
    // ==========================
    public void HandleTurret()
    {
        if (turretTransform == null || playerTransform == null) return;

        // Tính hướng từ tháp súng tới Player
        Vector3 targetDir = playerTransform.position - turretTransform.position;
        targetDir.y = 0; // Giữ tháp súng xoay ngang (không bị chúi lên/xuống)

        if (targetDir.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(targetDir);

            // Xoay tháp súng mượt mà về phía Player
            turretTransform.rotation = Quaternion.Slerp(
                turretTransform.rotation,
                targetRot,
                turretRotateSpeed * Time.deltaTime
            );
        }
    }

    void PickNextPoint()
    {
        if (movePoints.Count == 0) return;
        currentIndex = (currentIndex + 1) % movePoints.Count;
        agent.SetDestination(movePoints[currentIndex].position);
    }

    IEnumerator DetachMovePointsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (Transform point in movePoints)
        {
            if (point != null)
            {
                point.SetParent(null); // Đưa về gốc sau 3s
            }
        }
    }

}
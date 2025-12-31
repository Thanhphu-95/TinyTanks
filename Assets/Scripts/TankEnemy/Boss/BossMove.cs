using UnityEngine;
using UnityEngine.AI;

public class BossMove : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Cấu hình Thân xe (Chassis)")]
    public Transform bodyTransform; // Kéo Object Thân xe vào đây
    public float bodyTurnSpeed = 5f;
    public float moveSpeed = 1f;

    [Header("Cấu hình Tháp pháo (Turret)")]
    public Transform turretTransform;
    public float turretTurnSpeed = 10f;
    public Transform player;

    [Header("Khoảng cách & Di chuyển")]
    public float safeDistance = 5f;
    public float distanceTolerance = 2f;
    public float strafeChangeInterval = 2f;

    private int strafeDirection = 1;
    private float strafeTimer = 0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = moveSpeed;
            // Tắt Update Rotation để chúng ta tự điều khiển thân xe xoay mượt hơn
            agent.updateRotation = false;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, NavMesh.AllAreas))
                agent.Warp(hit.position);
        }
    }

    void Update()
    {
        if (player == null || agent == null || !agent.isOnNavMesh) return;

        TurretLookAt();     // Tháp pháo nhắm Player
        HandleTankAI();    // Thân xe di chuyển và xoay
    }

    public void HandleTankAI()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 targetDestination = transform.position;

        // 1. LOGIC TÍNH ĐIỂM ĐẾN (TIẾN - LÙI - NGANG)
        if (distance > safeDistance + distanceTolerance)
        {
            targetDestination = player.position; // Tiến
        }
        else if (distance < safeDistance - distanceTolerance)
        {
            Vector3 dirAway = (transform.position - player.position).normalized;
            targetDestination = transform.position + dirAway * 6f; // Lùi
        }
        else
        {
            strafeTimer += Time.deltaTime;
            if (strafeTimer >= strafeChangeInterval)
            {
                strafeDirection = Random.value > 0.5f ? 1 : -1;
                strafeTimer = 0f;
            }
            // Di chuyển ngang dựa trên hướng vuông góc với Player
            Vector3 sideDir = Vector3.Cross(Vector3.up, (player.position - transform.position).normalized);
            targetDestination = transform.position + (sideDir * strafeDirection * 5f);
        }

        // 2. THỰC THI DI CHUYỂN
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetDestination, out hit, 5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            agent.isStopped = false;
        }

        // 3. XOAY THÂN XE THEO HƯỚNG DI CHUYỂN (Chassis Rotation)
        // Xe tăng sẽ xoay mặt về hướng mà NavMeshAgent đang muốn đi
        if (agent.velocity.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * bodyTurnSpeed);
        }
    }

    public void TurretLookAt()
    {
        if (turretTransform == null) return;
        Vector3 direction = (player.position - turretTransform.position);
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            turretTransform.rotation = Quaternion.RotateTowards(turretTransform.rotation, targetRotation, turretTurnSpeed * Time.deltaTime);
        }
    }
}
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;

    private Transform currentTarget;

    [Header("Rotate Body When Switching Point")]
    public float bodyRotateSpeed = 120f;
    private bool isRotating = false;

    [Header("Turret")]
    public Transform turret;
    public float turretRotateSpeed = 60f;

    private Detector detector;
    private bool playerDetected = false;
    private Transform player;

    [Header("Shoot")]
    private GatlingShooting shooting;

    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        pointA.SetParent(null);
        pointB.SetParent(null);

        navMeshAgent = GetComponent<NavMeshAgent>();
        shooting = GetComponent<GatlingShooting>();
        detector = GetComponent<Detector>();

        currentTarget = pointA;

        if (detector != null)
            detector.turret = turret;

        navMeshAgent.speed = moveSpeed;
        navMeshAgent.SetDestination(currentTarget.position);
    }


    private void Update()
    {
        // PHÁT HIỆN PLAYER
        playerDetected = detector != null && detector.DetectPlayer(out player);

        if (!playerDetected)
        {
            if (!isRotating)
            {
                PatrolMovement();
            }
        }
        else
        {
            shooting.LogicShoot();
        }

        RotateTurret();
    }


    private void PatrolMovement()
    {
        float distance = Vector3.Distance(transform.position, currentTarget.position);

        if (distance < 1f)
        {
            // Chọn target mới
            currentTarget = (currentTarget == pointA) ? pointB : pointA;

            // Ngừng agent để xoay thân
            navMeshAgent.isStopped = true;

            // Xoay thân
            StartCoroutine(RotateBody());
        }
        else
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(currentTarget.position);
        }
    }


    private System.Collections.IEnumerator RotateBody()
    {
        isRotating = true;

        Vector3 dir = (currentTarget.position - transform.position);
        dir.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(dir);

        while (Quaternion.Angle(transform.rotation, targetRot) > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                bodyRotateSpeed * Time.deltaTime
            );

            yield return null;
        }

        // Sau khi xoay xong → cho chạy tiếp
        navMeshAgent.SetDestination(currentTarget.position);
        navMeshAgent.isStopped = false;

        isRotating = false;
    }


    private void RotateTurret()
    {
        if (playerDetected && player != null)
        {
            Vector3 direction = player.position - turret.position;
            direction.y = 0;

            Quaternion targetRot = Quaternion.LookRotation(direction);
            turret.rotation = Quaternion.RotateTowards(
                turret.rotation,
                targetRot,
                turretRotateSpeed * Time.deltaTime
            );
        }
        else
        {
            turret.Rotate(Vector3.up * turretRotateSpeed * Time.deltaTime);
        }
    }

    [Header("Movement")]
    public float moveSpeed = 3f;
}

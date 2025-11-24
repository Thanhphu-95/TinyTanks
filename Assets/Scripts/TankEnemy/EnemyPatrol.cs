using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;

    private Transform currentTarget;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float reachDistance = 1f;

    [Header("Turret")]
    public Transform turret;
    public float turretRotateSpeed = 60f;

    private Detector detector;
    private bool playerDetected = false;
    private Transform player;

    [Header("Shoot")]
    private GatlingShooting shooting;
    private void Start()
    {
        pointA.SetParent(null);
        pointB.SetParent(null);
        currentTarget = pointA;
        detector = GetComponent<Detector>();
        shooting = GetComponent<GatlingShooting>();
        if (detector != null)
        {
            detector.turret = turret;
        }
    }

    private void Update()
    {
        if (!playerDetected)
        {
            PatrolMovement();
        }

        if (detector != null)
        {
            playerDetected = detector.DetectPlayer(out player);
        }
        else
        {
            playerDetected = false;
            player = null;
        }
        if (detector.DetectPlayer(out player))
        {
            shooting.LogicShoot();
        }

        RotateTurret();
    }

    private void PatrolMovement()
    {
        Vector3 dir = (currentTarget.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, currentTarget.position) < reachDistance)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
        }
    }

    private void RotateTurret()
    {
        if (playerDetected && player != null)
        {
            Vector3 direction = player.position - turret.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            turret.rotation = Quaternion.RotateTowards(turret.rotation, targetRotation, turretRotateSpeed * Time.deltaTime);
        }
        else
        {
            turret.Rotate(Vector3.up * turretRotateSpeed * Time.deltaTime);
        }
    }
}

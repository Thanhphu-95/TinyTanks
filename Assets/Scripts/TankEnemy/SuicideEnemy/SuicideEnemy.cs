using UnityEngine;
using UnityEngine.AI;

public class SuicideEnemy : MonoBehaviour
{
    [Header("Target & Movement")]
    public Transform target;           // Player's Transform
    public NavMeshAgent agent;         // NavMeshAgent component
    public float movementSpeed = 5f;
    public float randomWanderDistance = 5f; // Bán kính thêm nhiễu ngẫu nhiên

    [Header("Attack & Explosion")]
    public float explosionRange = 3f;  // Phạm vi phát nổ (khi đến gần Player)
    public float explosionDelay = 0.5f; // Thời gian chờ trước khi phát nổ
    public float explosionForce = 500f; // Lực đẩy
    public int explosionDamage;
    public GameObject explosionEffectPrefab; // Hiệu ứng nổ (Particle System/Prefab)

    private Vector3 wanderTarget;
    private bool isExploding = false;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (target == null)
        {
            Debug.LogError("SuicideEnemy: Target (Player) chưa được gán!");
            return;
        }
        agent.speed = movementSpeed;
        SetNewWanderTarget();
    }
    void Update()
    {
        if (target == null || agent == null || isExploding) return;
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= explosionRange)
        {
            StartExplosionSequence();
            return;
        }
        if (Vector3.Distance(transform.position, wanderTarget) < 1f || !agent.hasPath)
        {
            SetNewWanderTarget();
        }
    }
    void SetNewWanderTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * randomWanderDistance;
        randomDirection += target.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, randomWanderDistance, NavMesh.AllAreas))
        {
            wanderTarget = hit.position;
            agent.SetDestination(wanderTarget);
        }
        else
        {
            agent.SetDestination(target.position);
        }
    }
    void StartExplosionSequence()
    {
        isExploding = true;
        agent.isStopped = true; 
        Invoke("Explode", explosionDelay);
    }
    void Explode()
    {
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange);
        foreach (Collider hit in colliders)
        {
            PlayerHealth playerHP = GetComponent<PlayerHealth>();
            if (hit.CompareTag("Player"))
            {
                playerHP.TakeDamage(explosionDamage);
            }

            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRange, 1.0f, ForceMode.Impulse);
            }
        }
        Destroy(gameObject);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
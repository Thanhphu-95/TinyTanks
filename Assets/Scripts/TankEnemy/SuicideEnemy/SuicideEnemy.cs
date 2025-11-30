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
    public float explosionDamage = 50f;
    public GameObject explosionEffectPrefab; // Hiệu ứng nổ (Particle System/Prefab)

    private Vector3 wanderTarget;
    private bool isExploding = false;

    void Start()
    {
        // 1. Kiểm tra các tham chiếu
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (target == null)
        {
            Debug.LogError("SuicideEnemy: Target (Player) chưa được gán!");
            // Tìm Player theo tag nếu cần, nhưng nên gán thủ công
            return;
        }

        agent.speed = movementSpeed;

        // Bắt đầu tìm kiếm mục tiêu đi lang thang đầu tiên
        SetNewWanderTarget();
    }

    void Update()
    {
        if (target == null || agent == null || isExploding) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // 1. KIỂM TRA PHẠM VI NỔ
        if (distanceToTarget <= explosionRange)
        {
            // Đã vào phạm vi nổ, ngừng di chuyển và bắt đầu đếm ngược
            StartExplosionSequence();
            return;
        }

        // 2. DI CHUYỂN BẰNG CÁCH DÙNG ĐIỂM NGẪU NHIÊN TRUNG GIAN
        // Nếu kẻ thù đã đến gần điểm lang thang (hoặc chưa có điểm)
        if (Vector3.Distance(transform.position, wanderTarget) < 1f || !agent.hasPath)
        {
            SetNewWanderTarget();
        }
    }

    // Thiết lập điểm đến ngẫu nhiên quanh Player
    void SetNewWanderTarget()
    {
        // Tính một điểm ngẫu nhiên xung quanh Player
        Vector3 randomDirection = Random.insideUnitSphere * randomWanderDistance;
        randomDirection += target.position;
        NavMeshHit hit;

        // Tìm điểm NavMesh gần nhất với điểm ngẫu nhiên đó
        if (NavMesh.SamplePosition(randomDirection, out hit, randomWanderDistance, NavMesh.AllAreas))
        {
            wanderTarget = hit.position;
            agent.SetDestination(wanderTarget);
        }
        else
        {
            // Nếu không tìm thấy điểm hợp lệ, thử lại lần sau
            // Hoặc đơn giản là đặt mục tiêu là Player
            agent.SetDestination(target.position);
        }
    }

    // Bắt đầu chuỗi phát nổ
    void StartExplosionSequence()
    {
        isExploding = true;
        agent.isStopped = true; // Ngừng di chuyển ngay lập tức

        // 🔥 Kích hoạt Animation cảnh báo (Nếu có)
        // GetComponent<Animator>()?.SetTrigger("Alert"); 

        // Gọi hàm Phát nổ sau một khoảng thời gian (explosionDelay)
        Invoke("Explode", explosionDelay);
    }

    // Thực hiện vụ nổ
    void Explode()
    {
        // 1. HIỆU ỨNG VÀ ÂM THANH
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
        // GetComponent<AudioSource>()?.Play(); 

        // 2. TẠO SÁT THƯƠNG VÀ LỰC ĐẨY
        // Tìm tất cả các Colliders trong phạm vi nổ
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange);
        foreach (Collider hit in colliders)
        {
            // Kiểm tra Player và gây sát thương
            if (hit.CompareTag("Player"))
            {
                // Thực hiện logic nhận sát thương của Player
                // hit.GetComponent<PlayerHealth>()?.TakeDamage(explosionDamage);
            }

            // Áp dụng lực đẩy vật lý
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRange, 1.0f, ForceMode.Impulse);
            }
        }

        // 3. HỦY ĐỐI TƯỢNG KẺ THÙ
        Destroy(gameObject);
    }

    // 🔥 Dùng để hiển thị phạm vi nổ trong Scene
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
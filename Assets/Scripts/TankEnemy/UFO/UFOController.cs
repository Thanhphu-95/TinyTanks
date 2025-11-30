using UnityEngine;
using UnityEngine.AI;

public class UFOController : MonoBehaviour
{
    public Transform target;
    public float hoverHeight = 4f;

    public float chaseDistance = 12f;     // phạm vi truy đuổi
    public float orbitDistance = 8f;      // tấn công và bay vòng
    public float orbitSpeed = 0.5f;         // tốc độ quay vòng

    public GatlingShooting shooting;      // script bắn đạn

    private NavMeshAgent agent;
    private float angle = 0f;

    [Header("Shoot")]
    public float timeCooldown = 5f;
    public float timeAttack = 2f;
    private float cooldown;
    private float attack;
    private bool isShooting = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.baseOffset = hoverHeight;
        agent.updateRotation = true;
        cooldown = timeCooldown;    
        attack = timeAttack;
    }

    void Update()
    {
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.position);

        // Nếu còn xa thì bay lại gần
        if (dist > chaseDistance)
        {
            ChasePlayer();
        }
        else
        {
            // Khi đã vào gần → bay vòng tròn + bắn
            if (isShooting == true)
            {
                    
                    shooting.LogicShoot();
                attack -= Time.deltaTime;
                if (attack<=0)
                {
                    isShooting = false;
                    attack = timeAttack;
                }
                Debug.Log("time tấn công");
            }
            else
            {
                cooldown -= Time.deltaTime;
                if (cooldown <= 0)
                {
                    isShooting = true;
                    cooldown = timeCooldown;
                }
                Debug.Log("hồi thời gian");

            }
        }
        
        OrbitAroundPlayer();
    }

    void ChasePlayer()
    {
        agent.SetDestination(target.position);
    }

    void OrbitAroundPlayer()
    {
        // Tăng góc quay theo thời gian
        angle += orbitSpeed * Time.deltaTime;

        // Tâm quỹ đạo chính là vị trí player
        Vector3 center = target.position;

        // Tính điểm cần đến trên quỹ đạo tròn
        Vector3 orbitOffset = new Vector3(
            Mathf.Cos(angle) * orbitDistance,
            0,
            Mathf.Sin(angle) * orbitDistance
        );

        Vector3 orbitPoint = center + orbitOffset;

        // Đặt hướng bay tới điểm đó
        agent.SetDestination(orbitPoint);

        // Xoay thân UFO hướng về player (nếu muốn)
        Vector3 lookDir = (center - transform.position).normalized;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(lookDir),
                Time.deltaTime * 5f
            );
    }
}

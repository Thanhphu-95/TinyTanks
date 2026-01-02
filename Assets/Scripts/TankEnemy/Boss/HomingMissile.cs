using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private Transform target;
    private Rigidbody rb;
    private float timer = 0f;
    private Vector3 randomDirection;
    private bool isInitialized = false;
    public GameObject ExplodeVFX;
    public GameObject ExplodeGroundVFX;

    [Header("Cấu hình thời gian")]
    public float duration = 4.0f;
    public float chaosDuration = 1.5f; // Giai đoạn bay loạn
    public float lockLookTime = 0.5f;  // Giai đoạn ngừng khóa

    [Header("Cấu hình chuyển động")]
    public float speed = 18f;
    public float rotationSpeedChaos = 5f; // Xoay chậm lúc đầu để tạo vòng lượn
    public float rotationSpeedLock = 2f;  // Xoay gắt khi khóa mục tiêu

    public void Initialize(Transform playerTarget)
    {
        target = playerTarget;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        // Tạo một hướng bay ngẫu nhiên hoàn toàn lúc vừa rời nòng
        randomDirection = transform.forward + Random.insideUnitSphere * 2f;
        isInitialized = true;
    }

    void FixedUpdate()
    {
        if (!isInitialized || target == null) return;

        timer += Time.fixedDeltaTime;
        float remainingTime = duration - timer;

        Vector3 desiredDirection;

        // GIAI ĐOẠN 1: BAY LOẠN XẠ (Chaos Phase)
        if (timer < chaosDuration)
        {
            // Tên lửa sẽ bay theo hướng ngẫu nhiên đã chọn + thêm nhiễu loạn theo thời gian
            Vector3 noise = new Vector3(
                Mathf.Sin(Time.time * 10f),
                Mathf.Cos(Time.time * 10f),
                Mathf.Sin(Time.time * 7f)
            ) * 0.5f;

            desiredDirection = (randomDirection + noise).normalized;
            RotateTowards(desiredDirection, rotationSpeedChaos);
        }
        // GIAI ĐOẠN 2: KHÓA MỤC TIÊU (Homing Phase)
        else if (remainingTime > lockLookTime)
        {
            desiredDirection = (target.position - transform.position).normalized;
            RotateTowards(desiredDirection, rotationSpeedLock);
        }
        // GIAI ĐOẠN 3: BAY THẲNG (Final Straight)
        else
        {
            // Giữ nguyên hướng hiện tại
        }

        // Luôn tiến về phía trước
        rb.linearVelocity = transform.forward * speed;
    }

    private void RotateTowards(Vector3 direction, float rotSpeed)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.fixedDeltaTime));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHP = other.gameObject.GetComponent<PlayerHealth>();
        if (other.CompareTag("Player"))
        {
            Instantiate(ExplodeVFX, transform.position, Quaternion.identity);
            playerHP.TakeDamage(20);
            Explode();
        }
        else
        {
            Instantiate(ExplodeGroundVFX, transform.position, Quaternion.identity);
            Explode();
        }
    }

    void Explode() { Destroy(gameObject); }
}
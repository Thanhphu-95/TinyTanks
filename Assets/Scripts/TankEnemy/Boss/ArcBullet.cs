using UnityEngine;

public class ArcBullet : MonoBehaviour
{
    private Vector3 startPoint;
    private Vector3 targetPoint;
    private float timer = 0f;
    private bool hasExploded = false; // Tránh nổ 2 lần

    [Header("Cấu hình quỹ đạo")]
    public float duration = 2.0f;
    public float height = 8.0f;

    [Header("Cấu hình đạn con (Fragments)")]
    public GameObject fragmentPrefab;
    public int fragmentCount = 10;
    public float fragmentSpeed = 12f;

    public void Initialize(Vector3 start, Vector3 target)
    {
        startPoint = start;
        targetPoint = target;
        timer = 0f;
    }

    void Update()
    {
        if (timer < 1.0f)
        {
            Vector3 previousPos = transform.position;
            timer += Time.deltaTime / duration;

            Vector3 currentPos = Vector3.Lerp(startPoint, targetPoint, timer);
            float arc = Mathf.Sin(timer * Mathf.PI) * height;
            currentPos.y += arc;

            transform.position = currentPos;

            Vector3 moveDirection = currentPos - previousPos;
            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }
        else
        {
            // Tự nổ khi hết thời gian bay (đến đích)
            Explode();
        }
    }

    // XÁC NHẬN VA CHẠM BẰNG TRIGGER
    private void OnTriggerEnter(Collider other)
    {
        // Nếu chạm đất (môi trường) hoặc chạm trực tiếp Player
        if (other.CompareTag("Ground"))
        {
            Explode();
        }
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        SpawnFragments();

        // Có thể thêm hiệu ứng nổ VFX tại đây
        // Instantiate(explosionVFX, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    void SpawnFragments()
    {
        if (fragmentPrefab == null) return;

        float angleStep = 360f / fragmentCount;
        float startAngle = Random.Range(0f, 360f);

        // Sử dụng offset cộng thêm vào vị trí hiện tại của đạn mẹ
        float yOffset = 1.0f;
        Vector3 spawnPosition = transform.position + Vector3.up * yOffset;

        for (int i = 0; i < fragmentCount; i++)
        {
            float angle = startAngle + (i * angleStep);
            float dirX = Mathf.Sin(angle * Mathf.Deg2Rad);
            float dirZ = Mathf.Cos(angle * Mathf.Deg2Rad);

            // Hướng bay tỏa ra xung quanh (chỉ trên mặt phẳng ngang)
            Vector3 moveDir = new Vector3(dirX, 0, dirZ).normalized;

            // Sinh đạn con tại vị trí đã cộng thêm 1
            GameObject fragment = Instantiate(fragmentPrefab, spawnPosition, Quaternion.LookRotation(moveDir));

            Rigidbody rb = fragment.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Đảm bảo vận tốc không có thành phần Y để đạn bay ngang song song mặt đất
                rb.linearVelocity = moveDir * fragmentSpeed;
            }
        }
    }
}
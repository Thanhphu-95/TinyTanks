using UnityEngine;

public class ArcBullet : MonoBehaviour
{
    private Vector3 startPoint;   // Vị trí nòng súng lúc bắn
    private Vector3 targetPoint;  // Vị trí Player đứng lúc Boss bắn
    private float timer = 0f;     // Tiến trình bay (từ 0 đến 1)

    [Header("Cấu hình quỹ đạo")]
    public float duration = 2.0f; // Tổng thời gian để đạn bay tới đích
    public float height = 8.0f;   // Độ cao cực đại của vòng cung

    // Hàm này giúp Boss "giao bài tập" cho viên đạn
    public void Initialize(Vector3 start, Vector3 target)
    {
        startPoint = start;
        targetPoint = target;
        timer = 0f;
    }

    void Update()
    {
        // Nếu tiến trình chưa đạt 100% (1.0)
        if (timer < 1.0f)
        {
            // Lưu vị trí của frame trước để tính hướng xoay đầu đạn
            Vector3 previousPos = transform.position;

            // Tăng tiến trình theo thời gian thực
            timer += Time.deltaTime / duration;

            // 1. TỰ ĐIỀU CHỈNH TOẠ ĐỘ PHẲNG (X và Z):
            // Di chuyển từ điểm đầu đến điểm cuối theo đường thẳng trên mặt đất
            Vector3 currentPos = Vector3.Lerp(startPoint, targetPoint, timer);

            // 2. TỰ ĐIỀU CHỈNH ĐỘ CAO (Y):
            // Dùng hàm Sin để tạo hình cầu vồng. 
            // Khi timer = 0.5 (giữa đường), Sin = 1 -> đạn cao nhất.
            float arc = Mathf.Sin(timer * Mathf.PI) * height;
            currentPos.y += arc;

            // Cập nhật vị trí mới cho viên đạn
            transform.position = currentPos;

            // 3. TỰ XOAY ĐẦU THEO HƯỚNG BAY:
            Vector3 moveDirection = currentPos - previousPos;
            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }
        else
        {
            // Khi timer >= 1.0, đạn đã chạm đúng vị trí Player đứng ban đầu
            OnReachTarget();
        }
    }

    void OnReachTarget()
    {
        // Bạn có thể sinh ra hiệu ứng nổ (Explosion Effect) tại đây
        Debug.Log("Đạn vòng cung đã trúng đích!");
        Destroy(gameObject);
    }
}
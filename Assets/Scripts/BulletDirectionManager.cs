using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletDirectionManager : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Dùng FixedUpdate vì Rigidbody tính toán vật lý tại đây
    void FixedUpdate()
    {
        // Kiểm tra nếu đạn đang di chuyển
        if (rb.linearVelocity.sqrMagnitude > 0.1f)
        {
            // Tạo góc xoay dựa trên hướng của vận tốc
            Quaternion targetRotation = Quaternion.LookRotation(rb.linearVelocity);

            // Cập nhật hướng cho viên đạn
            transform.rotation = targetRotation;
        }
    }
}
using UnityEngine;

public class FireArea : MonoBehaviour
{
    [Header("Cấu hình sát thương")]
    public int damagePerSecond = 10; // Lượng máu mất mỗi giây
    public float damageInterval = 0.5f; // Khoảng thời gian giữa mỗi lần trừ máu

    private float nextDamageTime; // Bộ đếm thời gian

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))// 1. Kiểm tra nếu đối tượng chạm vào là Enemy
        {
            // 2. Kiểm tra xem đã đến lúc gây sát thương chưa (tránh trừ máu mỗi frame quá nhanh)
            if (Time.time >= nextDamageTime)
            {
                EnemyHealth enemy = other.GetComponent<EnemyHealth>();// 3. Tìm script quản lý máu của Enemy (giả sử tên là EnemyHealth)
                if (enemy != null)
                {
                    enemy.TakeDamage(damagePerSecond);
                    nextDamageTime = Time.time + damageInterval;// Cập nhật thời điểm gây sát thương tiếp theo
                    Debug.Log("Enemy đang cháy! Mất " + damagePerSecond + " máu.");
                }
            }
        }
    }
}

using UnityEngine;

public class AcidArea : MonoBehaviour
{
    public int damagePerTick = 2;       // Sát thương mỗi lần nhảy số
    public float tickInterval = 0.5f;   // Tốc độ gây sát thương (0.5 giây/lần)
    public float lingeringDuration = 3f; // Thời gian vẫn mất máu sau khi thoát ra

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Tìm hoặc thêm script AcidDebuff vào Enemy
            AcidDebuff debuff = other.GetComponent<AcidDebuff>();
            if (debuff == null)
            {
                debuff = other.gameObject.AddComponent<AcidDebuff>();
            }

            // Cập nhật thông số và làm mới thời gian tác dụng
            debuff.ApplyDebuff(damagePerTick, tickInterval, lingeringDuration);
        }
    }
}
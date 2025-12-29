using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int totalHealAmount = 40;
    public float duration = 1f;

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem đối tượng va chạm có script PlayerHealth không
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null && !playerHealth.isDead)
        {
            // Yêu cầu PlayerHealth tự thực hiện việc hồi máu theo thời gian
            // Điều này đảm bảo khi vật phẩm bị Destroy, quá trình hồi vẫn chạy
            playerHealth.StartCoroutine(HealOverTimeRoutine(playerHealth));

            // Xóa ngay vật phẩm khỏi Scene
            Destroy(gameObject);
        }
    }

    private System.Collections.IEnumerator HealOverTimeRoutine(PlayerHealth playerHealth)
    {
        float healedRemainder = 0f;
        float healRate = (float)totalHealAmount / duration;
        float timer = 0f;

        while (timer < duration)
        {
            // Nếu Player bỗng nhiên chết trong lúc đang hồi, dừng ngay lập tức
            if (playerHealth == null || playerHealth.isDead) yield break;

            float healThisFrame = healRate * Time.deltaTime;
            healedRemainder += healThisFrame;

            int healInt = Mathf.FloorToInt(healedRemainder);
            if (healInt > 0)
            {
                // Gọi hàm Heal đã viết trong PlayerHealth
                playerHealth.Heal(healInt);
                healedRemainder -= healInt;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Hồi nốt phần dư cuối cùng nếu còn sót lại > 0.5 đơn vị máu
        if (playerHealth != null && !playerHealth.isDead && healedRemainder >= 0.5f)
        {
            playerHealth.Heal(1);
        }
    }


}
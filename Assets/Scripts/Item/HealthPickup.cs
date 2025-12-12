using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int totalHealAmount = 40;  // Tổng máu hồi
    public float duration = 1f;        // Thời gian hồi (giây)

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null && !playerHealth.isDead)
        {
            playerHealth.StartCoroutine(HealOverTime(playerHealth));
            Destroy(gameObject);
        }
    }

    private System.Collections.IEnumerator HealOverTime(PlayerHealth playerHealth)
    {
        float healed = 0f;
        float healRate = totalHealAmount / duration;  // máu hồi mỗi giây (float)

        float timer = 0f;

        while (timer < duration)
        {
            if (playerHealth.isDead) yield break;

            float healThisFrame = healRate * Time.deltaTime;
            healed += healThisFrame;

            // Heal phải là số nguyên (int), nên mỗi frame lấy phần nguyên để hồi
            int healInt = Mathf.FloorToInt(healed);
            if (healInt > 0)
            {
                playerHealth.Heal(healInt);
                healed -= healInt; // trừ phần đã hồi
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }
}

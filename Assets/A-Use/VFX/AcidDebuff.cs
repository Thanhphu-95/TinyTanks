using UnityEngine;

public class AcidDebuff : MonoBehaviour
{
    private int damage;
    private float interval;
    private float timer;
    private float nextTickTime;
    private EnemyHealth health;

    public void ApplyDebuff(int dmg, float inter, float duration)
    {
        damage = dmg;
        interval = inter;
        timer = duration; // Reset lại thời gian đếm ngược về mức tối đa
        health = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (health == null) return;

        // Đếm ngược thời gian hiệu ứng
        timer -= Time.deltaTime;

        // Gây sát thương theo chu kỳ
        if (Time.time >= nextTickTime)
        {
            health.TakeDamage(damage);
            nextTickTime = Time.time + interval;
            Debug.Log(gameObject.name + " đang bị Acid ăn mòn!");
        }

        // Nếu hết thời gian thì xóa script này đi
        if (timer <= 0)
        {
            Destroy(this);
        }
    }
}
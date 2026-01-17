using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 5f; // Tốc độ chậm (chỉnh thấp xuống để lơ lửng)
    public float lifeTime = 5f; // Tự hủy để tránh nặng máy
    public GameObject ExplosioVFX;
    

    void Start()
    {
        // Tự hủy sau một khoảng thời gian
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Di chuyển về phía trước theo hướng nòng súng lúc sinh ra
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // Xử lý va chạm
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHP = other.gameObject.GetComponent<PlayerHealth>();
        if (other.CompareTag("Player"))
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.library.electronic);
            }
            playerHP.TakeDamage(20);
            Instantiate(ExplosioVFX, transform.position, Quaternion.identity);
            // Gây sát thương cho Player ở đây
            Destroy(gameObject);
        }
    }
}
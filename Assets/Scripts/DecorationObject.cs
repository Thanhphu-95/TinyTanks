using UnityEngine;
using System.Collections;

public class DecorationObject : MonoBehaviour
{
    [Header("Cấu hình văng")]
    public float jumpHeight = 2.0f;    // Độ cao văng
    public float scatterRange = 1.5f;  // Độ văng xa (ngẫu nhiên sang ngang)
    public float duration = 0.6f;
    public GameObject destroyEffect;
    public LayerMask impactLayer;

    private bool isHit = false;

    public void OnHit()
    {
        if (isHit) return;
        isHit = true;

        // Tính toán vị trí rơi xuống ngẫu nhiên xung quanh vị trí cũ
        Vector3 randomDirection = new Vector3(
            Random.Range(-scatterRange, scatterRange),
            0,
            Random.Range(-scatterRange, scatterRange)
        );
        Vector3 targetLandPos = transform.position + randomDirection;

        StartCoroutine(JumpScatterRoutine(targetLandPos));
    }

    private IEnumerator JumpScatterRoutine(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0;

        // Tạo góc xoay ngẫu nhiên để vật thể văng trông tự nhiên hơn
        Vector3 randomRotation = new Vector3(Random.Range(5, 15), Random.Range(5, 15), Random.Range(5, 15));

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;

            // 1. Di chuyển ngang (văng ra xa)
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, percent);

            // 2. Di chuyển dọc (nhảy lên theo hình Parabol)
            float height = Mathf.Sin(percent * Mathf.PI) * jumpHeight;
            currentPos.y += height;

            transform.position = currentPos;

            // 3. Xoay và thu nhỏ
            transform.Rotate(randomRotation);
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, percent);

            yield return null;
        }

        if (destroyEffect != null)
            Instantiate(destroyEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & impactLayer) != 0) OnHit();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & impactLayer) != 0) OnHit();
    }
}
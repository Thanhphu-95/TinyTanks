using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        // Khi Play → tắt enemy
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;
            enemy.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            // Nếu enemy đã được trigger khác bật rồi
            if (enemy.activeSelf) continue;

            enemy.SetActive(true);
        }

        Destroy(gameObject);
    }
}

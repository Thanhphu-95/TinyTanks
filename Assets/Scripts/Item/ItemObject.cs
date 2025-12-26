using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData data; // Kéo file ScriptableObject đã tạo ở Bước 2 vào đây

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu xe (Player) chạm vào
        if (other.CompareTag("Player"))
        {
            // Gửi dữ liệu vào túi đồ (Chúng ta sẽ viết InventoryManager ở bước sau)
            // InventoryManager.Instance.AddItem(data);
            ItemInventoryManager.Instance.AddItem(data);
            Debug.Log("Đã nhặt: " + data.itemName);

            // Biến mất khỏi Map
            Destroy(gameObject);
        }
    }
}
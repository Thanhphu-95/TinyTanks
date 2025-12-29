using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType { UmbrellaShield, Health, AxitBullet, FireBullet } // Danh sách các loại vật phẩm
    public ItemType itemType; // Chọn loại vật phẩm trong Inspector

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ko tim thay");
        // 1. Kiểm tra xem đối tượng chạm vào có phải Player không
        if (other.CompareTag("Player"))
        {
            Debug.Log("Phatss hiện player");
            // 2. Tìm đến Class ItemManager nằm trên Player
            ItemManager manager = other.GetComponent<ItemManager>();
            
            if (manager != null)
            {
                //Tùy vào loại vật phẩm mà gọi hàm tương ứng trong ItemManager
                switch (itemType)
                {
                    case ItemType.UmbrellaShield: manager.CollectShield() ;break;
                    case ItemType.Health: manager.CollectHealth(); break;
                    case ItemType.AxitBullet: manager.CollectAxitBullet(); break;
                    case ItemType.FireBullet: manager.CollectFireBullet(); break;
                }
                Destroy(gameObject);
            }
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

public class ItemInventoryManager : MonoBehaviour
{
    public static ItemInventoryManager Instance { get; private set; }

    // Mảng cố định 5 ô để chứa dữ liệu vật phẩm
    public ItemData[] slots = new ItemData[5];

    private void Awake() { Instance = this; }

    // Hàm này gọi khi xe chạm vào vật phẩm trên Map
    public void AddItem(ItemData newItem)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) // Tìm ô trống đầu tiên
            {
                slots[i] = newItem;
                UpdateUI(); // Cập nhật hình ảnh lên UI
                return;
            }
        }
        Debug.Log("Túi đồ đã đầy!");
    }

    void Update()
    {
        // Kiểm tra phím bấm từ 1 đến 5
        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                UseItem(i);
            }
        }
    }

    void UseItem(int index)
    {
        if (slots[index] != null)
        {
            Debug.Log("Sử dụng vật phẩm: " + slots[index].itemName);

            // --- GỌI LOGIC SỬ DỤNG Ở ĐÂY ---
            // Ví dụ: ApplyEffect(slots[index]);

            slots[index] = null; // Xóa vật phẩm khỏi ô sau khi dùng
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        // Gửi danh sách mới nhất sang cho Script hiển thị UI
        GameEvents.OnInventoryChanged?.Invoke(new List<ItemData>(slots));
    }
}
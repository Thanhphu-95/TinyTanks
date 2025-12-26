using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryDisplayUI : MonoBehaviour
{
    // Kéo 5 cái Icon (Image) của 5 ô vào mảng này trong Inspector
    public Image[] slotIcons;

    private void OnEnable() { GameEvents.OnInventoryChanged += RedrawUI; }
    private void OnDisable() { GameEvents.OnInventoryChanged -= RedrawUI; }

    void RedrawUI(List<ItemData> items)
    {
        for (int i = 0; i < slotIcons.Length; i++)
        {
            if (i < items.Count && items[i] != null)
            {
                slotIcons[i].sprite = items[i].icon; // Gán hình ảnh từ dữ liệu vào UI
                slotIcons[i].gameObject.SetActive(true); // Hiện Icon lên
            }
            else
            {
                slotIcons[i].gameObject.SetActive(false); // Ẩn Icon nếu ô trống
            }
        }
    }
}
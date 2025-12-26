using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;          // Hình ảnh hiện lên ô vuông UI
    public ItemType type;        // Loại (Hồi máu, Tăng tốc, Khiên...)
    public GameObject effect;    // Prefab hiệu ứng (nếu có)
    public float power = 10f;    // Chỉ số tác động

    public enum ItemType { Heal, SpeedBoost, Shield, Weapon }
}
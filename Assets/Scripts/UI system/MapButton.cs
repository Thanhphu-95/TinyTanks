using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapButton : MonoBehaviour
{
    [SerializeField] private Button myButton;      // Kéo component Button vào đây
    [SerializeField] private string targetScene;    // Gõ tên Scene vào ô này ở Inspector
    [SerializeField] private bool isUnlocked;       // Tích chọn nếu map đã mở
    [SerializeField] private GameObject lockIcon;   // Kéo cái hình ổ khóa vào đây

    private void Start()
    {
        // Kiểm tra trạng thái nút ngay khi bắt đầu
        myButton.interactable = isUnlocked;

        if (lockIcon != null)
            lockIcon.SetActive(!isUnlocked);

        // Đăng ký sự kiện Click: Khi bấm thì báo lên GameEvents
        myButton.onClick.AddListener(() => {
            Debug.Log("Đang gửi yêu cầu vào Map: " + targetScene);
            GameEvents.RaiseMapSelected(targetScene);
        });
    }
}
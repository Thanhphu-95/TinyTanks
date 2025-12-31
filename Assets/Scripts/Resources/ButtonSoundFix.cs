using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSoundFix : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioClip hoverClip; // Kéo file âm thanh vào đây trong Prefab

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Tự động tìm AudioManager đang tồn tại trong bộ nhớ
        if (AudioManager.Instance != null && hoverClip != null)
        {
            AudioManager.Instance.PlaySFX(hoverClip);
        }
    }
}
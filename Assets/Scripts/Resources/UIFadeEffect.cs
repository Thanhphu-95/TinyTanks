using UnityEngine;
using System.Collections;

public class UISequenceFade : MonoBehaviour
{
    [Header("Cấu hình nhóm UI")]
    [SerializeField] private CanvasGroup backgroundGroup;
    [SerializeField] private CanvasGroup buttonsGroup;

    [Header("Thời gian")]
    [SerializeField] private float fadeDuration = 1.0f; // Thời gian hiện của mỗi nhóm
    [SerializeField] private float delayBetween = 0.5f; // Khoảng chờ giữa 2 nhóm

    void Awake()
    {
        // Đặt mọi thứ về trong suốt khi bắt đầu
        if (backgroundGroup) backgroundGroup.alpha = 0;
        if (buttonsGroup) buttonsGroup.alpha = 0;
    }

    void Start()
    {
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        // 1. Hiện hình nền
        if (backgroundGroup)
        {
            yield return StartCoroutine(FadeCanvas(backgroundGroup, 0, 1, fadeDuration));
        }

        // 2. Chờ một chút
        yield return new WaitForSeconds(delayBetween);

        // 3. Hiện các nút bấm
        if (buttonsGroup)
        {
            yield return StartCoroutine(FadeCanvas(buttonsGroup, 0, 1, fadeDuration));
        }
    }

    // Hàm bổ trợ để chạy hiệu ứng mờ dần
    IEnumerator FadeCanvas(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        cg.alpha = end;
    }
}
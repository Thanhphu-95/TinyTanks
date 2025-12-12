using UnityEngine;                                  // Dùng Unity API
using System.Collections.Generic;                   // Dùng List<T>

public class QuestReachPoint : Quest_Base           // Kế thừa lớp Quest_Base
{
    [Header("Quest Settings")]                      // Group trong Inspector
    public List<Transform> players;                 // Danh sách người chơi
    public Transform target;                        // Điểm cần đến
    public float distanceNeeded = 2f;               // Khoảng cách tối đa để tính là đã vào
    public float stayTimeRequired = 5f;             // Thời gian cần đứng trong vùng

    private float currentStayTime = 0f;             // Thời gian người chơi đã đứng trong vùng
    private Transform playerInside = null;          // Player đang đứng trong vùng

    [Header("Effect")]                              // Group effect
    public GameObject effect;                       // Prefab hiệu ứng hoàn thành

    public QuestReachPoint(string name, List<Transform> players, Transform target, GameObject effectPrefab, float stayTime = 5f)
    {                                               // Constructor: khởi tạo quest
        this.questName = name;                      // Gán tên quest
        this.players = players;                     // Gán danh sách player
        this.target = target;                       // Gán điểm đích
        this.stayTimeRequired = stayTime;           // Thời gian đứng
        this.effect = effectPrefab;                 // Gán hiệu ứng hoàn thành
        this.questText = "Hãy đến " + name;
    }

    public override void StartQuest()               // Hàm khi quest bắt đầu
    {
        Debug.Log("Bắt đầu: " + questName);         // Log tên quest
        currentStayTime = 0f;                       // Reset thời gian đứng
        playerInside = null;                        // Reset người trong vùng
    }

    public override void UpdateQuest()              // Gọi mỗi frame khi quest đang chạy
    {
        if (isCompleted) return;                    // Nếu hoàn thành thì không xử lý
        if (target == null || players == null) return;  // Nếu dữ liệu null thì dừng

        bool anyPlayerInside = false;               // Biến kiểm tra có player nào vào vùng chưa
        Transform found = null;                     // Player tìm được

        foreach (var p in players)                  // Lặp qua tất cả player
        {
            if (p == null) continue;                // Nếu player null → bỏ qua

            if (Vector3.Distance(p.position, target.position) <= distanceNeeded)
            {                                       // Nếu player ở gần target
                anyPlayerInside = true;             // Đánh dấu đã có người vào
                found = p;                          // Lưu player đó
                break;                              // Không cần tìm nữa
            }
        }

        if (anyPlayerInside)                        // Nếu có player đứng trong vùng
        {
            if (playerInside != found)              // Nếu player mới vào hoặc đổi player
            {
                playerInside = found;               // Lưu player đang trong vùng
                currentStayTime = 0f;               // Reset thời gian để bắt đầu tính
            }

            currentStayTime += Time.deltaTime;      // Tăng thời gian đứng trong vùng

            if (currentStayTime >= stayTimeRequired) // Nếu đã đứng đủ 5 giây
            {
                CompleteQuest();                    // Quest hoàn thành
            }
        }
        else                                        // Không có ai trong vùng
        {
            playerInside = null;                    // Không lưu player
            currentStayTime = 0f;                   // Reset timer
        }
    }

    public override void CompleteQuest()            // Hàm khi quest hoàn tất
    {
        if (isCompleted) return;                    // Tránh chạy lại nhiều lần

        isCompleted = true;                         // Đánh dấu đã hoàn thành
        Debug.Log("Hoàn thành: " + questName);      // Log quest

        if (effect != null)                         // Nếu có hiệu ứng hoàn thành
        {
            GameObject fx = GameObject.Instantiate(
                effect, target.position, Quaternion.identity);   // Spawn hiệu ứng tại target
            GameObject.Destroy(fx, 60f);             // Tự hủy sau 3 giây
        }
    }
}

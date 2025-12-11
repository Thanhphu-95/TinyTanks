using UnityEngine;                                  // Dùng API Unity

public class QuestKillBoss : Quest_Base             // Quest kiểm tra boss đã chết chưa
{
    private GameObject boss;                        // Tham chiếu đến object boss cần tiêu diệt

    public QuestKillBoss(string name, GameObject bossRef)
    {                                               // Constructor nhận tên và object boss
        this.questName = name;                      // Gán tên quest
        this.boss = bossRef;                        // Lưu object boss
    }

    public override void StartQuest()               // Gọi khi quest bắt đầu
    {
        Debug.Log("Bắt đầu: " + questName);         // Log thông báo quest bắt đầu
    }

    public override void UpdateQuest()              // Chạy mỗi frame
    {
        if (isCompleted) return;                    // Nếu đã hoàn thành → không làm nữa

        if (boss == null)                           // Nếu boss đã bị Destroy() → coi như chết
        {
            CompleteQuest();                        // Hoàn thành quest
        }
    }

    public override void CompleteQuest()            // Khi quest hoàn tất
    {
        isCompleted = true;                         // Đánh dấu quest đã xong
      /*  Debug.Log("Boss đã bị tiêu diệt!");   */      // In ra log xác nhận
    }
}

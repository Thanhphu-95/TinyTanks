public abstract class Quest_Base      // Lớp abstract: chỉ dùng làm cha, không tạo instance trực tiếp
{
    public string questName;          // Tên của quest
    public string questText;

    public bool isCompleted = false;  // Trạng thái hoàn thành của quest (mặc định là false)

    protected MissionUI missionUI;
    protected QuestTextUI questTextUI;
    public void SetUI(MissionUI ui)
    {
        missionUI = ui;
    }

    public void SetQuestTextUI(QuestTextUI ui)
    {
        questTextUI = ui;
    }

    public abstract void StartQuest();   // Hàm bắt đầu quest — class con bắt buộc phải override
    public abstract void UpdateQuest();  // Hàm update mỗi frame — class con bắt buộc phải override
    public abstract void CompleteQuest(); // Hàm khi quest hoàn tất — class con bắt buộc phải override
}

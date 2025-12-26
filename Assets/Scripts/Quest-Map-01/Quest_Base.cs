public abstract class Quest_Base
{
    public string questName;
    public string questText;
    public bool isCompleted = false;

    // THÊM ĐOẠN NÀY: Constructor nhận 1 tham số
    public Quest_Base(string name)
    {
        this.questName = name;
    }

    // Constructor mặc định (để tránh lỗi nếu có class con không truyền tham số)
    public Quest_Base() { }

    public virtual void StartQuest()
    {
        isCompleted = false;
        GameEvents.OnQuestTextChanged?.Invoke(questText);
    }

    public abstract void UpdateQuest();
    public abstract void CompleteQuest();

    public void SetQuestTextUI(QuestTextUI ui) { }
}
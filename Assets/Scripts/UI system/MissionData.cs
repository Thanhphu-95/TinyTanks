using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Game/Mission Data")]
public class MissionData : ScriptableObject
{
    public string sceneName;       // Tên scene (vd: Map01)
    [TextArea] public string missionContent; // Nội dung nhiệm vụ
}
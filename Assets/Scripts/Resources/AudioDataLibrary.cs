using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SceneMusic
{
    public string sceneName; // Tên chính xác của Scene trong Build Settings
    public List<AudioClip> musicClips;
}
[CreateAssetMenu(fileName = "NewAudioLibrary", menuName = "Audio/Library")]
public class AudioDataLibrary : ScriptableObject
{
    [Header("Cấu hình nhạc theo Scene")]
    public List<SceneMusic> sceneMusicList;

    [Header("Hiệu ứng SFX")]
    public AudioClip Pick_Up;
    public AudioClip ShootBullet;
    public AudioClip ShootEnemyBullet;
    public AudioClip Explode;
    public AudioClip EnginePlayer;
    public AudioClip EngineEnemy;
    public AudioClip EngineUfO;
    public AudioClip ClickButton;
    public AudioClip bossSoot;
    public AudioClip fireZone;
}
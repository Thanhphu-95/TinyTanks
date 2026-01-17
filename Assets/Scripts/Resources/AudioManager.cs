using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    // Singleton: Giúp bạn gọi âm thanh từ bất cứ đâu mà không cần kéo thả
    public static AudioManager Instance;
    public AudioDataLibrary library; 

    [Header("Cấu hình Mixer")]
    public AudioMixer mainMixer;

    [Header("Nguồn phát (Audio Sources)")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        // Đảm bảo chỉ có 1 AudioManager duy nhất tồn tại xuyên suốt các Scene
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Không bị xóa khi đổi Scene
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayRandomMusicForCurrentScene();
    }
    void Update()
    {
        // Kiểm tra nếu là nhạc nền và đã hát xong (không Loop thì isPlaying sẽ thành false)
        if (!musicSource.isPlaying && musicSource.clip != null)
        {
            PlayRandomMusicForCurrentScene();
        }
    }

    // Hàm dùng để phát nhạc nền
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    // Hàm dùng để phát tiếng động (va chạm, vỡ, bắn súng...)
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        // 1. Ép Object SFX_Player phải Active
        if (!sfxSource.gameObject.activeSelf)
        {
            sfxSource.gameObject.SetActive(true);
        }

        // 2. Ép Component AudioSource phải Enabled (Quan trọng nhất)
        if (!sfxSource.enabled)
        {
            sfxSource.enabled = true;
        }

        // 3. Bây giờ mới phát âm thanh
        sfxSource.PlayOneShot(clip);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. Tìm cấu hình nhạc cho Scene hiện tại
        SceneMusic data = library.sceneMusicList.Find(s => s.sceneName == scene.name);

        if (data != null && data.musicClips != null && data.musicClips.Count > 0)
        {
            // 2. Chọn ngẫu nhiên một chỉ số (Index) trong danh sách
            int randomIndex = Random.Range(0, data.musicClips.Count);
            AudioClip selectedClip = data.musicClips[randomIndex];

            // 3. Kiểm tra nếu bài ngẫu nhiên trùng với bài đang phát thì có thể chọn lại hoặc cứ để thế
            // Ở đây mình cứ cho phát bài mới nếu khác bài cũ
            if (musicSource.clip == selectedClip && musicSource.isPlaying) return;

            musicSource.clip = selectedClip;
            musicSource.Play();
        }
    }
    public void PlayRandomMusicForCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneMusic data = library.sceneMusicList.Find(s => s.sceneName == currentScene);

        if (data != null && data.musicClips.Count > 1)
        {
            AudioClip nextClip;
            do
            {
                nextClip = data.musicClips[Random.Range(0, data.musicClips.Count)];
            } while (nextClip == musicSource.clip); // Đảm bảo không phát lại đúng bài vừa xong

            musicSource.clip = nextClip;
            musicSource.Play();
        }
    }

}
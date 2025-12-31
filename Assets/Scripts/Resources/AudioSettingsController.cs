using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsController : MonoBehaviour
{
    [Header("Cấu hình Mixer")]
    public AudioMixer mainMixer;

    [Header("Thanh trượt UI")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // Lấy giá trị, nếu chưa có thì mặc định là 0.8f
        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        // Đảm bảo giá trị không được bằng 0 tuyệt đối trước khi tính Log10
        savedMusic = Mathf.Max(savedMusic, 0.0001f);
        savedSFX = Mathf.Max(savedSFX, 0.0001f);
        //if (savedMusic <= 0.001f) savedMusic = 0.75f;
        //if (savedSFX <= 0.001f) savedSFX = 0.75f;
        musicSlider.value = savedMusic;
        sfxSlider.value = savedSFX;

        // Chỉ áp dụng sau khi đã gán giá trị cho Slider
        SetMusicVolume(savedMusic);
        SetSFXVolume(savedSFX);
    }

    // Gắn hàm này vào OnValueChanged của Slider Music
    public void SetMusicVolume(float value)
    {
        // 1. Log giá trị ra Console để kiểm tra Slider có chạy không
        Debug.Log("Giá trị Slider: " + value);

        // 2. Chặn giá trị trong khoảng an toàn
        float clampedValue = Mathf.Clamp(value, 0.0001f, 1f);

        // 3. Công thức tính dB an toàn: 0.0001 -> -80dB, 1 -> 0dB
        float db = Mathf.Log10(clampedValue) * 20;

        // Giới hạn dB không thấp hơn -80 và không cao hơn 0 (hoặc 20 tùy bạn)
        db = Mathf.Clamp(db, -80f, 0f);

        // 4. Gửi vào Mixer và kiểm tra kết quả
        bool success = mainMixer.SetFloat("musicVol", db);

        if (!success) Debug.LogError("Không tìm thấy biến musicVol trong Mixer!");

        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    // Gắn hàm này vào OnValueChanged của Slider Sound Effects
    public void SetSFXVolume(float value)
    {
        float db = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20;
        mainMixer.SetFloat("sfxVol", db);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    // Gắn hàm này vào nút Back_Button
    public void CloseSettings()
    {
        // Ẩn bảng setting đi
        Destroy(gameObject);
    }
}
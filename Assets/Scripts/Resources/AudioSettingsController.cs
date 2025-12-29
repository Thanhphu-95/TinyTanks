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
        // 1. Load lại giá trị đã lưu từ máy (mặc định là 0.75 nếu chưa có)
        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        // 2. Cập nhật vị trí thanh trượt
        musicSlider.value = savedMusic;
        sfxSlider.value = savedSFX;

        // 3. Áp dụng ngay vào Mixer khi vừa mở bảng
        SetMusicVolume(savedMusic);
        SetSFXVolume(savedSFX);
    }

    // Gắn hàm này vào OnValueChanged của Slider Music
    public void SetMusicVolume(float value)
    {
        // Chuyển giá trị từ 0-1 sang Decibel (-80dB đến 20dB)
        float db = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20;
        mainMixer.SetFloat("musicVol", db);
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

        // Đảm bảo thời gian game chạy bình thường nếu bạn có dùng Pause
        Time.timeScale = 1f;
    }
}
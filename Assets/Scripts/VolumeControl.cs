using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider; // Ссылка на UI Slider
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Установка начального значения громкости
        if (volumeSlider != null)
        {
            volumeSlider.value = audioSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    // Метод для установки громкости
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
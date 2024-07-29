using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider; // —сылка на UI Slider
    public AudioSource backgroundMusic;
    public AudioSource audioSource;
    public AudioClip eatingSound;
    public AudioClip enemySound;
    public AudioClip harvestSound;
    public AudioClip peasantSound;
    public AudioClip warriorSound;

    void Start()
    {
        backgroundMusic = GetComponent<AudioSource>();

        if (volumeSlider != null)       // ”становка начального значени€ громкости
        {
            volumeSlider.value = backgroundMusic.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float volume)     // ћетод дл€ установки громкости
    {
        backgroundMusic.volume = volume;
    }

    public void EatingSound()
    {
        PlayAudioClip(eatingSound); // усилил звук, под рукой не было аудио редактора , а качать здесь в  рыму 
        PlayAudioClip(eatingSound); // в деревне это нечто, в сент€бре норм будет дома в —ѕЅ
        PlayAudioClip(eatingSound);
        PlayAudioClip(eatingSound);
        PlayAudioClip(eatingSound);
    }

    public void EnemySound()
    {
        PlayAudioClip(enemySound);
    }
    public void HarvestSound()
    {
        PlayAudioClip(harvestSound);  // усилил звук
        PlayAudioClip(harvestSound);
        PlayAudioClip(harvestSound);
        PlayAudioClip(harvestSound);
        PlayAudioClip(harvestSound);
    }
    public void PeasantSound()
    {
        PlayAudioClip(peasantSound);
    }
    public void WarriorSound()
    {
        PlayAudioClip(warriorSound); // усилил звук
        PlayAudioClip(warriorSound);
        PlayAudioClip(warriorSound);
        PlayAudioClip(warriorSound);
        PlayAudioClip(warriorSound);
        PlayAudioClip(warriorSound);
        PlayAudioClip(warriorSound);
        PlayAudioClip(warriorSound);
        PlayAudioClip(warriorSound);
        PlayAudioClip(warriorSound);
    }


    private void PlayAudioClip(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
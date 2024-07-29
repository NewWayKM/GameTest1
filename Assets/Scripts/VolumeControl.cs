using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider; // ������ �� UI Slider
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

        if (volumeSlider != null)       // ��������� ���������� �������� ���������
        {
            volumeSlider.value = backgroundMusic.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float volume)     // ����� ��� ��������� ���������
    {
        backgroundMusic.volume = volume;
    }

    public void EatingSound()
    {
        PlayAudioClip(eatingSound); // ������ ����, ��� ����� �� ���� ����� ��������� , � ������ ����� � ����� 
        PlayAudioClip(eatingSound); // � ������� ��� �����, � �������� ���� ����� ���� � ���
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
        PlayAudioClip(harvestSound);  // ������ ����
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
        PlayAudioClip(warriorSound); // ������ ����
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
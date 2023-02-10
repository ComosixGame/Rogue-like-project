using System;
using UnityEngine;
using MyCustomAttribute;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    private AudioSource audioSource;
    private GameManager gameManager;
    [SerializeField, ReadOnly] private float volumeSFX = 0.5f;
    public event Action<bool> OnMute;
    public event Action<float> OnChangeSFXVolume;
    public event Action<float> OnChangeMusicVolume;
    public event Action<bool> OnSetPlayMusic;

    private bool isMute;

    protected override void Awake()
    {
        base.Awake();
        if(TryGetComponent<AudioSource>(out AudioSource audioSource)) {
            this.audioSource = audioSource;
        } else {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.minDistance = 0;
        gameManager = GameManager.Instance;
    }

    private void Start() {
        isMute = gameManager.settingData.mute;
        audioSource.mute = isMute;
    }

    public void SetMusicBackGround(AudioClip audioClip) {
        audioSource.clip = audioClip;
    }

    public void SetPlayMusic(bool play) {
        if(play) {
            audioSource.Play();
        } else {
            audioSource.Stop();
        }
        OnSetPlayMusic?.Invoke(play);
    }

    public void PlaySound(AudioClip audioClip) {
        audioSource.PlayOneShot(audioClip, volumeSFX);
    }

    public void MuteGame(bool mute) {
        isMute = mute;
        audioSource.mute = mute;
        gameManager.settingData.mute = mute;
        gameManager.settingData.Save();
        OnMute?.Invoke(mute);
    }

    public void ChangeSFXVolume(float volume) {
        volumeSFX = volume;
        OnChangeSFXVolume?.Invoke(volumeSFX);
    }

    public void ChangeMusicVolume(float volume) {
        audioSource.volume = volume;
        OnChangeMusicVolume?.Invoke(volume);
    }
}

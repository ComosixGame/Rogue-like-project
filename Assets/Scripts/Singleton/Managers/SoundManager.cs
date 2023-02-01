using System;
using UnityEngine;
using MyCustomAttribute;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    private AudioSource audioSource;
    [SerializeField, ReadOnly] private float volumeSFX = 0.5f;
    public event Action<bool> OnMute;
    public event Action<float> OnChangeSFXVolume;
    public event Action<float> OnChangeMusicVolume;
    public event Action<bool> OnSetPlayMusic;

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

    public void PlayeSound(AudioClip audioClip) {
        audioSource.PlayOneShot(audioClip, volumeSFX);
    }

    public void Mute(bool mute) {
        audioSource.mute = mute;
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

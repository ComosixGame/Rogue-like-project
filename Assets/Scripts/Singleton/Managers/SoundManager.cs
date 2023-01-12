using UnityEngine;
using UnityEngine.Events;

public class SoundManager
{
    private AudioSource audioSource;
    public UnityEvent<bool> OnMute = new UnityEvent<bool>();
    private GameManager gameManager;
    private bool isMute;

    private void Start() {
        audioSource.mute = isMute;
    }

    public void PlayOneShot(AudioClip audioClip, float volumeScale = 1){
        audioSource.PlayOneShot(audioClip, volumeScale);
    }

    public AudioSource AddAudioSource(GameObject parent){
        AudioSource audioSource = parent.AddComponent<AudioSource>();
        audioSource.mute = isMute;
        return audioSource;
    }

    public void MuteGame(bool mute){
        isMute = mute;
        audioSource.mute = mute;
        OnMute?.Invoke(mute);
    }
}

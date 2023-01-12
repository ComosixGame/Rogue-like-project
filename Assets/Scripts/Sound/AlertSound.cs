using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertSound : MonoBehaviour
{
    private GameManager gameManager;
    private SoundManager soundManager;
    private AudioSource audioSource;

    private void Awake() {
        gameManager = GameManager.Instance;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        
    }

    private void OnEnable() {
        soundManager.OnMute.AddListener(onMuteGame);
    }

    private void onMuteGame(bool mute){
        audioSource.mute = mute;
    }

    private void OnDisable() {
        soundManager.OnMute.RemoveListener(onMuteGame);
    }
}

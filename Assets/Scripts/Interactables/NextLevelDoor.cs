using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class NextLevelDoor : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private MapGeneration mapGeneration;
    [SerializeField] private GameObject door;
    public Vector3 playerStartPosition;
    private int _door;
    private bool levelCleared;

    private Animator _animator;

    public static event Action OnWinGame;
    private void Awake() {
        _animator = GetComponent<Animator>();
        _door = Animator.StringToHash("door");
    }

    private void Start() {
        _animator.SetBool(_door, false);
    }

    private void OnEnable() {
        mapGeneration.OnLevelCleared += OnlevelCleared;
    }

    private void OnTriggerEnter(Collider other) {
        if((layer & (1<<other.gameObject.layer)) != 0) {
            if(levelCleared) {
                StartCoroutine(CoroutineNextlevel(other.transform));
            }
        }
    }

    private void OnDisable() {
        mapGeneration.OnLevelCleared -= OnlevelCleared;
    }

    private void OnlevelCleared() {
        door.SetActive(true);
        levelCleared =  true;
        _animator.SetBool(_door, true);
    }

    IEnumerator CoroutineNextlevel(Transform player) {
        yield return new WaitForSeconds(0.5f);
        NextLevel(player);
    }

    private void NextLevel(Transform player) {
        door.SetActive(false);
        mapGeneration.NextLevel();
        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = false;
        player.position = playerStartPosition;
        controller.enabled = true;
    }
}

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NextLevelDoor : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private MapGeneration mapGeneration;
    [SerializeField] private GameObject door;
    public Vector3 playerStartPosition;
    private bool levelCleared;

    private void Awake() {
        mapGeneration.OnLevelCleared += OnlevelCleared;
    }

    private void OnTriggerEnter(Collider other) {
        if((layer & (1<<other.gameObject.layer)) != 0) {
            if(levelCleared) {
                StartCoroutine(CoroutineNextlevel(other.transform));
            }
        }
    }

    private void OnlevelCleared() {
        door.SetActive(true);
        levelCleared =  true;
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

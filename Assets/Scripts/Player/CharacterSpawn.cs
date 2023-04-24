using UnityEngine;
using Cinemachine;

public class CharacterSpawn : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private CharacterScripable characterScripable;
    private GameManager gameManager;

    public void Spawn(Vector3 position) {
        gameManager = GameManager.Instance;
        int index = gameManager.characterSeleted;
        GameObject prefab = characterScripable.characters[index].prefab;
        GameObject player = Instantiate(prefab, position, Quaternion.identity);
        cinemachineVirtualCamera.Follow = player.transform;
        cinemachineVirtualCamera.LookAt = player.transform;
        gameManager.SelectPlayer(player.transform);
        
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1f);
    }
#endif
}

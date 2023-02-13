
using UnityEngine;
using Cinemachine;

public class CharacterSpawn : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private CharacterScripable characterScripable;
    private GameManager gameManager;

    private void Awake() {
        gameManager = GameManager.Instance;
        int index = gameManager.characterSeleted;
        Debug.Log(characterScripable.characters[index].nameCharacter);
        GameObject prefab = characterScripable.characters[index].prefab;
        GameObject player = Instantiate(prefab, transform.position, Quaternion.identity);
        cinemachineVirtualCamera.Follow = player.transform;
        cinemachineVirtualCamera.LookAt = player.transform;
        //
        ///
        ///

        gameManager.SelectPlayer(player.transform);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1f);
    }
}

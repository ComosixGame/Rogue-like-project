
using UnityEngine;
using Cinemachine;

public class CharacterSpawn : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private CharacterScripable characterScripable;
    public Vector3 spawmPos;

    private GameManager gameManager;

    private void Awake() {
        gameManager = GameManager.Instance;
        int index = gameManager.characterSeleted;
        Debug.Log(characterScripable.characters[index].nameCharacter);
        GameObject prefab = characterScripable.characters[index].prefab;
        GameObject player = Instantiate(prefab, spawmPos, Quaternion.identity);
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
        Gizmos.DrawSphere(spawmPos, 2f);
    }
}

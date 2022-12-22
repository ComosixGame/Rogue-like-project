using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [System.Serializable]
    public class ObjectSpawn {
        [SerializeField] private string keyObject;
        [SerializeField] private int chanceToSpawn;

        public int GetChanceToSpawn() {
            return chanceToSpawn;
        }
        public string GetKey() {
            return keyObject;
        }
    }
    [SerializeField] private Transform startPosition;
    [SerializeField] private int row, collumn;
    [SerializeField] private int minObstacle, maxObstacle;
    [SerializeField] private ObjectSpawn[] obstacles;
    [SerializeField] private int minEnemy, maxEnemy;
    public float delaySpawnEnemy;
    public ParticleSystem spawnEffect;
    [SerializeField] private ObjectSpawn[] enemies;
    private List<Vector3> gridPositions;
    private List<int> listIndexEnemy;
    private List<int> listIndexObstacle;
    private int quantityObstacle, quantityEnemy;
    private ObjectPooler objectPooler;

    private void Awake() {
        objectPooler = ObjectPooler.Instance;
        gridPositions = new List<Vector3>();
        listIndexEnemy = new List<int>();
        listIndexObstacle = new List<int>();
        quantityObstacle = Random.Range(minObstacle, maxObstacle + 1);
        quantityEnemy = Random.Range(minEnemy, maxEnemy + 1);

        // tạo hệ thống spawn obstacle theo tỉ lệ
        for(int i = 0; i < obstacles.Length; i ++) {
            for(int j = 0; j < obstacles[i].GetChanceToSpawn(); j++) {
                listIndexObstacle.Add(i);
            }
        }

        // tạo hệ thống spawn enemy theo tỉ lệ
        for(int i = 0; i < enemies.Length; i ++) {
            for(int j = 0; j < enemies[i].GetChanceToSpawn(); j++) {
                listIndexEnemy.Add(i);
            }
        }
        //chờ object pool tạo xong các object
        objectPooler.OnCreatedObject += SpawnGameObject;
    }

    void Start()
    {
        //tạo hệ thống grid vị trí có thể spawn
        CreateGridBoard();
    }

    private void OnDisable() {
        objectPooler.OnCreatedObject += SpawnGameObject;
    }

    private void CreateGridBoard() {
        // tạo grid tự động
        for(int y = 0; y <  collumn ; y ++) {
            // tạo grid cho từng hàng
            for(int x = 0; x < row ; x++) {
                // 5 là độ lớn của 1 ô grid
                Transform gridPos=  Instantiate(startPosition, startPosition.position + new Vector3(x * 5, 0, -y * 5), startPosition.rotation);
                gridPos.SetParent(transform);
                gridPositions.Add(gridPos.position);
            }
        }
    }

    private void RandomSpawnObstacle() {
        // random spawn obstacle
        for(int i = 0 ; i < quantityObstacle ; i ++) {
            int randomIndexObstacle  = listIndexObstacle[Random.Range(0, listIndexObstacle.Count)];
            int randomIndexPos = Random.Range(0,gridPositions.Count);
            objectPooler.SpawnObject(obstacles[randomIndexObstacle].GetKey(), gridPositions[randomIndexPos], Quaternion.identity);
            gridPositions.RemoveAt(randomIndexPos);
        }
    }

    private void RandomSpawnEnemy() {
        // random spawn enemy
        for(int i = 0 ; i < quantityEnemy ; i ++) {
            int randomIndexPos = Random.Range(0,gridPositions.Count);
            Vector3 spawnPos = gridPositions[randomIndexPos];
            ParticleSystem effect = Instantiate(spawnEffect, spawnPos + Vector3.up * 0.001f, Quaternion.LookRotation(Vector3.up));
            gridPositions.RemoveAt(randomIndexPos);
            StartCoroutine(SpawnEnemy(spawnPos, effect));
        }
    }

    IEnumerator SpawnEnemy(Vector3 position, ParticleSystem spawnEffect) {
        yield return new WaitForSeconds(delaySpawnEnemy);
        int randomIndexEnemy = listIndexEnemy[Random.Range(0, listIndexEnemy.Count)];
        objectPooler.SpawnObject(enemies[randomIndexEnemy].GetKey(),  position, Quaternion.identity);
        spawnEffect.Stop();
        Destroy(spawnEffect.gameObject, 0.5f);
    }

    private void SpawnGameObject() {
        //random vật cản
        RandomSpawnObstacle();
        //random enemy
        Invoke("RandomSpawnEnemy", 1f);
    }

}

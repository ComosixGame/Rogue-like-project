using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [System.Serializable]
    public class ObjectSpawn {
        [SerializeField] private GameObjectPool gameObjectPool;
        [SerializeField] private int chanceToSpawn;

        public int GetChanceToSpawn() {
            return chanceToSpawn;
        }

        public GameObjectPool GetGameObjectPool() {
            return(gameObjectPool);
        }
    }
    [SerializeField] private Transform startPosition;
    [SerializeField] private int row, collumn;
    [SerializeField] private int minObstacle, maxObstacle;
    [SerializeField] private ObjectSpawn[] obstacles;
    [SerializeField] private int minEnemy, maxEnemy;
    public float delaySpawnEnemy;
    [SerializeField] private ObjectSpawn[] enemies;
    public EffectObjectPool teleEffect;
    private List<Vector3> gridPositions;
    private List<int> listIndexEnemy;
    private List<int> listIndexObstacle;
    private int quantityObstacle, quantityEnemy;
    private List<Vector3> enemyPosSpawn;
    private ObjectPoolerManager ObjectPoolerManager;

    private void Awake() {
        ObjectPoolerManager = ObjectPoolerManager.Instance;
        gridPositions = new List<Vector3>();
        listIndexEnemy = new List<int>();
        listIndexObstacle = new List<int>();
        enemyPosSpawn = new List<Vector3>();
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
        ObjectPoolerManager.OnCreatedObject += SpawnGameObject;
    }
    

    private void OnDisable() {
        ObjectPoolerManager.OnCreatedObject += SpawnGameObject;
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
            ObjectPoolerManager.SpawnObject(obstacles[randomIndexObstacle].GetGameObjectPool(), gridPositions[randomIndexPos], Quaternion.identity);
            gridPositions.RemoveAt(randomIndexPos);
        }
    }

    private void RandomSpawnEnemy() {
        // random spawn enemy
        for(int i = 0 ; i < quantityEnemy ; i ++) {
            int randomIndexPos = Random.Range(0,gridPositions.Count);
            Vector3 spawnPos = gridPositions[randomIndexPos];
            //hiệu ứng tele
            GameObjectPool effect = ObjectPoolerManager.SpawnObject(teleEffect, spawnPos + Vector3.up * 0.001f, Quaternion.LookRotation(Vector3.up));
            gridPositions.RemoveAt(randomIndexPos);
            StartCoroutine(SpawnEnemy(spawnPos, effect.gameObject));
        }
    }

    IEnumerator SpawnEnemy(Vector3 position, GameObject spawnEffect) {
        yield return new WaitForSeconds(delaySpawnEnemy);
        int randomIndexEnemy = listIndexEnemy[Random.Range(0, listIndexEnemy.Count)];
        ObjectPoolerManager.SpawnObject(enemies[randomIndexEnemy].GetGameObjectPool(), position, Quaternion.identity);
        spawnEffect.GetComponent<ParticleSystem>().Stop();
    }

    private void SpawnGameObject() {
        //tạo hệ thống grid vị trí có thể spawn
        CreateGridBoard();
        //random vật cản
        RandomSpawnObstacle();
        //random enemy
        Invoke("RandomSpawnEnemy", 1f);
    }

}

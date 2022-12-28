using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyCustomAttribute;

public class ObjectPoolerManager : Singleton<ObjectPoolerManager>
{
    [System.Serializable]
    public class ObjectPrefab 
    {
        public string key;
        public int size;  
        public GameObject prefab;
        public int active, inactive;
        public Queue<GameObject> objectPool = new Queue<GameObject>();

        public ObjectPrefab(string key, int size, GameObject prefab) {
            this.key = key;
            this.size = size;
            this.prefab = prefab;
        }
    }

    [SerializeField] private ObjectPoolerScriptable ObjectPoolerScriptable;
    [ReadOnly, SerializeField] private List<ObjectPrefab> objectPrefabs;
    private Dictionary<string, ObjectPrefab> dictionary;
    public event Action OnCreatedObject;

    protected override void Awake()
    {
        base.Awake();
        // tạo list các object để dễ quan sát
        objectPrefabs = new List<ObjectPrefab>();
        dictionary = new Dictionary<string, ObjectPrefab>();
    }

    private void Start() {
        // khởi tạo object pooler
        foreach(ObjectPoolerScriptable.ScripblePrefab scripblePrefab in ObjectPoolerScriptable.prefabs) {
            ObjectPrefab objectPrefab = new ObjectPrefab(scripblePrefab.GameObjectPool.key, scripblePrefab.size, scripblePrefab.GameObjectPool.GetGameObject());
            //thêm object vào list quan sát
            objectPrefabs.Add(objectPrefab);
            //thêm objectprefab vào dic để truy vấn sau này
            dictionary.Add(objectPrefab.key, objectPrefab);
            // tạo ra các object mới cho object pooler
            for(int i = 0; i < objectPrefab.size; i++) {
                GameObject gameObj = Instantiate(objectPrefab.prefab);
                gameObj.transform.SetParent(transform);
                gameObj.SetActive(false);
                objectPrefab.inactive ++;
                // thêm vào queue để chờ sử dụng
                objectPrefab.objectPool.Enqueue(gameObj);
            }
        }
        OnCreatedObject?.Invoke();
    }

    public GameObjectPool SpawnObject(GameObjectPool gameObjectPool, Vector3 position, Quaternion rotation) {
        ObjectPrefab objectPrefab = dictionary[gameObjectPool.key];
        GameObject gameObj;
        // kiểm tra nếu object có sẵn ko có đủ thì tạo cái mới
        if(objectPrefab.inactive <=0) {
            gameObj = Instantiate(objectPrefab.prefab, position, rotation);
            gameObj.transform.SetParent(transform, false);
            gameObj.SetActive(true);
            objectPrefab.active ++;
            objectPrefab.size ++;
            //thêm lại vào queue để chờ sử dụng
            objectPrefab.objectPool.Enqueue(gameObj);
        } else {
            //nếu còn object thì dequeue từ queue để sử dụng
            gameObj = objectPrefab.objectPool.Dequeue();
            gameObj.SetActive(false);
            Transform gameObjTransform = gameObj.transform;
            gameObjTransform.position = position;
            gameObjTransform.rotation = rotation;
            gameObj.SetActive(true);
            objectPrefab.active ++;
            objectPrefab.inactive --;
            // thêm lại vào queue để chờ sử dụng
            objectPrefab.objectPool.Enqueue(gameObj);
        }
        return gameObj.GetComponent<GameObjectPool>();
    }

    public void DeactiveObject(GameObjectPool gameObjectPool) {
        ObjectPrefab objectPrefab = dictionary[gameObjectPool.key];
        gameObjectPool.gameObject.SetActive(false);
        objectPrefab.inactive ++;
        objectPrefab.active --;
    }

    public void ResetObjectPoolerManager() {
        //ẩn tất cả object
        for(int i = 0; i < transform.childCount; i ++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        //reset chỉ số
        foreach(ObjectPoolerScriptable.ScripblePrefab scripblePrefab in ObjectPoolerScriptable.prefabs) {
            ObjectPrefab objectPrefab = dictionary[scripblePrefab.GameObjectPool.key];
            objectPrefab.inactive = objectPrefab.size;
            objectPrefab.active = 0;
        }
    }
}
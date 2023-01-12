using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyCustomAttribute;

public class LoadSceneManager : Singleton<LoadSceneManager>
{
    /** 
     * Sử dụng LoadScene để load scene mới
     * LoadScene có thể nhận cả index hoặc đường dẫn của scene để load, tham số loadSceneMode 
     * Để chọn mode load single hoặc Additive ko nhập mặc định là single
     * Sử dụng ResetScene để load lại scene đang chạy
     * Sử dụng sử kiện OnLoadProgresscing để lắng nghe quá trình load scene và nhận giá trị progress
     * OnLoadScene để lắng nghe sự kiện load scene
     * Sử dụng sử kiện OnResetScene để lắng nghe quá trình Reset scene
    */
    [SerializeField, ReadOnly] private int currentSceneIndex;
    [SerializeField, ReadOnly] private string currentScene;
    private AsyncOperation operation;
    public event Action OnLoadScene;
    public event Action<float> OnLoadProgresscing;
    public event Action OnResetScene;
    
    protected override void Awake() {
        base.Awake();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void ResetScene() {
        LoadScene(currentSceneIndex);
        OnResetScene?.Invoke();
    }
    
    public void LoadScene(int index, LoadSceneMode loadSceneMode = LoadSceneMode.Single) {
        StartCoroutine(LoadAsync(index, loadSceneMode));
        OnLoadScene?.Invoke();
    }

    public void LoadScene(string scenePath, LoadSceneMode loadSceneMode = LoadSceneMode.Single) {
        StartCoroutine(LoadAsync(scenePath, loadSceneMode));
    }

    IEnumerator LoadAsync(int sceneIndex, LoadSceneMode loadSceneMode = LoadSceneMode.Single) {
        operation = SceneManager.LoadSceneAsync(sceneIndex, loadSceneMode);
        while(!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress/ 0.9f);
            OnLoadProgresscing?.Invoke(progress);
            yield return null;
        }
    }

    IEnumerator LoadAsync(string scenePath, LoadSceneMode loadSceneMode = LoadSceneMode.Single) {
        operation = SceneManager.LoadSceneAsync(scenePath, loadSceneMode);
        while(!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress/ 0.9f);
            OnLoadProgresscing?.Invoke(progress);
            yield return null;
        }
    }

}

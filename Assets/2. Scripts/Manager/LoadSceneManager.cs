using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : Singleton<LoadSceneManager>
{
    public event Action<float> OnLoadingProgressChanged;
    public float               LoadingProgress { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            LoadingProgress = asyncLoad.progress;
            OnLoadingProgressChanged?.Invoke(LoadingProgress);

            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UIManager.Instance.InitializeUIRoot(); // 공개 메서드로 변경 필요
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
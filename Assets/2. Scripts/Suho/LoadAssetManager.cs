using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class LoadAssetManager : Singleton<LoadAssetManager>
{
    
    //비동기 로딩 시 사용할 핸들
    private List<AsyncOperationHandle<AudioClip>> loadAudioClipHandles = new();
    private List<AsyncOperationHandle<IList<AudioClip>>> loadSceneAudiohandles = new();
    
    // 레이블을 사용해서 에셋번들의 로케이션을 받아오는 메서드
    public void LoadAssetBundle(string labelName)
    {
        Addressables.LoadResourceLocationsAsync(labelName).Completed +=
            (handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var locations = handle.Result;
                    Debug.Log($"로케이션 {locations.Count}개 가져옴");
                    OnLoadAssetsChangeScene(labelName, locations);
                }
                else
                {
                    Debug.LogError($"로케이션 로드 실패: {labelName}");
                }
            });
    }
    
    // 받아온 로케이션을 통해 에셋을 로드해오는 메서드
    public void OnLoadAssetsChangeScene(string lableName, IList<IResourceLocation> locations)
    {   
        //스테이지에 필요한 사운드레이블을 가져오기
        Addressables.LoadAssetsAsync<AudioClip>(locations,null).Completed +=
            (handle =>
            {
                loadSceneAudiohandles.Add(handle);
                foreach (AudioClip clip in handle.Result)
                {
                    AudioManager.Instance.AudioDictionary.TryAdd(clip.name, clip);
                }
            });
    }
    
    //비동기 오디오클립 로드 메서드
    public void LoadAudioClipAsync(string assetName,Action<string> onLoaded)
    {
        Addressables.LoadAssetAsync<AudioClip>(assetName).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var clip = handle.Result;
                loadAudioClipHandles.Add(handle);
                AudioManager.Instance.AudioDictionary.TryAdd(assetName, handle.Result);
                onLoaded?.Invoke(assetName); // 로드 완료 후 콜백 호출
            }
            else
            {
                Debug.LogError($"AudioClip 로드 실패: {assetName}");
                onLoaded?.Invoke(null); // 실패했을 때 처리
            }
        };
    }


    // 메모리에 올라온 오디오클립을 릴리즈
    public void ReleaseAudioClips()
    {
        foreach (var handle in loadAudioClipHandles)
        {
            Addressables.Release(handle);
        }

        foreach (var handle in loadSceneAudiohandles)
        {
            Addressables.Release(handle);
        }
        loadAudioClipHandles.Clear();
        loadSceneAudiohandles.Clear();
        AudioManager.Instance.AudioDictionary.Clear();
    }
    
    
    
}

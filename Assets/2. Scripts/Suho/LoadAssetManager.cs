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
    
    // 레이블을 사용해서 에셋번들의 로케이션을 받아오는 메서드
    public void LoadAssetBundle(string labelName)
    {
        Addressables.LoadResourceLocationsAsync(labelName).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var locations = handle.Result;

                // 로케이션이 없을 경우 바로 종료
                if (locations == null || locations.Count == 0)
                {
                    Debug.LogWarning($"[LoadAssetBundle] '{labelName}'에 해당하는 오디오 클립이 없습니다.");
                    return;
                }

                OnLoadAssetsChangeScene(labelName, locations);
            }
            else
            {
                Debug.LogError($"로케이션 로드 실패: {labelName}");
            }
        };
    }

// 받아온 로케이션을 통해 에셋을 로드해오는 메서드
    public void OnLoadAssetsChangeScene(string labelName, IList<IResourceLocation> locations)
    {
        foreach (var location in locations)
        {
            var handle = Addressables.LoadAssetAsync<AudioClip>(location);

            handle.Completed += (clipHandle) =>
            {
                if (clipHandle.Status == AsyncOperationStatus.Succeeded && clipHandle.Result != null)
                {
                    AudioClip clip = clipHandle.Result;
                    string addressKey = location.PrimaryKey;

                    if (!AudioManager.Instance.AudioDictionary.TryAdd(addressKey, clip))
                    {
                        Debug.LogWarning($"[OnLoadAssetsChangeScene] 이미 등록된 키: {addressKey}");
                    }
                }
                else
                {
                    Debug.LogError($"오디오 로딩 실패: {location.PrimaryKey}");
                }
            };

            loadAudioClipHandles.Add(handle);
        }
    }

    
    //비동기 오디오클립 로드 메서드
    public void LoadAudioClipAsync(string assetName, Action<string> onLoaded)
    {
        if (string.IsNullOrEmpty(assetName) || assetName == "None")
        {
            Debug.Log("[LoadAudioClipAsync] 요청된 오디오 이름이 None이거나 비어있음");
            onLoaded?.Invoke(null);
            return;
        }

        // 1. 먼저 키 유효성 검사
        Addressables.LoadResourceLocationsAsync(assetName).Completed += (locHandle) =>
        {
            if (locHandle.Status == AsyncOperationStatus.Succeeded && locHandle.Result.Count > 0)
            {
                // 2. 유효한 키면 실제 AudioClip 로드
                Addressables.LoadAssetAsync<AudioClip>(assetName).Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
                    {
                        var clip = handle.Result;

                        // 메모리 해제를 위해 핸들 저장
                        loadAudioClipHandles.Add(handle);

                        // Dictionary에 추가 (중복 방지)
                        if (!AudioManager.Instance.AudioDictionary.TryAdd(assetName, clip))
                        {
                            Debug.LogWarning($"[LoadAudioClipAsync] 이미 등록된 키: {assetName}");
                        }

                        onLoaded?.Invoke(assetName);
                    }
                    else
                    {
                        Debug.LogWarning($"[LoadAudioClipAsync] AudioClip 로드 실패: {assetName}");
                        onLoaded?.Invoke(null);
                    }
                };
            }
            else
            {
                // 3. 키가 존재하지 않을 때 예외 없이 경고
                Debug.LogWarning($"[LoadAudioClipAsync] Addressables에서 '{assetName}' 키를 찾을 수 없습니다.");
                onLoaded?.Invoke(null);
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
 
        loadAudioClipHandles.Clear();
        AudioManager.Instance.AudioDictionary.Clear();
    }
    
    
    
}

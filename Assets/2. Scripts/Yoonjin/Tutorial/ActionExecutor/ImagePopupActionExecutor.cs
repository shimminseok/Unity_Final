using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ImagePopupActionExecutor : TutorialActionExecutor
{
    private GameObject instance;
    private AsyncOperationHandle<GameObject>? loadHandle;

    public override void Enter(TutorialActionData actionData)
    {
        var imageData = actionData as ImagePopupActionData;
        if (imageData == null)
        {
            manager.NotifyActionComplete();
            return;
        }

        // Addressables 로드
        loadHandle = Addressables.LoadAssetAsync<GameObject>(imageData.prefabAddress);
        loadHandle.Value.Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var prefab = handle.Result;
                instance = GameObject.Instantiate(prefab);

                var parent = GameObject.Find(imageData.parentCanvasName)?.transform;

                if (parent != null)
                {
                    instance.transform.SetParent(parent, false); // 유지
                }
                else
                {
                    Debug.LogWarning("[튜토리얼] UIRoot를 찾을 수 없습니다. 이미지 팝업이 계층에 추가되지 않았습니다.");
                }
            }
            else
            {
                Debug.LogError($"[튜토리얼] Addressable 로드 실패: {imageData.prefabAddress}");
            }

            // 스텝은 유지되고, UI는 띄워진 상태로 전환
            manager.NotifyActionComplete();
        };
    }

    public override void Exit()
    {
        if (instance != null)
        {
            GameObject.Destroy(instance);
            instance = null;
        }

        if (loadHandle.HasValue)
        {
            Addressables.Release(loadHandle.Value);
            loadHandle = null;
        }
    }
}

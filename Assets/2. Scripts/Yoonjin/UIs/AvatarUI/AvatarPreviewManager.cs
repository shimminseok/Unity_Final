using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarPreviewManager : SceneOnlySingleton<AvatarPreviewManager>
{
    [System.Serializable]
    public class AvatarEntry
    {
        // 고유 캐릭터SO와 전용 카메라
        public PlayerUnitSO characterSO;
        public Camera avatarCam;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    [Header("아바타 프리뷰 카메라 목록")]
    [SerializeField] private List<AvatarEntry> avatarEntries = new();


    /// 해당 캐릭터에 해당하는 아바타 카메라를 찾아 활성화
    /// 해당 카메라의 렌더 결과를 RawImage에 연결
    public void ShowAvatar(PlayerUnitSO characterSo, RawImage targetRawImage)
    {
        foreach (var entry in avatarEntries)
        {
            // 선택된 캐릭터만 카메라 활성화
            bool isMatch = entry.characterSO == characterSo;
            entry.avatarCam.gameObject.SetActive(isMatch);

            if (isMatch)
            {
                // 선택된 카메라의 렌더텍스처를 RawImage에 연결
                targetRawImage.texture = entry.avatarCam.targetTexture;
            }
        }
    }

    // 모든 아바타 카메라 비활성화
    public void HideAllAvatars()
    {
        foreach (var entry in avatarEntries)
        {
            entry.avatarCam.gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarPreviewManager : SceneOnlySingleton<AvatarPreviewManager>
{
    [SerializeField] private Camera avatarCam;

    [Header("아바타 프리뷰 카메라 목록")]
    [SerializeField] private List<AvatarEntry> avatarEntries = new();

    [SerializeField] private Transform avatarPoolTransform;
    [SerializeField] private List<Transform> deckSlotTransforms = new();
    [SerializeField] private List<GameObject> unitAvatars = new();

    [System.Serializable]
    public class AvatarEntry
    {
        // 고유 캐릭터SO와 전용 카메라
        public GameObject avatar;
        public PlayerUnitSO characterSo;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }


    /// 해당 캐릭터에 해당하는 아바타 카메라를 찾아 활성화
    /// 해당 카메라의 렌더 결과를 RawImage에 연결
    public void ShowAvatar(PlayerUnitSO characterSo)
    {
        avatarCam.gameObject.SetActive(true);
        foreach (var entry in avatarEntries)
        {
            entry.avatar.SetActive(entry.characterSo == characterSo);
        }
    }

    // 모든 아바타 카메라 비활성화
    public void HideAllAvatars()
    {
        avatarCam.gameObject.SetActive(false);
    }


    public void ShowAvatar(int index, JobType jobType)
    {
        unitAvatars[(int)jobType].SetActive(true);
        unitAvatars[(int)jobType].transform.SetParent(deckSlotTransforms[index]);
        unitAvatars[(int)jobType].transform.localPosition = Vector3.zero;
        unitAvatars[(int)jobType].transform.localRotation = Quaternion.identity;
    }

    public void HideAvatar(JobType jobType)
    {
        unitAvatars[(int)jobType].transform.SetParent(avatarPoolTransform);
        unitAvatars[(int)jobType].SetActive(false);
        unitAvatars[(int)jobType].transform.localPosition = Vector3.zero;
        unitAvatars[(int)jobType].transform.localRotation = Quaternion.identity;
    }

    public void HideAllBuilindUIAvatars()
    {
        foreach (var avatar in unitAvatars)
        {
            avatar.SetActive(false);
        }
    }
}
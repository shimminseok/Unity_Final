using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarPreviewManager : SceneOnlySingleton<AvatarPreviewManager>
{
    [SerializeField] private Camera avatarCam;
    [SerializeField] private Transform avatarCamTransform;


    [SerializeField] private Transform avatarPoolTransform;
    [SerializeField] private List<Transform> deckSlotTransforms = new();
    [SerializeField] private List<GameObject> unitAvatars = new();


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
        unitAvatars[(int)characterSo.JobType].SetActive(true);
        unitAvatars[(int)characterSo.JobType].transform.SetParent(avatarCamTransform);
        unitAvatars[(int)characterSo.JobType].transform.localPosition = Vector3.zero;
        unitAvatars[(int)characterSo.JobType].transform.localRotation = Quaternion.identity;
    }

    // 모든 아바타 카메라 비활성화
    public void HideAvatar(PlayerUnitSO characterSo)
    {
        avatarCam.gameObject.SetActive(false);
        if (characterSo != null)
            HideAvatar(characterSo.JobType);
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
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

    private readonly Dictionary<int, GameObject> unitAvatarDict = new();


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
        if (!unitAvatarDict.TryGetValue(characterSo.ID, out GameObject avatar))
        {
            GameObject go = Resources.Load<GameObject>($"Character/{characterSo.UnitPrefab.name}");
            avatar = Instantiate(go, avatarPoolTransform);
            unitAvatarDict.Add(characterSo.ID, avatar);
        }

        avatar.SetActive(true);
        avatar.transform.SetParent(avatarCamTransform);
        avatar.transform.localPosition = Vector3.zero;
        avatar.transform.localRotation = Quaternion.identity;
    }

    // 모든 아바타 카메라 비활성화
    public void HideAvatar(PlayerUnitSO characterSo)
    {
        avatarCam.gameObject.SetActive(false);
        if (!unitAvatarDict.TryGetValue(characterSo.ID, out GameObject avatar))
        {
            GameObject go = Resources.Load<GameObject>($"Character/{characterSo.UnitPrefab.name}");
            avatar = Instantiate(go, avatarPoolTransform);
            unitAvatarDict.Add(characterSo.ID, avatar);
        }

        avatar.SetActive(false);
        avatar.transform.SetParent(avatarPoolTransform);
        avatar.transform.localPosition = Vector3.zero;
        avatar.transform.localRotation = Quaternion.identity;
    }


    public void ShowAvatar(int index, PlayerUnitSO characterSo)
    {
        if (!unitAvatarDict.TryGetValue(characterSo.ID, out GameObject avatar))
        {
            GameObject go = Resources.Load<GameObject>($"Character/{characterSo.UnitPrefab.name}");
            avatar = Instantiate(go, avatarPoolTransform);
            unitAvatarDict.Add(characterSo.ID, avatar);
        }

        avatar.SetActive(true);
        avatar.transform.SetParent(deckSlotTransforms[index]);
        avatar.transform.localPosition = Vector3.zero;
        avatar.transform.localRotation = Quaternion.identity;
    }

    public void HideAllBuilindUIAvatars()
    {
        foreach (GameObject avatar in unitAvatarDict.Values)
        {
            avatar.SetActive(false);
        }
    }
}
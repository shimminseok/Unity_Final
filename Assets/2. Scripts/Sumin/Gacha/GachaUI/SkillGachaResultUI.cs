using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkillGachaResultUI : UIBase
{
    [SerializeField] private Button resultExitBtn;
    [SerializeField] private SkillGachaSlotUI[] slots;

    void Start()
    {
        resultExitBtn.onClick.RemoveAllListeners();
        resultExitBtn.onClick.AddListener(() => OnResultExitBtn());
        ResetSlots();
    }

    // 스킬 뽑기 결과 업데이트
    public void ShowSkills(GachaResult<ActiveSkillSO>[] skills)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].Initialize(skills[i].GachaReward);

            if (skills[i].IsDuplicate) // 중복이면 티어 대신 보상금 띄워주기
            {
                slots[i].ShowCompensation(skills[i].CompensationAmount);
            }

            slots[i].transform.localScale = Vector3.zero;
            slots[i].transform.DOScale(Vector3.one, 0.3f)
                .SetEase(Ease.OutBack)
                .SetDelay(i * 0.1f);
        }
    }

    public void OnResultExitBtn()
    {
        ResetSlots();
        UIManager.Instance.Close(this);
    }

    private void ResetSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
    }
}

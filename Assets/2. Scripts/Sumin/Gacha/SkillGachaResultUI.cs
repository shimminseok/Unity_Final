using UnityEngine;
using UnityEngine.UI;


public class SkillGachaResultUI : UIBase
{
    [SerializeField] private Button resultExitBtn;
    [SerializeField] private SkillGachaSlotUI[] slots;

    private void Start()
    {
        resultExitBtn.onClick.RemoveAllListeners();
        resultExitBtn.onClick.AddListener(() => OnResultPanelExitBtn());
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
        }
    }

    // 결과창 나가기 버튼
    public void OnResultPanelExitBtn()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
        UIManager.Instance.Close(this);
    }
}

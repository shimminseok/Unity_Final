using UnityEngine;

public class SkillGachaUI : MonoBehaviour
{
    [SerializeField] private SkillGachaSystem gachaSystem;
    [SerializeField] private GameObject resultPannel;
    [SerializeField] private SkillSlotUI[] slots;

    // 1회 뽑기 버튼
    public void OnDrawOneBtn()
    {
        DrawAndDisplayResult(1);
    }

    // 10회 뽑기 버튼
    public void OnDrawTenBtn()
    {
        DrawAndDisplayResult(10);
    }

    // 스킬 뽑기 UI 디스플레이
    private void DrawAndDisplayResult(int count)
    {
        ActiveSkillSO[] skills = gachaSystem.DrawSkills(count);
        resultPannel.SetActive(true);

        for (int i = 0; i < skills.Length; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].Initialize(skills[i]);
        }
    }

    // 결과창 나가기 버튼
    public void OnResultPannelExitBtn()
    {
        resultPannel.SetActive(false);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
    }

}

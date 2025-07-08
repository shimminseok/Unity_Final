using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaSkillSlotUI : MonoBehaviour
{
    [SerializeField] private Image skillIamge;
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillTierText;

    // 스킬 슬롯 내용 업데이트
    public void Initialize(ActiveSkillSO skill)
    {
        skillIamge.sprite = skill.skillIcon;
        skillNameText.text = skill.skillName;
        skillTierText.text = $"{skill.activeSkillTier}";
    }

    // 스킬 슬롯 결과 보상 업데이트
    public void ShowCompensation(int compensation)
    {
        skillTierText.text = $"{compensation} Opal";
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] private Image skillIamge;
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillTierText;

    public void Initialize(ActiveSkillSO skill)
    {
        skillIamge.sprite = skill.skillIcon;
        skillNameText.text = skill.skillName;
        skillTierText.text = $"{skill.activeSkillTier}";
    }
}

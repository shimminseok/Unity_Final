using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillGachaUI : MonoBehaviour
{
    [SerializeField] private SkillGachaSystem gachaSystem;
    [SerializeField] private GameObject resultPannel;

    [SerializeField] private Image skillIamge;
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillTierText;

    public void OnGachaOneDrawBtn()
    {
        var skill = gachaSystem.RollSkill();
        if (skill != null)
        {
            Debug.Log("1회 뽑기 시작");
            Debug.Log($"스킬: {skill.skillName}, 티어: {skill.activeSkillTier}, 직업: {skill.jobType}");
            resultPannel.SetActive(true);
            updateResult(skill);
        }
        else
        {
            Debug.LogError("스킬 뽑기 실패");
        }
    }

    public void OnResultPannelExitBtn()
    {
        resultPannel.SetActive(false);
    }

    private void updateResult(ActiveSkillSO skill)
    {
        skillIamge.sprite = skill.skillIcon;
        skillNameText.text = skill.skillName;
        skillTierText.text = $"{skill.activeSkillTier}";
    }
}

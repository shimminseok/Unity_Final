using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillGachaUI : MonoBehaviour
{
    [SerializeField] private SkillGachaSystem gachaSystem;
    [SerializeField] private GameObject resultPannel;
    [SerializeField] private SkillSlotUI[] slots;

    public void OnDrawOneBtn()
    {
        var skill = gachaSystem.DrawSkill();
        if (skill != null)
        {
            Debug.Log($"1뽑 결과 : {skill.skillName} ({skill.activeSkillTier})");
            resultPannel.SetActive(true);
            slots[0].gameObject.SetActive(true);
            slots[0].Initialize(skill);
        }
        else
        {
            Debug.LogError("스킬 뽑기 실패");
        }
    }

    public void OnDrawTenBtn()
    {
        List<ActiveSkillSO> skills = gachaSystem.DrawSkillMultiple(10);
        foreach (var skill in skills)
        {
            Debug.Log($"10연 결과 : {skill.skillName} ({skill.activeSkillTier})");
            resultPannel.SetActive(true);
        }
        for (int i=0; i<skills.Count; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].Initialize(skills[i]);
        }
    }

    public void OnResultPannelExitBtn()
    {
        resultPannel.SetActive(false);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
    }

}

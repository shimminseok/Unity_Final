using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image skillTier;
    [SerializeField] private TextMeshProUGUI skillName;


    private ActiveSkillSO activeSkillSo;
    private PassiveSO passiveSo;

    public void SetSkillIcon(ActiveSkillSO skillSo)
    {
        this.activeSkillSo = skillSo;
        skillIcon.sprite = activeSkillSo.skillIcon;
        skillName.text = activeSkillSo.skillName;
    }


    public void SetSkillIcon(PassiveSO skillSo)
    {
        passiveSo = skillSo;
        skillIcon.sprite = skillSo.PassiveIcon;
        skillName.text = skillSo.PassiveName;
    }
}
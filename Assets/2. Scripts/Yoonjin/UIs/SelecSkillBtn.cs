using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecSkillBtn : MonoBehaviour
{
    [SerializeField]
    private SkillData skillData;
    public int slotIndex;

    public void OnSkillClick()
    {
        DeckSelectManager.Instance.SelectSkill(skillData, slotIndex);
    }
}

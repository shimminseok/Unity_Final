using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : UIBase
{
    [Header("패시브 스킬 선택 영역")]
    [SerializeField] private Transform passiveSkillParent;

    [Header("액티브 스킬 선택 영역")]
    [SerializeField] private Transform activeSkillParent;

    [Header("장착 스킬 영역")]
    [SerializeField] private Transform selectedSkillParent;
}

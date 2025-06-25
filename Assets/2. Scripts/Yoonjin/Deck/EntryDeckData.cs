using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntryDeckData
{
    // 캐릭터 직업, 액티브 스킬 3개, 패시브 스킬
    // 장비 등등 equipItem
    // 패시브SO
    public PlayerUnitSO characterSO;  

    public SkillData[] skillDatas = new SkillData[3];
    public SkillData passiveSkill;

    // 딕셔너리로 줘!!! 
    public EquipmentItemSO weapon;
    public EquipmentItemSO armor;
    public EquipmentItemSO accessory;
}

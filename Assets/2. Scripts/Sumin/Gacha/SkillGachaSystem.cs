using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGachaSystem : MonoBehaviour
{
    [SerializeField] ActiveSkillTable activeSkillTable;

    private GachaManager<ActiveSkillSO> gachaManager;

    private void Awake()
    {
        gachaManager = new GachaManager<ActiveSkillSO>(new RandoomSkillGachaStrategy());
    }

    public ActiveSkillSO RollSkill()
    {
        List<ActiveSkillSO> skillData = new();

        // 몬스터 스킬은 제외
        for (int i=0; i<(int)JobType.Monster; i++)
        {
            var jobSkills = activeSkillTable.GetActiveSkillsByJob((JobType)i);
            skillData.AddRange(jobSkills);
        }

        return gachaManager.Draw(skillData, Define.TierRates);
    }
}

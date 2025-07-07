using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SkillGachaSystem : MonoBehaviour
{
    [SerializeField] ActiveSkillTable activeSkillTable;

    private GachaManager<ActiveSkillSO> gachaManager;

    private void Awake()
    {
        gachaManager = new GachaManager<ActiveSkillSO>(new RandoomSkillGachaStrategy());
    }

    // 몬스터 스킬 제외
    private List<ActiveSkillSO> GetSkillDatas()
    {
        List<ActiveSkillSO> skills = new();

        for (int i = 0; i < (int)JobType.Monster; i++)
        {
            skills.AddRange(activeSkillTable.GetActiveSkillsByJob((JobType)i));
        }

        return skills;
    }

    public ActiveSkillSO DrawSkill()
    {
        List<ActiveSkillSO> skillData = GetSkillDatas();

        return gachaManager.Draw(skillData, Define.TierRates);
    }

    public List<ActiveSkillSO> DrawSkillMultiple(int count)
    {
        List<ActiveSkillSO> skillData = GetSkillDatas();
        List<ActiveSkillSO> results = new();

        for (int i=0; i<count; i++)
        {
            var skill = gachaManager.Draw(skillData, Define.TierRates);
            if (skill != null)
            {
                results.Add(skill);
            }
            else
            {
                Debug.LogWarning($"{i}번째 뽑기에 실패했습니다.");
            }
        }

        return results;
    }
}

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

    // 스킬 count회 뽑기 
    // 어차피 1뽑, 10뽑만 할거니까 array로
    public ActiveSkillSO[] DrawSkills(int count)
    {
        List<ActiveSkillSO> skillData = GetSkillDatas();
        ActiveSkillSO[] results = new ActiveSkillSO[count];

        for (int i=0; i<count; i++)
        {
            var skill = gachaManager.Draw(skillData, Define.TierRates);
            if (skill != null)
            {
                results[i] = skill;
            }
            else
            {
                Debug.LogWarning($"{i}번째 뽑기에 실패했습니다.");
            }
        }

        return results;
    }
}

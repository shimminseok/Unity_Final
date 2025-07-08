using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 뽑기 결과를 저장하는 구조체
public struct GachaResult<T> where T : ScriptableObject
{
    public T GachaReward;           // 뽑기 보상 스킬 or 장비 or 유닛
    public bool IsDuplicate;        // 이미 보유하고있는지 확인
    public int CompensationAmount;  // 중복 보상
}

public class SkillGachaSystem : MonoBehaviour
{
    [SerializeField] ActiveSkillTable activeSkillTable;

    public bool CanDraw = false;
    public int drawCost = 0;

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
    // 가챠 결과를 구조체로 가지고 UI에게 넘겨줌
    public GachaResult<ActiveSkillSO>[] DrawSkills(int count)
    {
        List<ActiveSkillSO> skillData = GetSkillDatas();
        GachaResult<ActiveSkillSO>[] results = new GachaResult<ActiveSkillSO>[count];

        AccountManager.Instance.UseOpal(drawCost * count); // 오팔 사용

        for (int i=0; i<count; i++)
        {
            ActiveSkillSO skill = gachaManager.Draw(skillData, Define.TierRates); // 하나씩 뽑아서 skill에 저장

            if (skill != null)
            {
                results[i].GachaReward = skill; // 저장한 skill Data는 구조체의 GachaReward에
                
                AccountManager.Instance.AddSkill(skill, out bool isDuplicate); // 중복이 아니라면 스킬 추가
                results[i].IsDuplicate = isDuplicate;
                
                // 중복이면 재화 보상 일부 지급
                if (results[i].IsDuplicate)
                {
                    results[i].CompensationAmount = (int)(drawCost * Define.GetCompensationAmount(skill.activeSkillTier));
                    AccountManager.Instance.AddOpal(results[i].CompensationAmount);
                }
            }
            else
            {
                Debug.LogWarning($"{i}번째 뽑기에 실패했습니다.");
            }
        }

        return results;
    }

    // 오팔 사용 가능 여부 체크.
    // false 되면 UI쪽에서 다른 창 띄우면서 못뽑게 할 것.
    public bool CheckCanDraw(int drawCount)
    {
        drawCost = Define.GachaDrawCosts[GachaType.Skill];
        bool canUse = AccountManager.Instance.CanUseOpal(drawCost * drawCount);

        return canUse;
    }
}

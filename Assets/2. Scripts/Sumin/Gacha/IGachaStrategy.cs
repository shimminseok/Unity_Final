using System.Collections.Generic;
using UnityEngine;

// 가챠 확률 로직 추상화.
public interface IGachaStrategy<T>
{
    T Pull(List<T> candidates, Dictionary<Tier, float> tierRates);
}

// 쌩랜덤 가챠.
// 나중에 픽업이나 천장 등 시스템 구현하게 되면 더 늘릴수도.
public class RandoomSkillGachaStrategy : IGachaStrategy<ActiveSkillSO>
{
    public ActiveSkillSO Pull(List<ActiveSkillSO> candidates, Dictionary<Tier, float> tierRates)
    {
        // 티어별로 데이터 후보 분리
        Dictionary<Tier, List<ActiveSkillSO>> skillTierGroups = new();

        foreach(ActiveSkillSO skill in candidates)
        {
            if (!skillTierGroups.ContainsKey(skill.activeSkillTier))
            {
                skillTierGroups[skill.activeSkillTier] = new List<ActiveSkillSO>();
            }
            skillTierGroups[skill.activeSkillTier].Add(skill);
        }

        float rand = Random.Range(0f, 100f); // 0~100 사이 무작위 뽑기
        float accumulated = 0; // 누적 확률 값
        
        // 티어를 확률에 따라 뽑음
        foreach(var pair in tierRates)
        {
            accumulated += pair.Value; // Dic의 확률을 앞에서부터 계속 더해가면서 누적 확률 값을 지정
            if (rand <= accumulated) // 누적 확률 값이 계속 더해지면서 Random값보다 커지면 해당 티어가 선택됨.
            {
                if(skillTierGroups.TryGetValue(pair.Key, out var group) && group.Count > 0)
                {
                    return group[Random.Range(0, group.Count)]; // 해당 티어 그룹 중에서도 하나를 뽑음
                }

                Debug.LogWarning($"{pair.Key} 티어에 해당되는 스킬이 존재하지 않습니다. rand={rand}, 누적확률={accumulated}");
                break; // 해당 티어가 선택되면 루프 종료
            }
        }

        return null; // 아무 티어도 선택되지 않았을 경우의 예외
    }
}
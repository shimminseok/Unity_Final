using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUnitInfoUI : MonoBehaviour
{
    [SerializeField] private List<MonsterUnitInfoSlotUI> slots;

    private List<Unit> units;
    private void Start()
    {
        StartCoroutine(WaitForBattleManagerInit());
    }

    // 호출 순서 문제 때문에 BattleManager 준비되면 참조
    private IEnumerator WaitForBattleManagerInit()
    {
        yield return new WaitUntil(() => BattleManager.Instance != null && BattleManager.Instance.EnemyUnits.Count > 0);

        units = BattleManager.Instance.EnemyUnits;

        // 유닛 수 만큼 켜주고 정보 업데이트
        for (int i = 0; i < units.Count; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].Initialize(units[i]);
        }
    }
}

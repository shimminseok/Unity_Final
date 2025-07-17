using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitInfoUI : MonoBehaviour
{
    [SerializeField] private List<PlayerUnitInfoSlotUI> slots;
    private void Start()
    {
        StartCoroutine(WaitForBattleManagerInit());
    }

    private IEnumerator WaitForBattleManagerInit()
    {
        yield return new WaitUntil(() => BattleManager.Instance != null && BattleManager.Instance.PartyUnits.Count > 0);

        List<Unit> units = BattleManager.Instance.PartyUnits;

        for (int i = 0; i < units.Count; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].UpdateUnitInfo(units[i]);
            slots[i].UpdateHpBar(units[i] as IDamageable);
        }
    }
}

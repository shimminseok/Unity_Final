using UnityEngine;
using UnityEngine.UI;

public class MonsterUnitInfoSlotUI : MonoBehaviour
{
    [SerializeField] private Image MonsterIcon;
    [SerializeField] private Image combatActionIcon;
    [SerializeField] private Image TargetIcon;
    [SerializeField] private Sprite baseAtkIcon;

    public void Initialize(Unit monsterUnit)
    {
        MonsterIcon.sprite = monsterUnit.UnitSo.UnitIcon;
        if (monsterUnit.SkillController.CurrentSkillData != null)
        {
            combatActionIcon.sprite = monsterUnit.SkillController.CurrentSkillData.skillSo.skillIcon;
        }
        else
        {
            combatActionIcon.sprite = baseAtkIcon;
        }
        IDamageable target = monsterUnit.Target;
        Unit targetUnit = target as Unit;
        TargetIcon.sprite = targetUnit.UnitSo.UnitIcon;
    }
}

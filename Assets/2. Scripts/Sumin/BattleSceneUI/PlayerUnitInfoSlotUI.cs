using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitInfoSlotUI : MonoBehaviour
{
    [SerializeField] private PlayerUnitInfoSlotHpBarUI hpBarUI;
    [SerializeField] private GameObject commandSlot;
    [SerializeField] private Image unitIcon;
    [SerializeField] private Image combatActionIcon;
    [SerializeField] private Image targetIcon;
    [SerializeField] private Sprite baseAtkIcon;

    private IActionCommand command;

    public void UpdateUnitInfo(Unit playerUnit)
    {
        unitIcon.sprite = playerUnit.UnitSo.UnitIcon;
    }

    public void UpdateUnitSelect(Unit playerUnit)
    {
        if (CommandPlanner.Instance.HasPlannedCommand(playerUnit))
        {
            commandSlot.SetActive(true);
            command = CommandPlanner.Instance.GetPlannedCommand(playerUnit);
            if (command.SkillData != null)
            {
                combatActionIcon.sprite = command.SkillData.skillSo.skillIcon;
            }
            else
            {
                combatActionIcon.sprite = baseAtkIcon;
            }
            targetIcon.sprite = command.Target.UnitSo.UnitIcon;
        }
        else
        {
            commandSlot.SetActive(false);
        }
    }

    public void UpdateHpBar(IDamageable owner)
    {
        hpBarUI.Initialize(owner);
    }
}

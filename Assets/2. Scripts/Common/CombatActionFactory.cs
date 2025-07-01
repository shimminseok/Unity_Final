using System;

public static class CombatActionFactory
{
    public static ICombatAction Create(Unit unit)
    {
        if (unit.CurrentAttackAction.DistanceType == AttackDistanceType.Melee)
        {
            return new MeleeCombatAction();
        }

        if (unit.CurrentAttackAction.DistanceType == AttackDistanceType.Range)
        {
            return new RangeCombatAction(unit.CurrentAttackAction.ActionSo as RangeActionSo, unit.Target);
        }

        throw new InvalidOperationException("Invalid Action Type");
    }
}
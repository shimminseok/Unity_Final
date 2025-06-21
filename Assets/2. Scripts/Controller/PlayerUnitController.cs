public class PlayerUnitController : BaseController<PlayerUnitController, PlayerUnitState>
{
    public override StatBase    AttackStat { get; protected set; }
    public override IDamageable Target     { get; protected set; }

    protected override IState<PlayerUnitController, PlayerUnitState> GetState(PlayerUnitState state)
    {
        return null;
    }

    public override void Attack()
    {
        if (Target == null || Target.IsDead)
            return;

        Target.TakeDamage(StatManager.GetValue(StatType.AttackPow));
    }

    public override void TakeDamage(float amount, StatModifierType modifierType = StatModifierType.Base)
    {
        if (IsDead)
            return;

        float finalDam = amount;
        if (modifierType == StatModifierType.Base)
        {
            //방어력 계산.
        }

        var curHp = StatManager.GetStat<ResourceStat>(StatType.CurHp);
        StatManager.Consume(StatType.CurHp, StatModifierType.Base, finalDam);
        if (curHp.Value <= 0)
        {
            Dead();
        }
    }

    public override void Dead()
    {
    }
}
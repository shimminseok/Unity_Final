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
    }

    public override void TakeDamage(IAttackable attacker)
    {
        if (IsDead)
            return;

        float finalDam = attacker.AttackStat.Value;
        var   curHp    = StatManager.GetStat<ResourceStat>(StatType.CurHp);
        StatManager.Consume(StatType.CurHp, StatModifierType.Base, finalDam);
    }

    public override void Dead()
    {
    }
}
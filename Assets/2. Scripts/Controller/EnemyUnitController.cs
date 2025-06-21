using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitController : BaseController<EnemyUnitController, EnemyUnitState>
{
    // Start is called before the first frame update
    public override StatBase    AttackStat { get; protected set; }
    public override IDamageable Target     { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (IsDead)
            return;

        base.Update();
    }

    protected override IState<EnemyUnitController, EnemyUnitState> GetState(EnemyUnitState unitState)
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
        if (curHp.Value <= 0)
        {
            Dead();
        }
    }

    public override void Dead()
    {
    }
}
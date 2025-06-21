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
        StatManager.Consume(StatType.CurHp, modifierType, finalDam);
        if (curHp.Value <= 0)
        {
            Dead();
        }
    }

    public override void Dead()
    {
    }
}
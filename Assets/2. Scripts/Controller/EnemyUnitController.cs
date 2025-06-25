using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitController : BaseController<EnemyUnitController, EnemyUnitState>
{
    [SerializeField] private int id;
    public EnemyUnitSO MonsterSO { get; private set; }
    // Start is called before the first frame update

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

    public override void Initialize()
    {
        MonsterSO = TableManager.Instance.GetTable<MonsterTable>().GetDataByID(id);
        if (MonsterSO == null)
            return;

        MonsterSO.AttackType.Initialize(this);
        StatManager.Initialize(MonsterSO);
    }

    protected override IState<EnemyUnitController, EnemyUnitState> GetState(EnemyUnitState unitState)
    {
        // return unitState switch
        // {
        //     EnemyUnitState.Idle   => new IdleState(),
        //     EnemyUnitState.Move   => new MoveState(),
        //     EnemyUnitState.Attack => new AttackState(weaponController.StatManager.GetValue(StatType.AttackSpd), weaponController.StatManager.GetValue(StatType.AttackRange)),
        //     EnemyUnitState.Die    => new DieState(),
        //     _                     => null
        // };

        return null;
    }

    public override void Attack()
    {
        if (Target == null || Target.IsDead)
            return;

        MonsterSO.AttackType.Attack();
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

    public override void StartTurn()
    {
    }

    public override void EndTurn()
    {
    }
}
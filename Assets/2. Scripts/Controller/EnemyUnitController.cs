using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyState;

public class EnemyUnitController : BaseController<EnemyUnitController, EnemyUnitState>
{
    [SerializeField] private int id;
    public EnemyUnitSO MonsterSO { get; private set; }
    // Start is called before the first frame update

    private HPBarUI hpBar;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    protected override void Start()
    {
        base.Start();
        hpBar = HealthBarManager.Instance.SpawnHealthBar(this);
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

        StatManager.Initialize(MonsterSO);
    }

    protected override IState<EnemyUnitController, EnemyUnitState> GetState(EnemyUnitState unitState)
    {
        return unitState switch
        {
            EnemyUnitState.Idle   => new IdleState(),
            EnemyUnitState.Move   => new MoveState(),
            EnemyUnitState.Attack => new AttackState(),
            EnemyUnitState.Stun   => new StunState(),
            EnemyUnitState.Die    => new DeadState(),

            _ => null
        };
    }

    public override void Attack()
    {
        if (Target == null || Target.IsDead)
        {
            //Test
            var enemies = BattleManager.Instance.GetEnemies(this);
            Target = enemies[Random.Range(0, enemies.Count)];
            return;
        }

        //어택 타입에 따라서 공격 방식을 다르게 적용
        IDamageable finalTarget = Target;


        float hitRate = StatManager.GetValue(StatType.HitRate);
        if (CurrentEmotion is IEmotionOnAttack emotionOnAttack)
            emotionOnAttack.OnBeforeAttack(this, ref finalTarget);

        else if (CurrentEmotion is IEmotionOnHitChance emotionOnHit)
            emotionOnHit.OnCalculateHitChance(ref hitRate);

        bool isHit = Random.value < hitRate;
        if (!isHit)
        {
            Debug.Log("빗나갔지롱");
            return;
        }

        MonsterSO.AttackType.Attack(this);
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
        Debug.Log($"공격 받음 {finalDam} 남은 HP : {curHp.Value}");
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
        if (!IsDead)
            CurrentEmotion.AddStack(this);
    }
}
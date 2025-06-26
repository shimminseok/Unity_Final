using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyState;
using System;
using Random = UnityEngine.Random;

public class EnemyUnitController : BaseController<EnemyUnitController, EnemyUnitState>
{
    [SerializeField] private int id;
    public EnemyUnitSO MonsterSO { get; private set; }
    // Start is called before the first frame update

    private HPBarUI hpBar;

    protected override void Awake()
    {
        base.Awake();
        Initialize(TableManager.Instance.GetTable<MonsterTable>().GetDataByID(id));
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

    public override void ChangeUnitState(Enum newState)
    {
        stateMachine.ChangeState(states[Convert.ToInt32(newState)]);
        CurrentState = (EnemyUnitState)newState;
    }

    public override void Initialize(UnitSO unitSO)
    {
        //TODO : 스폰 하는곳에서 So 생성해서 보내주기
        UnitSo = unitSO;
        if (UnitSo is EnemyUnitSO enemyUnitSo)
        {
            MonsterSO = enemyUnitSo;
        }

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
        // if (Target == null || Target.IsDead)
        // {
        //     //Test
        var enemies = BattleManager.Instance.GetEnemies(this);
        Target = enemies[Random.Range(0, enemies.Count)];
        //     // return;
        // }
        // Target = 

        //어택 타입에 따라서 공격 방식을 다르게 적용
        IDamageable finalTarget = Target;


        float hitRate = StatManager.GetValue(StatType.HitRate);
        if (CurrentEmotion is IEmotionOnAttack emotionOnAttack)
            emotionOnAttack.OnBeforeAttack(this, ref finalTarget);

        else if (CurrentEmotion is IEmotionOnHitChance emotionOnHit)
            emotionOnHit.OnCalculateHitChance(this, ref hitRate);

        bool isHit = Random.value < hitRate;
        if (!isHit)
        {
            Debug.Log("빗나갔지롱");
            return;
        }

        //TODO: 크리티컬 구현
        MonsterSO.AttackType.Attack(this);

        //Test
        EndTurn();
    }

    public override void MoveTo(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime);
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

        var curHp  = StatManager.GetStat<ResourceStat>(StatType.CurHp);
        var shield = StatManager.GetStat<ResourceStat>(StatType.Shield);

        if (shield.CurrentValue > 0)
        {
            float shieldUsed = Mathf.Min(shield.CurrentValue, finalDam);
            StatManager.Consume(StatType.Shield, modifierType, shieldUsed);
            finalDam -= shieldUsed;
        }

        if (finalDam > 0)
            StatManager.Consume(StatType.CurHp, modifierType, finalDam);
        Debug.Log($"공격 받음 {finalDam} 남은 HP : {curHp.Value}");
        if (curHp.Value <= 0)
        {
            Dead();
        }
    }

    public override void Dead()
    {
        IsDead = true;
    }

    public override void StartTurn()
    {
        if (IsDead || IsStunned)
        {
            BattleManager.Instance.TurnHandler.OnUnitTurnEnd();
            return;
        }

        Attack();
    }

    public override void EndTurn()
    {
        if (!IsDead)
            CurrentEmotion.AddStack(this);

        Debug.Log($"Turn 종료 현재 스택 {CurrentEmotion.Stack}");
        BattleManager.Instance.TurnHandler.OnUnitTurnEnd();
        Debug.Log("EndTurn");
    }
}
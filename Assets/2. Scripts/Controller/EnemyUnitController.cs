 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerState;
using System;
using IdleState = EnemyState.IdleState;
using MoveState = EnemyState.MoveState;
using Random = UnityEngine.Random;
using StunState = EnemyState.StunState;

[RequireComponent(typeof(EnemySkillContorller))]
public class EnemyUnitController : BaseController<EnemyUnitController, EnemyUnitState>
{
    [SerializeField] private int id;
    public EnemyUnitSO MonsterSo { get; private set; }
    // Start is called before the first frame update

    private HPBarUI hpBar;
    public override bool IsAtTargetPosition => Agent.remainingDistance < setRemainDistance;
    public float setRemainDistance;

    public override bool IsAnimationDone
    {
        get
        {
            var info = Animator.GetCurrentAnimatorStateInfo(0);
            return info.IsTag("Action") && info.normalizedTime >= 0.9f;
        }
    }

    private float remainDistance;
    public Vector3 StartPostion { get; private set; }

    protected override void Awake()
    {
        SkillController = GetComponent<EnemySkillContorller>();
        base.Awake();
    }

    protected override void Start()
    {
        hpBar = HealthBarManager.Instance.SpawnHealthBar(this);
        StartPostion = transform.position;
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
        UnitSo = unitSO;
        if (UnitSo is EnemyUnitSO enemyUnitSo)
        {
            MonsterSo = enemyUnitSo;
        }

        if (MonsterSo == null)
            return;

        StatManager.Initialize(MonsterSo);
        AnimatorOverrideController = new AnimatorOverrideController(Animator.runtimeAnimatorController);
        ChangeClip(Define.IdleClipName, MonsterSo.IdleAniClip);
        ChangeClip(Define.MoveClipName, MonsterSo.MoveAniClip);
        ChangeClip(Define.AttackClipName, MonsterSo.AttackAniClip);
        ChangeClip(Define.DeadClipName, MonsterSo.DeadAniClip);
        foreach (var skillData in MonsterSo.SkillDatas)
        {
            SkillManager.AddActiveSkill(skillData.skillSO);
        }
        SkillManager.InitializeSkillManager(this);
        EnemySkillContorller sc = SkillController as EnemySkillContorller;
        if (sc != null)
        {
            sc.InitSkillSelector();       
        }
    }

    protected override IState<EnemyUnitController, EnemyUnitState> GetState(EnemyUnitState unitState)
    {
        return unitState switch
        {
            EnemyUnitState.Idle   => new IdleState(),
            EnemyUnitState.Move   => new MoveState(),
            EnemyUnitState.Return => new EnemyState.ReturnState(),
            EnemyUnitState.Attack => new EnemyState.AttackState(),
            EnemyUnitState.Skill => new EnemyState.SkillState(),
            EnemyUnitState.Stun   => new StunState(),
            EnemyUnitState.Die    => new EnemyState.DeadState(),

            _ => null
        };
    }

    public override void Attack()
    {
        var enemies = BattleManager.Instance.GetEnemies(this);
        Target = enemies[Random.Range(0, enemies.Count)];


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
        MonsterSo.AttackType.Execute(this);
    }

    public override void MoveTo(Vector3 destination)
    {
        Agent.SetDestination(destination);
    }

    public override void UseSkill()
    {
        SkillController.UseSkill();
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
        ChangeUnitState(EnemyUnitState.Die);
        StatusEffectManager.RemoveAllEffects();
        hpBar.UnLink();
        gameObject.SetActive(false);
    }

    public bool ShouldUseSkill()
    {
        if(SkillController.CheckAllSkills() && Random.value < MonsterSo.skillActionProbability) return true;
        else return false;
    }

    public override void StartTurn()
    {
        if (IsDead || IsStunned)
        {
            BattleManager.Instance.TurnHandler.OnUnitTurnEnd();
            return;
        }

        if (ShouldUseSkill())
        {
            EnemySkillContorller sc = SkillController as EnemySkillContorller;
            if (sc != null)
            {
                sc.SelectSkill();
                ChangeAction(ActionType.SKill);           
            }
        }
        else
        {
            ChangeAction(ActionType.Attack);
        }
        var enemies = BattleManager.Instance.GetEnemies(this);
        SetTarget(enemies[Random.Range(0, enemies.Count)]);
        TurnStateMachine.ChangeState(new StartTurnState());
    }

    public override void EndTurn()
    {
        if (!IsDead)
            CurrentEmotion.AddStack(this);

        ChangeAction(ActionType.None);
        BattleManager.Instance.TurnHandler.OnUnitTurnEnd();
        ChangeUnitState(PlayerUnitState.Idle);
    }
}
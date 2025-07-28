using UnityEngine;
using PlayerState;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public enum ActionType
{
    None,
    Attack,
    SKill
}

[RequireComponent(typeof(PlayerSkillController))] public class PlayerUnitController : BaseController<PlayerUnitController, PlayerUnitState>
{
    [SerializeField]
    private int id;

    [SerializeField]
    private AnimationClip idleClip;

    [SerializeField]
    private AnimationClip moveClip;

    [SerializeField]
    private AnimationClip victoryClip;

    [SerializeField]
    private AnimationClip readyActionClip;

    [SerializeField]
    private AnimationClip deadClip;

    [SerializeField]
    private AnimationClip hitClip;

    public EquipmentManager EquipmentManager { get; private set; }
    public Vector3          StartPostion     { get; private set; }
    public PlayerUnitSO     PlayerUnitSo     { get; private set; }
    public PassiveSO        PassiveSo        { get; private set; }

    // public override bool IsAnimationDone
    // {
    //     get
    //     {
    //         AnimatorStateInfo info = Animator.GetCurrentAnimatorStateInfo(0);
    //         return info.IsTag("Action") && info.normalizedTime >= 0.9f;
    //     }
    // }

    public override bool IsTimeLinePlaying => TimeLineManager.Instance.isPlaying;

    public override bool IsAtTargetPosition => Agent.remainingDistance < setRemainDistance;

    public float setRemainDistance;

    private HPBarUI hpBar;


    private float remainDistance;

    protected override IState<PlayerUnitController, PlayerUnitState> GetState(PlayerUnitState state)
    {
        return state switch
        {
            PlayerUnitState.Idle        => new IdleState(),
            PlayerUnitState.Move        => new MoveState(),
            PlayerUnitState.Return      => new PlayerState.ReturnState(),
            PlayerUnitState.Attack      => new AttackState(),
            PlayerUnitState.Die         => new DeadState(),
            PlayerUnitState.Skill       => new SkillState(),
            PlayerUnitState.Victory     => new VictoryState(),
            PlayerUnitState.ReadyAction => new ReadyAction(),
            PlayerUnitState.Hit         => new HitState(),
            _                           => null
        };
    }

    protected override void Awake()
    {
        SkillController = GetComponent<PlayerSkillController>();
        base.Awake();
        EquipmentManager = new EquipmentManager(this);
    }

    protected override void Start()
    {
        base.Start();
        hpBar = HealthBarManager.Instance.SpawnHealthBar(this);

        StartPostion = transform.position;
    }

    public override void ChangeUnitState(Enum newState)
    {
        stateMachine.ChangeState(states[Convert.ToInt32(newState)]);
        CurrentState = (PlayerUnitState)newState;
    }

    public override void Initialize(UnitSpawnData deckData)
    {
        UnitSo = deckData.UnitSo;

        if (UnitSo is PlayerUnitSO playerUnitSo)
        {
            PlayerUnitSo = playerUnitSo;
        }

        if (PlayerUnitSo == null)
        {
            return;
        }

        PassiveSo = PlayerUnitSo.PassiveSkill;
        PassiveSo.Initialize(this);
        if (PlayerDeckContainer.Instance.SelectedStage == null)
        {
            StatManager.Initialize(PlayerUnitSo);
        }
        else
        {
            StatManager.Initialize(PlayerUnitSo, this, deckData.DeckData.Level, PlayerDeckContainer.Instance.SelectedStage.MonsterIncrease);
        }

        foreach (SkillData skillData in deckData.DeckData.SkillDatas)
        {
            if (skillData == null)
            {
                SkillManager.AddActiveSkill(null);
                continue;
            }

            SkillManager.AddActiveSkill(skillData.skillSo);
        }

        foreach (EquipmentItem deckDataEquippedItem in deckData.DeckData.EquippedItems.Values)
        {
            EquipmentManager.EquipItem(deckDataEquippedItem);
        }

        SkillManager.InitializeSkillManager(this);
        AnimationEventListener.Initialize(this);
        AnimatorOverrideController = new AnimatorOverrideController(Animator.runtimeAnimatorController);
        ChangeClip(Define.AttackClipName, UnitSo.AttackAniClip);
        ChangeClip(Define.IdleClipName, idleClip);
        ChangeClip(Define.MoveClipName, moveClip);
        ChangeClip(Define.VictoryClipName, victoryClip);
        ChangeClip(Define.ReadyActionClipName, readyActionClip);
        ChangeClip(Define.DeadClipName, deadClip);
        ChangeClip(Define.HitClipName, hitClip);

        CurrentAttackAction = UnitSo.AttackType;
    }


    public override void Attack()
    {
        //어택 타입에 따라서 공격 방식을 다르게 적용
        IsCompletedAttack = false;
        IDamageable finalTarget = IsCounterAttack ? CounterTarget : Target;

        if (finalTarget == null || finalTarget.IsDead)
        {
            return;
        }

        float hitRate = StatManager.GetValue(StatType.HitRate);
        if (CurrentEmotion is IEmotionOnAttack emotionOnAttack)
        {
            emotionOnAttack.OnBeforeAttack(this, ref finalTarget);
        }

        else if (CurrentEmotion is IEmotionOnHitChance emotionOnHit)
        {
            emotionOnHit.OnCalculateHitChance(this, ref hitRate);
        }

        bool isHit = Random.value < hitRate;
        if (!isHit)
        {
            DamageFontManager.Instance.SetDamageNumber(this, 0, DamageType.Miss);
            if (CurrentAttackAction.DistanceType == AttackDistanceType.Melee)
            {
                OnMeleeAttackFinished += InvokeHitFinished;
            }
            else
            {
                OnRangeAttackFinished += InvokeHitFinished;
                InvokeRangeAttackFinished();
            }

            return;
        }

        PlayerUnitSo.AttackType.Execute(this, finalTarget);
        IsCompletedAttack = true;
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
        {
            return;
        }

        if (CurrentEmotion is IEmotionOnTakeDamage emotionOnTakeDamage)
        {
            emotionOnTakeDamage.OnBeforeTakeDamage(this, out bool isIgnore);
            if (isIgnore)
            {
                return;
            }
        }

        float finalDam = amount;

        //이게 반격 스킬에 대한거임.
        StatusEffectManager?.TryTriggerAll(TriggerEventType.OnAttacked);

        float damageReduction = 0;
        if (modifierType == StatModifierType.Base)
        {
            float defense = StatManager.GetValue(StatType.Defense);
            damageReduction = defense / (defense + Define.DefenseReductionBase);
        }

        finalDam *= 1f - damageReduction;
        ResourceStat curHp  = StatManager.GetStat<ResourceStat>(StatType.CurHp);
        ResourceStat shield = StatManager.GetStat<ResourceStat>(StatType.Shield);

        if (shield.CurrentValue > 0)
        {
            float shieldUsed = Mathf.Min(shield.CurrentValue, finalDam);
            StatManager.Consume(StatType.Shield, modifierType, shieldUsed);
            DamageFontManager.Instance.SetDamageNumber(this, shieldUsed, DamageType.Shield);
            finalDam -= shieldUsed;
        }

        if (finalDam > 0)
        {
            DamageFontManager.Instance.SetDamageNumber(this, finalDam, DamageType.Normal);
            StatManager.Consume(StatType.CurHp, modifierType, finalDam);
        }

        if (curHp.Value <= 0)
        {
            Dead();
            return;
        }

        ChangeUnitState(PlayerUnitState.Hit);
    }

    public override void Dead()
    {
        if (IsDead)
        {
            return;
        }

        IsDead = true;
        if (LastAttacker != null)
        {
            if (LastAttacker.CurrentAttackAction.DistanceType == AttackDistanceType.Melee)
            {
                LastAttacker.OnMeleeAttackFinished += InvokeHitFinished;
            }
            else
            {
                LastAttacker.OnRangeAttackFinished += InvokeHitFinished;
            }
        }

        ChangeUnitState(PlayerUnitState.Die);


        //아군이 죽으면 발동되는 패시브를 가진 유닛이 있으면 가져와서 발동 시켜줌
        List<IPassiveAllyDeathTrigger> allyDeathPassives = BattleManager.Instance.GetAllies(this)
            .Select(u => (u as PlayerUnitController)?.PassiveSo)
            .OfType<IPassiveAllyDeathTrigger>()
            .ToList();

        foreach (IPassiveAllyDeathTrigger unit in allyDeathPassives)
        {
            unit.OnAllyDead();
        }
    }

    public override void StartTurn()
    {
        if (IsDead || IsStunned || CurrentAction == ActionType.None || Target == null || Target.IsDead)
        {
            if (CurrentAction == ActionType.None || Target == null)
            {
                EndTurn();
                return;
            }
            else
            {
                BattleManager.Instance.TurnHandler.OnUnitTurnEnd();
                return;
            }
        }

        if (PassiveSo is IPassiveTurnStartTrigger turnStartTrigger)
        {
            turnStartTrigger.OnTurnStart(this);
        }

        ChangeTurnState(TurnStateType.StartTurn);
    }


    public override void EndTurn()
    {
        //내 턴이 끝날때의 로직을 쓸꺼임.
        if (PassiveSo is IPassiveEmotionStackTrigger stackPassive)
        {
            stackPassive.OnEmotionStackIncreased(CurrentEmotion);
        }

        if (!IsDead)
        {
            CurrentEmotion.AddStack(this);
        }

        Target = null;
        ChangeAction(ActionType.None);
        ChangeUnitState(PlayerUnitState.ReadyAction);
        SkillController.EndTurn();
        // TimeLineManager.Instance.StopTimeLine(TimeLineManager.Instance.director);
        BattleManager.Instance.TurnHandler.OnUnitTurnEnd();
    }
}
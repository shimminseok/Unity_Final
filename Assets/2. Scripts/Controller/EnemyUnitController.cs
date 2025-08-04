using DissolveExample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EnemyState;
using System.Text;
using Random = UnityEngine.Random;


[RequireComponent(typeof(EnemySkillContorller))] public class EnemyUnitController : BaseController<EnemyUnitController, EnemyUnitState>
{
    [SerializeField]
    private int id;

    public EnemyUnitSO MonsterSo { get; private set; }
    // Start is called before the first frame update

    private DissolveChilds dissolveChilds;
    private HPBarUI hpBar;
    public override bool IsAtTargetPosition => Agent.remainingDistance < setRemainDistance;
    public float setRemainDistance;

    public override bool IsTimeLinePlaying => TimeLineManager.Instance.isPlaying;

    private float remainDistance;
    public Vector3 StartPostion { get; private set; }
    public WeightedSelector<Unit> mainTargetSelector;

    public override event Action OnDead;

    protected override void Awake()
    {
        SkillController = GetComponent<EnemySkillContorller>();
        base.Awake();
        dissolveChilds = GetComponentInChildren<DissolveChilds>();
    }

    protected override void Start()
    {
        hpBar = HealthBarManager.Instance.SpawnHealthBar(this);
        StartPostion = transform.position;

        Agent.speed = 15f;
        Agent.acceleration = 100f;
        Agent.angularSpeed = 1000f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (IsDead)
        {
            return;
        }

        base.Update();
    }


    public override void ChangeUnitState(Enum newState)
    {
        stateMachine.ChangeState(states[Convert.ToInt32(newState)]);
        CurrentState = (EnemyUnitState)newState;
    }

    public override void Initialize(UnitSpawnData spawnData)
    {
        UnitSo = spawnData.UnitSo;
        if (UnitSo is EnemyUnitSO enemyUnitSo)
        {
            MonsterSo = enemyUnitSo;
        }

        if (MonsterSo == null)
        {
            return;
        }


        StatManager.Initialize(MonsterSo, this, PlayerDeckContainer.Instance.SelectedStage.MonsterLevel, PlayerDeckContainer.Instance.SelectedStage.MonsterIncrease);

        AnimatorOverrideController = new AnimatorOverrideController(Animator.runtimeAnimatorController);
        AnimationEventListener.Initialize(this);
        ChangeClip(Define.IdleClipName, MonsterSo.IdleAniClip);
        ChangeClip(Define.MoveClipName, MonsterSo.MoveAniClip);
        ChangeClip(Define.AttackClipName, MonsterSo.AttackAniClip);
        ChangeClip(Define.DeadClipName, MonsterSo.DeadAniClip);
        ChangeClip(Define.HitClipName, MonsterSo.HitAniClip);
        foreach (EnemySkillData skillData in MonsterSo.SkillDatas)
        {
            SkillManager.AddActiveSkill(skillData.skillSO);
        }

        SkillManager.InitializeSkillManager(this);
        EnemySkillContorller sc = SkillController as EnemySkillContorller;
        if (sc != null)
        {
            sc.InitSkillSelector();
        }

        InitTargetSelector();
        ChangeEmotion(MonsterSo.StartEmotion);
        CurrentAttackAction = UnitSo.AttackType;
    }

    protected override IState<EnemyUnitController, EnemyUnitState> GetState(EnemyUnitState unitState)
    {
        return unitState switch
        {
            EnemyUnitState.Idle   => new IdleState(),
            EnemyUnitState.Move   => new MoveState(),
            EnemyUnitState.Return => new EnemyState.ReturnState(),
            EnemyUnitState.Attack => new EnemyState.AttackState(),
            EnemyUnitState.Skill  => new SkillState(),
            EnemyUnitState.Stun   => new StunState(),
            EnemyUnitState.Die    => new EnemyState.DeadState(),
            EnemyUnitState.Hit    => new HitState(),

            _ => null
        };
    }

    public override void PlayHitVoiceSound()
    {
        EnemyUnitSO so = UnitSo as EnemyUnitSO;
        if (so.HitVoiceSound != SFXName.None)
        {
            AudioManager.Instance.PlaySFX(so.HitVoiceSound.ToString());
        }
        else
        {
            MonsterType monsterType = so.monsterType;
            if (monsterType == MonsterType.None)
            {
                return;
            }

            StringBuilder sb = new();
            sb.Append(monsterType.ToString());
            sb.Append("HitSound");
            AudioManager.Instance.PlaySFX(sb.ToString());
        }
    }

    public override void PlayDeadSound()
    {
        EnemyUnitSO so = UnitSo as EnemyUnitSO;
        if (so.DeadSound != SFXName.None)
        {
            AudioManager.Instance.PlaySFX(so.DeadSound.ToString());
        }
        else
        {
            MonsterType monsterType = so.monsterType;
            if (monsterType == MonsterType.None)
            {
                return;
            }

            StringBuilder sb = new();
            sb.Append(monsterType.ToString());
            sb.Append("DeadSound");
            AudioManager.Instance.PlaySFX(sb.ToString());
        }
    }

    public override void PlayAttackVoiceSound()
    {
        EnemyUnitSO so = UnitSo as EnemyUnitSO;
        if (so.AttackVoiceSound != SFXName.None)
        {
            AudioManager.Instance.PlaySFX(so.AttackVoiceSound.ToString());
        }
        else
        {
            MonsterType monsterType = so.monsterType;
            if (monsterType == MonsterType.None)
            {
                return;
            }

            StringBuilder sb = new();
            sb.Append(monsterType.ToString());
            sb.Append("AttackSound");
            AudioManager.Instance.PlaySFX(sb.ToString());
        }
    }

    public override void Attack()
    {
        IsCompletedAttack = false;
        //어택 타입에 따라서 공격 방식을 다르게 적용
        IDamageable finalTarget = IsCounterAttack ? CounterTarget : Target;

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
                InvokeRangeAttackFinished();
            }

            return;
        }

        //TODO: 크리티컬 구현
        finalTarget.SetLastAttacker(this);
        MonsterSo.AttackType.Execute(this, finalTarget);
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

                DamageFontManager.Instance.SetDamageNumber(this, 0, DamageType.Immune);
                return;
            }
        }

        float finalDam = amount;

        //방어력 적용
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

        ChangeUnitState(EnemyUnitState.Hit);
    }

    public override void Dead()
    {
        IsDead = true;
        OnDead?.Invoke();
        ChangeUnitState(EnemyUnitState.Die);
        StatusEffectManager.RemoveAllEffects();
        hpBar.UnLink();

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

        Agent.enabled = false;
        Obstacle.carving = false;
        Obstacle.enabled = false;
        dissolveChilds.PlayDissolve(Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }

    public bool ShouldUseSkill()
    {
        if (SkillController.CheckAllSkills() && Random.value < MonsterSo.skillActionProbability)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void InitTargetSelector()
    {
        mainTargetSelector = new WeightedSelector<Unit>();
        List<Unit> playerUnits = BattleManager.Instance.PartyUnits;
        foreach (Unit playerUnit in playerUnits)
        {
            mainTargetSelector.Add(
                playerUnit,
                () => playerUnit.StatManager.GetValue(StatType.Aggro),
                () => !playerUnit.IsDead
            );
        }
    }

    public void WeightedMainTargetSelector(SelectCampType campType)
    {
        if (campType == SelectCampType.Enemy)
        {
            SetTarget(mainTargetSelector.Select());
        }
        else if (campType == SelectCampType.Player)
        {
            List<Unit> allies = BattleManager.Instance.GetAllies(this);
            if (allies.Count > 0)
            {
                SetTarget(allies[Random.Range(0, allies.Count)]);
            }
        }
    }

    public void SelectMainTarget(ActionType actionType)
    {
        if (actionType == ActionType.SKill)
        {
            EnemySkillContorller sc = SkillController as EnemySkillContorller;
            WeightedMainTargetSelector(sc.CurrentSkillData.skillSo.selectCamp);
        }
        else if (actionType == ActionType.Attack)
        {
            WeightedMainTargetSelector(SelectCampType.Enemy);
        }
    }

    public void ChoiceAction()
    {
        if (IsDead)
        {
            return;
        }

        if (ShouldUseSkill())
        {
            EnemySkillContorller sc = SkillController as EnemySkillContorller;
            if (sc != null)
            {
                sc.WeightedSelectSkill();
                ChangeAction(ActionType.SKill);
                SelectMainTarget(ActionType.SKill);
            }
        }
        else
        {
            ChangeAction(ActionType.Attack);
            SelectMainTarget(ActionType.Attack);
        }
    }


    public override void StartTurn()
    {
        if (IsDead || IsStunned || Target == null || Target.IsDead)
        {
            BattleManager.Instance.TurnHandler.OnUnitTurnEnd();
            return;
        }

        ChangeTurnState(TurnStateType.StartTurn);
    }

    public override void EndTurn()
    {
        ChangeAction(ActionType.None);
        BattleManager.Instance.TurnHandler.OnUnitTurnEnd();
        ChangeUnitState(EnemyUnitState.Idle);
    }
}
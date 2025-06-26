using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using PlayerState;
using System.Linq;
using UnityEngine.Serialization;


public enum PlayerActionType
{
    None,
    Attack,
    SKill
}

[RequireComponent(typeof(PlayerSkillController))]
public class PlayerUnitController : BaseController<PlayerUnitController, PlayerUnitState>
{
    [SerializeField] private int id;
    public Animator animator;
    public PassiveSO passiveSo;
    public EquipmentManager      EquipmentManager      { get; private set; }
    public PlayerActionType      CurrentAction         { get; private set; } = PlayerActionType.None;
    public PlayerSkillController PlayerSkillController { get; private set; }
    private HPBarUI hpBar;
    public PlayerUnitSO PlayerUnitSo { get; private set; }


    protected override IState<PlayerUnitController, PlayerUnitState> GetState(PlayerUnitState state)
    {
        return state switch
        {
            PlayerUnitState.Idle    => new IdleState(),
            PlayerUnitState.Attack  => new AttackState(),
            PlayerUnitState.Die     => new DeadState(),
            PlayerUnitState.EndTurn => new EndTurnState(),
            PlayerUnitState.Skill   => new SkillState(),
            _                       => null
        };
    }

    protected override void Awake()
    {
        base.Awake();
        EquipmentManager = new EquipmentManager(this);
        Initialize();
    }

    protected override void Start()
    {
        base.Start();
        hpBar = HealthBarManager.Instance.SpawnHealthBar(this);
    }

    public override void Initialize()
    {
        PlayerUnitSo = TableManager.Instance.GetTable<PlayerUnitTable>().GetDataByID(id);
        if (PlayerUnitSo == null)
            return;

        passiveSo.Initialize(this);
        StatManager.Initialize(PlayerUnitSo);

        PlayerSkillController = GetComponent<PlayerSkillController>();
    }

    public override void Attack()
    {
        if (Target == null || Target.IsDead)
            return;


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

        Target = finalTarget;
        PlayerUnitSo.AttackType.Attack(this);

        //Test
        EndTurn();
    }

    public void SetTarget(IDamageable target)
    {
        Target = target;
    }

    public void UseSkill()
    {
        //이펙트 생성
        //
        PlayerSkillController.UseSkill();

        EndTurn();
    }

    private AnimatorOverrideController ChangeClip()
    {
        // AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        // for (int i = 0; i < m_Clips.Count; i++)
        // {
        //     overrideController[m_Clips[i].name] = m_Clips[i];
        // }
        //
        // return overrideController;

        return null;
    }

    public override void TakeDamage(float amount, StatModifierType modifierType = StatModifierType.Base)
    {
        if (IsDead)
            return;

        if (CurrentEmotion is IEmotionOnTakeDamage emotionOnTakeDamage)
        {
            emotionOnTakeDamage.OnBeforeTakeDamage(this, out bool isIgnore);
            if (isIgnore)
                return;
        }

        float finalDam = amount;

        //이게 반격 스킬에 대한거임.
        StatusEffectManager?.TryTriggerAll(TriggerEventType.OnAttacked);


        if (StatManager.GetValue(StatType.Counter) >= Random.value) //반격 로직
        {
            //반격을 한다.
            return;
        }

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
        if (IsDead)
            return;

        IsDead = true;


        //아군이 죽으면 발동되는 패시브를 가진 유닛이 있으면 가져와서 발동 시켜줌
        var allyDeathPassives = BattleManager.Instance.GetAllies(this)
            .Select(u => (u as PlayerUnitController)?.passiveSo)
            .OfType<IPassiveAllyDeathTrigger>()
            .ToList();

        foreach (var unit in allyDeathPassives)
        {
            unit.OnAllyDead();
        }
    }

    public override void StartTurn()
    {
        if (IsDead || IsStunned || CurrentAction == PlayerActionType.None)
        {
            BattleManager.Instance.TurnHandler.OnUnitTurnEnd();
            return;
        }

        if (passiveSo is IPassiveTurnStartTrigger turnStartTrigger)
        {
            turnStartTrigger.OnTurnStart(this);
        }

        if (CurrentAction == PlayerActionType.Attack)
            Attack();
        else if (CurrentAction == PlayerActionType.SKill)
            UseSkill();
    }


    public override void EndTurn()
    {
        //내 턴이 끝날때의 로직을 쓸꺼임.
        if (passiveSo is IPassiveEmotionStackTrigger stackPassive)
        {
            stackPassive.OnEmotionStackIncreased(CurrentEmotion);
        }

        if (!IsDead)
            CurrentEmotion.AddStack(this);

        ChangeAction(PlayerActionType.None);
        BattleManager.Instance.TurnHandler.OnUnitTurnEnd();
    }

    public void ChangeAction(PlayerActionType action)
    {
        CurrentAction = action;
    }
}
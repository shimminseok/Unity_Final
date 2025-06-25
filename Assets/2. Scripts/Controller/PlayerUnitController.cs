using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using PlayerState;
using UnityEngine.Serialization;

public class PlayerUnitController : BaseController<PlayerUnitController, PlayerUnitState>
{
    [SerializeField] private int id;
    public Animator animator;
    public PassiveSO passiveSo;
    public EquipmentManager EquipmentManager { get; private set; }


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

        PlayerUnitSo.AttackType.Initialize(this);
        passiveSo.Initialize(this);
        StatManager.Initialize(PlayerUnitSo);
    }

    public override void Attack()
    {
        // if (Target == null || Target.IsDead)
        //     return;

        //어택 타입에 따라서 공격 방식을 다르게 적용
        IDamageable finalTarget = Target;


        float hitRate = StatManager.GetValue(StatType.HitRate);
        if (CurrentEmotion is IEmotionOnAttack emotionOnAttack)
            emotionOnAttack.OnBeforeAttack(ref finalTarget);

        else if (CurrentEmotion is IEmotionOnHitChance emotionOnHit)
            emotionOnHit.OnCalculateHitChance(ref hitRate);

        bool isHit = Random.value < hitRate;
        if (!isHit)
        {
            Debug.Log("빗나갔지롱");
            return;
        }

        PlayerUnitSo.AttackType.Attack();
    }

    public void UseSkill()
    {
        //이펙트 생성
        //
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


        float finalDam = amount;

        //이게 반격 스킬에 대한거임.
        StatusEffectManager?.TryTriggerAll(TriggerEventType.OnAttacked);


        if (StatManager.GetValue(StatType.Counter) < Random.value) //반격 로직
        {
            //반격을 한다.
            return;
        }

        if (modifierType == StatModifierType.Base)
        {
            //방어력 계산.
        }

        var curHp = StatManager.GetStat<ResourceStat>(StatType.CurHp);
        StatManager.Consume(StatType.CurHp, StatModifierType.Base, finalDam);
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
        if (IsDead || IsStunned)
        {
            BattleManager.Instance.TurnHandler.OnUnitTurnEnd();
        }

        if (passiveSo is ITurnStartTrigger turnStartTrigger)
        {
            turnStartTrigger.OnTurnStart(this);
        }

        //선택한 행동에 따라서 실행되는 메서드를 구분
        // 기본공격이면
        Attack();
        //스킬이면
        // UseSkill();
    }


    public override void EndTurn()
    {
        //내 턴이 끝날때의 로직을 쓸꺼임.
        if (passiveSo is IEmotionStackApplier stackPassive)
        {
            stackPassive.ApplyStack(CurrentEmotion);
        }

        if (!IsDead)
            CurrentEmotion.AddStack();

        Debug.Log($"Turn 종료 현재 스택 {CurrentEmotion.Stack}");
    }
}
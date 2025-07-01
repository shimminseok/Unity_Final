using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class Unit : MonoBehaviour, IDamageable, IAttackable, ISelectable, IUnitFsmControllable
{
    private const float ResistancePerStack = 0.08f;

    [SerializeField] protected BattleSceneUnitIndicator unitIndicator;

    public BaseEmotion CurrentEmotion { get; protected set; }
    public bool        IsStunned      { get; private set; }

    protected BattleManager              BattleManager              => BattleManager.Instance;
    public    BaseEmotion[]              Emotions                   { get; private set; }
    public    ActionType                 CurrentAction              { get; private set; } = ActionType.None;
    public    TurnStateMachine           TurnStateMachine           { get; protected set; }
    public    ITurnState[]               TurnStates                 { get; private set; }
    public    TurnStateType              CurrentTurnState           { get; private set; }
    public    StatManager                StatManager                { get; protected set; }
    public    StatusEffectManager        StatusEffectManager        { get; protected set; }
    public    StatBase                   AttackStat                 { get; protected set; }
    public    SkillManager               SkillManager               { get; protected set; }
    public    Animator                   Animator                   { get; protected set; }
    public    BaseSkillController        SkillController            { get; protected set; }
    public    AnimatorOverrideController AnimatorOverrideController { get; protected set; }
    public    Collider                   Collider                   { get; protected set; }
    public    NavMeshAgent               Agent                      { get; protected set; }
    public    UnitSO                     UnitSo                     { get; protected set; }
    public    IDamageable                Target                     { get; protected set; }
    public    IAttackAction              CurrentAttackAction        { get; private set; }
    public    bool                       IsDead                     { get; protected set; }
    public    AnimationEventListener     AnimationEventListener     { get; protected set; }

    public virtual bool IsAtTargetPosition => false;
    public virtual bool IsAnimationDone    => false;
    public         Unit SelectedUnit       => this;

    public abstract void StartTurn();
    public abstract void EndTurn();
    public abstract void UseSkill();
    public abstract void TakeDamage(float amount, StatModifierType modifierType = StatModifierType.Base);

    public abstract void Dead();


    protected void Start()
    {
    }

    public void SetStunned(bool isStunned)
    {
        IsStunned = isStunned;
    }

    protected void CreateEmotion()
    {
        Emotions = new BaseEmotion[Enum.GetValues(typeof(EmotionType)).Length];
        for (int i = 0; i < Emotions.Length; i++)
        {
            Emotions[i] = EmotionFactory.CreateEmotion((EmotionType)i);
        }

        CurrentEmotion = Emotions[(int)EmotionType.Neutral];
    }

    protected void CreateTurnStates()
    {
        TurnStates = new ITurnState[Enum.GetValues(typeof(TurnStateType)).Length];
        for (int i = 0; i < TurnStates.Length; i++)
        {
            TurnStates[i] = CreateTurnState((TurnStateType)i);
        }

        TurnStateMachine.Initialize(this, TurnStates[(int)TurnStateType.StartTurn]);
    }

    public void ChangeTurnState(TurnStateType state)
    {
        TurnStateMachine.ChangeState(TurnStates[(int)state]);
        CurrentTurnState = state;
    }

    private ITurnState CreateTurnState(TurnStateType state)
    {
        return state switch
        {
            TurnStateType.StartTurn    => new StartTurnState(),
            TurnStateType.MoveToTarget => new MoveToTargetState(),
            TurnStateType.Return       => new ReturnState(),
            TurnStateType.Act          => new ActState(),
            TurnStateType.EndTurn      => new EndTurnState(),
            _                          => null
        };
    }

    public void ChangeEmotion(EmotionType newType)
    {
        if (CurrentEmotion.EmotionType != newType)
        {
            if (Random.value < CurrentEmotion.Stack * ResistancePerStack)
                return;

            CurrentEmotion.Exit(this);
            CurrentEmotion = Emotions[(int)newType];
            CurrentEmotion.Enter(this);

            if (this is PlayerUnitController playerUnit && playerUnit.passiveSo is IPassiveChangeEmotionTrigger passiveChangeEmotion)
            {
                passiveChangeEmotion.OnChangeEmotion();
            }
        }
        else
        {
            CurrentEmotion.AddStack(this);
        }
    }


    public abstract void Attack();
    public abstract void MoveTo(Vector3 destination);

    public void SetTarget(Unit target)
    {
        Target = target;
        if (CurrentAction == ActionType.SKill)
        {
            SkillController.SelectTargets(target);
        }
    }

    // 유닛 선택 가능 토글
    public void ToggleSelectableIndicator(bool toggle)
    {
        if (unitIndicator == null)
            Debug.LogError("유닛에 unitIndicator을 추가해주세요.");

        unitIndicator.ToggleSelectableIndicator(toggle);
    }

    // 유닛 선택됨 표시 토글
    public void ToggleSelectedIndicator(bool toggle)
    {
        if (unitIndicator == null)
            Debug.LogError("유닛에 unitIndicator을 추가해주세요.");

        unitIndicator.ToggleSelectedIndicator(toggle);
    }

    // 유닛 선택 파티클 재생
    public void PlaySelectEffect()
    {
        if (unitIndicator == null)
            Debug.LogError("유닛에 unitIndicator을 추가해주세요.");

        unitIndicator.PlaySelectEffect();
    }

    public void ChangeAction(ActionType action)
    {
        CurrentAction = action;
        if (action == ActionType.Attack)
            CurrentAttackAction = UnitSo.AttackType;
        else if (action == ActionType.SKill)
        {
            CurrentAttackAction = SkillController.CurrentSkillData.skillType;
        }
    }

    public void ExecuteCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public abstract void ChangeUnitState(Enum newState);

    public abstract void Initialize(UnitSO unit);

    public void ChangeClip(string changedClipName, AnimationClip changeClip)
    {
        AnimatorOverrideController[changedClipName] = changeClip;
        Animator.runtimeAnimatorController = AnimatorOverrideController;
    }

    public Vector3 GetCenter()
    {
        return Collider.bounds.center;
    }
}
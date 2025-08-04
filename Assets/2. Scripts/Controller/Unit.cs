using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class Unit : MonoBehaviour, IDamageable, IAttackable, ISelectable, IUnitFsmControllable, IEffectProvider
{
    private const float ResistancePerStack = 0.08f;

    [SerializeField]
    protected BattleSceneUnitIndicator unitIndicator;


    protected BattleManager BattleManager => BattleManager.Instance;

    public BaseEmotion               CurrentEmotion { get; private set; }
    public BaseEmotion[]             Emotions       { get; private set; }
    public event Action<BaseEmotion> EmotionChanged; // 감정이 바뀌었을 때 알리는 이벤트
    public ActionType                CurrentAction    { get; private set; } = ActionType.None;
    public TurnStateMachine          TurnStateMachine { get; protected set; }
    public ITurnState[]              TurnStates       { get; private set; }
    public TurnStateType             CurrentTurnState { get; private set; }
    public StatManager               StatManager      { get; protected set; }

    public StatusEffectManager        StatusEffectManager        { get; protected set; }
    public SkillManager               SkillManager               { get; protected set; }
    public Animator                   Animator                   { get; protected set; }
    public BaseSkillController        SkillController            { get; protected set; }
    public AnimatorOverrideController AnimatorOverrideController { get; protected set; }
    public Collider                   Collider                   { get; protected set; }
    public NavMeshAgent               Agent                      { get; protected set; }
    public NavMeshObstacle            Obstacle                   { get; protected set; }
    public UnitSO                     UnitSo                     { get; protected set; }
    public AnimationEventListener     AnimationEventListener     { get; protected set; }
    public Unit                       CounterTarget              { get; private set; }
    public Unit                       LastAttacker               { get; private set; }

    public IDamageable   Target              { get; protected set; } //MainTarget, SubTarget => SkillController
    public IAttackAction CurrentAttackAction { get; protected set; }
    public bool          IsDead              { get; protected set; }
    public bool          IsCompletedAttack   { get; protected set; }
    public bool          IsStunned           { get; private set; }
    public bool          IsCounterAttack     { get; private set; }

    public virtual bool IsAtTargetPosition => false;
    public virtual bool IsAnimationDone    { get; set; }
    public virtual bool IsTimeLinePlaying  { get; set; }

    public event          Action OnHitFinished;
    public event          Action OnMeleeAttackFinished;
    public event          Action OnRangeAttackFinished;
    public event          Action OnSkillFinished;
    public abstract event Action OnDead;
    public          Unit         SelectedUnit => this;
    public abstract void         StartTurn();
    public abstract void         EndTurn();
    public abstract void         UseSkill();
    public abstract void         TakeDamage(float amount, StatModifierType modifierType = StatModifierType.Base);


    public abstract void Dead();


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

        // 스택 변경에 대한 반응 등록
        CurrentEmotion.StackChanged += OnEmotionStackChanged;
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
        if (newType == EmotionType.None)
        {
            return;
        }

        if (CurrentEmotion.EmotionType != newType)
        {
            if (Random.value < CurrentEmotion.Stack * ResistancePerStack)
            {
                return;
            }

            // 이전 감정 스택 이벤트 제거
            CurrentEmotion.StackChanged -= OnEmotionStackChanged;

            CurrentEmotion?.Exit(this);
            CurrentEmotion = Emotions[(int)newType];
            CurrentEmotion.Enter(this);

            // 새 감정 스택 이벤트 연결
            CurrentEmotion.StackChanged += OnEmotionStackChanged;

            // 외부 알림
            EmotionChanged?.Invoke(CurrentEmotion);

            // 감정이 새로 바뀐 경우에 즉시 1스택에서 시작
            CurrentEmotion.AddStack(this);

            if (this is PlayerUnitController playerUnit && playerUnit.PassiveSo is IPassiveChangeEmotionTrigger passiveChangeEmotion)
            {
                passiveChangeEmotion.OnChangeEmotion();
            }
        }
        else
        {
            CurrentEmotion.AddStack(this);
        }
    }

    // 감정 스택이 바뀔 때마다 호출됨
    private void OnEmotionStackChanged(int newStack)
    {
        // Debug.Log($"{name}의 감정 스택이 {newStack}로 변경됨");
    }


    public abstract void PlayAttackVoiceSound();
    public abstract void PlayHitVoiceSound();
    public abstract void PlayDeadSound();
    public abstract void Attack();
    public abstract void MoveTo(Vector3 destination);

    public void SetTarget(IDamageable target)
    {
        Target = target;
        SkillController.SelectSkillSubTargets(target);
    }

    // 유닛 선택 가능 토글
    public void ToggleSelectableIndicator(bool toggle)
    {
        if (unitIndicator == null)
        {
            Debug.LogError("유닛에 unitIndicator을 추가해주세요.");
        }

        unitIndicator.ToggleSelectableIndicator(toggle);
    }

    // 유닛 선택됨 표시 토글
    public void ToggleSelectedIndicator(bool toggle)
    {
        if (unitIndicator == null)
        {
            Debug.LogError("유닛에 unitIndicator을 추가해주세요.");
        }

        unitIndicator.ToggleSelectedIndicator(toggle);
    }

    // 유닛 선택 파티클 재생
    public void PlaySelectEffect()
    {
        if (unitIndicator == null)
        {
            Debug.LogError("유닛에 unitIndicator을 추가해주세요.");
        }

        unitIndicator.PlaySelectEffect();
    }

    public void ChangeAction(ActionType action)
    {
        CurrentAction = action;
        if (action == ActionType.SKill)
        {
            CurrentAttackAction = SkillController.CurrentSkillData.skillSo.SkillType;
        }
        else
        {
            CurrentAttackAction = UnitSo.AttackType;
        }
    }

    public void ExecuteCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public abstract void ChangeUnitState(Enum newState);

    public abstract void Initialize(UnitSpawnData spawnData);

    public void ChangeClip(string changedClipName, AnimationClip changeClip)
    {
        AnimatorOverrideController[changedClipName] = changeClip;
        Animator.runtimeAnimatorController = AnimatorOverrideController;
    }

    public Vector3 GetCenter()
    {
        return Collider.bounds.center;
    }

    public bool CanCounterAttack(Unit attacker)
    {
        if (IsDead)
        {
            return false;
        }

        if (attacker == null)
        {
            return false;
        }

        if (attacker.CurrentAttackAction.DistanceType == AttackDistanceType.Range)
        {
            return false;
        }

        if (attacker.CurrentAction == ActionType.SKill)
        {
            return false;
        }

        if (Random.value > StatManager.GetValue(StatType.Counter))
        {
            return false;
        }


        return true;
    }


    public void StartCountAttack(Unit attacker)
    {
        Debug.Log($"{CurrentAction}");
        CounterTarget = attacker;
        IsCounterAttack = true;
        attacker.SetLastAttacker(this);
        if (this is PlayerUnitController)
        {
            ChangeUnitState(PlayerUnitState.Attack);
        }
        else if (this is EnemyUnitController)
        {
            ChangeUnitState(EnemyUnitState.Attack);
        }

        if (CurrentAttackAction.DistanceType == AttackDistanceType.Melee)
        {
            OnMeleeAttackFinished += EndCountAttack;
            OnMeleeAttackFinished += attacker.InvokeHitFinished;
        }
        else
        {
            OnRangeAttackFinished += EndCountAttack;
            OnRangeAttackFinished += attacker.InvokeHitFinished;
        }
    }

    private void EndCountAttack()
    {
        IsCounterAttack = false;
        CounterTarget = null;
    }

    public void OnToggleNavmeshAgent(bool isOn)
    {
        if (isOn)
        {
            Obstacle.carving = false;
            Obstacle.enabled = false;
            Agent.enabled = true;
        }
        else
        {
            Agent.enabled = false;
            Obstacle.enabled = true;
            Obstacle.carving = true;
        }
    }

    public void SetLastAttacker(IAttackable attacker)
    {
        LastAttacker = attacker as Unit;
    }

    public void InvokeHitFinished()
    {
        //반격하는 유닛의 HitFinished가 Null임
        IsAnimationDone = true;
        OnHitFinished?.Invoke();
        OnHitFinished = null;


        if (IsDead)
        {
            LastAttacker?.InvokeHitFinished();
        }

        SetLastAttacker(null);
    }

    public void InvokeAttackFinished()
    {
        IsAnimationDone = true;
        OnMeleeAttackFinished?.Invoke();
        OnMeleeAttackFinished = null;
    }

    public void InvokeRangeAttackFinished()
    {
        IsAnimationDone = true;
        OnRangeAttackFinished?.Invoke();
        OnRangeAttackFinished = null;
    }

    public void InvokeSkillFinished()
    {
        IsAnimationDone = true;
        OnSkillFinished?.Invoke();

        OnSkillFinished = null;
    }
}
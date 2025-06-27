using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Unit : MonoBehaviour, IDamageable, IAttackable, ISelectable, IUnitFsmControllable
{
    private const float ResistancePerStack = 0.08f;

    [SerializeField] private GameObject SelectionEffect; // 유닛 선택 시 표시해주는 이펙트

    public BaseEmotion CurrentEmotion { get; protected set; }
    public bool        IsStunned      { get; private set; }

    protected BattleManager    BattleManager    => BattleManager.Instance;
    public    BaseEmotion[]    Emotions         { get; private set; }
    public    ActionType       CurrentAction    { get; private set; } = ActionType.None;
    public    TurnStateMachine TurnStateMachine { get; protected set; }

    public StatManager         StatManager         { get; protected set; }
    public StatusEffectManager StatusEffectManager { get; protected set; }
    public StatBase            AttackStat          { get; protected set; }
    public IDamageable         Target              { get; protected set; }
    public Collider            Collider            { get; protected set; }
    public bool                IsDead              { get; protected set; }
    public int                 Index               { get; protected set; }
    public UnitSO              UnitSo              { get; protected set; }
    public IAttackAction       CurrentAttackAction { get; private set; }

    public virtual  bool IsAtTargetPosition => false;
    public virtual  bool IsAnimationDone    => false;
    public          Unit SelectedUnit       => this;
    public abstract void StartTurn();

    public abstract void EndTurn();

    public abstract void TakeDamage(float amount, StatModifierType modifierType = StatModifierType.Base);

    public abstract void Dead();

    public event Action<Unit> OnAnimationComplete;

    private void Awake()
    {
        if (SelectionEffect != null)
            SelectionEffect.SetActive(false);
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

    public void SetTarget(IDamageable target)
    {
        Target = target;
    }

    // 유닛 선택 했을 때
    public void OnSelect()
    {
        if (SelectionEffect == null)
            Debug.LogError("SelectionEffect가 null입니다!");
        SelectionEffect.SetActive(true);
    }

    // 유닛 선택 해제 했을 때
    public void OnDeselect()
    {
        if (SelectionEffect == null)
            Debug.LogError("SelectionEffect가 null입니다!");
        SelectionEffect.SetActive(false);
    }

    public void ChangeAction(ActionType action)
    {
        CurrentAction = action;
        if (action == ActionType.Attack)
            CurrentAttackAction = UnitSo.AttackType;
        else if (action == ActionType.SKill)
        {
        }
    }

    public void ExecuteCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public abstract void ChangeUnitState(Enum newState);

    public abstract void Initialize(UnitSO unit);
}
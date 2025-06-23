using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnitController : BaseController<PlayerUnitController, PlayerUnitState>
{
    public override StatBase    AttackStat { get; protected set; }
    public override IDamageable Target     { get; protected set; }
    public Animator Animator;


    protected override IState<PlayerUnitController, PlayerUnitState> GetState(PlayerUnitState state)
    {
        return null;
    }

    protected override void Awake()
    {
        Animator.runtimeAnimatorController = ChangeClip();
    }
    // public override void Attack()
    // {

    //     
    //
    // }

    public override void Attack()
    {
        if (Target == null || Target.IsDead)
            return;

        //이모션따라서
        AttackTypeSo.Attack();
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

        if (StatManager.GetValue(StatType.Defense) < Random.Range(0, 100)) //반격 로직
        {
            //반격을 한다.
            //확 올려버린다.
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
        if (CurrentState == PlayerUnitState.Stun) //스턴이다
            return;

        //선택한 행동에 따라서 실행되는 메서드를 구분
        // 기본공격이면
        Attack();
        //스킬이면
        UseSkill();
    }


    public override void EndTurn()
    {
        //내 턴이 끝날때의 로직을 쓸꺼임.
        //내 상태에 따라서 스택을 쌓는다.
        CurrentEmotion.Execute();
    }
}
using System;
using System.Collections;
using UnityEngine;

public class RangeCombatAction : ICombatAction
{
    private readonly RangeActionSo attackData;
    private readonly IDamageable target;

    public event Action OnActionComplete;
    

    public RangeCombatAction(RangeActionSo so, IDamageable target)
    {
        attackData = so;
        this.target = target;
    }

    public void Execute(Unit attacker)
    {
        TimeLineManager.Instance.PlayTimeLine(CameraManager.Instance.cinemachineBrain,CameraManager.Instance.skillCameraController,attacker);
        if (attackData.IsProjectile)
        {
            attacker.ExecuteCoroutine(WaitForSpawnProjectile());
        }
        else
        {
            attacker.ExecuteCoroutine(WaitForAnimationDone(attacker));
        }
    }

    private IEnumerator WaitForSpawnProjectile()
    {
        yield return new WaitUntil(() => attackData.ProjectileComponent != null);

        attackData.ProjectileComponent.trigger.OnTriggerTarget += () =>
        {
            OnActionComplete?.Invoke();
        };
        CameraManager.Instance.ChangeFollowTarget(attackData.ProjectileComponent);
    }

    private IEnumerator WaitForAnimationDone(Unit attacker)
    {
        yield return new WaitUntil(() => attacker.IsAnimationDone);
        OnActionComplete?.Invoke();
    }
}
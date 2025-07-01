using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeSkillSO", menuName = "ScriptableObjects/SKillType/Range", order = 0)]
public class RangeSkillSO : RangeActionSo
{
    public string subProjectilePoolID;

    public override AttackDistanceType DistanceType => AttackDistanceType.Range;
    public override CombatActionSo     ActionSo     => this;

    public override void Execute(Unit attacker)
    {
        var skillController = attacker.SkillController;

        TargetSelect targetSelect = new TargetSelect(skillController.mainTarget);

        foreach (var effect in skillController.CurrentSkillData.skillEffect.skillEffectDatas)
        {
            skillController.targets = targetSelect.FindTargets(effect.selectTarget, effect.selectCamp);
            foreach (Unit target in skillController.targets)
            {
                if (target == null) continue;
                ProjectileComponent = ObjectPoolManager.Instance.GetObject(effect.projectileID).GetComponent<SkillProjectile>();
                ProjectileComponent.Initialize(effect, skillController.SkillManager.Owner.GetCenter(), target.GetCenter(), target);
            }
        }


        ProjectileComponent.trigger.OnTriggerTarget += ResetProjectile;
    }


    private void ResetProjectile()
    {
        ProjectileComponent = null;
    }
}
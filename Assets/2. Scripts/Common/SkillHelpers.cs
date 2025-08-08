public static class SkillHelpers
{
    public static bool IsProjectileSkillFromCurrent(Unit owner)
    {
        ActiveSkillSO active = owner.SkillController?.CurrentSkillData?.skillSo as ActiveSkillSO;
        if (active == null)
        {
            return false;
        }

        bool byEffect = active.effect != null && active.effect.IsProjectileSkill;

        bool byType = (active.SkillType as RangeSkillSO)?.IsProjectile ?? false;

        return byEffect || byType;
    }
}
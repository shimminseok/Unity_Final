public interface IEmotionOnAttack
{
    void OnBeforeAttack(Unit attacker, ref IDamageable target);
}

public interface IEmotionOnTakeDamage
{
    void OnBeforeTakeDamage(Unit unit, out bool ignoreDamage);
}

public interface IEmotionOnHitChance
{
    void OnCalculateHitChance(ref float hitRate);
}